using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    private GameObject startPoint;
    private PlayerMover pMover;
    private Rigidbody pRB;
    // Start is called before the first frame update
    void Start()
    {
        startPoint = GameObject.FindGameObjectWithTag("StartPoint");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider in_collider)
    {
        if (in_collider.CompareTag("Player"))
        {
            pRB = in_collider.gameObject.GetComponent<Rigidbody>();
            pMover = in_collider.gameObject.GetComponent<PlayerMover>();
            pMover._inGame = false;
            if (pRB.velocity.magnitude > 0)
            {
                pRB.AddForce(-in_collider.transform.forward * 10);
            }
            else
            {
                in_collider.transform.position = startPoint.transform.position;
            }            
        }
    }
}
