using UnityEngine;

public static class ColorExtension
{
    public static Color ToEmissionColor(this Color baseColor, float intensity)
    {
        return new Color(
            baseColor.r / 255f * intensity,
            baseColor.g / 255f * intensity,
            baseColor.b / 255f * intensity,
            1f);
    }
}