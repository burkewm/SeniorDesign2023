using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR;
using UnityEngine.InputSystem;

public class ShowKeyboardToggle : MonoBehaviour
{
    public InputActionReference showKeyboardRef = null;
    public GameObject keyboard;
    bool keyboardDisplayed = false;
    // Start is called before the first frame update
    void Start()
    {
        showKeyboardRef.action.performed += ToggleKeyboard;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ToggleKeyboard(InputAction.CallbackContext context) {
        if (!keyboardDisplayed) {
            keyboard.gameObject.SetActive(true);
            keyboardDisplayed = true;
        } else {
            keyboard.gameObject.SetActive(false);
            keyboardDisplayed = false;
        }
    }
}
