using System;
using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RealtimeInteraction : MonoBehaviour
{
    public RealtimeTransform rt;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void TakeOwnership(bool isGravity)
    {
        rt.RequestOwnership();
        if (isGravity)
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<XRGrabInteractable>().movementType = XRBaseInteractable.MovementType.Kinematic;
        }
    }

    public void ClearOwnership()
    {
        GetComponent<XRGrabInteractable>().movementType = XRBaseInteractable.MovementType.VelocityTracking;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
    }
    
}
