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
        if (!weapon.IsAttackFinished()) return;
        if (!weapon.HasTarget())
        {
            entity.ChangeState(new IdleState());
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

