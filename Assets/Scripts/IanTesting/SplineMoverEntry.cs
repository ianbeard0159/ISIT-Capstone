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

        public Quaternion startingRotation;
        public bool flag_onPath = false;
        public float initialVelocity;

        public SplineMoverEntry(IMover in_owner) {
            owner = in_owner;
            ownerTR = in_owner.GetTransform();
            ownerRB = in_owner.GetRigidbody();
            startingRotation = ownerRB.rotation;
        }
        
        public void SetPosition(float in_time) {
            Vector3 targetPos = StaticFunctions.GetSplinePosition(currentWaypoint.transform.position, nextWaypoint.transform.position, currentWaypoint.control.position, StaticFunctions.GetReflection(nextWaypoint.transform.position, nextWaypoint.control.position), pathPercent);
            float moveDistance = Vector3.Distance(ownerTR.position, targetPos);
            velocity = (targetPos - ownerTR.position).normalized * (moveDistance / Time.deltaTime);

            ownerTR.position = targetPos;
            ownerTR.rotation = Quaternion.LookRotation(velocity.normalized, Vector3.up);
            ownerRB.velocity = Vector3.zero;
            ownerRB.angularVelocity = Vector3.zero;
        }

        public void SetPosition(float in_time, Vector3 in_startPos, Vector3 in_startControl, Vector3 in_endPos, Vector3 in_endControl, bool in_lerpRotation = false) {
            Vector3 targetPos = StaticFunctions.GetSplinePosition(in_startPos, in_endPos, in_startControl, in_endControl, pathPercent);
            float moveDistance = Vector3.Distance(ownerTR.position, targetPos);
            velocity = (targetPos - ownerTR.position).normalized * (moveDistance / Time.deltaTime);

            ownerTR.position = targetPos;
            if (in_lerpRotation) ownerTR.rotation = Quaternion.Lerp(startingRotation, Quaternion.LookRotation(velocity.normalized, Vector3.up), pathPercent);
            else ownerTR.rotation = Quaternion.LookRotation(velocity.normalized, Vector3.up);
            ownerRB.velocity = Vector3.zero;
            ownerRB.angularVelocity = Vector3.zero;
        }
    }
}

