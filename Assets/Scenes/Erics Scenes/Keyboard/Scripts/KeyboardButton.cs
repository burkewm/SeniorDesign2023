using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using TMPro;

public class KeyboardButton : MonoBehaviour
{
    protected Keyboard keyboard;
    protected TextMeshProUGUI buttonText;
    protected GameObject presser;
    protected Material material;
    protected Color color;

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

    // Update is called once per frame
    public void NameToButtonText() {
        buttonText.text = gameObject.name;
    }

    public void OnSelectEnter() {
    }
    
    public void OnSelectExit() {
        keyboard.InsertChar(buttonText.text);
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
