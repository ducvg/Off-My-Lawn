using UnityEngine;

public abstract class Shield : Equipment
{
    [SerializeField] private ShieldConfigSO config;
    protected float health;

    public override void Equip(Entity entity)
    {
        base.Equip(entity);
        health = config.BaseHealth;
    }

    public abstract void Block(float damage);
}
