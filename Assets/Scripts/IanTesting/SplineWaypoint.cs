using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineWaypoint : MonoBehaviour
{
    private Transform control;
    public Vector3 controlPoint {get; set;} = Vector3.zero;
    private Vector3 controlInverse {get; set;} = Vector3.zero;
    private bool flag_isInitialized = false;
    // Start is called before the first frame update
    void Init() {
        control = transform.Find("Control").transform;
        flag_isInitialized = true;
    }
    void Awake()
    {
        control = transform.Find("Control").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetControlPositions(transform.position);
    }

    void OnDrawGizmos() {
        SetControlPositions(transform.position);
        Gizmos.DrawSphere(controlInverse, 0.5f);
    }
    void SetControlPositions(Vector3 in_originPoint) {
        if (!flag_isInitialized) Init();
        controlPoint = control.position;
        controlInverse = StaticFunctions.GetReflection(transform.position, controlPoint);
    }
}
