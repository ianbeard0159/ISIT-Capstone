using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;

    private PlayerMover player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerMover>(); ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider in_collider)
    {
        if (in_collider.gameObject.CompareTag("Player"))
        {
            player.markerScore += 100;
            source.Play();
        }
    }
}
