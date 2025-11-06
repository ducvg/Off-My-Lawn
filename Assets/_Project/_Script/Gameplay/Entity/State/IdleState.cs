using System.Runtime.CompilerServices;
using UnityEngine;

public struct IdleState : IState
{
    public void OnEnter(Entity entity)
    {
        if (entity is Monster)
        {
            entity.ChangeState(new WalkState());
            return;
        }
        entity.GraphicController.PlayAnimation(Animation.IdleHash, 0.1f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnUpdate(Entity entity)
    {
        Weapon weapon = entity.EquipmentController.Weapon;
        if (weapon && weapon.HasTarget())
        {
            entity.ChangeState(new AttackState());
        }
    }

    public void OnExit(Entity entity)
    {
    }
}
