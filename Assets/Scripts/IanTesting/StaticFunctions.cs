using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spline 
{
    public static class StaticFunctions
    {
        public static Vector3 GetSplinePosition(Vector3 in_pointStart, Vector3 in_pointEnd, Vector3 in_controlA, Vector3 in_controlB, float in_percentage) {
            // Enforce percentage
            float percentage = in_percentage;

            // Linear Movements
            Vector3 linear_start_A = Vector3.Lerp(in_pointStart, in_controlA, percentage);
            Vector3 linear_A_B = Vector3.Lerp(in_controlA, in_controlB, percentage);
            Vector3 linear_B_end = Vector3.Lerp(in_controlB, in_pointEnd, percentage);

            // Secondary Movements
            Vector3 secondary_start_A_B = Vector3.Lerp(linear_start_A, linear_A_B, percentage);
            Vector3 secondary_A_B_end = Vector3.Lerp(linear_A_B, linear_B_end, percentage);

            // Primary Movement
            return Vector3.Lerp(secondary_start_A_B, secondary_A_B_end, percentage);
        }

        public static Vector3 GetReflection(Vector3 in_origin, Vector3 in_point) {
            return in_origin + ((in_origin - in_point).normalized * Vector3.Distance(in_origin, in_point));
        }
    }
}

