using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public bool flag_engaged = false;
    private Vector3 startPosition;
    private float startTime;
    private Rigidbody rigidBody;
    [SerializeField] private float resetTime = 10;

    void Awake() {
        startPosition = transform.position;
        startTime = Time.time;
        rigidBody = transform.GetComponent<Rigidbody>();
    }

    void Update() {
        if (Time.time > startTime + resetTime) {
            transform.position = startPosition;
            startTime = Time.time;
            flag_engaged = false;
            rigidBody.velocity = Vector3.zero;
        }
    }
}
