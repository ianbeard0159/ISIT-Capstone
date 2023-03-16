using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    //    - Use[RequireComponent(typeof(ComponentName))] to make sure required components are used
    //- Use GetComponent<ComponentName>() to access other components

    public bool Flag_trick = false;
    public bool Flag_baseZone = false;
    public bool Flag_liftZone = false;
    public bool Flag_landing = false;
 
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }

    void AtEnd()
    {

    }

    //- Create public boolean values to use as flags for if the player is:
    //	- Doing a trick
    //	- Entering feature “engagement zone” (Used to allow player to try to engage with feature)
    //	- Entering a feature(Used to lock movement onto feature)
    //	- Engaged with a feature
    //	- May need a separate set of flags for each feature?
    void EnterFeature()
    {

    }

    //- Create functions to manage the state the player is in based on the flags that are active
    void CheckState()
    {

    }

    //- Create function to check the players proximity towards either a feature’s “exit” zone or the ground if they are engaged with a feature
    void FeatureExitorLanding()
    {

    }

    //- Create a Move() function that takes the VR Rig’s look direction as input and moves/rotates the player accordingly
    void Move()
    {

    }

    //- Use OnCollision() to stop the player if they collide with anything that isn’t the ground(Check the tag of the thing the player collided with, and stop the player if the tag isn’t Ground)
    void OnCollision()
    {

    }
}
