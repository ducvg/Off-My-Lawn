using System;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity
{
    [SerializeField] private List<DetachableBodyPart> bodyParts;

    public override void Init(EntityConfigSO config)
    {
        base.Init(config);
        int count = bodyParts.Count;
        for (int i = 0; i < count; ++i) bodyParts[i].Init();
    }

    public override void TakeDamage(float damage, float damageForce = 3f, Action OnKill = null)
    {
        base.TakeDamage(damage, damageForce, OnKill);
        TryDetachBodyParts(damageForce);
    }

    private void TryDetachBodyParts(float force)
    {
        float healthRatio = health / Config.MaxHealth;

        for (int i = bodyParts.Count - 1; i >= 0; --i)
        {
            var bodyPart = bodyParts[i];
            if (healthRatio <= bodyPart.BreakThreshold && !bodyPart.IsDetached)
            {
                bodyPart.BreakOff(force);
            }
        }
    }

    public override void Despawn()
    {
        EntityFactory.Instance.Release(this);
    }
}

