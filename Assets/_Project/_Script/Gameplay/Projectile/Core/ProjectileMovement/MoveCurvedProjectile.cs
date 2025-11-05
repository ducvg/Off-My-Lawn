using System.Collections.Generic;
using UnityEngine;

public class MoveCurvedProjectile : IMoveProjectile
{
    public void Move(List<Projectile> projectiles)
    {
        Vector3 checkPos;
        int count = projectiles.Count;
        for (int i = 0; i < count; i++)
        {
            checkPos = projectiles[i].transform.position;
            if (checkPos.x > GameConstant.GRID_BOUND_X_MAX
                || checkPos.y < GameConstant.GRID_BOUND_Y_MIN)
            {
                projectiles[i].OnDespawn();
                continue;
            }
            projectiles[i].OnUpdate();
        }
    }
}