using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using Normal.Realtime;

public delegate void EnterPressedEventHandler(object sender, EnterPressedEventArgs e);

public class Keyboard : MonoBehaviour
{
    public event EnterPressedEventHandler EnterPressed;
    public GameObject parentCanvas;
    public TMP_InputField inputField;
    public GameObject lowerCase;
    public GameObject upperCase;
    public Realtime realtime;
    public string roomID;
    private bool caps;

    void Start()
    {
        caps = false;
    }

    public void InsertChar(string c) {
        inputField.text += c;
    }

    public void DeleteChar() {
        if (inputField.text.Length > 0) {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
    }

    public void InsertSpace() {
        inputField.text += " ";
    }

    public void CapsPressed() {
        if (!caps) {
            lowerCase.SetActive(false);
            upperCase.SetActive(true);
            caps = true;
        } else {
            lowerCase.SetActive(true);
            upperCase.SetActive(false);
            caps = false;
        }
    }

    public string GetText() {
        return inputField.text;
    }

    public void Enter() {
        EnterPressedEventArgs args = new EnterPressedEventArgs();
        args.text = inputField.text + roomID;
        EnterPressed?.Invoke(this, args);
        inputField.text = "";
        parentCanvas.SetActive(false);
    }
}   
