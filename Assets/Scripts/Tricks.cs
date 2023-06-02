using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using UnityEngine.InputSystem;
using static PauseMenuScript;
using UnityEngine.SceneManagement;

public class Tricks : MonoBehaviour
{
    public MenuManager Menu;
    public Scene scene;

    public bool initBool = false;

    //Animation Controller
    private Animator animator;
    AnimatorStateInfo animStateInfo;
    public float NTime;
    bool animationFinished;

    //Player mover script
    GameObject player;
    private PlayerMover pMover;
    Rigidbody pRB;

    //int to manage time to hover
    public static float hoverBuffer;

    //Voice comands
    //keywords are the phrases the game will be looking for
    public string[] keywords = new string[] {
        "reset", "go", "unstuck","resume","stop",
        "pause", "negative one eighty", "one eighty",
        "Backflip", "Backslide","Frontflip", "Frontslide",
        "seven twenty", "three sixty", "Options", "Boards",
        "Play", "Stages", "Stats", "Back", "Video", "Difficulty",
        "Accessibility", "Audio", "one", "two", "three", "four",
        "Tutorial", "Main Menu", "Menu" };
public ConfidenceLevel confidence = ConfidenceLevel.Low;
    protected PhraseRecognizer recognizer;
    public string results; //results might be extra, consider deleting
    protected string word = ""; //what the player has said

    public GameObject pauseMenuUI;
    public Transform playerReference;
    public Vector3 offset;
    private bool isGamePaused = false;
    private bool shouldBePaused = false;
    Vector3 returnMenuPosition;
    public PauseMenuScript pauseMenu;


    //Inputs for the tricks, button triggers are made in the unity inspector
    public InputAction negativeOneEightyAction;
    public InputAction oneEightyAction;
    public InputAction threeSixtyAction;
    public InputAction sevenTwentyAction;    
    public InputAction backflipAction;
    public InputAction backslideAction;
    public InputAction frontflipAction;
    public InputAction frontslideAction;
    public InputAction jumpAction;
    public InputAction unstuck;

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
        jumpAction.Enable();
        unstuck.Enable();
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
        jumpAction.Disable();
        unstuck.Disable();
    }

    public void init()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "StartingScene")
        {
            Menu = GameObject.FindGameObjectWithTag("Menu").GetComponent<MenuManager>();    
        }
        if (scene.name == "FinalScene")
        {
            returnMenuPosition = GameObject.FindGameObjectWithTag("ReturnMenuPosition").transform.position;
        }
        //get the player mover and animator
        player = GameObject.FindGameObjectWithTag("Player");
        pMover = player.GetComponent<PlayerMover>();
        pRB = player.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
        
        initBool = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        //set pausereset to false
        pauseReset = false;
        hoverTime = 0;
        hoverBuffer = 0f;

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
        Debug.Log(results);
    }

    // Update is called once per frame
    void Update()
    {
        if (!initBool)
        {        
            init();
        }
        if (!pMover.initBool)
        {
            pMover.init();
        }
        //logs the last trick used
        var lastTrick = "";
        if (scene.name == "StartingScene"){

        
            if (Menu.context == "Panel_Main")
            {
                if (word == "options")
                {
                    Menu.SetCurrentFromVoice("Panel_Options");
                    word = "";
                }
                if (word == "Boards")
                {
                    Menu.SetCurrentFromVoice("Panel_Boards");
                    word = "";
                }
                if (word == "Stages")
                {
                    Menu.SetCurrentFromVoice("Panel_Stages");
                    word = "";
                }
            }
            if (Menu.context == "Panel_Options")
            {
                if (word == "Back")
                {
                    Menu.SetCurrentFromVoice("Panel_Main");
                    word = "";
                }
                if (word == "Video")
                {
                    Debug.Log("VIDEO SETTINGS");
                    word = "";
                }
                if (word == "Difficulty")
                {
                    Debug.Log("DIFFICULTY SETTINGS");
                    word = "";
                }
                if (word == "Accessibility")
                {
                    Debug.Log("ACCESSIBILITY SETTINGS");
                    word = "";
                }
                if (word == "Audio")
                {
                    Debug.Log("AUDIO SETTINGS");
                    word = "";
                }
            }
            if (Menu.context == "Panel_Boards")
            {
                if (word == "Back")
                {
                    Menu.SetCurrentFromVoice("Panel_Main");
                    word = "";
                }
                if (word == "One")
                {
                    Debug.Log("BOARD ONE");
                    word = "";
                }
                if (word == "Two")
                {
                    Debug.Log("BOARD TWO");
                    word = "";
                }
                if (word == "Three")
                {
                    Debug.Log("BOARD THREE");
                    word = "";
                }
                if (word == "Four")
                {
                    Debug.Log("BOARD FOUR");
                    word = "";
                }
            }
            if (Menu.context == "Panel_Stage")
            {
                if (word == "Back")
                {
                    Menu.SetCurrentFromVoice("Panel_Main");
                    word = "";
                }
                if (word == "One")
                {
                    Debug.Log("STAGE ONE");
                    word = "";
                }
                if (word == "Two")
                {
                    Debug.Log("STAGE TWO");
                    word = "";
                }
                if (word == "Three")
                {
                    Debug.Log("STAGE THREE");
                    word = "";
                }
                if (word == "Four")
                {
                    Debug.Log("STAGE FOUR");
                    word = "";
                }
                if (word == "Tutorial")
                {
                    Debug.Log("STAGE TUTORIAL");
                    word = "";
                }
            }
            if (Menu.context == "Panel_Stats")
            {
                if (word == "Back")
                {
                    Menu.SetCurrentFromVoice("Panel_Main");
                    word = "";
                }
            }
        }

            //if the player is not on the ground, or in a trick,
            //they can call out a trick name or 'gamepad' input
            Debug.Log(pMover);
        if (!pMover._closeToGround)
        {
            animator.enabled = true;
            if (!pMover.inTrick)
            {
                if (pMover.flag_engaged)
                {
                    if (negativeOneEightyAction.triggered || word == "negative one eighty")
                    {
                        //once in a trick, set intrick to true, play the animation
                        pMover.inTrick = true;
                        animator.Play("-180");
                        //animator.SetBool();
                        //animator.SetTrigger();
                        //in this trick matches the last trick, player recieves reduced points
                        if (word == lastTrick)
                        {
                            pMover.potTrickScore += 500;
                        }
                        else
                        {
                            pMover.potTrickScore += 1000;
                        }
                        //set this trick as the last trick
                        lastTrick = word;
                        word = "";
                    }
                    if (oneEightyAction.triggered || word == "one eighty")
                    {
                        pMover.inTrick = true;
                        animator.Play("180");
                        if (word == lastTrick)
                        {
                            pMover.potTrickScore += 500;
                        }
                        else
                        {
                            pMover.potTrickScore += 1000;
                        }
                        lastTrick = word;
                        word = "";
                    }
                    if (backslideAction.triggered || word == "Backslide")
                    {
                        pMover.inTrick = true;
                        animator.Play("Backslide");
                        if (word == lastTrick)
                        {
                            pMover.potTrickScore += 500;
                        }
                        else
                        {
                            pMover.potTrickScore += 1000;
                        }
                        lastTrick = word;
                        word = "";
                    }
                    if (frontslideAction.triggered || word == "Frontslide")
                    {
                        pMover.inTrick = true;
                        animator.Play("Frontslide");
                        if (word == lastTrick)
                        {
                            pMover.potTrickScore += 500;
                        }
                        else
                        {
                            pMover.potTrickScore += 1000;
                        }
                        lastTrick = word;
                        word = "";
                    }
                }
                else
                {
                    if (backflipAction.triggered || word == "Backflip")
                    {
                        pMover.inTrick = true;
                        animator.Play("Backflip");
                        if (word == lastTrick)
                        {
                            pMover.potTrickScore += 500;
                        }
                        else
                        {
                            pMover.potTrickScore += 1000;
                        }
                        lastTrick = word;
                        word = "";
                    }

                    if (frontflipAction.triggered || word == "Frontflip")
                    {
                        pMover.inTrick = true;
                        animator.Play("Frontflip");
                        if (word == lastTrick)
                        {
                            pMover.potTrickScore += 500;
                        }
                        else
                        {
                            pMover.potTrickScore += 1000;
                        }
                        lastTrick = word;
                        word = "";
                    }

                    if (sevenTwentyAction.triggered || word == "seven twenty")
                    {
                        pMover.inTrick = true;
                        animator.Play("720");
                        if (word == lastTrick)
                        {
                            pMover.potTrickScore += 500;
                        }
                        else
                        {
                            pMover.potTrickScore += 1000;
                        }
                        lastTrick = word;
                        word = "";
                    }
                    if (threeSixtyAction.triggered || word == "three sixty")
                    {
                        pMover.inTrick = true;
                        animator.Play("360");
                        if (word == lastTrick)
                        {
                            pMover.potTrickScore += 500;
                        }
                        else
                        {
                            pMover.potTrickScore += 1000;
                        }
                        lastTrick = word;
                        word = "";
                    }
                }
                if (pMover._onGround)
                {
                    //animator.enabled = false;
                    pMover.playerRgbody.AddForce(pMover.board.transform.forward * pMover.potTrickScore / 100, ForceMode.Impulse);
                    pMover.actTrickScore += pMover.potTrickScore;
                    pMover.potTrickScore = 0;
                    pMover.inTrick = false;
                    lastTrick = "";
                    word = "";
                }
            }
            else
            {
                if (pMover._onGround)
                {
                    //animator.enabled = false;
                    pMover.playerRgbody.AddForce(pMover.board.transform.forward * pMover.potTrickScore / 100, ForceMode.Impulse);
                    pMover.actTrickScore += pMover.potTrickScore;
                    pMover.potTrickScore = 0;
                    pMover.inTrick = false;
                    lastTrick = "";
                    word = "";
                }
                if (pMover._onGround)
                {
                    //crash
                    //visual animation
                    pMover.playerRgbody.AddForce(pMover.board.transform.forward * -20, ForceMode.Impulse);
                    pMover.potTrickScore = 0;
                    pMover.inTrick = false;
                    lastTrick = "";
                    word = "";
                }
            }
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            {
                animationFinished = true;
                pMover.inTrick = false;
            }
        }
        switch (word){
            case "pause":
            case "stop":
                Debug.Log("CALLING STOP!");
                if (shouldBePaused)
                    shouldBePaused = false;
                else
                    shouldBePaused = true;

                Debug.Log("SHOULD BE PAUSED : " + shouldBePaused);
                word = "";
                break;
            case "resume":
            case "go":
                Debug.Log("UNPAUSE!");
                shouldBePaused = false;
                word = "";
                break;
            case "unstuck":
                Vector3 temp = transform.position;
                temp.y += 10;
                player.transform.position = temp;
                word = "";
                break;
            case "jump":
                pRB.AddForce(100 * Vector3.up, ForceMode.Impulse);
                word = "";
                break;
            default:
                break;
        }
        
        

        /*
        if (word == "pause" || word == "stop")
        {
            Debug.Log("CALLING STOP!");
            if (shouldBePaused)
                shouldBePaused = false;
            else
                shouldBePaused = true;

            Debug.Log("SHOULD BE PAUSED : " + shouldBePaused);
            word = "";
        }
        if (word == "resume" || word == "go")
        {
            Debug.Log("UNPAUSE!");
            shouldBePaused = false;
            word = "";
        }
        if (word == "unstuck" || unstuck.triggered)
        {
            Vector3 temp = transform.position;
            temp.y += 10;
            player.transform.position = temp;
            word = "";
        }
        if (word == "jump" || jumpAction.triggered)
        {
            pRB.AddForce(100 * Vector3.up, ForceMode.Impulse);
            word = "";
        }

        */

        if (unstuck.triggered)
        {
            Vector3 temp = transform.position;
            temp.y += 10;
            player.transform.position = temp;
            word = "";
        }
        else if (jumpAction.triggered)
        {
            pRB.AddForce(100 * Vector3.up, ForceMode.Impulse);
            word = "";
        }

        if (shouldBePaused && !isGamePaused)
        {
            Pause();

        }
        else if (!shouldBePaused && isGamePaused)
        {
            Resume();
        }
        if (isGamePaused)
        {
            if (word == "reset")
            {
                pauseMenu.TpToStartPoint();
                word = "";
            }
        }
        if (pMover._onGround)
        {
            word = "";
        }
    }

    public void Resume()
    {
        //make the time sacle change over time
        pauseMenuUI.SetActive(false);
        //ReturnMenuPosition();
        Time.timeScale = 1f;
        isGamePaused = false;
        Debug.Log("Resume");
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        
        isGamePaused = true;
        Debug.Log(isGamePaused);
        SetMenuPosition();
        Time.timeScale = 0f;
        Debug.Log("Pause");
    }

    void SetMenuPosition()
    {
        pauseMenuUI.transform.position = player.transform.position + player.transform.forward * 5 + offset;
        pauseMenuUI.transform.rotation = player.transform.rotation;
    }

    void ReturnMenuPosition()
    {
        pauseMenuUI.transform.position = returnMenuPosition;
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
