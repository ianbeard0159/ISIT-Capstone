using System.Diagnostics;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Transform playerReference;
    public Vector3 offset;
    public GameObject startPoint;
    public PlayerMover pMover;
    private bool isGamePaused = false;
    private bool shouldBePaused = false;
    //private float gameSpeed = 1.0f;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            TpToStartPoint();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            UnityEngine.Debug.Log("Space");
            if (shouldBePaused)
                shouldBePaused = false;
            else
                shouldBePaused = true;
        }
        if (shouldBePaused && !isGamePaused)
        {
            Pause();                 
        }
        else if (!shouldBePaused && isGamePaused)
        {
            Resume();
        }
    }
    //one bool for game should be paused
    //one bool for game should resume
    //if the game is paused but should be paused
    public void Resume()
    {
        //make the time scale change over time
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;

        //added the below line, allowing pause to be done through the pause menu "Resume" button
        shouldBePaused = false;

        UnityEngine.Debug.Log("Resume");
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        SetMenuPosition();
        UnityEngine.Debug.Log("Pause");
    }

    //teleports player to start point
    //currently does not function when on a spline
    public void TpToStartPoint()
    {
        //need to toss user out of a spline
        //set global pauseReset to true to trigger the spline tossout
        Tricks.pauseReset = true;
        //UnityEngine.Debug.Log("pauseReset is: " + Tricks.pauseReset);

        //actually move the player
        pMover.transform.position = startPoint.transform.position;
        pMover.playerRgbody.velocity = Vector3.zero;
        //resume the game
        Resume();

        //now, toggle pauseReset back to false AFTER DELAYING ONE SECOND
        //this prevents the user from being teleported back on immediately
        Invoke("delayedPauseReset", 1);
    }

    //possible timescale adjustment method?
    //public void TimescaleAdjust(float x)
    //{
    //    Time.timeScale = x;
    //}
    
    void SetMenuPosition()
    {
        pauseMenuUI.transform.position = pMover.transform.position + pMover.transform.forward * 5 + offset;
        pauseMenuUI.transform.rotation = pMover.transform.rotation;
    }

    void delayedPauseReset()
    {
        Tricks.pauseReset = false;
    }
}
