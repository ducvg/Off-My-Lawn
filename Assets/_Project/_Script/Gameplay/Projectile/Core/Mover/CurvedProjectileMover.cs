using System.Collections.Generic;
using UnityEngine;

public class CurvedProjectileMover : IProjectileMover
{
    // NativeArray<NativeArray<float>> sampleCurves;

    public void Move(List<Projectile> projectiles) //normal update()
    {
        Vector3 checkPos;
        int count = projectiles.Count;
        for (int i = count - 1; i >= 0; --i)
        {
            checkPos = projectiles[i].transform.position;
            if (checkPos.x > GameConstant.GRID_BOUND_X_MAX
                || checkPos.y < GameConstant.GRID_BOUND_Y_MIN)
            {
                projectiles[i].Despawn();
                continue;
            }
        }
    }
}