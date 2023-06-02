using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeManager : MonoBehaviour
{

    public Camera viewCamera;
    private GameObject lastGazedUpon;

    // Update is called once per frame
    void Update()
    {
        CheckGaze();
    }

    private void CheckGaze()
    {
        if (lastGazedUpon)
        {
            lastGazedUpon.SendMessage("NotGazingUpon", SendMessageOptions.DontRequireReceiver);
        }

        Ray gazeRay = new Ray(viewCamera.transform.position, viewCamera.transform.rotation * Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(gazeRay, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.tag == "PauseMenu")
            {
                hit.transform.SendMessage("GazingUpon", SendMessageOptions.DontRequireReceiver);
                lastGazedUpon = hit.transform.gameObject;
            }
        }
    }
}
