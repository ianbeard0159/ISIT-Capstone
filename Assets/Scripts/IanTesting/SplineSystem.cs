using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spline 
{
    public class SplineSystem : MonoBehaviour
    {
        private enum SplineStates {
            Entering,
            Engaged,
            Exiting
        }
        private List<SplineMoverEntry> engagedMovers;
        private List<SplineWaypoint> waypoints;
        private SplineEntryZone entryZone;
        private SplineStartPoint startPoint;
        private bool flag_isInitialized = false;
        private Transform startPositionBox;
        private Transform entryControl;

        void Init() {
            engagedMovers = new List<SplineMoverEntry>();
            waypoints = new List<SplineWaypoint>();
            foreach (Transform obj in transform) {
                if (obj.name == "Starting Point") {
                    startPositionBox = obj.transform;
                }
                if (obj.GetComponent<SplineWaypoint>() != null) {
                    waypoints.Add(obj.GetComponent<SplineWaypoint>());
                }
            }
            flag_isInitialized = true;

        }
        void Awake()
        {
            if (!flag_isInitialized) Init();
            entryZone = transform.Find("Entry Zone").GetComponent<SplineEntryZone>();
            startPoint = transform.Find("Starting Point").GetComponent<SplineStartPoint>();

            entryZone.on_enterTrigger += Event_EnterSystem;
        }
        void Start() {
            entryControl = entryZone.transform.Find("Control");
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            foreach (SplineMoverEntry entry in engagedMovers) {
                // Go from entry zone to start point
                if (entry.nextWaypoint == null) {
                    if (entry.startTime == -1) {
                        entry.startTime = Time.time;
                        entry.entryPosition = entry.owner.transform.position;
                    }
                    entry.pathPercent = (Time.time - entry.startTime) / startPoint.pathTime;
                    if (entry.pathPercent >= 1.0f) {
                        entry.startTime = Time.time;
                        entry.nextWaypoint = waypoints[0];
                    }
                    else {
                        Vector3 targetPos = StaticFunctions.GetSplinePosition(entry.entryPosition, startPoint.transform.position, entryControl.position, entryControl.position, entry.pathPercent);
                        
                        float moveDistance = Vector3.Distance(entry.owner.transform.position, targetPos);
                        Vector3 velocity = (targetPos - entry.owner.transform.position).normalized * (moveDistance / Time.deltaTime);
                        entry.owner.transform.position = targetPos;
                        entry.owner.transform.rotation = Quaternion.LookRotation(velocity.normalized);
                        entry.ownerRB.velocity = Vector3.zero;
                        entry.ownerRB.angularVelocity = Vector3.zero;
                    }
                    continue;
                }

                // Go from start point to first waypoint
                if (entry.nextWaypoint == waypoints[0]) {
                    entry.pathPercent = (Time.time - entry.startTime) / entry.nextWaypoint.pathTime;
                    if (entry.pathPercent >= 1.0f) {
                        entry.startTime = Time.time;
                        entry.currentWaypoint = waypoints[0];
                        entry.nextWaypoint = waypoints[1];
                    }
                    else {
                        Vector3 targetPos =  StaticFunctions.GetSplinePosition(startPoint.transform.position, entry.nextWaypoint.centerPoint, entry.nextWaypoint.controlInverse, entry.nextWaypoint.controlInverse, entry.pathPercent);
                        
                        float moveDistance = Vector3.Distance(entry.owner.transform.position, targetPos);
                        Vector3 velocity = (targetPos - entry.owner.transform.position).normalized * (moveDistance / Time.deltaTime);
                        entry.owner.transform.position = targetPos;
                        entry.owner.transform.rotation = Quaternion.LookRotation(velocity.normalized);
                        entry.ownerRB.velocity = Vector3.zero;
                        entry.ownerRB.angularVelocity = Vector3.zero;
                    }
                    continue;
                }

                // Check if mover is at the end of it's current path
                entry.pathPercent = (Time.time - entry.startTime) / entry.nextWaypoint.pathTime;
                if (entry.pathPercent >= 1.0f) {
                    int waypointIndex = waypoints.IndexOf(entry.nextWaypoint);
                    // Check if mover has finished following all paths on the spline curve
                    if (waypointIndex + 1 >= waypoints.Count) {
                        entry.owner.flag_engaged = false;
                        entry.ownerRB.velocity = entry.velocity;
                        entry.flag_onPath = false;
                        continue;
                    }
                    else {
                        entry.currentWaypoint = waypoints[waypointIndex];
                        entry.nextWaypoint = waypoints[waypointIndex + 1];
                        entry.startTime = Time.time;
                        entry.pathPercent = 0;
                        continue;
                    }

                }
                entry.SetPosition(Time.time);
            }

            // Remove any movers that are no longer on the path
            for (int i = engagedMovers.Count -1; i >= 0; i--) {
                if (!engagedMovers[i].flag_onPath) {
                    engagedMovers.Remove(engagedMovers[i]);
                }
            }
            
        }
        void OnDestroy() {
            entryZone.on_enterTrigger -= Event_EnterSystem;
        }

        void OnDrawGizmos() {
            if (!flag_isInitialized) Init();

            Gizmos.color = Color.green;
            float numberOfPoints = 10;

            // Draw path from starting point to first waypoint
            for (int i = 0; i < numberOfPoints; i++) {
                    float precent = i / numberOfPoints;
                    Vector3 drawPoint = StaticFunctions.GetSplinePosition(startPositionBox.localPosition, waypoints[0].centerPoint, waypoints[0].controlInverse, waypoints[0].controlInverse, precent);
                    Gizmos.DrawSphere(drawPoint, 0.1f);
            }

            // Draw path between each waypoint in the system
            for (int i = 0; i < waypoints.Count - 1; i++) {
                for (int j = 0; j < numberOfPoints; j++) {
                    float precent = j / numberOfPoints;
                    Vector3 drawPoint = StaticFunctions.GetSplinePosition(waypoints[i].centerPoint, waypoints[i+1].centerPoint, waypoints[i].controlPoint, waypoints[i+1].controlInverse, precent);
                    Gizmos.DrawSphere(drawPoint, 0.1f);
                }
            }
        }

        private void Event_EnterSystem(Mover in_mover) {
            in_mover.flag_engaged = true;
            SplineMoverEntry entry = new SplineMoverEntry(in_mover);
            entry.flag_onPath = true;
            engagedMovers.Add(entry);
        }
    }   
}