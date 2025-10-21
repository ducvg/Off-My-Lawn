using UnityEngine;

public abstract class EntityConfigSO<TEntity> : ScriptableObject where TEntity : Entity
{
    [field: Header("General")]
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public TEntity Prefab { get; private set; }

    [field: Header("Equipment")]
    [field: SerializeField] public WeaponConfigSO DefaultWeapon { get; private set; }
    [field: SerializeField] public ShieldConfigSO DefaultShield { get; private set; }
    [field: SerializeField] public ArmorConfigSO[] DefaultArmors { get; private set; }

    [field: Header("Stats")]
    [field: SerializeField] public float Cost { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public float CardCooldown { get; private set; }
}
