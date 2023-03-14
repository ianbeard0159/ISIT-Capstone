using System;
using UnityEngine;
using UnityEngine.Events;

public class StartPoint : MonoBehaviour
{
    public Action<Transform> on_enterStartPoint;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider in_object) {
        Mover obj = in_object.gameObject.GetComponent<Mover>();
        if (obj != null && !obj.flag_engaged) {
            obj.flag_engaged = true;
            on_enterStartPoint?.Invoke(in_object.transform);
        }
    }
}
