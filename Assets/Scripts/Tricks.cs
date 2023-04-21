using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;

public class Tricks : MonoBehaviour
{
    private Animator animator;
    private PlayerMover player;
    AnimatorStateInfo animStateInfo;
    public float NTime;

    bool animationFinished;

    public string[] keywords = new string[] { "-180", "down", "left", "right" };
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;

    protected PhraseRecognizer recognizer;
    protected string word = "right";

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMover>();
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

        animStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //Create keywords for keyword recognizer
        keywords.Add("activate", () =>
        {
            // action to be performed when this keyword is spoken
        });

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

    // Update is called once per frame
    void Update()
    {
        NTime = animStateInfo.normalizedTime;
        if (!player._onGround)
        {
            if (!player.inTrick)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    animator.Play("-180", -1, 0);
                    player.potScore += 1000;
                    player.inTrick = true;
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    animator.Play("180", -1, 0);
                    player.potScore += 1000;
                    player.inTrick = true;
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    animator.Play("Backflip", -1, 0);
                    player.potScore += 1000;
                    player.inTrick = true;
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    animator.Play("Backslide", -1, 0);
                    player.potScore += 1000;
                    player.inTrick = true;
                }
                if (Input.GetKeyDown(KeyCode.T))
                {
                    animator.Play("Frontflip", -1, 0);
                    player.potScore += 1000;
                    player.inTrick = true;
                }
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    animator.Play("Frontslip", -1, 0);
                    player.potScore += 1000;
                    player.inTrick = true;
                }
                if (Input.GetKeyDown(KeyCode.U))
                {
                    animator.Play("720", -1, 0);
                    player.potScore += 1000;
                    player.inTrick = true;
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    animator.Play("360", -1, 0);
                    player.potScore += 1000;
                    player.inTrick = true;
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
            if (NTime > 1.0f)
            {
                animationFinished = true;
                player.inTrick = false;
            }
        }
    }
}
