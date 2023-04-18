using System;
using UnityEngine;

namespace Spline 
{
    public class SplineEntryZone : MonoBehaviour
    {
        public Action<IMover> on_enterTrigger;
        [SerializeField] public float cutoffSpeed = 50f;
        void OnTriggerEnter(Collider in_collider) {
            IMover mover = in_collider.gameObject.GetComponent<IMover>();
            if (mover != null && !mover.flag_engaged && mover.GetRigidbody().velocity.magnitude < cutoffSpeed) {
                on_enterTrigger?.Invoke(mover);
            }
        }
    }
}

