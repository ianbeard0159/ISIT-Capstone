using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ButtonObject : MonoBehaviour, iGazeReceiver
{
    private bool isGazingUpon;
    float _t = 0f;

    private Button button = null;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isGazingUpon)
        {
            _t += Time.deltaTime;

            if (_t >= 2f)
            {
                //click the button
                button.onClick.Invoke();
                //transform.Rotate(0, 3, 0);
            }
            
        } else
        {
            _t = 0f;
        }
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
