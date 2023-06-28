using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.InputSystem;
//using UnityEngine.Windows.Speech;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerMover : MonoBehaviour, IMover
{
    public bool initBool = false;
    CharacterController characterController;
    public Rigidbody playerRgbody;
    public Board boardStat;

    //Allows player movement, Will be set true via menu play button, set to false at the end of the map
    public bool _inGame = false;
    public bool _inMenu = false;

    //Flags to track player postion
    private bool _inFeature;
    public bool _onGround = true;
    public bool _closeToGround = true;
    public float distanceToGround;
    public float onGroundOffset;
    public float bufferCheckDistance = 0.001f;

    //Get objects, board used for movement
    //Board for movement, ground and prox for context
    public GameObject board;
    private GameObject ground;
    private GameObject prox;

    public InputAction jumpAction;

    private void OnEnable()
    {
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        jumpAction.Disable();
    }

    //Score
    public int actTrickScore = 0;
    public int potTrickScore = 0;
    public int markerScore = 0;
    public float highSpeed = 0;
    public double airTime = 0;
    public Stopwatch runTimer; //start on menu button press
    public double runTime = 0; //at endzone, stop timer
    public Stopwatch airTimer;
    public float highestAir = 0;

    

    public bool inTrick;


    // Mover Interface
    public bool flag_engaged { get; set; }
    public Transform GetTransform()
    {
        return this.transform;
    }
    public Rigidbody GetRigidbody()
    {
        return this.playerRgbody;
    }

    // Start is called before the first frame update
    void Start()
    {
        runTimer = new Stopwatch();
        airTimer = new Stopwatch();

        runTimer.Start();

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "StartingScene")
        {
            _inGame = false;
        }
        if (scene.name == "FinalScene")
        {
            _inGame = true;
        }

        //delete after menu intergration
        //_inGame = true;
    }

    public void init()
    {
        characterController = GetComponent<CharacterController>();
        playerRgbody = GetComponent<Rigidbody>();
        board = GameObject.FindGameObjectWithTag("Board");
        boardStat = board.GetComponent<Board>();
        initBool = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!initBool)
        {
            init();  
        }
        //Automatic unstuck
        //if (playerRgbody.velocity.magnitude < 0.01)
        //{
        //    Vector3 temp = transform.position;
        //    temp.y += 5;
        //    transform.position = temp;
        //}
        onGroundOffset = (GetComponent<CapsuleCollider>().height) + bufferCheckDistance;

        RaycastHit hit;
        Physics.Raycast(transform.position, -Vector3.up, out hit);
        distanceToGround = hit.distance;
       
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
        var rot = transform.rotation;
        //Controls rotation, using Lerp to rotate over time, given the object to rotate (self), final rotation, and speed
        rot = Quaternion.Lerp(transform.rotation, boardRotation, boardStat.boardRotSpeed * Time.deltaTime);
        transform.rotation = rot;

        Vector3 gravity = Physics.gravity; // + new Vector3(0,-50f,0);

        if (_inGame)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                if (distanceToGround < 0.3)
                {
                    _onGround = true;
                }
                else
                {
                    _onGround = false;
                    //UnityEngine.Debug.Log("DISTANCE FROM GROUND:" + distanceToGround);
                    if (highestAir < distanceToGround)
                    {
                        highestAir = distanceToGround;
                    }
                }
                if (distanceToGround < 5)
                {
                    _closeToGround = true;
                }
                else
                {
                    _closeToGround = false;
                }
            }
            if (_inFeature || flag_engaged)
            {

            }
            else if (!_onGround)
            {
                UnityEngine.Debug.Log("FALLING");
                UnityEngine.Debug.Log("GRAVITY : " + gravity);
                //UnityEngine.Debug.Log("Going : " + desiredVelocity);
                //playerRgbody.velocity = desiredVelocity + gravity;
                UnityEngine.Debug.Log("VELOCITY : " + playerRgbody.velocity);
                //playerRgbody.velocity = desiredVelocity;
                playerRgbody.AddForce(gravity);
            }            
            else if (_onGround)
            {
                UnityEngine.Debug.Log("ON GROUND");
                float speed = playerRgbody.velocity.magnitude;

                playerRgbody.velocity = Vector3.zero;
                Vector3 desiredVelocity = speed * newRotation;// boardRotation.eulerAngles.normalized;
                
                //adding a constant force based on where the board is facing (not player camera)
                //note: this is due to the forward property being Normalized, meaning it has a magnitude of 1,
                //so that it is positioned 1 unit in front of the player.
                if ((playerRgbody.velocity.magnitude < boardStat.boardMaxSpeed))
                {                    
                    playerRgbody.AddForce(boardDir * boardStat.boardSpeed);
                    UnityEngine.Debug.Log("MOVING");
                    UnityEngine.Debug.Log("ADDING SPEED : " + boardDir * boardStat.boardSpeed);
                }
                UnityEngine.Debug.Log("Going : " + desiredVelocity);
                playerRgbody.velocity = desiredVelocity;

                //UnityEngine.Debug.Log("speed : " + speed);
                //UnityEngine.Debug.Log("board rotation : " + newRotation);
                // UnityEngine.Debug.Log("new velocity : " + speed * newRotation);


                //old code, use if things get hard
                //transform.position += cameraDir * Time.deltaTime;       

            }
            if (!_closeToGround)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, boardRotation, (boardStat.boardRotSpeed / 10) * Time.deltaTime);
            }
        }

        //fasted speed
        if (playerRgbody.velocity.magnitude > highSpeed)
        {
            highSpeed = playerRgbody.velocity.magnitude;
        }

        //most air time            
        if (!_onGround)
        {
            airTimer.Start();
        }
        else
        {
            airTimer.Stop();
            if (airTimer.Elapsed.TotalSeconds > airTime)
            {
                airTime = airTimer.Elapsed.TotalSeconds;
                airTimer.Restart();
            }
        }
    }
    public void StatReset()
    {
        actTrickScore = 0;
        potTrickScore = 0;
        markerScore = 0;
        highSpeed = 0;
        airTime = 0;
        runTimer.Reset(); //start on menu button press
        runTime = 0; //at endzone, stop timer
        airTimer.Reset();
        highestAir = 0;

        runTimer.Start();
        airTimer.Start();
    }
}