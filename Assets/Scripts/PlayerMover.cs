using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMover : MonoBehaviour
{
    private CharacterController characterController;
    private bool _inFeature;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get Lerp
        Vector3 dir = Camera.main.transform.forward;

        if (_inFeature)
        { 
                
        }
        else
        {
            transform.position -= dir * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
           // Vector3 newDirection = Vector3.RotateTowards(transform.forward, raycast.direction, 10, 0.0f);
        }
    }
}