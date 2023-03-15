using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineTest : MonoBehaviour
{
    [SerializeField] private Transform point_start;
    [SerializeField] private Transform point_end;
    [SerializeField] private Transform control_A;
    [SerializeField] private bool control_A_inverse;
    [SerializeField] private Transform control_B;
    private Transform mover;
    [SerializeField] private float finishTime;
    private float interpolateAmount;
    private float startTime;
    private bool flag_pathStarted = false;
    private Rigidbody moverBody;
    private Vector3 lastVelocity = Vector3.zero;
    private Vector3 secondLastVelocity = Vector3.zero;
    private Vector3 thirdLast = Vector3.zero;
    private StartPoint startPointInstance;
    private EndPoint endPointInstance;
    private bool init = false;
    private Vector3 control_A_Position;
    private Vector3 startPoint;
    private Vector3 endPoint;
    
    void Init() {
        startPointInstance = point_start.gameObject.GetComponent<StartPoint>();
        startPointInstance.on_enterStartPoint += StartPath;
        
        endPointInstance = point_end.gameObject.GetComponent<EndPoint>();
        endPointInstance.on_enterEndPoint += EndPath;

        init = true;
    }
    void Awake() {
        Init();
    }
    void FixedUpdate()
    {
        if (!init) Init();

        if (flag_pathStarted) interpolateAmount = (Time.time - startTime) / finishTime;
        if (flag_pathStarted && interpolateAmount < 1 && interpolateAmount > 0) {

            control_A_Position = (control_A_inverse) ? StaticFunctions.GetReflection(startPoint, control_A.position): control_A.position;
            Vector3 targetPos = StaticFunctions.GetSplinePosition(startPoint, point_end.position, control_A_Position, control_B.position, interpolateAmount);
            
            Vector3 currentPos = mover.position;
            float moveDistance = Vector3.Distance(currentPos, targetPos);
            thirdLast = secondLastVelocity;
            secondLastVelocity = lastVelocity;
            lastVelocity = (targetPos - currentPos).normalized * (moveDistance / Time.deltaTime);
            
            mover.position = targetPos;
            moverBody.velocity = Vector3.zero;
        }
    }
    void OnDestroy() {
        startPointInstance.on_enterStartPoint -= StartPath;
        endPointInstance.on_enterEndPoint -= EndPath;
    }
    public void StartPath(Transform in_mover) {
        if (flag_pathStarted) return;

        mover = in_mover;
        moverBody = mover.GetComponent<Rigidbody>();
        moverBody.velocity = Vector3.zero;
        startPoint = mover.position;
        flag_pathStarted = true;
        startTime = Time.time;
    }
    public void EndPath(Transform in_mover, bool in_lastEndPoint) {
        if (in_mover == mover) {
            flag_pathStarted = false;
            mover.GetComponent<Mover>().flag_engaged = false;
            if (in_lastEndPoint) moverBody.velocity = lastVelocity;
            mover = null;
            moverBody = null;
            thirdLast = Vector3.zero;
            secondLastVelocity = Vector3.zero;
            lastVelocity = Vector3.zero;
        }
    }

}
