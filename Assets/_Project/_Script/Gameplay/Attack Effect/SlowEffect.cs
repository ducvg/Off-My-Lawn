using System;
using UnityEngine;

[Serializable]
public class SlowEffect : IAttackEffect
{
    [SerializeField, Range(0, 1)] private float speedMultiplier; //0 for stun
    [SerializeField] private float duration;

    public void Execute(Entity target)
    {
        // target.ApplySlow(speedMultiplier, duration);
    }
}
