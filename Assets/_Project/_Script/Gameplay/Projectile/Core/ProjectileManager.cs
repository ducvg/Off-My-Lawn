using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine;
using UnityEngine.Jobs;

public class ProjectileManager : Singleton<ProjectileManager>
{
    private PoolFactory<Projectile> projectileFactory = new();
    private List<Projectile> activeLineProjectiles = new();
    private List<Projectile> activeCurvedProjectiles = new();
    IMoveProjectile lineMover = new MoveLineProjectile();
    IMoveProjectile curveMover = new MoveCurvedProjectile();

    public Projectile Spawn(Projectile prefab, Vector3 position, Weapon weapon, Transform parent = null)
    {
        var projectile = projectileFactory.Spawn(prefab, position, parent);
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

    public void OnUpdate()
    {
        lineMover.Move(activeLineProjectiles);
        curveMover.Move(activeCurvedProjectiles); //unemployed
        ProcessCollisions();
    }

#region Collision handling
    void ProcessCollisions()
    {
        RunRayCasts(out ReadOnlySpan<RaycastHit> hits);

        int lineCount = activeLineProjectiles.Count;
        int hitCount = hits.Length;
        for (int i = hitCount - 1; i >= 0; --i)
        {
            RaycastHit hit = hits[i];
            if (!hit.collider || !LevelManager.Instance.TryGetEntityByCollider(hit.collider, out Entity entity))
            {
                continue;
            }
            Projectile projectile = i < lineCount
                ? activeLineProjectiles[i] : activeCurvedProjectiles[i - lineCount];
            projectile.OnHit(entity);
        }
    }

    void RunRayCasts(out ReadOnlySpan<RaycastHit> hits)
    {
        int lineCount = activeLineProjectiles.Count;
        int curveCount = activeCurvedProjectiles.Count;

        NativeArray<RaycastCommand> rayCommands = new(lineCount + curveCount, Allocator.TempJob);
        NativeArray<RaycastHit> hitResults = new(lineCount + curveCount, Allocator.TempJob);

        Vector3 from, to, dir, dirNormalized;
        const float offsetGuard = 0.01f;
        for (int i = 0; i < lineCount; i++)
        {
            from = activeLineProjectiles[i].LastPosition;
            to = activeLineProjectiles[i].transform.position;
            dir = to - from;
            dirNormalized = dir.normalized;
            from -= dirNormalized * offsetGuard;
            float dist = dir.magnitude + offsetGuard;

            QueryParameters qp = new QueryParameters
            {
                layerMask = activeLineProjectiles[i].OwnerWeapon.targetLayerMask,
                hitTriggers = QueryTriggerInteraction.Ignore,
                hitMultipleFaces = false,
                hitBackfaces = false,
            };
            rayCommands[i] = new RaycastCommand(from, dirNormalized, qp, dist);
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
                layerMask = activeCurvedProjectiles[i].OwnerWeapon.targetLayerMask,
                hitTriggers = QueryTriggerInteraction.Ignore,
                hitMultipleFaces = false,
                hitBackfaces = false,
            };
            rayCommands[i + lineCount] = new RaycastCommand(from, dirNormalized, qp, dist);
        }

        JobHandle handle = RaycastCommand.ScheduleBatch(rayCommands, hitResults, 50);
        handle.Complete();

        hits = hitResults.AsReadOnlySpan();

        rayCommands.Dispose();
        hitResults.Dispose();
    }
    #endregion

}