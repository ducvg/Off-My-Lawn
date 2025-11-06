using UnityEngine;

public abstract class Shield : Equipment
{
    public ShieldConfigSO Config { get; private set; }
    protected float health;

    public void SetConfig(ShieldConfigSO config)
    {
        Config = config;
    }

    public override void Equip(Entity entity)
    {
        health = Config.BaseHealth;

        entity.GraphicController
            .WithOverrideAnimation(Animation.EQUIP, Config.EquipAnimation)
            .WithOverrideAnimation(Animation.IDLE, Config.IdleAnimation)
            .WithOverrideAnimation(Animation.HURT, Config.HurtAnimation)
            .ApplyAnimatorOverrides();
    }

    public abstract void Block(Entity entity, ref float damage);

    public override void Unequip(Entity entity)
    {
        var idle = entity.EquipmentController.Weapon ? entity.EquipmentController.Weapon.Config.IdleAnimation : null;

        entity.GraphicController
            .WithOverrideAnimation(Animation.EQUIP, null)
            .WithOverrideAnimation(Animation.IDLE, idle)
            .WithOverrideAnimation(Animation.HURT, null)
            .ApplyAnimatorOverrides();

        entity.EquipmentController.UnequipShield(this);

        Destroy(gameObject);
    }
}