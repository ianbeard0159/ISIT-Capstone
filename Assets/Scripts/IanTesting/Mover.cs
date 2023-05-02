using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spline 
{
    public class Mover : MonoBehaviour, IMover
    {
        public bool flag_engaged {get; set;} = false;
        private Vector3 startPosition;
        private float startTime;
        private Rigidbody rigidBody;
        [SerializeField] private float resetTime = 10;

        public Transform GetTransform() {
            return this.transform;
        }
        public Rigidbody GetRigidbody() {
            return this.rigidBody;
        }

        void Awake() {
            startPosition = transform.position;
            startTime = Time.time;
            rigidBody = transform.GetComponent<Rigidbody>();
        }

        void Update() {
            if (Time.time > startTime + resetTime && !flag_engaged) {
                transform.position = startPosition;
                startTime = Time.time;
                flag_engaged = false;
                rigidBody.velocity = Vector3.zero;
            }
        }
    }
}

