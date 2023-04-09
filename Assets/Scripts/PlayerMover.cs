using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMover : MonoBehaviour, IMover
{
    CharacterController characterController;
    Rigidbody playerRgbody;

    //Flags to track player postion
    private bool _inFeature;
    private bool _onGround;
    private bool _closeToGournd;

    //Get board used for movement
    private GameObject board;

    //Linear Movement
    [SerializeField]
    float boardSpeed;

    //Rotational Movement
    [SerializeField]
    float boardRotSpeed = 0.01f;
    float timeCount = 0.0f;

    // Mover Interface
    public bool flag_engaged {get; set;}
    public Transform GetTransform() {
        return this.transform;
    }
    public Rigidbody GetRigidbody() {
        return this.playerRgbody;
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerRgbody = GetComponent<Rigidbody>();
        board = GameObject.FindGameObjectWithTag("Board");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //take camera direction and rotation,
        //currently not using camera rotation.
        Vector3 cameraDir = Camera.main.transform.forward;
        Quaternion cameraRot = Camera.main.transform.rotation;
        //take board direction
        Vector3 boardDir = board.transform.forward;

        //player rotation needs to ignore vertical rotation
        //take camerDir as the direction to for the rotation
        var newRotation = cameraDir;
        //change the y axis point to zero
        newRotation.y = 0f;
        //renormalize the vector
        newRotation.Normalize();
        //make rot quaternion using the new look forRotation, using Vector3.up for the world up direction
        var boardRotation = Quaternion.LookRotation(newRotation, Vector3.up);

        if (_inFeature || flag_engaged)
        {

        }
        else if(!_onGround)
        {
            //adding a constant force based on where the board is facing (not player camera)
            //note: this is due to the forward property being Normalized, meaning it has a magnitude of 1,
            //so that it is positioned 1 unit in front of the player.
            playerRgbody.AddForce(boardDir * boardSpeed);

            

            //Controls rotation, using Lerp to rotate over time, given the object to rotate (self), final rotation, and speed
            transform.rotation = Quaternion.Lerp(transform.rotation, boardRotation, boardRotSpeed * timeCount);
            timeCount = timeCount + Time.deltaTime;

            //old code, use if things get hard
            //transform.position += cameraDir * Time.deltaTime;
        }
    }
}