using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class Keyboard : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject lowerCase;
    public GameObject upperCase;
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


}   
