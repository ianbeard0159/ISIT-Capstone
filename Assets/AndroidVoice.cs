using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidVoice : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FindMicrophones()
    {
        //logs all the available microphones
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
    }
}
