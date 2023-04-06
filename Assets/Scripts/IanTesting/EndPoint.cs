using System;
using UnityEngine;

namespace Spline {
    public class EndPoint : MonoBehaviour
    {
        [SerializeField] public bool lastEndPoint;
        public Action<Transform, bool> on_enterEndPoint;
        // Start is called before the first frame update
        private void OnTriggerEnter(Collider in_object) {
            on_enterEndPoint?.Invoke(in_object.transform, lastEndPoint);
        }
    }
}
