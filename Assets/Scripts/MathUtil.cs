using UnityEngine;

namespace MathUtil
{
    class MathUtilHelpers
    {
        public static float RemapF(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
        {
            Mathf.Clamp(OldValue, OldMin, OldMax);

            float OldRange = (OldMax - OldMin);
            float NewRange = (NewMax - NewMin);
            float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

            return (NewValue);
        }
    }
}
