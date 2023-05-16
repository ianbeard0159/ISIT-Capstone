using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionPauseMenu : MonoBehaviour
{
    public Transform playerReference;
    public Vector3 offset;

    private void Update()
    {
        transform.position = playerReference.position + offset;
    }
}

