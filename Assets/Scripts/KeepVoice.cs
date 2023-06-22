using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Voice;

public class KeepVoice : MonoBehaviour
{

    public bool notListening = true;
        public AppVoiceExperience _voice;

    // Start is called before the first frame update
    void Start()
    {
        _voice = FindObjectOfType<AppVoiceExperience>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            _voice.Activate();
        }
        if (notListening)
        {
            _voice.Activate();
            notListening = false;
        }
        else
        {
            notListening = true;
        }
    }
}
