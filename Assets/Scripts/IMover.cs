using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMover
{
    bool flag_engaged {get; set;}
    Rigidbody GetRigidbody();
    Transform GetTransform();
}
