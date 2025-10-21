using UnityEngine;

public abstract class Armor : Equipment
{
    [SerializeField] private ArmorConfigSO config;
    protected float health;

    public override void Equip(Entity entity)
    {
        base.Equip(entity);
        health = config.BaseHealth;
    }

    public abstract void TakeDamage(float damage);
}