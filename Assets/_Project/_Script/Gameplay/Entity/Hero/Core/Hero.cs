using System.Collections;
using UnityEngine;

public class Hero : Entity
{
    public GameCell PlacedCell { get; private set; }
    public (int pathIndex, int upgradeIndex) CurrentUpgrade { get; private set; } = (-1, -1);

    public override void OnCellPlaced(GameCell cell)
    {
        base.OnCellPlaced(cell);
        PlacedCell = cell;
        ChangeState(new DropInState());
    }

    protected override void SetupGraphics()
    {
        GraphicController
            // .WithOverrideAnimation(Animation.MOVE, Config.MoveAnimation[Random.Range(0, Config.MoveAnimation.Length)])
            .WithOverrideAnimation(Animation.DIE, Config.DieAnimation)
            .ApplyAnimatorOverrides();
    }

    public override void Upgrade(int pathIndex)
    {
        CurrentUpgrade = (pathIndex, CurrentUpgrade.upgradeIndex + 1);

        foreach (var strategy in Config.UpgradePaths[pathIndex].Upgrades[CurrentUpgrade.upgradeIndex].UpgradeStrategies)
        {
            strategy.ApplyUpgrade(this);
        }
    }

    public override void Despawn()
    {
        base.Despawn();
        PlacedCell.OnEntityDespawn(this);
        PlacedCell = null;
        Destroy(gameObject);
    }
}
