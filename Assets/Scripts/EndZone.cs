using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    private GameObject startPoint;
    private PlayerMover pMover;
    private Rigidbody pRB;
    private Collider inCollider;
    private bool activate = false;

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
        startPoint = GameObject.FindGameObjectWithTag("StartPoint");
        currentState = SlowdownState.normalTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (activate)
        {
            pMover.runTimer.Stop();
            pMover.runTime = pMover.runTimer.Elapsed.TotalSeconds;
            pRB = inCollider.gameObject.GetComponent<Rigidbody>();
            pMover = inCollider.gameObject.GetComponent<PlayerMover>();
            pMover._inGame = false;
            if (currentState == SlowdownState.normalTime) currentState = SlowdownState.slowing;

            if (currentState == SlowdownState.slowing)
            {
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
            if (Time.timeScale == 0)
            {
                inCollider.transform.position = startPoint.transform.position;
                pMover._inGame = true;
                activate = false;
                pMover.runTimer.Restart();
            }
        }
    }

    void OnTriggerEnter(Collider in_collider)
    {
        if (in_collider.CompareTag("Player") && !activate)
        {
            inCollider = in_collider;
            activate = true;
        }
    }
}
