using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Transform playerReference;
    public Vector3 offset;
    private bool isGamePaused = false;
    private bool shouldBePaused = false;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Space");
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
}
