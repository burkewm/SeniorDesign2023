using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Normal.Realtime;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Brush : MonoBehaviour {
    // Reference to Realtime to use to instantiate brush strokes
    [SerializeField] private Realtime _realtime = null;

    // Prefab to instantiate when we draw a new brush stroke
    [SerializeField] private GameObject _brushStrokePrefab = null;

    // Which hand should this brush instance track?
    private enum Hand { LeftHand, RightHand };
    [SerializeField] private Hand _hand = Hand.RightHand;

    // Used to keep track of the current brush tip position and the actively drawing brush stroke
    public  Vector3     _handPosition;
    public Transform _handtrack;
    public  Quaternion  _handRotation;
    private BrushStroke _activeBrushStroke;

    public InputActionProperty inputProp;

    public InputAction inputAct;

    public UnityEvent<InputAction.CallbackContext> OnStarted;
    public UnityEvent<InputAction.CallbackContext> OnPerformed;
    public UnityEvent<InputAction.CallbackContext> OnCancled;

    public bool triggerPressed;


    public void Awake()
    {
        inputAct = inputProp.action;

        if (OnStarted != null)
        {
            inputAct.started += OnStarted.Invoke;
        }
        if (OnPerformed != null)
        {
            inputAct.performed += OnPerformed.Invoke;
        }
        if (OnCancled != null)
        {
            inputAct.canceled += OnCancled.Invoke;
        }

    }

    private void Update() {
        if (!_realtime.connected)
            return;

        // Start by figuring out which hand we're tracking
        XRNode node    = _hand == Hand.LeftHand ? XRNode.LeftHand : XRNode.RightHand;
        string trigger = _hand == Hand.LeftHand ? "Left Trigger" : "Right Trigger";

        // Get the position & rotation of the hand
        bool handIsTracking = UpdatePose(node, ref _handPosition, ref _handRotation);

        // Figure out if the trigger is pressed or not
        //bool triggerPressed = Input.GetAxisRaw(trigger) > 0.1f;
        
        // If the trigger is pressed and we haven't created a new brush stroke to draw, create one!
        if (triggerPressed && _activeBrushStroke == null) {
            // Instantiate a copy of the Brush Stroke prefab, set it to be owned by us.
            GameObject brushStrokeGameObject = Realtime.Instantiate(_brushStrokePrefab.name, ownedByClient: true, useInstance: _realtime);

            // Grab the BrushStroke component from it
            _activeBrushStroke = brushStrokeGameObject.GetComponent<BrushStroke>();

            // Tell the BrushStroke to begin drawing at the current brush position
            _activeBrushStroke.BeginBrushStrokeWithBrushTipPoint(_handtrack.position, _handRotation);
        }

        // If the trigger is pressed, and we have a brush stroke, move the brush stroke to the new brush tip position
        if (triggerPressed)
            _activeBrushStroke.MoveBrushTipToPoint(_handtrack.position, _handRotation);

        // If the trigger is no longer pressed, and we still have an active brush stroke, mark it as finished and clear it.
        if (!triggerPressed && _activeBrushStroke != null) {
            _activeBrushStroke.EndBrushStrokeWithBrushTipPoint(_handtrack.position, _handRotation);
            _activeBrushStroke = null;
        }
    }

    public void IsDrawing()
    {
        triggerPressed = true;
        Debug.Log("test");
    }

    public void IsReleased()
    {
        triggerPressed = false;
    }
    //// Utility

    // Given an XRNode, get the current position & rotation. If it's not tracking, don't modify the position & rotation.
    private static bool UpdatePose(XRNode node, ref Vector3 position, ref Quaternion rotation) {
        List<XRNodeState> nodeStates = new List<XRNodeState>();
        InputTracking.GetNodeStates(nodeStates);

        foreach (XRNodeState nodeState in nodeStates) {
            if (nodeState.nodeType == node) {
                Vector3    nodePosition;
                Quaternion nodeRotation;
                bool gotPosition = nodeState.TryGetPosition(out nodePosition);
                bool gotRotation = nodeState.TryGetRotation(out nodeRotation);

                if (gotPosition)
                    position = nodePosition;
                if (gotRotation)
                    rotation = nodeRotation;

                return gotPosition;
            }
        }

        return false;
    }
}
