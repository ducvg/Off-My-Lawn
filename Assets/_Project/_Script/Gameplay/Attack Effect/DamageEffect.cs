using System;
using UnityEngine;

[Serializable]
public class DamageEffect : IAttackEffect
{
    [SerializeField] private int damage;

    public void Execute(Entity target)
    {
        // target.TakeDamage(damageAmount);
    }
}
