using System.Runtime.CompilerServices;
using UnityEngine;

public struct DropInState : IState
{
    private float spawnTime;

    public void OnEnter(Entity entity)
    {
        entity.GraphicController.PlayAnimation(Animation.SpawnAirHash, 0f);
        spawnTime = 0.4f;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnUpdate(Entity entity)
    {
        spawnTime -= Time.deltaTime;
        if (spawnTime <= 0f)
        {
            entity.ChangeState(new EquipState());
            return;
        }

        var weapon = entity.EquipmentController.Weapon;
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