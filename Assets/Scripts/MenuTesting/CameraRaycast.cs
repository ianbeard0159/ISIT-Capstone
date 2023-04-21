using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;

    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out hit, 100000))
        {
            print(hit.transform.name);
        }
    }
}
