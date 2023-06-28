using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        [SerializeField] private float distanceModifier = 1.5f;


        // Makes sure everything required by the game is intialized before it is needed
        void Init() {
            // Initialize Lists
            engagedMovers = new List<SplineMoverEntry>();
            waypoints = new List<SplineWaypoint>();

            // Find all waypoints in the system
            foreach (Transform obj in transform) {
                if (obj.name == "Starting Point") {
                    startPositionBox = obj.transform;
                }
                if (obj.GetComponent<SplineWaypoint>() != null) {
                    waypoints.Add(obj.GetComponent<SplineWaypoint>());
                }
            }

            // Initialize Control Points
            entryZone = transform.Find("Entry Zone").GetComponent<SplineEntryZone>();
            startPoint = transform.Find("Starting Point").GetComponent<SplineStartPoint>();
            entryControl = entryZone.transform.Find("Control");
            entryInverse = StaticFunctions.GetReflection(startPoint.transform.position, entryControl.transform.position);

            // Let the game know the system has been initialized
            flag_isInitialized = true;
        }

        void Awake()
        {
            // Make sure the system is initialized
            if (!flag_isInitialized) Init();
            entryZone = transform.Find("Entry Zone").GetComponent<SplineEntryZone>();
            startPoint = transform.Find("Starting Point").GetComponent<SplineStartPoint>();

            // Subscribe to the entry zone's OnTriggerEnter event
            entryZone.on_enterTrigger += Event_EnterSystem;
        }
        void Start() {

            // Find the length of the path in the spline system
            entryControl = entryZone.transform.Find("Control");
            CalculatePathDistance();
        }

        void FixedUpdate()
        {
            //check if pauseReset has been flipped; if so, toss out all movers
            if (Tricks.pauseReset == true)
            {
                TossMovers();
            }

            // Remove any movers that are no longer on the path
            for (int i = engagedMovers.Count - 1; i >= 0; i--)
            {
                if (!engagedMovers[i].flag_onPath)
                {
                    engagedMovers.Remove(engagedMovers[i]);
                }
            }

            // Move any movers in the system
            foreach (SplineMoverEntry entry in engagedMovers) {

                //
                // --=|| Go from entry zone to start point ||=--
                //
                if (entry.nextWaypoint == null) {

                    // If the timer has not been started
                    if (entry.startTime == -1) {
                        entry.startTime = Time.time;
                        entry.entryPosition = entry.ownerTR.position;
                    }
                    
                    // If the mover is at the end of the current segment, move them to the next segment
                    entry.pathPercent = (Time.time - entry.startTime) / ((Vector3.Distance(entry.entryPosition, startPoint.transform.position) * distanceModifier) / entry.initialVelocity);
                    if (entry.pathPercent >= 1.0f) {
                        entry.startTime = Time.time;
                        entry.nextWaypoint = waypoints[0];
                    }
                    // Else, move the mover along the path
                    else {
                        entry.SetPosition(Time.time, entry.entryPosition, entryControl.position, startPoint.transform.position, entryControl.position, true);
                    }

                    // Skip to the next itreration of the loop
                    continue;
                }

                // 
                // --=|| Go from start point to first waypoint ||=--
                //
                if (entry.nextWaypoint == waypoints[0]) {
                    
                    // If the mover is at the end of the current segment, move them to the next segment
                    entry.pathPercent = (Time.time - entry.startTime) / (entry.nextWaypoint.pathDistance / entry.initialVelocity);
                    if (entry.pathPercent >= 1.0f) {
                        entry.startTime = Time.time;
                        entry.currentWaypoint = waypoints[0];
                        entry.nextWaypoint = waypoints[1];
                    }
                    // Else, move the mover along the path
                    else {
                        entry.SetPosition(Time.time, startPoint.transform.position, entryInverse, entry.nextWaypoint.transform.position, StaticFunctions.GetReflection(entry.nextWaypoint.transform.position, entry.nextWaypoint.control.position));
                    }

                    // Skip to the next itreration of the loop
                    continue;
                }

                //
                // --=|| If the mover is past the first waypoint ||=--
                //

                // Check if mover is at the end of it's current path
                entry.pathPercent = (Time.time - entry.startTime) / (entry.nextWaypoint.pathDistance / entry.initialVelocity);
                if (entry.pathPercent >= 1.0f) {

                    // If mover has finished following all paths on the spline curve, eject them from the system
                    int waypointIndex = waypoints.IndexOf(entry.nextWaypoint);
                    if (waypointIndex + 1 >= waypoints.Count) {
                        entry.owner.flag_engaged = false;
                        entry.ownerRB.velocity = entry.velocity;
                        entry.flag_onPath = false;

                        // Skip to the next itreration of the loop
                        continue;
                    }

                    // Else, move them to the next segment
                    else {
                        entry.currentWaypoint = waypoints[waypointIndex];
                        entry.nextWaypoint = waypoints[waypointIndex + 1];
                        entry.startTime = Time.time;
                        entry.pathPercent = 0;

                        // Skip to the next itreration of the loop
                        continue;
                    }

                }
                
                // If none of the above if statements triggered, move the mover along the path
                entry.SetPosition(Time.time);
            }           
        }

        void CalculatePathDistance() {

            // Make sure the system is initialized
            if (!flag_isInitialized) Init();
            entryInverse = StaticFunctions.GetReflection(startPoint.transform.position, entryControl.transform.position);

            //
            // --=|| Draw path from starting point to first waypoint ||=--
            //

            float numberOfPoints = 10;
            float distanceSum = 0;
            Vector3 lastPoint = Vector3.zero;

            // Make sure the first waypoint is initialized
            if(!waypoints[0].flag_isInitialized) waypoints[0].Init();

            // Subdivide the path by numberOfPoints
            for (int i = 0; i < numberOfPoints; i++) {
                    float precent = i / numberOfPoints;
                    Vector3 drawPoint = StaticFunctions.GetSplinePosition(startPoint.transform.position, waypoints[0].transform.position, entryInverse, StaticFunctions.GetReflection(waypoints[0].transform.position, waypoints[0].control.position), precent);
                    if (i != 0) {
                        distanceSum += Vector3.Distance(drawPoint, lastPoint);
                    }
                    else {
                        distanceSum += Vector3.Distance(drawPoint, startPoint.transform.position);
                    }
                    lastPoint = drawPoint;
            }

            // Set the distance property of the first waypoint
            waypoints[0].pathDistance = distanceSum * distanceModifier;

            //
            // --=|| Draw path between each waypoint in the system ||=--
            //
            for (int i = 0; i < waypoints.Count - 1; i++) {

                distanceSum = 0;
                lastPoint = waypoints[i].transform.position;

                // Make sure the waypoint is initialized
                if(!waypoints[i+1].flag_isInitialized) waypoints[i+1].Init();
                
                // Subdivide the path by numberOfPoints
                for (int j = 0; j < numberOfPoints; j++) {
                    float precent = j / numberOfPoints;
                    Vector3 drawPoint = StaticFunctions.GetSplinePosition(waypoints[i].transform.position, waypoints[i+1].transform.position, waypoints[i].control.position, StaticFunctions.GetReflection(waypoints[i+1].transform.position, waypoints[i+1].control.position), precent);
                    
                    distanceSum += Vector3.Distance(drawPoint, lastPoint);
                    
                    lastPoint = drawPoint;
                }
                
                // Set the distance property of the current waypoint
                waypoints[i+1].pathDistance = distanceSum * distanceModifier;
            }
        }

        void OnDestroy() {
            // Unsubscribe from events
            entryZone.on_enterTrigger -= Event_EnterSystem;
        }

        void OnDrawGizmos() {

            // Make sure the system is initialized
            if (!flag_isInitialized) Init();

            Gizmos.color = Color.green;
            float numberOfPoints = 10;

            entryInverse = StaticFunctions.GetReflection(startPoint.transform.position, entryControl.transform.position);
            Gizmos.DrawSphere(entryInverse, 1f);

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

            // Create a SplineMoverEntry for the input mover and initialize it
            in_mover.flag_engaged = true;
            SplineMoverEntry entry = new SplineMoverEntry(in_mover);
            entry.flag_onPath = true;
            entry.initialVelocity = entry.ownerRB.velocity.magnitude;

            // Add the entry to the list of movers in the system
            engagedMovers.Add(entry);
        }

        //toss out all movers immediately
        void TossMovers()
        {
            for (int i = engagedMovers.Count - 1; i >= 0; i--)
            {
                engagedMovers[i].owner.flag_engaged = false;
                engagedMovers.Remove(engagedMovers[i]);
            }
            //UnityEngine.Debug.Log("Movers tossed");
        }
    }   
}