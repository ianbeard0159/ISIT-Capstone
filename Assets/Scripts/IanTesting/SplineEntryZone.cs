using System;
using UnityEngine;

namespace Spline 
{
    public class SplineEntryZone : MonoBehaviour
    {
        public Action<Mover> on_enterTrigger;
        void OnTriggerEnter(Collider in_collider) {
            Mover mover = in_collider.gameObject.GetComponent<Mover>();
            if (mover != null && !mover.flag_engaged) {
                on_enterTrigger?.Invoke(mover);
            }
        }
    }
}

