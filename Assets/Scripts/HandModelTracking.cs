using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandModelTracking : MonoBehaviour
{

public GameObject hand;

public string whatHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hand)
        {
            hand = GameObject.FindGameObjectWithTag(whatHand);
            hand.transform.position = this.transform.localPosition;
            hand.transform.parent = this.transform;
        }
    }
}
