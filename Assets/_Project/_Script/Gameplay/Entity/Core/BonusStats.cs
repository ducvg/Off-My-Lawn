public class BonusStats
{
    private Entity owner;
    public float MoveSpeedMultiplier { get; set; }
    public float AttackSpeedMultiplier { get; set; }
    public float DamageMultiplier { get; set; }

    public void Init(Entity entity)
    {
        owner = entity;
        MoveSpeedMultiplier = 1f;
        AttackSpeedMultiplier = 1f;
        DamageMultiplier = 1f;
    }

    public float GetFinalMoveSpeed()
    {
        float baseSpeed = owner.Config.BaseSpeed;
        return baseSpeed * MoveSpeedMultiplier;
    }

    public float GetFinalAttackSpeed()
    {
        float baseSpeed = owner.EquipmentController.Weapon.Config.AttackSpeed;
        return baseSpeed * AttackSpeedMultiplier;
    }
}