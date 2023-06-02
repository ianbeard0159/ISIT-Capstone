using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowboardSound : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;

    public PlayerMover player;
    public Tricks pTricks;

    public bool initBool;

    public void init()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMover>();
        pTricks = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Tricks>();
        initBool = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!initBool)
        {
            init();
        }
        if (player._onGround || !pTricks.isGamePaused)
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
