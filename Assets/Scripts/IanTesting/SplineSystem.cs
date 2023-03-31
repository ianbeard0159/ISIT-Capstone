using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineSystem : MonoBehaviour
{
    private enum SplineStates {
        Entering,
        Engaged,
        Exiting
    }
    private Dictionary<Mover, SplineStates> engagedMovers;
    private List<SplineWaypoint> waypoints;
    // Start is called before the first frame update
    void Awake()
    {
        engagedMovers = new Dictionary<Mover, SplineStates>();
        waypoints = new List<SplineWaypoint>();
        foreach (Transform obj in transform) {
            if (obj.GetComponent<SplineWaypoint>() != null) {
                waypoints.Add(obj.GetComponent<SplineWaypoint>());
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
