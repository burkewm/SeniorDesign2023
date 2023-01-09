using Normal.Realtime;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace Normcore.Realtime
{
	/// <summary>
	/// This script is intended to be attached to an object with a RealtimeTransform in Rigidbody mode. We have to use reflection 
	/// to override the behaviour because the relevant normcore methods/classes are private/internal/sealed/etc.
	/// 
	/// RealtimeTransform's FixedUpdate method when using the Rigidbody strategy performs some operations multiple
	/// times instead of caching and re-using the results. For one object its a small amount, but when you have dozens
	/// of objects and multiple fixed updates running in one frame it can add up quickly. 
	/// 
	/// While profiling i saw that I had 83 realtime transforms with rigidbodies and on average 2 fixed updates per frame. 
	/// The default behaviour accesses RealtimeModel isOwnedLocallySelf and ownerIdSelf twice each, which is already 332 calls.
	/// This was taking .76ms on my high-end development PC. I didn't test on quest, because its already too high.
	/// 
	/// I added this script to some of the realtime transforms in the scene to roughly compare values in the profiler. 
	/// In a frame with three fixed updates, 30 instances using the original RealtimeTransform took 0.37ms to complete.
	/// In that same frame, 54 instances with this script added took 0.21ms to complete. It's a small sample size and imprecise
	/// metrics, but thats roughly 3 or more times faster if you scale to the same quantity. It would likely be faster again if
	/// this was using normcore methods directly instead of using reflection.
	/// </summary>
	[DefaultExecutionOrder(-94)] // One higher than RealtimeTransform
	public class RealtimeTransformOptimizer : MonoBehaviour
	{
		[SerializeField] RealtimeTransform _realtimeTransform;
		[SerializeField] RealtimeView _realtimeView;
		[SerializeField] Rigidbody _rigidbody;

		bool _isInitialized;

		delegate void IncrementFixedRoomTimeDelegate();
		IncrementFixedRoomTimeDelegate _incrementFixedRoomTimeMethod;

		delegate void RemoteFixedUpdateDelegate(RealtimeTransformModel model);
		RemoteFixedUpdateDelegate _remoteFixedUpdateDelegate;
		RealtimeTransformModel _realtimeTransformModel;

		void Reset()
		{
			_realtimeTransform = GetComponent<RealtimeTransform>();
			_realtimeView = GetComponent<RealtimeView>();
			_rigidbody = GetComponent<Rigidbody>();
		}

		void OnEnable()
		{
			if (_isInitialized) {
				StartAlternateFixedUpdate();
			}
		}

		void Awake()
		{
			_realtimeView.didReplaceAllComponentModels += HandleReplacedAllComponentModels;
		}

		void OnDestroy()
		{
			_realtimeView.didReplaceAllComponentModels -= HandleReplacedAllComponentModels;
		}

		void StartAlternateFixedUpdate()
		{
			_realtimeTransform.StopAllCoroutines();
			_realtimeTransform.StartCoroutine(AlternateCoroutine());
		}

		IEnumerator AlternateCoroutine()
		{
			var wait = new WaitForFixedUpdate();
			int clientId = _realtimeTransform.realtime.clientID;

			int previousOwnerId = _realtimeTransform.ownerIDSelf;
			bool isAlreadyOwned = previousOwnerId == clientId;

			while (true) {
				_incrementFixedRoomTimeMethod();

				if (_realtimeTransform.ownerIDSelf == clientId) {
					if (!isAlreadyOwned) {
						_rigidbody.WakeUp();
						isAlreadyOwned = true;
					}
				} else {
					_remoteFixedUpdateDelegate(_realtimeTransformModel);
					isAlreadyOwned = false;
				}

				yield return wait;
			}
		}

		void HandleReplacedAllComponentModels(RealtimeView view)
		{
			var stategyFieldInfo = _realtimeTransform.GetType().GetField("_strategy", BindingFlags.Instance | BindingFlags.NonPublic);
			object strategyValue = stategyFieldInfo.GetValue(_realtimeTransform);

			var modelPropertyInfo = _realtimeTransform.GetType().GetProperty("model", BindingFlags.Instance | BindingFlags.NonPublic);
			_realtimeTransformModel = (RealtimeTransformModel) modelPropertyInfo.GetValue(_realtimeTransform);

			var strategyType = strategyValue.GetType();

			var incrementMethod = strategyType.GetMethod("IncrementFixedRoomTime", BindingFlags.Instance | BindingFlags.NonPublic);
			_incrementFixedRoomTimeMethod = (IncrementFixedRoomTimeDelegate) Delegate.CreateDelegate(typeof(IncrementFixedRoomTimeDelegate), strategyValue, incrementMethod, true);

			var remoteUpdateMethod = strategyType.GetMethod("RemoteFixedUpdate", BindingFlags.Instance | BindingFlags.NonPublic);
			_remoteFixedUpdateDelegate = (RemoteFixedUpdateDelegate) Delegate.CreateDelegate(typeof(RemoteFixedUpdateDelegate), strategyValue, remoteUpdateMethod, true);

			_isInitialized = true;

			StartAlternateFixedUpdate();
		}
	}
}