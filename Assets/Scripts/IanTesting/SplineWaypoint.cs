using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spline 
{
    public class SplineWaypoint : MonoBehaviour
    {
        [SerializeField] public Transform control;
        public Vector3 controlPoint {get; set;}
        public Vector3 controlInverse {get; set;}
        public Vector3 centerPoint {get; set;}
        public float pathTime = 1.0f;
        public float pathDistance {get; set;}
        public bool flag_isInitialized {get; set;} = false;
        // Start is called before the first frame update
        public void Init() {
            flag_isInitialized = true;
        }
        void Awake()
        {
        }
        void Start() {
            SetControlPositions(transform.position, control.position);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            SetControlPositions(transform.position, control.position);
        }

        void OnDrawGizmos() {
            SetControlPositions(transform.position, control.position);
            Gizmos.DrawSphere(controlInverse, 0.5f);
        }
        void SetControlPositions(Vector3 in_originPoint, Vector3 in_controlPoint) {
            if (!flag_isInitialized) Init();
            controlPoint = in_controlPoint;
            controlInverse = StaticFunctions.GetReflection(in_originPoint, controlPoint);
            centerPoint = in_originPoint;
        }
    }
}
