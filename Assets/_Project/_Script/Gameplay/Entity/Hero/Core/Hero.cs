using System.Collections;
using UnityEngine;

public class Hero : Entity
{
    [field: SerializeField] public HeroConfigSO Config { get; private set; }

    public (int pathIndex, int upgradeIndex) CurrentUpgrade { get; private set; } = (-1, -1);

    public void Init()
    {
        GraphicController.Init();
        EquipmentController.Init(this);

        EquipmentController
            .WithEquipment(Config.DefaultWeapon)
            .WithEquipment(Config.DefaultShield)
            .WithEquipment(Config.DefaultArmors);
    }

    public void Upgrade(int pathIndex) //set path to selected path, should disable others in UI
    {
        if (CurrentUpgrade.upgradeIndex >= Config.UpgradePaths[pathIndex].Upgrades.Length) return;

        CurrentUpgrade = (pathIndex, CurrentUpgrade.upgradeIndex + 1);

        foreach (var strategy in Config.UpgradePaths[pathIndex].Upgrades[CurrentUpgrade.upgradeIndex].UpgradeStrategies)
        {
            strategy.ApplyUpgrade(this);
        }
    }
}
