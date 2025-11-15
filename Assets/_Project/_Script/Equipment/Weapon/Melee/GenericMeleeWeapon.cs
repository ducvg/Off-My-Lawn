using System.Collections;
using UnityEngine;

public class GenericMeleeWeapon : Weapon
{
    protected override void ExecuteAttack()
    {
        var hits = Physics.RaycastAll(ray, Config.AttackRange, TargetLayerMask);
        int hitCount = Mathf.Min(hits.Length, Config.AttackPierce);
        if (hitCount <= 0) return;
        for (int i = 0; i < hitCount; i++)
        {
            if (!LevelManager.Instance.TryGetEntityByCollider(hits[i].collider, out var target)) continue;
            foreach (var effect in Config.AttackEffects)
            {
                effect.Apply(target);
            }
        }
        
    }
}
