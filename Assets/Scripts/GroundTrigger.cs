using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    private PlayerMover player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerMover>(); 
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider in_collider)
    {
        if (in_collider.gameObject.CompareTag("Ground"))
        {
            player._onGround = true;
        }
    }

    void OnCollisionStay(Collision in_collider)
    {
        if (in_collider.gameObject.CompareTag("Ground"))
        {
            player._onGround = true;
        }
    }

    void OnTriggerExit(Collider in_collider)
    {
        if (in_collider.gameObject.CompareTag("Ground"))
        {
            player._onGround = false;
        }
    }
}