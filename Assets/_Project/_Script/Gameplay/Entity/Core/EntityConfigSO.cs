using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity Config", menuName = "Data Object/Entity Config")]
public class EntityConfigSO : ScriptableObject
{
    [field: Header("General")]
    [field: SerializeField] public EntityID Id { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public Color CardColor { get; private set; }
    [field: SerializeField] public Entity Prefab { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public AnimationClip[] MoveAnimation { get; private set; }
    [field: SerializeField] public AnimationClip DieAnimation { get; private set; }

    [field: Header("Equipment")]
    [field: SerializeField] public WeaponConfigSO DefaultWeaponConfig { get; private set; }
    [field: SerializeField] public ShieldConfigSO DefaultShieldConfig { get; private set; }
    [field: SerializeField] public ArmorConfigSO[] DefaultArmorConfigs { get; private set; }

    [field: Header("Stats")]
    [field: SerializeField] public float CardCooldown { get; private set; }
    [field: SerializeField] public float CrystalCost { get; private set; }
    [field: SerializeField] public float MaxHealth { get; private set; } = 100f;

    [field: Header("Upgrades")]
    [field: SerializeField] public UpgradePath[] UpgradePaths { get; private set; }

    [field: Header("Movement")]
    [field: SerializeField] public float BaseSpeed { get; private set; } = 1;
    [field: SerializeField] public AnimationCurve SpeedCurve { get; private set; }

    [field: Header("Spawn Data")]
    [field: SerializeField] public int SpawnCost { get; private set; }
}

public enum EntityID
{
    Archer,
    Engineer,
    Knight,
    Druid,
    Warrior,
    Wizard,

    Skeleton,
}