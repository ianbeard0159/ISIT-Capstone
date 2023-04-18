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
        private Vector3 entryInverse;

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
            entryZone = transform.Find("Entry Zone").GetComponent<SplineEntryZone>();
            startPoint = transform.Find("Starting Point").GetComponent<SplineStartPoint>();
            entryControl = entryZone.transform.Find("Control");
            entryInverse = StaticFunctions.GetReflection(startPoint.transform.position, entryControl.transform.position);

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
            CalculatePathDistance();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            foreach (SplineMoverEntry entry in engagedMovers) {
                // Go from entry zone to start point
                if (entry.nextWaypoint == null) {
                    if (entry.startTime == -1) {
                        entry.startTime = Time.time;
                        entry.entryPosition = entry.ownerTR.position;
                    }
                    // Velocity = distance/time Velocity * time = d
                    // time = distance / velocity
                    entry.pathPercent = (Time.time - entry.startTime) / ((Vector3.Distance(entry.entryPosition, startPoint.transform.position) * 1.5f) / entry.initialVelocity);
                    Debug.Log($"{entry.pathPercent} = ({Time.time} - {entry.startTime}) / ({Vector3.Distance(entry.entryPosition, startPoint.transform.position)} / {entry.initialVelocity})");
                    if (entry.pathPercent >= 1.0f) {
                        entry.startTime = Time.time;
                        entry.nextWaypoint = waypoints[0];
                    }
                    else {
                        // Vector3 targetPos = StaticFunctions.GetSplinePosition(entry.entryPosition, startPoint.transform.position, entryControl.position, entryControl.position, entry.pathPercent);
                        entry.SetPosition(Time.time, entry.entryPosition, entryControl.position, startPoint.transform.position, entryControl.position);
                    }
                    continue;
                }

                // Go from start point to first waypoint
                if (entry.nextWaypoint == waypoints[0]) {
                    entry.pathPercent = (Time.time - entry.startTime) / (entry.nextWaypoint.pathDistance / entry.initialVelocity);
                    Debug.Log($"{entry.pathPercent} = ({Time.time} - {entry.startTime}) / ({entry.nextWaypoint.pathDistance} / {entry.initialVelocity})");
                    if (entry.pathPercent >= 1.0f) {
                        entry.startTime = Time.time;
                        entry.currentWaypoint = waypoints[0];
                        entry.nextWaypoint = waypoints[1];
                    }
                    else {
                        //Vector3 targetPos =  StaticFunctions.GetSplinePosition(startPoint.transform.position, entry.nextWaypoint.centerPoint, entry.nextWaypoint.controlInverse, entry.nextWaypoint.controlInverse, entry.pathPercent);
                        entry.SetPosition(Time.time, startPoint.transform.position, entryInverse, entry.nextWaypoint.centerPoint, entry.nextWaypoint.controlInverse);
                    }
                    continue;
                }

                // Check if mover is at the end of it's current path
                entry.pathPercent = (Time.time - entry.startTime) / (entry.nextWaypoint.pathDistance / entry.initialVelocity);
                Debug.Log($"{entry.pathPercent} = ({Time.time} - {entry.startTime}) / ({entry.nextWaypoint.pathDistance} / {entry.initialVelocity})");
                if (entry.pathPercent >= 1.0f) {
                    int waypointIndex = waypoints.IndexOf(entry.nextWaypoint);
                    // Check if mover has finished following all paths on the spline curve
                    if (waypointIndex + 1 >= waypoints.Count) {
                        entry.owner.flag_engaged = false;
                        entry.ownerRB.velocity = entry.velocity;
                        Debug.Log(entry.velocity.magnitude);
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
        void CalculatePathDistance() {
            if (!flag_isInitialized) Init();

            float numberOfPoints = 10;

            // Draw path from starting point to first waypoint
            float distanceSum = 0;
            Vector3 lastPoint = Vector3.zero;
            if(!waypoints[0].flag_isInitialized) waypoints[0].Init();
            for (int i = 0; i < numberOfPoints; i++) {
                    float precent = i / numberOfPoints;
                    Vector3 drawPoint = StaticFunctions.GetSplinePosition(startPoint.transform.position, waypoints[0].centerPoint, entryInverse, waypoints[0].controlInverse, precent);
                    Debug.Log(drawPoint);
                    if (i != 0) {
                        distanceSum += Vector3.Distance(drawPoint, lastPoint);
                    }
                    else {
                        distanceSum += Vector3.Distance(drawPoint, startPoint.transform.position);
                    }
                    lastPoint = drawPoint;
            }
            waypoints[0].pathDistance = distanceSum * 1.5f;

            // Draw path between each waypoint in the system
            for (int i = 0; i < waypoints.Count - 1; i++) {
                distanceSum = 0;
                lastPoint = Vector3.zero;
                if(!waypoints[i+1].flag_isInitialized) waypoints[i+1].Init();
                for (int j = 0; j < numberOfPoints; j++) {
                    float precent = j / numberOfPoints;
                    Vector3 drawPoint = StaticFunctions.GetSplinePosition(waypoints[i].centerPoint, waypoints[i+1].centerPoint, waypoints[i].controlPoint, waypoints[i+1].controlInverse, precent);
                    if (j != 0) {
                        distanceSum += Vector3.Distance(drawPoint, lastPoint);
                    }
                    else {
                        distanceSum += Vector3.Distance(drawPoint, waypoints[i].centerPoint);
                    }
                    lastPoint = drawPoint;
                }
                waypoints[i+1].pathDistance = distanceSum * 1.5f;
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
                    Vector3 drawPoint = StaticFunctions.GetSplinePosition(startPoint.transform.position, waypoints[0].centerPoint, entryInverse, waypoints[0].controlInverse, precent);
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

        private void Event_EnterSystem(IMover in_mover) {
            in_mover.flag_engaged = true;
            SplineMoverEntry entry = new SplineMoverEntry(in_mover);
            entry.flag_onPath = true;
            Debug.Log(entry.ownerRB.velocity.magnitude);
            entry.initialVelocity = entry.ownerRB.velocity.magnitude;
            engagedMovers.Add(entry);
        }
    }   
}