using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kicker : MonoBehaviour
{
    [SerializeField]
    float LiftSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider in_collider)
    {
        if (in_collider.CompareTag("Player") )
        {
            Debug.Log("PLAYER IN LIFT ZONE");
            Rigidbody mover = in_collider.gameObject.GetComponent<Rigidbody>();

            mover.AddForce(in_collider.transform.forward * LiftSpeed, ForceMode.Impulse);
        }
    }
}
