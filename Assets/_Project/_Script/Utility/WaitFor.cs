using System.Collections.Generic;
using UnityEngine;

public static class WaitFor {
    static readonly Dictionary<float, WaitForSeconds> WaitForSecondsDict = new(100, new FloatComparer());
    public static WaitForSeconds Seconds(float seconds) {
        // if (seconds < 1f / Application.targetFrameRate) return null;
        if (!WaitForSecondsDict.TryGetValue(seconds, out var forSeconds)) {
            forSeconds = new WaitForSeconds(seconds);
            WaitForSecondsDict[seconds] = forSeconds;
        }
        return forSeconds;
    }

    class FloatComparer : IEqualityComparer<float> {
        public bool Equals(float x, float y) => Mathf.Approximately(x, y);
        public int GetHashCode(float obj) => obj.GetHashCode();
    }
}