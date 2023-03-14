using System;
using UnityEngine;
using UnityEngine.Events;

public class StartPoint : MonoBehaviour
{
    public Action<Transform> on_enterStartPoint;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider in_object) {
        on_enterStartPoint?.Invoke(in_object.transform);
    }
}
