using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class EnterButton : KeyboardButton
{
    void Start()
    {
        keyboard = GetComponentInParent<Keyboard>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        material = GetComponent<Renderer>().material;
        color = material.color;

        if (buttonText.text.Length == 1) {
            NameToButtonText();
        }

        isPressed = false;
    }
    
    public void OnSelectExit() {
        //if (isPressed) 
            //do something here
    }
}
