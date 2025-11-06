using System.Collections;
using UnityEngine;

public class CrystalStaff : Weapon
{
    private float spawnTime;

    public override void Equip(Entity entity)
    {
        base.Equip(entity);
        lastAttackTime = Time.time;
        spawnTime = Mathf.Max(1f, Config.AttackCooldown);
        spawnTime += Time.time;
    }

    protected override void ExecuteAttack()
    {
        Crystal crystal = CrystalFactory.Instance.SpawnNormal(OwnerEntity.AttackPoint.position);
        crystal.Fling(force: 6f);
    }

    public override bool HasTarget()
    {
        return IsFinishedSpawning();
    }

    bool IsFinishedSpawning()
    {
        return Time.time > spawnTime;
    }

}