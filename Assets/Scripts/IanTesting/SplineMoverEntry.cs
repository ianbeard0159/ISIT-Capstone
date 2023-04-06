using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spline 
{
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
        public bool flag_onPath = false;

        public SplineMoverEntry(Mover in_owner) {
            owner = in_owner;
            ownerRB = owner.transform.GetComponent<Rigidbody>();
        }
        
        public void SetPosition(float in_time) {
            Vector3 targetPos = StaticFunctions.GetSplinePosition(currentWaypoint.centerPoint, nextWaypoint.centerPoint, currentWaypoint.controlPoint, nextWaypoint.controlInverse, pathPercent);
            float moveDistance = Vector3.Distance(owner.transform.position, targetPos);
            velocity = (targetPos - owner.transform.position).normalized * (moveDistance / Time.deltaTime);

            owner.transform.position = targetPos;
            owner.transform.rotation = Quaternion.LookRotation(velocity.normalized);
            ownerRB.velocity = Vector3.zero;
            ownerRB.angularVelocity = Vector3.zero;
        }
    }
}

