using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterScaler : MonoBehaviour
{
    public Transform lHand, rHand, characterTransform;
    
    public float minScale = 0.1f;
    public float maxScale = 1.0f;
    public float maxDistance = 10.0f;

    private float originalDistance;

    private bool hasMoved = false;

    public InputActionProperty leftGrip, rightGrip;
    // Start is called before the first frame update
    void Start()
    {
        originalDistance = Vector3.Distance(lHand.position, rHand.position);
    }

    // Update is called once per frame
    void Update()
    {
        CompareHandDistance();
    }

    private void CompareHandDistance()
    {
        if (leftGrip.action.ReadValue<float>() >= 0.5f && rightGrip.action.ReadValue<float>() >= 0.5f)
        {
            float currentDistance = Vector3.Distance(lHand.position, rHand.position);
            float distance = Mathf.Abs(currentDistance - originalDistance);
            float scale = Mathf.Lerp(minScale, maxScale, distance / maxDistance);
            characterTransform.transform.localScale = new Vector3(scale, scale, scale);
        }
        
        //Debug.Log(normalizedDistance);
    }
}
