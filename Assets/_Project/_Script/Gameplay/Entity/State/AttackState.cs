using System.Runtime.CompilerServices;
using UnityEngine;

public struct AttackState : IState
{
    Weapon weapon;

    public void OnEnter(Entity entity)
    {
        weapon = entity.EquipmentController.Weapon;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnUpdate(Entity entity)
    {
        if (!weapon.HasTargetInRange())
        {
            if (entity is Hero) entity.ChangeState(new IdleState());
            else entity.ChangeState(new WalkState());
            return;
        }
        if (weapon.IsOnCooldown()) return;
        
        entity.GraphicController.PlayAnimation(Animation.AttackHash, 0.1f);
        weapon.Attack(entity);
    }

    public void OnExit(Entity entity)
    {
        weapon.CancelAttack();
    }
}

