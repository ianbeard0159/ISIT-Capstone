using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spline 
{
    public class SplineMoverEntry
    {
        public IMover owner;
        public Transform ownerTR;
        public SplineWaypoint currentWaypoint;
        public SplineWaypoint nextWaypoint;
        public Vector3 velocity;
        public float startTime = -1;
        public float pathPercent;
        public Rigidbody ownerRB;
        public Vector3 entryPosition;
        public bool flag_onPath = false;

        public SplineMoverEntry(IMover in_owner) {
            owner = in_owner;
            ownerTR = in_owner.GetTransform();
            ownerRB = in_owner.GetRigidbody();
        }
        
        public void SetPosition(float in_time) {
            Vector3 targetPos = StaticFunctions.GetSplinePosition(currentWaypoint.centerPoint, nextWaypoint.centerPoint, currentWaypoint.controlPoint, nextWaypoint.controlInverse, pathPercent);
            float moveDistance = Vector3.Distance(ownerTR.position, targetPos);
            velocity = (targetPos - ownerTR.position).normalized * (moveDistance / Time.deltaTime);

            ownerTR.position = targetPos;
            ownerTR.rotation = Quaternion.LookRotation(velocity.normalized);
            ownerRB.velocity = Vector3.zero;
            ownerRB.angularVelocity = Vector3.zero;
        }

        public void SetPosition(float in_time, Vector3 in_startPos, Vector3 in_startControl, Vector3 in_endPos, Vector3 in_endControl) {
            Vector3 targetPos = StaticFunctions.GetSplinePosition(in_startPos, in_endPos, in_startControl, in_endControl, pathPercent);
            float moveDistance = Vector3.Distance(ownerTR.position, targetPos);
            velocity = (targetPos - ownerTR.position).normalized * (moveDistance / Time.deltaTime);

            ownerTR.position = targetPos;
            ownerTR.rotation = Quaternion.LookRotation(velocity.normalized);
            ownerRB.velocity = Vector3.zero;
            ownerRB.angularVelocity = Vector3.zero;
        }
    }
}

