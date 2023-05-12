using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowboardSound : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;

    public PlayerMover player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMover>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player._onGround)
        {
            if (!source.isPlaying)
            {
                source.Play();
            }            
        }
        else
        {
            source.Stop();
        }
    }
}
