using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tricks : MonoBehaviour
{
    private Animator animator;
    private PlayerMover player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMover>();
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player._onGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.Play("Backflip", -1, 0);
            }
        }
    }
}
