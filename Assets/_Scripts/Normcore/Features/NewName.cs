using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewName : MonoBehaviour
{

    public TMP_Text tmText;
    public NameSync syn;
    private void Awake()
    {
        syn = GetComponent<NameSync>();
        tmText.text = PlayerPrefs.GetString("PlayerID");
        syn.SetText(tmText.text);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
   
    }
}
