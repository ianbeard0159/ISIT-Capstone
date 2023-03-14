using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineTest : MonoBehaviour
{
    [SerializeField] private Transform point_start;
    [SerializeField] private Transform point_end;
    [SerializeField] private Transform control_A;
    [SerializeField] private Transform control_B;
    private Transform mover;
    [SerializeField] private float finishTime;
    private float interpolateAmount;
    private float startTime;
    private bool flag_pathStarted = false;
    private Rigidbody moverBody;
    private Vector3 lastVelocity;
    private StartPoint startPointInstance;
    private EndPoint endPointInstance;
    private bool init = false;
    
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
    void Update()
    {
        if (!init) Init();

        if (flag_pathStarted) interpolateAmount = (Time.time - startTime) / finishTime;
        if (flag_pathStarted && interpolateAmount < 1) {

            Vector3 targetPos = StaticFunctions.GetSplinePosition(point_start.position, point_end.position, control_A.position, control_B.position, interpolateAmount);
            Vector3 currentPos = mover.position;
            float moveDistance = Vector3.Distance(currentPos, targetPos);
            lastVelocity = (targetPos - currentPos).normalized * (moveDistance / Time.deltaTime);
            
            mover.position = Vector3.MoveTowards(currentPos, targetPos, moveDistance);
        }
    }
    void OnDestroy() {
        startPointInstance.on_enterStartPoint -= StartPath;
        endPointInstance.on_enterEndPoint -= EndPath;
    }
    public void StartPath(Transform in_mover) {
        Debug.Log("Start Path");
        mover = in_mover;
        moverBody = mover.GetComponent<Rigidbody>();
        moverBody.velocity = Vector3.zero;
        flag_pathStarted = true;
        startTime = Time.time;
    }
    public void EndPath(Transform in_mover, bool in_lastEndPoint) {
        if (in_mover == mover) {
            if (in_lastEndPoint) {
                Debug.Log(lastVelocity);
                moverBody.velocity = (lastVelocity);
            }
            flag_pathStarted = false;
            mover = null;
            moverBody = null;
        }
    }

}
