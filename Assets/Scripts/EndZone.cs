using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    private GameObject startPoint;
    private PlayerMover pMover;
    private Rigidbody pRB;
    public PauseMenuScript pauseMenu;
    private Collider in_collider;
    private bool activate = false;
    public Tricks pTricks;
    public AndroidVoice pAndroidVoice;

    float timerOne;
    float timerTwo;
    float timerThree;
    Animator animator;

    [SerializeField] float slowdownPercent = 0.25f;
    [SerializeField] float slowdownStep = 0.01f;
    float startTime = 1;

    private enum SlowdownState
    {
        normalTime,
        slowing,
        slowTime,
        normalizing
    }
    private SlowdownState currentState = SlowdownState.normalTime;

    // Start is called before the first frame update
    void Start()
    {
        animator = GameObject.FindGameObjectWithTag("Blindfold").GetComponent<Animator>();
        startPoint = GameObject.FindGameObjectWithTag("StartPoint");
        currentState = SlowdownState.normalTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (activate)
        {
            pMover = in_collider.gameObject.GetComponent<PlayerMover>();
            pMover.runTimer.Stop();
            pMover.runTime = pMover.runTimer.Elapsed.TotalSeconds;
            pRB = in_collider.gameObject.GetComponent<Rigidbody>();
            pMover._inGame = false;
            Debug.Log("in endzone slowdown");
            if (currentState == SlowdownState.normalTime) currentState = SlowdownState.slowing;

            if (currentState == SlowdownState.slowing)
            {
                Debug.Log("slowing down");
                if (Time.timeScale > slowdownPercent)
                {
                    if (Time.timeScale - slowdownStep < slowdownPercent) Time.timeScale = slowdownPercent;
                    else Time.timeScale -= slowdownStep;
                }
                else
                {
                    startTime = Time.time;
                    currentState = SlowdownState.slowTime;
                }
            }
            if (Time.timeScale <= slowdownPercent)
            {
                Time.timeScale = 1;
                pMover._inGame = true;
                //pMover._onGround = true;
                activate = false;
                in_collider.transform.position = startPoint.transform.position;
                pRB.velocity = Vector3.zero;
                pMover.runTimer.Restart();
                ////Corutine?
                //Debug.Log("Done slowing down");
                //if (timerOne > 4)
                //{
                //    animator.SetBool("isActive", true);
                //    if (timerTwo > 2)
                //    {
                //        in_collider.transform.position = startPoint.transform.position;
                //        animator.SetBool("isActive", false);
                //        if (timerThree > 2)
                //        {
                //            Time.timeScale = 1;
                //            pMover._inGame = true;
                //            pMover._onGround = true;
                //            pMover._closeToGround = true;
                //            activate = false;
                //            pRB.velocity = Vector3.zero;
                //            pMover.runTimer.Restart();                            
                //        }
                //    }
                //}

            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))// && !activate)
        {
            pTricks.isGamePaused = true;
            pAndroidVoice.isGamePaused = true;
            pauseMenu.Pause();
            //in_collider = other;
            //activate = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        activate = false;
    }
}
