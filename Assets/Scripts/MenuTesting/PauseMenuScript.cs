using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject Leaderboard;
    public Transform playerReference;
    public Vector3 offset;
    public Vector3 rotationOffset;

    public AudioSource snowboardSound;
    public GameObject startPoint;
    public PlayerMover pMover;
    public Tricks pTricks;
    private bool isGamePaused = false;
    private bool shouldBePaused = false;

    //stat texts
    public TMP_Text TimeText;
    public TMP_Text TrickText;
    public TMP_Text MarkerText;
    public TMP_Text SpeedText;
    public TMP_Text AirText;
    public TMP_Text HighText;

    public bool initBool = false;

    void init()
    {
        TimeText.text = "";
        TrickText.text = "";
        MarkerText.text = "";
        SpeedText.text = "";
        AirText.text = "";
        HighText.text = "";
        initBool = true;
        pTricks = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Tricks>();
        pMover = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMover>();
    }

    void Start()
    {
        init();
    }

    private void Update()
    {
        if (initBool == false)
        {
            init();
        }
        else
        {
            TimeText.text = pMover.runTimer.Elapsed.TotalSeconds.ToString("F1");
            TrickText.text = pMover.actTrickScore.ToString("F1");
            MarkerText.text = pMover.markerScore.ToString("F1");
            SpeedText.text = pMover.highSpeed.ToString("F1");
            AirText.text = pMover.airTime.ToString("F1");
            HighText.text = pMover.highestAir.ToString("F1");
            //    if (Input.GetKeyUp(KeyCode.Escape))
            //    {
            //        TpToStartPoint();
            //    }
            //    if (Input.GetKeyUp(KeyCode.Space))
            //    {
            //        UnityEngine.Debug.Log("Space");
            //        if (shouldBePaused)
            //            shouldBePaused = false;
            //        else
            //            shouldBePaused = true;
            //    }
            //    if (shouldBePaused && !isGamePaused)
            //    {
            //        Pause();
            //    }
            //    else if (!shouldBePaused && isGamePaused)
            //    {
            //        Resume();
            //    }
        }
    }
    //one bool for game should be paused
    //one bool for game should resume
    //if the game is paused but should be paused
    public void Resume()
    {
        pMover.runTimer.Start();
        //make the time scale change over time
        pauseMenuUI.SetActive(false);
        Leaderboard.SetActive(false);
        Time.timeScale = 1f;
        Tricks.hoverBuffer = 1f;
        isGamePaused = false;
        pTricks.isGamePaused = false;

        //added the below line, allowing pause to be done through the pause menu "Resume" button
        shouldBePaused = false;

        UnityEngine.Debug.Log("Resume");
    }

    public void Pause()
    {
        pMover.runTimer.Stop();
        pauseMenuUI.SetActive(true);
        Leaderboard.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        pTricks.isGamePaused = true;
        SetMenuPosition();

        //show leaderboard on pause for now
        SetLeaderboardPosition();
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
        pMover.StatReset();
        Resume();
        

        //now, toggle pauseReset back to false AFTER DELAYING ONE SECOND
        //this prevents the user from being teleported back on immediately
        Invoke("delayedPauseReset", 1);
    }
    
    void SetMenuPosition()
    {
        pauseMenuUI.transform.position = pMover.transform.position + pMover.transform.forward * 5 + offset;
        pauseMenuUI.transform.rotation = pMover.transform.rotation;
    }

    void SetLeaderboardPosition()
    {
        Leaderboard.transform.position = pMover.transform.position + pMover.transform.forward * 5 + offset + pMover.transform.right * 6;
        Leaderboard.transform.LookAt(2 * Leaderboard.transform.position - pMover.transform.position);
    }

    void delayedPauseReset()
    {
       Tricks.pauseReset = false;
    }

    public void LoadMenu()
    {
        Resume();
        SceneManager.LoadScene("StartingScene");
    }


}
