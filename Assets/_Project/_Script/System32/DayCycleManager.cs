using System;
using UnityEngine;

public class DayCycleManager : Singleton<DayCycleManager>
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private float fullDaySeconds = 240f;
    [SerializeField] CyclePreset dayTimePreset, nightTimePreset;


    void Update()
    {
        var currentTime = Mathf.PingPong(Time.time, fullDaySeconds);
        var progress = Mathf.InverseLerp(0, fullDaySeconds, currentTime);
        
        directionalLight.color = Color.Lerp(dayTimePreset.lightColor, nightTimePreset.lightColor, progress);
        directionalLight.intensity = Mathf.Lerp(dayTimePreset.lightIntensity, nightTimePreset.lightIntensity, progress);
        directionalLight.colorTemperature = Mathf.Lerp(dayTimePreset.lightTemperature, nightTimePreset.lightTemperature, progress);
    }

    [Serializable]
    public struct CyclePreset
    {
        [field: SerializeField] public Color lightColor {get; private set;}
        [field: SerializeField] public float lightTemperature {get; private set;}
        [field: SerializeField] public float lightIntensity {get; private set;}
    }
}