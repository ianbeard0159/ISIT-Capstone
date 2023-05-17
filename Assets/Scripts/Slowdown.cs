using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowdown : MonoBehaviour
{
    private Collider inCollider;
    private bool activate = false;
    float timer;

    [SerializeField]
    float delay = 1;
    [SerializeField] float slowdownPercent = 0.25f;
    [SerializeField] float slowdownStep = 0.01f;
    float startTime = 1;

    private enum SlowdownState {
        normalTime,
        slowing,
        slowTime,
        normalizing
    }
    private SlowdownState currentState = SlowdownState.normalTime;

    // Start is called before the first frame update
    void Start()
    {
        currentState = SlowdownState.normalTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (activate)
        {
            if (currentState == SlowdownState.normalTime) currentState = SlowdownState.slowing;

            if (currentState == SlowdownState.slowing) {
                if (Time.timeScale > slowdownPercent)
                {
                    if (Time.timeScale - slowdownStep < slowdownPercent) Time.timeScale = slowdownPercent;
                    else Time.timeScale -= slowdownStep;
                }
                else {
                    startTime = Time.time;
                    currentState = SlowdownState.slowTime;
                }
            }

            else if (currentState == SlowdownState.slowTime)
            {
                timer = Time.time;
                if (timer > delay + startTime)
                {
                    currentState = SlowdownState.normalizing;
                }   
            }

            else if (currentState ==  SlowdownState.normalizing) 
                {
                    if (Time.timeScale < 1)
                    {
                        if (Time.timeScale + slowdownStep > 1) Time.timeScale = 1;
                        else Time.timeScale += slowdownStep;
                    }
                    else
                    {
                        activate = false;
                        currentState = SlowdownState.normalTime;
                    }
                }
            
        }
    }

    void OnTriggerEnter(Collider in_collider)
    {
        
        if (in_collider.CompareTag("Player") && !activate) {
            inCollider = in_collider;
            activate = true;
        }
    }
}


