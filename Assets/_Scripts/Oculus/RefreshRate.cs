using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshRate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Unity.XR.Oculus.Performance.TrySetDisplayRefreshRate(90f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
