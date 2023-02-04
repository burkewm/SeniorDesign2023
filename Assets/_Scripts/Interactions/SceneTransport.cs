using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransport : MonoBehaviour
{
    [SerializeField] private bool teleportToNewScene;
    [SerializeField] private int sceneNumber;
    [SerializeField] private Transform transformTeleport;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 7) 
            return;
        
        if (teleportToNewScene)
        {
            SceneManager.LoadScene(sceneNumber);
        }
        else
        {
            other.gameObject.transform.position = transformTeleport.position;
        }
    }
}
