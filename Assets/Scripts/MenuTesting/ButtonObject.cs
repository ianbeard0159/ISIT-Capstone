using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ButtonObject : MonoBehaviour, iGazeReceiver
{
    private bool isGazingUpon;
    float _t;

    private Button button = null;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        _t = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Tricks.hoverBuffer > 0f)
        {
            _t = 0f;
            Tricks.hoverBuffer -= Time.unscaledDeltaTime;
            UnityEngine.Debug.Log("Hover Buffer: " + Tricks.hoverBuffer);
        }
        else if (isGazingUpon)
        {
            //if (_t >= 1f)
            //if (timetest >= 10)
            if(_t >= 2f)
            {
                UnityEngine.Debug.Log("BUTTON BEING INVOKED");
                //click the button
                button.onClick.Invoke();
                
                //_t = 0f;
                //transform.Rotate(0, 3, 0);
            }
            else
            {
                _t += Time.unscaledDeltaTime;
                UnityEngine.Debug.Log("Gazing " + _t);
            }
            
        } else
        {
            //Tricks.hoverTime = 0;
            //UnityEngine.Debug.Log("Somehow called every frame");
            _t = 0f;
            UnityEngine.Debug.Log("SURELY THIS IS BEING CALLED " + _t);
            //UnityEngine.Debug.Log("This should be getting called");
        }
        //UnityEngine.Debug.Log("_t = " + timetest);

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
