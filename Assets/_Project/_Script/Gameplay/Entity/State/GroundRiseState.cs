using System.Runtime.CompilerServices;
using UnityEngine;

public struct GroundRiseState : IState
{
    private float spawnTime;

    public void OnEnter(Entity entity)
    {
        entity.GraphicController.PlayAnimation(Animation.SpawnGroundHash, 0f);
        spawnTime = 1f;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnUpdate(Entity entity)
    {
        spawnTime -= Time.deltaTime;
        if (spawnTime <= 0f)
        {
            entity.ChangeState(new IdleState());
            return;
        }
    }

    public void OnExit(Entity entity)
    {
    }
}