using System.Collections;
using UnityEngine;

//any weapon that spawn a projectile when attack
public class GenericRangeWeapon : Weapon
{
    protected override void ExecuteAttack()
    {
        Projectile projectile = ProjectileManager.Instance
            .Spawn(Config.ProjectileConfig.Prefab, OwnerEntity.AttackPoint.position, this);
    }
}