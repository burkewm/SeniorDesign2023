using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class CapsButton : KeyboardButton
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
    }
    
    public void OnSelectExit() {
        keyboard.CapsPressed();
    }
}
