using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponUpgrade : IUpgradeStrategy
{
    [SerializeField] private WeaponConfigSO upgradeWeaponConfig;

    public void ApplyUpgrade(Hero hero)
    {

    }
}
