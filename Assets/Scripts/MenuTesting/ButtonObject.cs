using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ButtonObject : MonoBehaviour, iGazeReceiver
{
    private bool isGazingUpon;
    float _t = 0f;

    //test w/o timescale
    int timetest;

    private Button button = null;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        timetest = 0;
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * 
         * Gaze functionality too broken with pause at the moment. fix later
         * 
        if (isGazingUpon)
        {
            //if (_t >= 1f)
            //if (timetest >= 10)
            if(Tricks.hoverTime >= 5000)
            {
                //click the button
                Tricks.hoverTime = 0;
                button.onClick.Invoke();
                
                //_t = 0f;
                //transform.Rotate(0, 3, 0);
            }
            else
            {
                //_t += Time.unscaledDeltaTime;
                Tricks.hoverTime += 1;
                UnityEngine.Debug.Log("Gazing " + Tricks.hoverTime);
            }
            
        } else
        {
            //Tricks.hoverTime = 0;
            //UnityEngine.Debug.Log("Somehow called every frame");
            //_t = 0f;
            //UnityEngine.Debug.Log("This should be getting called");
        }
        //UnityEngine.Debug.Log("_t = " + timetest);


        */
    }

    public void GazingUpon()
    {
        isGazingUpon = true;
    }

    public void NotGazingUpon()
    {
        isGazingUpon = false;
    }

}
