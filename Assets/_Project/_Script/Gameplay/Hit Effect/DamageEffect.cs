using System;
using UnityEngine;

[Serializable]
public class DamageEffect : IHitEffect
{
    [SerializeField] private int damage;

    public void Execute(Entity target)
    {
        // target.TakeDamage(damageAmount);
    }
}
