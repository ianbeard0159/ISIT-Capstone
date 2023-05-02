using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;


public class PlayerMover : MonoBehaviour, IMover
{
    private bool debug = false;

    CharacterController characterController;
    public Rigidbody playerRgbody;
    private Board boardStat;

    //Allows player movement, Will be set true via menu play button, set to false at the end of the map
    public bool _inGame = false;

    //Flags to track player postion
    private bool _inFeature;
    public bool _onGround;
    public bool _closeToGround;

    //Get objects, board used for movement
    //Board for movement, ground and prox for context
    public GameObject board;
    private GameObject ground;
    private GameObject prox;
    
    float timeCount = 0.0f;

    //Score
    public int actScore;
    public int potScore;
    public int flagScore;
    public float highSpeed = 0;
    public double airTime = 0;
    public Stopwatch runTimer; //start on menu button press
    public float runTime; //at endzone, stop timer
    public Stopwatch airTimer;

    public bool inTrick;


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
        boardStat = board.GetComponent<Board>();
        runTimer = new Stopwatch();
        airTimer = new Stopwatch();
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

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (debug)
            {
                debug = false;
            }
            else
            {
                debug = true;
            }
        }

        if (debug)
        {
            var dir = transform.position;
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                dir.x += 1;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                dir.x -= 1;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                dir.z += 1;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                dir.z -= 1;
            }
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                dir.y += 1;
            }
            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                dir.y -= 1;
            }
            transform.position = dir;
        }
        else
        {
            //delete after menu intergration
            _inGame = true;
            if (_inGame)
            {
                if (_inFeature || flag_engaged)
                {

                }
                else if (!_onGround)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, boardRotation, (boardStat.boardRotSpeed / 10) * timeCount);
                    timeCount = timeCount + Time.deltaTime;
                }
                else
                {
                    //adding a constant force based on where the board is facing (not player camera)
                    //note: this is due to the forward property being Normalized, meaning it has a magnitude of 1,
                    //so that it is positioned 1 unit in front of the player.
                    playerRgbody.AddForce(boardDir * boardStat.boardSpeed);


                    //Controls rotation, using Lerp to rotate over time, given the object to rotate (self), final rotation, and speed
                    transform.rotation = Quaternion.Lerp(transform.rotation, boardRotation, boardStat.boardRotSpeed * timeCount);
                    timeCount = timeCount + Time.deltaTime;

                    //old code, use if things get hard
                    //transform.position += cameraDir * Time.deltaTime;
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
    }
    //get stats

    //highest jump
}