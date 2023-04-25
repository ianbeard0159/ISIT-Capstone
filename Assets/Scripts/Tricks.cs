using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using UnityEngine.InputSystem;

public class Tricks : MonoBehaviour
{
    private Animator animator;
    private PlayerMover player;
    AnimatorStateInfo animStateInfo;
    public float NTime;

    bool animationFinished;

    public string[] keywords = new string[] { "-180", "180", "Backflip", "Backslide","Frontflip", "Frontslide", "720", "360"};
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;

    protected PhraseRecognizer recognizer;
    public string results;
    protected string word = "";

    public InputAction backflipAction;
    public InputAction frontflipAction;

    private void OnEnable()
    {
        backflipAction.Enable();
    }

    private void OnDisable()
    {
        backflipAction.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMover>();
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

        //checks animation states, used for seeing if animation is finish
        animStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
            Debug.Log(recognizer.IsRunning);
        }

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
        var lastTrick = "";
        NTime = animStateInfo.normalizedTime;
        if (!player._onGround)
        {
            if (!player.inTrick)
            { 
                if (Input.GetKeyDown(KeyCode.Q) || word == "-180")
                {
                    player.inTrick = true;
                    animator.Play("-180", -1, 0);
                    if (word == lastTrick)
                    {
                        player.potScore += 1000;
                    }
                    else
                    {
                        player.potScore += 500;
                    }
                    lastTrick = word;
                }
                if (Input.GetKeyDown(KeyCode.W) || word == "180")
                {
                    player.inTrick = true;
                    animator.Play("180", -1, 0);
                    if (word == lastTrick)
                    {
                        player.potScore += 1000;
                    }
                    else
                    {
                        player.potScore += 500;
                    }
                    lastTrick = word;
                }
                if (backflipAction.triggered || word == "Backflip")
                {
                    player.inTrick = true;
                    animator.Play("Backflip", -1, 0);
                    if (word == lastTrick)
                    {
                        player.potScore += 1000;
                    }
                    else
                    {
                        player.potScore += 500;
                    }
                    lastTrick = word;
                }
                if (Input.GetKeyDown(KeyCode.R) || word == "Backslide") 
                {
                    player.inTrick = true;
                    animator.Play("Backslide", -1, 0);
                    if (word == lastTrick)
                    {
                        player.potScore += 1000;
                    }
                    else
                    {
                        player.potScore += 500;
                    }
                    lastTrick = word;
                }
                if (Input.GetKeyDown(KeyCode.T) || word == "Frontflip")
                {
                    player.inTrick = true;
                    animator.Play("Frontflip", -1, 0);
                    if (word == lastTrick)
                    {
                        player.potScore += 1000;
                    }
                    else
                    {
                        player.potScore += 500;
                    }
                    lastTrick = word;
                }
                if (Input.GetKeyDown(KeyCode.Y) || word == "Frontslip")
                {
                    player.inTrick = true;
                    animator.Play("Frontslip", -1, 0);
                    if (word == lastTrick)
                    {
                        player.potScore += 1000;
                    }
                    else
                    {
                        player.potScore += 500;
                    }
                    lastTrick = word;
                }
                if (Input.GetKeyDown(KeyCode.U) || word == "720")
                {
                    player.inTrick = true;
                    animator.Play("720", -1, 0);
                    if (word == lastTrick)
                    {
                        player.potScore += 1000;
                    }
                    else
                    {
                        player.potScore += 500;
                    }
                    lastTrick = word;
                }
                if (Input.GetKeyDown(KeyCode.I) || word == "360")
                {
                    player.inTrick = true;
                    animator.Play("360", -1, 0);
                    if (word == lastTrick)
                    {
                        player.potScore += 1000;
                    }
                    else
                    {
                        player.potScore += 500;
                    }
                    lastTrick = word;
                }
            }
            else
            {
                if (player._onGround)
                {
                    //crash
                    //visual animation
                    player.playerRgbody.AddForce(player.board.transform.forward * -20, ForceMode.Impulse);
                    player.potScore = 0;
                }
            }

            if (player._onGround)
            {
                player.playerRgbody.AddForce(player.board.transform.forward * player.potScore / 100, ForceMode.Impulse);
                player.actScore += player.potScore;
                player.potScore = 0;
                player.inTrick = false;
            }
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            {
                animationFinished = true;
                player.inTrick = false;
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }
}
