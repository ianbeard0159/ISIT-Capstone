using System;
using UnityEngine;

namespace Spline 
{
    public class SplineEntryZone : MonoBehaviour
    {
        public Action<IMover> on_enterTrigger;
        void OnTriggerEnter(Collider in_collider) {
            IMover mover = in_collider.gameObject.GetComponent<IMover>();
            if (mover != null && !mover.flag_engaged) {
                on_enterTrigger?.Invoke(mover);
            }
        }
    }
}

