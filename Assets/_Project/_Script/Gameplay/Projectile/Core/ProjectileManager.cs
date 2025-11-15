using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class ProjectileManager : Singleton<ProjectileManager>
{
    private PoolFactory<Projectile> projectileFactory = new();
    private List<Projectile> activeLineProjectiles = new();
    private List<Projectile> activeCurvedProjectiles = new();
    IProjectileMover lineMover = new LineProjectileMover();
    IProjectileMover curveMover = new CurvedProjectileMover();

    public Projectile Spawn(Projectile prefab, Vector3 position, Weapon weapon, Transform parent = null)
    {
        var projectile = projectileFactory.Spawn(prefab, position, parent);
        projectile.transform.forward = weapon.OwnerEntity.transform.forward;
        projectile.Init(weapon);

        if (weapon.Config.ProjectileConfig.UseCurve) activeCurvedProjectiles.Add(projectile);
        else activeLineProjectiles.Add(projectile);

        return projectile;
    }

    public void Release(Projectile prefab, Projectile projectile)
    {
        if (projectile.OwnerWeapon.Config.ProjectileConfig.UseCurve) activeCurvedProjectiles.Remove(projectile);
        else activeLineProjectiles.Remove(projectile);

        projectileFactory.Release(prefab, projectile);
    }

    void Update()
    {
        lineMover.Move(activeLineProjectiles);
        curveMover.Move(activeCurvedProjectiles); //unemployed
        ProcessCollisions();
    }

    void ProcessCollisions()
    {
        RunRayCasts(out NativeArray<RaycastHit> hitResults);
        CheckCollision(in hitResults);
        hitResults.Dispose();
    }

    void RunRayCasts(out NativeArray<RaycastHit> hitResults)
    {
        int lineCount = activeLineProjectiles.Count;
        int curveCount = activeCurvedProjectiles.Count;

        NativeArray<RaycastCommand> rayCommands = new(lineCount + curveCount, Allocator.TempJob);
        hitResults = new(lineCount + curveCount, Allocator.TempJob);

        Vector3 from, to, dir, dirNormalized;
        const float offsetGuard = 0.01f;
        
        for (int i = 0; i < lineCount; i++)
        {
            //raycast from last position to current position
            from = activeLineProjectiles[i].LastPosition;
            to = activeLineProjectiles[i].transform.position;
            dir = to - from;
            dirNormalized = dir.normalized;
            from -= dirNormalized * offsetGuard;
            float rayLength = dir.magnitude + offsetGuard;

            QueryParameters qp = new QueryParameters
            {
                layerMask = activeLineProjectiles[i].OwnerWeapon.TargetLayerMask,
                hitTriggers = QueryTriggerInteraction.Ignore,
                hitMultipleFaces = false,
                hitBackfaces = false,
            };
            rayCommands[i] = new RaycastCommand(from, dirNormalized, qp, rayLength);
        }

        for (int i = 0; i < curveCount; i++)
        {
            from = activeCurvedProjectiles[i].LastPosition;
            to = activeCurvedProjectiles[i].transform.position;
            dir = to - from;
            dirNormalized = dir.normalized;
            from -= dirNormalized * offsetGuard;
            float dist = dir.magnitude + offsetGuard;

            QueryParameters qp = new QueryParameters
            {
                layerMask = activeCurvedProjectiles[i].OwnerWeapon.TargetLayerMask,
                hitTriggers = QueryTriggerInteraction.Ignore,
                hitMultipleFaces = false,
                hitBackfaces = false,
            };
            rayCommands[i + lineCount] = new RaycastCommand(from, dirNormalized, qp, dist);
        }

        JobHandle handle = RaycastCommand.ScheduleBatch(rayCommands, hitResults, 100);
        handle.Complete();

        rayCommands.Dispose();
    }

    void CheckCollision(in NativeArray<RaycastHit> hitResults)
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

}