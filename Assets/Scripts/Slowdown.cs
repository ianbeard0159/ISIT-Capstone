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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activate)
        {
            if (inCollider.CompareTag("Player"))
            {
                if (Time.timeScale != 0.1f)
                {
                    Time.timeScale -= 0.1f;
                }
                else
                {
                    timer += Time.deltaTime;
                    if (timer > delay)
                    {
                        if (Time.timeScale != 1)
                        {
                            Time.timeScale += 0.1f;
                        }
                        else
                        {
                            activate = false;
                        }
                    }   
                }
            }
        }
    }

    void OnTriggerEnter(Collider in_collider)
    {
        inCollider = in_collider;
        activate = true;
    }
}


