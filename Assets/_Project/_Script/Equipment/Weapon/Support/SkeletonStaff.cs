using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SummonStaff : Weapon
{
    new Transform transform;

    public override void Equip(Entity entity)
    {
        base.Equip(entity);
        transform = base.transform;
        lastAttackTime = Time.time;
    }

    protected override void ExecuteAttack()
    {
        Config.AttackEffects[0].Apply(OwnerEntity);
    }

    public override bool HasTarget()
    {
        return CanStayAttackState();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool CanStayAttackState()
    {
        return transform.position.x < GameConstant.GRID_BOUND_X_MAX && !IsOnCooldown();
    }
    

}
