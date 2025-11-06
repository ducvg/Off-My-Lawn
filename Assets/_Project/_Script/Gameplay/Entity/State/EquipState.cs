
using System.Runtime.CompilerServices;
using UnityEngine;

public struct EquipState : IState
{
    private float equipClipTimer;

    public void OnEnter(Entity entity)
    {
        entity.GraphicController.PlayAnimation(Animation.EquipHash, 1f);
        equipClipTimer = 0.5f;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnUpdate(Entity entity)
    {
        equipClipTimer -= Time.deltaTime;
        if (equipClipTimer <= 0f)
        {
            entity.ChangeState(new IdleState());
            return;
        }
        
        Weapon weapon = entity.EquipmentController.Weapon;
        if (weapon && weapon.HasTarget())
        {
            entity.ChangeState(new AttackState());
            return;
        }
    }

    public void OnExit(Entity entity)
    {
    }
}