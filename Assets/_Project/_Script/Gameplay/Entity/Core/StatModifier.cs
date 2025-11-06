public class StatModifier
{
    public Modifier MoveSpeedModifier { get; private set; } = new();
    public Modifier AttackSpeedModifier { get; private set; } = new();
    public Modifier DamageModifier { get; private set; } = new();
    private Entity owner;

    public void Init(Entity entity)
    {
        owner = entity;
        MoveSpeedModifier.Init();
        AttackSpeedModifier.Init();
        DamageModifier.Init();
    }

    public float GetFinalMoveSpeed()
    {
        return MoveSpeedModifier.GetFinalValue(owner.Config.BaseSpeed);
    }

    public float GetFinalAttackSpeed()
    {
        return AttackSpeedModifier.GetFinalValue(owner.Config.BaseAttackSpeed);
    }

    public float GetFinalDamage(float baseDamage)
    {
        return DamageModifier.GetFinalValue(baseDamage);
    }


    public class Modifier 
    {
        public float baseMul;
        public float baseAdd;
        public float bonusMul;
        public float bonusAdd;

        public void Init()
        {
            baseMul = 1f;
            baseAdd = 0f;
            bonusMul = 1f;
            bonusAdd = 0f;
        }

        public float GetFinalValue(float baseValue)
        {
            float value = baseValue;
            value *= baseMul;
            value += baseAdd;

            value *= bonusMul;
            value += bonusAdd;

            return value;
        }
    }
}