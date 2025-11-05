
using System.Runtime.CompilerServices;
using UnityEngine;

public struct IdleState : IState
{
    public void OnEnter(Entity entity)
    {
        entity.GraphicController.PlayAnimation(Animation.IdleHash, 1f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnUpdate(Entity entity)
    {
        Weapon weapon = entity.EquipmentController.Weapon;
        if (weapon && weapon.HasTargetInRange())
        {
            entity.ChangeState(new AttackState());
        }

        if(entity is Monster)
        {
            entity.ChangeState(new WalkState());
            return;
        }
    }

    public void OnExit(Entity entity)
    {
    }
}
