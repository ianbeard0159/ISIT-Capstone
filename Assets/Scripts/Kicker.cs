using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kicker : MonoBehaviour
{
    private GameObject player;
    private GameObject board;
    private Vector3 playerSpeed;
    Rigidbody playerRgbody;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRgbody = player.GetComponent<Rigidbody>();
        board = GameObject.FindGameObjectWithTag("Board");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Vector3 boardDir = board.transform.forward;
        playerRgbody.AddForce(boardDir * 20, ForceMode.Impulse);
    }
}
