using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMover : MonoBehaviour
{
    private CharacterController characterController;
    private bool _inFeature; //bool for whether player is in feature or not
    public Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //take camera direction and rotation,
        //currently not using camera rotation.
        Vector3 cameraDir = Camera.main.transform.forward;
        Quaternion cameraRot = Camera.main.transform.rotation;

        //player rotation needs to ignore vertical rotation
        //take camerDir as the direction to for the rotation
        var forRotation = cameraDir;
        //change the y axis point to zero
        forRotation.y = 0f;
        //renormalize the vector
        forRotation.Normalize();
        //make rot quaternion using the new look forRotation, using Vector3.up for the world up direction
        var rot = Quaternion.LookRotation(forRotation, Vector3.up);

        if (_inFeature)
        {

        }
        else
        {
            //Uses the rigid body to move the player
            //rb.AddRelativeForce(cameraDir * 2);

            //Just moved the players position in the world by where the Camera is facing,
            //note: this is due to the forward property being Normalized, meaning it has a magnitude of 1,
            //so that it is positioned 1 unit in front of the player.
            transform.position += cameraDir * Time.deltaTime;

            //Controls rotation, using Lerp to rotate over time, given the object to rotate (self), final rotation, and speed
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, 1 * Time.deltaTime);
        }
    }
}