using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;
using TMPro;


public class NameSync : RealtimeComponent<NameSyncModel>
{
    [SerializeField]
    public TMP_Text saveText;

    private void Awake()
    {
        saveText = GetComponent<TMP_Text>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator newName()
    {
        yield return new WaitForSeconds(15f);
        SetText("name test");
    }

    protected override void OnRealtimeModelReplaced(NameSyncModel previousModel, NameSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.nameDidChange -= NameDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.name = saveText.text;
            }
            UpdateText();

            currentModel.nameDidChange += NameDidChange;
        }
    }

    private void NameDidChange(NameSyncModel model, string value)
    {
        UpdateText();
    }

    private void UpdateText()
    {
        saveText.text = model.name;
    }

    public void SetText(string text)
    {
        model.name = saveText.text;
    }
}
