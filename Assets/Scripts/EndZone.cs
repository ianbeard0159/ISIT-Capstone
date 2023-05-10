using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    private GameObject startPoint;
    private PlayerMover pMover;
    private Rigidbody pRB;
    private Collider inCollider;
    private bool activate = false;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = GameObject.FindGameObjectWithTag("StartPoint");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (activate)
        {
            if (inCollider.CompareTag("Player"))
            {
                pRB = inCollider.gameObject.GetComponent<Rigidbody>();
                pMover = inCollider.gameObject.GetComponent<PlayerMover>();
                pMover._inGame = false;
                if (pRB.velocity.magnitude != 0)
                {
                    pRB.AddForce(-pRB.velocity, ForceMode.Acceleration);
                }
                
                if (pRB.velocity.magnitude < 0.01)
                {
                    inCollider.transform.position = startPoint.transform.position;
                    pMover._inGame = true;
                    activate = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider in_collider)
    {
        inCollider = in_collider;
        activate = true;
    }
}
