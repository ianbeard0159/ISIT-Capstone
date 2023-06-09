using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowboardSound : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;

    public PlayerMover player;
    public Tricks pTricks;
    public bool pOnGround;
    public bool gamePaused;

    public bool initBool;
    public bool shouldBePlaying;

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
        pOnGround = player._onGround;
        gamePaused = pTricks.isGamePaused;

        if (!initBool)
        {
            init();
        }
        else
        {
            shouldBePlaying = true;
            if (!pOnGround || gamePaused)
            {
                    shouldBePlaying = false;
            }
            if (shouldBePlaying)
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
}
