using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.InputSystem;
using UnityEngine.Windows.Speech;
using System.Linq;


public class PlayerMover : MonoBehaviour, IMover
{
    CharacterController characterController;
    public Rigidbody playerRgbody;
    public Board boardStat;

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

    ////Voice comands
    ////keywords are the phrases the game will be looking for
    //public string[] keywords = new string[] { "pause" };
    //public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    //protected PhraseRecognizer recognizer;
    //public string results; //results might be extra, consider deleting
    //protected string word = ""; //what the player has said

    public InputAction jumpAction;

    private void OnEnable()
    {
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        jumpAction.Disable();
    }

    float timeCount = 0.0f;

    //Score
    public int actTrickScore;
    public int potTrickScore;
    public int markerScore;
    public float highSpeed = 0;
    public double airTime = 0;
    public Stopwatch runTimer; //start on menu button press
    public double runTime; //at endzone, stop timer
    public Stopwatch airTimer;

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
        characterController = GetComponent<CharacterController>();
        playerRgbody = GetComponent<Rigidbody>();
        board = GameObject.FindGameObjectWithTag("Board");
        boardStat = board.GetComponent<Board>();
        runTimer = new Stopwatch();
        airTimer = new Stopwatch();

        //fills the voice conrol recognizer with the keyword list
        //if (keywords != null)
        //{
        //    recognizer = new KeywordRecognizer(keywords, confidence);
        //    recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
        //    recognizer.Start();
        //}

        //delete after menu intergration
        _inGame = true;
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
        var rot = transform.rotation;

        if (_inGame)
        {
            if (_inFeature || flag_engaged)
            {

            }
            else if (!_closeToGround)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, boardRotation, (boardStat.boardRotSpeed / 10) * timeCount);
                timeCount = timeCount + Time.deltaTime;


            }
            else if (!_onGround)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, boardRotation, (boardStat.boardRotSpeed) * timeCount);
                timeCount = timeCount + Time.deltaTime;


            }
            else
            {
                //adding a constant force based on where the board is facing (not player camera)
                //note: this is due to the forward property being Normalized, meaning it has a magnitude of 1,
                //so that it is positioned 1 unit in front of the player.
                if (playerRgbody.velocity.magnitude < boardStat.boardMaxSpeed)
                {
                    playerRgbody.AddForce(boardDir * boardStat.boardSpeed);
                }

                //Controls rotation, using Lerp to rotate over time, given the object to rotate (self), final rotation, and speed
                rot = Quaternion.Lerp(transform.rotation, boardRotation, boardStat.boardRotSpeed * timeCount);
                transform.rotation = rot;
                float speed = playerRgbody.velocity.magnitude;

                playerRgbody.velocity = Vector3.zero;
                Vector3 desiredVelocity = speed * newRotation;// boardRotation.eulerAngles.normalized;
                Vector3 gravity = Physics.gravity; // + new Vector3(0,-50f,0);
                if (!_onGround)
                {
                    playerRgbody.velocity = desiredVelocity + gravity * Time.fixedDeltaTime;
                }
                else
                {
                    playerRgbody.velocity = desiredVelocity;
                }
                
                UnityEngine.Debug.Log("gravity : " + gravity);
                //playerRgbody.rotation = rot;
                timeCount = timeCount + Time.deltaTime;
                UnityEngine.Debug.Log("speed : " + speed);
                UnityEngine.Debug.Log("board rotation : " + newRotation);
                UnityEngine.Debug.Log("new velocity : " + speed * newRotation);


                //old code, use if things get hard
                //transform.position += cameraDir * Time.deltaTime;

                

                if (jumpAction.triggered)
                {
                    //player jumps
                    playerRgbody.velocity = new Vector3(0, 10, 0);
                }

                
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

    ////shuts down voice recognition when the game closes
    //private void OnApplicationQuit()
    //{
    //    if (recognizer != null && recognizer.IsRunning)
    //    {
    //        recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
    //        recognizer.Stop();
    //    }
    //}
    //get stats

    //highest jump
}