using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class KeyboardButton : MonoBehaviour
{
    protected Keyboard keyboard;
    protected TextMeshProUGUI buttonText;
    protected GameObject presser;
    protected Material material;
    protected Color color;
    protected bool isPressed;

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

    // Update is called once per frame
    public void NameToButtonText() {
        buttonText.text = gameObject.name;
    }

    public void OnSelectEnter() {
        isPressed = true;
    }
    
    public void OnSelectExit() {
        if (isPressed) {
            keyboard.InsertChar(buttonText.text);
            isPressed = false;
        }
    }
    public void OnHoverEnter()
    {
        material.color = Color.blue;
    }

    public void OnHoverExit()
    {
        material.color = color;
    }
}
