using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal static class Helpers {

    public static float Map(float value, float minIn, float maxIn, float minOut, float maxOut, bool clamp = false) {
        float val = minOut + (maxOut - minOut) * ((value - minIn) / (maxIn - minIn));

        return clamp ? Mathf.Clamp(val, Mathf.Min(minOut, maxOut), Mathf.Max(minOut, maxOut)) : val;
    }
}