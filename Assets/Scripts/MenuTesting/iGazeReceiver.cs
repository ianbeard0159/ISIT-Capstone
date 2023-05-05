using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iGazeReceiver
{
    /// <summary>
    /// Should be called when the object is being looked at
    /// </summary>
    void GazingUpon();

    /// <summary>
    /// Should be called when the object is no longer being looked at
    /// </summary>
    void NotGazingUpon();
}
