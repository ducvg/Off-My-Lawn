using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero", menuName = "Data Object/Entity/Hero Config")]
public class HeroConfigSO : ScriptableObject
{
    [field: Header("General")]
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public Hero Prefab { get; private set; }
    [field: SerializeField] public AnimationClip SpawnAnimation { get; private set; }
    [field: SerializeField] public AnimationClip DieAnimation { get; private set; }

    [field: Header("Equipment")]
    [field: SerializeField] public Weapon DefaultWeapon { get; private set; }
    [field: SerializeField] public Shield DefaultShield { get; private set; }
    [field: SerializeField] public Armor[] DefaultArmors { get; private set; }

    [field: Header("Stats")]
    [field: SerializeField] public float Cost { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public float CardCooldown { get; private set; }
    [field: SerializeField] public HeroType HeroType { get; private set; }

    [field: Header("Upgrades")]
    [field: SerializeField] public UpgradePath[] UpgradePaths { get; private set; }
}

public enum HeroType
{
    Melee,
    Ranged,
    Magic,
    Support
}