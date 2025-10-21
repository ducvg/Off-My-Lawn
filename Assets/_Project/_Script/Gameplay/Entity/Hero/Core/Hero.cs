using System.Collections;
using UnityEngine;

public class Hero : Entity
{
    [field: SerializeField] public HeroConfigSO Config { get; private set; }

    public (int pathIndex, int upgradeIndex) CurrentUpgrade { get; private set; } = (-1, -1);

    public override void Init()
    {
        health = Config.MaxHealth;

        GraphicController.Init(this);
        EquipmentController.Init(this);

        EquipmentController
            .WithWeapon(Config.DefaultWeapon)
            .WithShield(Config.DefaultShield)
            .WithArmor(Config.DefaultArmors);
    }

    public void Upgrade(int pathIndex)
    {
        if (CurrentUpgrade.upgradeIndex >= Config.UpgradePaths[pathIndex].Upgrades.Length) return;

        CurrentUpgrade = (pathIndex, CurrentUpgrade.upgradeIndex + 1);

        foreach (var strategy in Config.UpgradePaths[pathIndex].Upgrades[CurrentUpgrade.upgradeIndex].UpgradeStrategies)
        {
            strategy.ApplyUpgrade(this);
        }
    }
}
