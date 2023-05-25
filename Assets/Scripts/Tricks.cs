using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using UnityEngine.InputSystem;

public class Tricks : MonoBehaviour
{
    //Animation Controller
    private Animator animator;
    AnimatorStateInfo animStateInfo;
    public float NTime;
    bool animationFinished;

    //Player mover script
    private PlayerMover player;
    
    //Voice comands
    //keywords are the phrases the game will be looking for
    public string[] keywords = new string[] {"stop", "pause", "negative one eighty", "one eighty", "Backflip", "Backslide","Frontflip", "Frontslide", "seven twenty", "three sixty"};
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    protected PhraseRecognizer recognizer;
    public string results; //results might be extra, consider deleting
    protected string word = ""; //what the player has said

    public GameObject pauseMenuUI;
    public Transform playerReference;
    public Vector3 offset;
    private bool isGamePaused = false;
    private bool shouldBePaused = false;


    //Inputs for the tricks, button triggers are made in the unity inspector
    public InputAction negativeOneEightyAction;
    public InputAction oneEightyAction;
    public InputAction threeSixtyAction;
    public InputAction sevenTwentyAction;    
    public InputAction backflipAction;
    public InputAction backslideAction;
    public InputAction frontflipAction;
    public InputAction frontslideAction;

    //boolean to toss the player off of a spline when reset from pause menu
    public static bool pauseReset;
    //int to manage time to hover
    public static int hoverTime;

    //When this script is enabled, all the actions are also enabled
    private void OnEnable()
    {
        negativeOneEightyAction.Enable();
        oneEightyAction.Enable();
        threeSixtyAction.Enable();
        sevenTwentyAction.Enable();
        backflipAction.Enable();
        backslideAction.Enable();
        frontflipAction.Enable();
        frontslideAction.Enable();
    }

    //When this script is disabled, all actions are also disabled
    private void OnDisable()
    {
        negativeOneEightyAction.Disable();
        oneEightyAction.Disable();
        threeSixtyAction.Disable();
        sevenTwentyAction.Disable();
        backflipAction.Disable();
        backslideAction.Disable();
        frontflipAction.Disable();
        frontslideAction.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        //set pausereset to false
        pauseReset = false;
        hoverTime = 0;

        //get the player mover and animator
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMover>();
        animator = GetComponentInChildren<Animator>();
        animator.enabled = false;

        //checks animation states, used for seeing if animation is finish
        //animStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //fills the voice conrol recognizer with the keyword list
        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
            Debug.Log(recognizer.IsRunning);
        }

        //logs all the avalibe microphones
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        results = "You said: <b>" + word + "</b>";
    }

    // Update is called once per frame
    void Update()
    {
        
        //logs the last trick used
        var lastTrick = "";
        NTime = animStateInfo.normalizedTime; //grabs the time the animation plays, should max out at 1 (second)
        if (animationFinished)
        {
            //if animation is finished, player should not be considered in a trick
           // player.inTrick = false;
        }
        //if the player is not on the ground, or in a trick,
        //they can call out a trick name or 'gamepad' input
        if (!player._onGround)
        {
            animator.enabled = true;
            if (!player.inTrick)
            {
                if (negativeOneEightyAction.triggered || word == "negative one eighty")
                {
                    //once in a trick, set intrick to true, play the animation
                    player.inTrick = true;
                    animator.Play("-180");
                    //animator.SetBool();
                    //animator.SetTrigger();
                    //in this trick matches the last trick, player recieves reduced points
                    if (word == lastTrick)
                    {
                        player.potTrickScore += 500;
                    }
                    else
                    {
                        player.potTrickScore += 1000;
                    }
                    //set this trick as the last trick
                    lastTrick = word;
                    word = "";
                }
                if (oneEightyAction.triggered || word == "one eighty")
                {
                    player.inTrick = true;
                    animator.Play("180");
                    if (word == lastTrick)
                    {
                        player.potTrickScore += 500;
                    }
                    else
                    {
                        player.potTrickScore += 1000;
                    }
                    lastTrick = word;
                    word = "";
                }
                if (backflipAction.triggered || word == "Backflip")
                {
                    player.inTrick = true;
                    animator.Play("Backflip");
                    if (word == lastTrick)
                    {
                        player.potTrickScore += 500;
                    }
                    else
                    {
                        player.potTrickScore += 1000;
                    }
                    lastTrick = word;
                    word = "";
                }
                if (backslideAction.triggered || word == "Backslide")
                {
                    player.inTrick = true;
                    animator.Play("Backslide");
                    if (word == lastTrick)
                    {
                        player.potTrickScore += 500;
                    }
                    else
                    {
                        player.potTrickScore += 1000;
                    }
                    lastTrick = word;
                    word = "";
                }
                if (frontflipAction.triggered || word == "Frontflip")
                {
                    player.inTrick = true;
                    animator.Play("Frontflip");
                    if (word == lastTrick)
                    {
                        player.potTrickScore += 500;
                    }
                    else
                    {
                        player.potTrickScore += 1000;
                    }
                    lastTrick = word;
                    word = "";
                }
                if (frontslideAction.triggered || word == "Frontslide")
                {
                    player.inTrick = true;
                    animator.Play("Frontslide");
                    if (word == lastTrick)
                    {
                        player.potTrickScore += 500;
                    }
                    else
                    {
                        player.potTrickScore += 1000;
                    }
                    lastTrick = word;
                    word = "";
                }
                if (sevenTwentyAction.triggered || word == "seven twenty")
                {
                    player.inTrick = true;
                    animator.Play("720");
                    if (word == lastTrick)
                    {
                        player.potTrickScore += 500;
                    }
                    else
                    {
                        player.potTrickScore += 1000;
                    }
                    lastTrick = word;
                    word = "";
                }
                if (threeSixtyAction.triggered || word == "three sixty")
                {
                    player.inTrick = true;
                    animator.Play("360");
                    if (word == lastTrick)
                    {
                        player.potTrickScore += 500;
                    }
                    else
                    {
                        player.potTrickScore += 1000;
                    }
                    lastTrick = word;
                    word = "";
                }
                if (player._onGround)
                {
                    //animator.enabled = false;
                    player.playerRgbody.AddForce(player.board.transform.forward * player.potTrickScore / 100, ForceMode.Impulse);
                    player.actTrickScore += player.potTrickScore;
                    player.potTrickScore = 0;
                    player.inTrick = false;
                    lastTrick = "";
                    word = "";
                }
            }
            else
            {
                if (player._onGround)
                {
                    //animator.enabled = false;
                    player.playerRgbody.AddForce(player.board.transform.forward * player.potTrickScore / 100, ForceMode.Impulse);
                    player.actTrickScore += player.potTrickScore;
                    player.potTrickScore = 0;
                    player.inTrick = false;
                    lastTrick = "";
                    word = "";
                }
                if (player._onGround)
                {
                    //crash
                    //visual animation
                    player.playerRgbody.AddForce(player.board.transform.forward * -20, ForceMode.Impulse);
                    player.potTrickScore = 0;
                    player.inTrick = false;
                    lastTrick = "";
                    word = "";
                }
            }
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            {
                animationFinished = true;
                player.inTrick = false;
            }
        }
        if (word == "pause" || word == "stop")
        {
            if (shouldBePaused)
                shouldBePaused = false;
            else
                shouldBePaused = true;
            if (shouldBePaused && !isGamePaused)
            {
                Pause();

            }
            else if (!shouldBePaused && isGamePaused)
            {
                Resume();
            }
        }
    }

    public void Resume()
    {
        //make the time sacle change over time
        //pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        Debug.Log("Resume");
    }

    void Pause()
    {
        //pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        SetMenuPosition();
        Debug.Log("Pause");
    }

    void SetMenuPosition()
    {
        pauseMenuUI.transform.position = playerReference.position + offset;
        pauseMenuUI.transform.rotation = playerReference.rotation;
    }

    //shuts down voice recognition when the game closes
    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }
}
