using System;
using UnityEngine;

[Serializable]
public class StatusAttack : IAttackEffect
{
    [SerializeReference] private IStatusEffect statusEffect; 

    public void Apply(Entity target)
    {
        target.ApplyStatusEffect(statusEffect.Clone());
    }
}
