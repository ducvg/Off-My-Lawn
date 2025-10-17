using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero", menuName = "Data Object/Entity/Hero Config")]
public class HeroConfigSO : EntityConfigSO<Hero>
{
    [field: SerializeField] public HeroType HeroType { get; private set; }

    [field: Header("Stats")]
    [field: SerializeField] public float Cost { get; private set; }
    [field: SerializeField] public float CardCooldown { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }

    [field: Header("Upgrades")]
    [field: SerializeField] public UpgradePath[] UpgradePaths { get; private set; }

    [field: Header("Equipment")]
    [field: SerializeField] public WeaponConfigSO DefaultWeapon { get; private set; }
    [field: SerializeField] public ShieldConfigSO DefaultShield { get; private set; }
    [field: SerializeField] public ArmorConfigSO[] DefaultArmors { get; private set; }



}

public enum HeroType
{
    Melee,
    Ranged,
    Magic,
    Support
}