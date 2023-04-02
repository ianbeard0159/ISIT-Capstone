using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineMoverEntry
{
    public Mover owner;
    public SplineWaypoint currentWaypoint;
    public SplineWaypoint nextWaypoint;
    public Vector3 velocity;
    public float startTime = -1;
    public float pathPercent;
    public Rigidbody ownerRB;
    public Vector3 entryPosition;

    public SplineMoverEntry(Mover in_owner) {
        owner = in_owner;
        ownerRB = owner.transform.GetComponent<Rigidbody>();
    }
    
    public void SetPosition(float in_time) {
        Vector3 targetPos = StaticFunctions.GetSplinePosition(currentWaypoint.centerPoint, nextWaypoint.centerPoint, currentWaypoint.controlPoint, nextWaypoint.controlInverse, pathPercent);
        float moveDistance = Vector3.Distance(owner.transform.position, targetPos);
        velocity = (targetPos - owner.transform.position).normalized * (moveDistance / Time.deltaTime);

        owner.transform.position = targetPos;
        ownerRB.velocity = Vector3.zero;
    }
}
