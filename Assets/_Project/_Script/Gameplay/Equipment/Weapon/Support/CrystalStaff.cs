using System.Collections;
using UnityEngine;

public class CrystalStaff : Weapon
{
    private float spawnTime;

    public override void Equip(Entity entity)
    {
        base.Equip(entity);
        lastAttackTime = Time.time;
        spawnTime = Mathf.Max(2f, Config.AttackCooldown);
        spawnTime += Time.time;
    }

    protected override void ExecuteAttack()
    {
        Crystal crystal = CrystalFactory.Instance.SpawnNormal(ownerEntity.AttackPoint.position);
        crystal.Fling(force: 5f);
    }
    
    public override bool HasTargetInRange() //spawn -> always attack
    {
        return Time.time > spawnTime;
    }


}