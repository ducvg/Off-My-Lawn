using System;
using UnityEngine;

[Serializable]
public class KnockbackEffect : IAttackEffect
{
    [SerializeField] private float distance;

    public void Apply(Entity target)
    {
        
        target.transform.position -= target.transform.forward * distance;
    }
}
