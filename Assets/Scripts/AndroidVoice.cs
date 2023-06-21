using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidVoice : MonoBehaviour
{
    public MenuManager pMenu;
    public bool initBool = false;

    //Animation Controller
    private Animator animator;

    //Player mover script
    GameObject player;
    private PlayerMover pMover;
    Rigidbody pRB;

    //int to manage time to hover
    public static float hoverBuffer;

    public Transform playerReference;
    public Vector3 offset;
    public bool isGamePaused = false;
    public PauseMenuScript pauseScript;

    //boolean to toss the player off of a spline when reset from pause menu
    public static bool pauseReset;
    //int to manage time to hover
    public static int hoverTime;
    public string lastTrick = ""; 

    public void init()
    {
        pMenu = GameObject.FindGameObjectWithTag("pauseMenuUI").GetComponent<MenuManager>();
        pauseScript = GameObject.FindGameObjectWithTag("PauseScript").GetComponent<PauseMenuScript>();
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
        //set pausereset to false
        pauseReset = false;
        hoverTime = 0;
        hoverBuffer = 0f;
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

        if (pMover._onGround && pMover.inTrick)
        {
            //crash
            //visual animation
            pMover.playerRgbody.AddForce(pMover.board.transform.forward * -20, ForceMode.Impulse);
            pMover.potTrickScore = 0;
            pMover.inTrick = false;
            lastTrick = "";
        }
        else if (pMover._onGround)
        {
            //animator.enabled = false;
            pMover.playerRgbody.AddForce(pMover.board.transform.forward * pMover.potTrickScore / 100, ForceMode.Impulse);
            pMover.actTrickScore += pMover.potTrickScore;
            pMover.potTrickScore = 0;
            pMover.inTrick = false;
            lastTrick = "";
        }
    }

    public void PauseMenuVoice(string Panel)
    {
        if (isGamePaused)
        {
            pMenu.SetCurrentFromVoice(Panel);
        }
        else
        {
            Debug.Log("GAME IS NOT PAUSED!");
        }
    }

    public void MainMenu()
    {
        if (isGamePaused)
        {
            pauseScript.LoadMenu();
        }
    }

    public void Pause()
    {
        Debug.Log("CALLING STOP!");
        if (!isGamePaused)
        {
            isGamePaused = true;
            pauseScript.Pause();
        }
    }
    
    public void Unpause()
    {
        Debug.Log("UNPAUSE!");
        if (isGamePaused)
        {
            pauseScript.Resume();
            isGamePaused = false;
        }
    }

    public void Unstuck()
    {
        Vector3 temp = transform.position;
        temp.y += 10;
        player.transform.position = temp;
    }

    public void Reset()
    {
        if (isGamePaused)
        {
            pauseScript.TpToStartPoint();
            pMover.StatReset();
            isGamePaused = false;
        }
    }

    public void Quit()
    {
        if (isGamePaused)
        {
            UnityEngine.Debug.Log("quitting game");
            Application.Quit();
        }
    }

    public void DoTrickRail(string trick)
    {
        if (!pMover._closeToGround)
        {
            animator.enabled = true;
            if (!pMover.inTrick)
            {
                if (pMover.flag_engaged)
                {
                    //once in a trick, set intrick to true, play the animation
                    pMover.inTrick = true;
                    animator.Play(trick);
                    //animator.SetBool();
                    //animator.SetTrigger();
                    //in this trick matches the last trick, player recieves reduced points
                    if (trick == lastTrick)
                    {
                        pMover.potTrickScore += 500;
                    }
                    else
                    {
                        pMover.potTrickScore += 1000;
                    }
                    //set this trick as the last trick
                    lastTrick = trick;
                }
                else
                {
                    Debug.Log("NONVALID TRICK!");
                }
            }
            else
            {
                Debug.Log("PLAYER ALREADY DOING TRICK!");
            }
            
        }
        else
        {
            Debug.Log("PLAYER TOO CLOSE TO GROUND!");
        }
    }

    public void DoTrick(string trick)
    {
        if (!pMover._closeToGround)
        {
            animator.enabled = true;
            if (!pMover.inTrick)
            {
                if (!pMover.flag_engaged)
                {
                    //once in a trick, set intrick to true, play the animation
                    pMover.inTrick = true;
                    animator.Play(trick);
                    //animator.SetBool();
                    //animator.SetTrigger();
                    //in this trick matches the last trick, player recieves reduced points
                    if (trick == lastTrick)
                    {
                        pMover.potTrickScore += 500;
                    }
                    else
                    {
                        pMover.potTrickScore += 1000;
                    }
                    //set this trick as the last trick
                    lastTrick = trick;
                }
                else
                {
                    Debug.Log("NONVALID TRICK!");
                }
            }
            else
            {
                Debug.Log("PLAYER ALREADY DOING TRICK!");
            }

        }
        else
        {
            Debug.Log("PLAYER TOO CLOSE TO GROUND!");
        }
    }

    public void FindMicrophones()
    {
        //logs all the available microphones
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
    }
}
