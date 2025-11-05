using System;
using UnityEngine;

[Serializable]
public class DamageEffect : IAttackEffect
{
    [SerializeField] private float damage;

    public void Apply(Entity target)
    {
        target.TakeDamage(damage);
    }
}