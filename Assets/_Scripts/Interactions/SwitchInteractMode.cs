using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class SwitchInteractMode : MonoBehaviour
{
    
    public InputActionProperty inputProp;

    public InputAction inputAct;

    public UnityEvent<InputAction.CallbackContext> OnStarted;
    public UnityEvent<InputAction.CallbackContext> OnPerformed;
    public UnityEvent<InputAction.CallbackContext> OnCancled;

    public List<GameObject> pointerMode;
    public List<GameObject> handMode;

    public bool isPointer;

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
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchMode()
    {
        if (isPointer)
        {
            foreach (var obj in pointerMode)
            {
                obj.SetActive(false);
            }

            isPointer = false;
        }
        else
        {
            foreach (var obj in pointerMode)
            {
                obj.SetActive(true);
            }

            isPointer = true;
        }
    }
}
