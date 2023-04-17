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
            if (Input.GetKeyDown(KeyCode.Q))
            {
                animator.Play("-180", -1, 0);
                player.potScore += 1000;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                animator.Play("180", -1, 0);
                player.potScore += 1000;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                animator.Play("Backflip", -1, 0);
                player.potScore += 1000;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                animator.Play("Backslide", -1, 0);
                player.potScore += 1000;
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                animator.Play("Frontflip", -1, 0);
                player.potScore += 1000;
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                animator.Play("Frontslip", -1, 0);
                player.potScore += 1000;
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                animator.Play("720", -1, 0);
                player.potScore += 1000;
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                animator.Play("360", -1, 0);
                player.potScore += 1000;
            }

        }
        if (player._onGround)
        {
            player.playerRgbody.AddForce(player.board.transform.forward * player.potScore/100, ForceMode.Impulse);
            player.actScore += player.potScore;
            player.potScore = 0;            
        }
    }
}
