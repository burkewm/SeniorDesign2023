using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using TMPro;
public class GameManager : MonoBehaviour
{
    public Realtime realtime;
    public TMP_Text roomStatus;
    public Keyboard keyboard;
    // Start is called before the first frame update
    void Start()
    {
        realtime.didConnectToRoom += DidConnectToRoom;
        keyboard.EnterPressed += ConnectToRoom;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DidConnectToRoom(Realtime rt) {
        roomStatus.text = "Current room: " + rt.room.name;
    }

    public void ConnectToRoom(object sender, EnterPressedEventArgs e) {
        Room.ConnectOptions options = new Room.ConnectOptions();
        options.appKey = realtime.normcoreAppSettings.normcoreAppKey;
        options.matcherURL = realtime.normcoreAppSettings.matcherURL;
        realtime.Disconnect();
        //roomStatus.text = "Connecting to: " + e.text;
        realtime.Connect(e.text, options);
    }
}
