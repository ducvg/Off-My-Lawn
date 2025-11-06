using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class LineProjectileMover : IProjectileMover
{
    private TransformAccessArray projectileTransforms;
    private NativeArray<float> projectileSpeeds;

    public void Move(List<Projectile> projectiles)
    {
        int count = projectiles.Count;

        using (projectileSpeeds = new NativeArray<float>(count, Allocator.TempJob))
        using (projectileTransforms = new TransformAccessArray(count))
        {
            for (int i = count - 1; i >= 0; --i)
            {
                var projectile = projectiles[i];
                if (projectile.transform.position.x > GameConstant.GRID_BOUND_X_MAX)
                {
                    projectile.Despawn();
                    continue;
                }
                projectileTransforms.Add(projectile.transform);
                projectileSpeeds[i] = projectile.OwnerWeapon.Config.ProjectileConfig.Speed;
            }

            MoveJob moveLineJob = new MoveJob
            {
                projectileSpeeds = projectileSpeeds,
                deltaTime = Time.deltaTime
            };

            JobHandle jobHandle = moveLineJob.Schedule(projectileTransforms);
            jobHandle.Complete();
        }
    }

    [BurstCompile]
    struct MoveJob : IJobParallelForTransform
    {
        [ReadOnly] public NativeArray<float> projectileSpeeds;
        public float deltaTime;

        public void Execute(int index, TransformAccess transform)
        {
            Vector3 forward = transform.rotation * Vector3.forward;
            transform.position += forward * projectileSpeeds[index] * deltaTime;
        }
    }
}
