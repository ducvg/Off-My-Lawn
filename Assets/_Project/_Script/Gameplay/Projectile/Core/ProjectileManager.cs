using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class ProjectileManager : Singleton<ProjectileManager>
{
    private PoolFactory<Projectile> projectileFactory = new();
    private List<Projectile> activeLineProjectiles = new();
    private List<Projectile> activeCurvedProjectiles = new();
    private List<Projectile> projectileToReturn = new();

    public Projectile Spawn(Projectile prefab, Vector3 position, Weapon weapon, Transform parent = null)
    {
        var projectile = projectileFactory.Spawn(prefab, position, parent);
        projectile.transform.forward = weapon.OwnerEntity.transform.forward;
        projectile.Init(weapon);

        if (weapon.Config.ProjectileConfig.UseCurve) activeCurvedProjectiles.Add(projectile);
        else activeLineProjectiles.Add(projectile);

        return projectile;
    }

    public void Release(Projectile projectile)
    {
        if (projectile.Config.UseCurve) activeCurvedProjectiles.Remove(projectile);
        else activeLineProjectiles.Remove(projectile);

        projectile.OnDespawn();
        projectileFactory.Release(projectile.Config.Prefab, projectile);
    }

    void Update()
    {
        MoveStraight(activeLineProjectiles);
        MoveSelf(activeCurvedProjectiles); //unemployed, temp solution
        RemoveOutOfBound();
        ProcessCollisions();
    }

    #region Movement
    private void MoveStraight(List<Projectile> projectiles)
    {
        int count = projectiles.Count;

        var projectileSpeeds = new NativeArray<float>(count, Allocator.TempJob);
        var projectileTransforms = new TransformAccessArray(count);

        for (int i = count - 1; i >= 0; --i)
        {
            var projectile = projectiles[i];
            projectile.OnMove();
            
            projectileTransforms.Add(projectile.transform);
            projectileSpeeds[i] = projectile.Config.Speed;
        }

        MoveJob moveLineJob = new MoveJob
        {
            projectileSpeeds = projectileSpeeds,
            deltaTime = Time.deltaTime
        };

        JobHandle jobHandle = moveLineJob.Schedule(projectileTransforms);
        jobHandle.Complete();

        projectileSpeeds.Dispose();
        projectileTransforms.Dispose();
    }

    private void MoveSelf(List<Projectile> projectiles) //normal update()
    {
        int count = projectiles.Count;
        for (int i = 0; i < count; ++i)
        {
            var projectile = projectiles[i];
            projectile.OnMove();
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
#endregion

    private void RemoveOutOfBound()
    {
        int count = projectileToReturn.Count;
        for(int i = 0; i < count; ++i)
        {
            var projectile = projectileToReturn[i];
            projectile.OnDespawn();
            Release(projectile);
        }
        projectileToReturn.Clear();
    }

#region Collision
    private void ProcessCollisions()
    {
        RunRayCasts(out NativeArray<RaycastHit> hitResults);
        CheckCollision(in hitResults);
        hitResults.Dispose();
    }

    private void RunRayCasts(out NativeArray<RaycastHit> hitResults)
    {
        int lineCount = activeLineProjectiles.Count;
        int curveCount = activeCurvedProjectiles.Count;

        NativeArray<RaycastCommand> rayCommands = new(lineCount + curveCount, Allocator.TempJob);
        hitResults = new(lineCount + curveCount, Allocator.TempJob);

        Vector3 from, to, dir;
        const float offsetGuard = 0.001f;
        
        for (int i = 0; i < lineCount; ++i)
        {
            //raycast from last position to current position
            from = activeLineProjectiles[i].LastPosition;
            to = activeLineProjectiles[i].transform.position;
            dir = to - from;
            float rayLength = dir.magnitude + offsetGuard;

            QueryParameters qp = new QueryParameters
            {
                layerMask = activeLineProjectiles[i].OwnerWeapon.TargetLayerMask,
                hitTriggers = QueryTriggerInteraction.Ignore,
                hitMultipleFaces = false,
                hitBackfaces = false,
            };
            rayCommands[i] = new RaycastCommand(from, dir.normalized, qp, rayLength);
        }

        for (int i = 0; i < curveCount; ++i)
        {
            from = activeCurvedProjectiles[i].LastPosition;
            to = activeCurvedProjectiles[i].transform.position;
            dir = to - from;
            float rayLength = dir.magnitude + offsetGuard;

            QueryParameters qp = new QueryParameters
            {
                layerMask = activeCurvedProjectiles[i].OwnerWeapon.TargetLayerMask,
                hitTriggers = QueryTriggerInteraction.Ignore,
                hitMultipleFaces = false,
                hitBackfaces = false,
            };
            rayCommands[i + lineCount] = new RaycastCommand(from, dir.normalized, qp, rayLength);
        }

        JobHandle handle = RaycastCommand.ScheduleBatch(rayCommands, hitResults, 100);
        handle.Complete();

        rayCommands.Dispose();
    }

    private void CheckCollision(in NativeArray<RaycastHit> hitResults)
    {
        int lineCount = activeLineProjectiles.Count;
        int hitCount = hitResults.Length;
        for (int i = hitCount - 1; i >= 0; --i)
        {
            RaycastHit hit = hitResults[i];
            if (!hit.collider || !LevelManager.Instance.TryGetEntityByCollider(hit.collider, out Entity entity))
            {
                continue;
            }
            Projectile projectile = i < lineCount
                ? activeLineProjectiles[i]
                : activeCurvedProjectiles[i - lineCount];
            projectile.OnHit(entity);
        }
    }
#endregion
}