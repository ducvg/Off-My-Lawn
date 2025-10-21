using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero", menuName = "Data Object/Entity/Hero Config")]
public class HeroConfigSO : EntityConfigSO<Hero>
{
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