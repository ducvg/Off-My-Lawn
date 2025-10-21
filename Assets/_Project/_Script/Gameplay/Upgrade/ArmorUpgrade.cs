using System;
using UnityEngine;

[Serializable]
public class ArmorUpgrade : IUpgradeStrategy
{
    [SerializeField] private ArmorConfigSO upgradeArmorConfig;

    public void ApplyUpgrade(Hero hero)
    {

    }
}
