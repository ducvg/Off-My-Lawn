
public class Hero : Entity
{
    public GameCell PlacedCell { get; private set; }
    public (int pathIndex, int upgradeIndex) CurrentUpgrade { get; private set; } = (-1, -1);

    public void OnCellPlaced(GameCell cell)
    {
        PlacedCell = cell;
        ChangeState(new DropInState());
    }

    protected override void SetupGraphics()
    {
        GraphicController
            .WithOverrideAnimation(Animation.DIE, Config.DieAnimation)
            .ApplyAnimatorOverrides();
    }

    public void Upgrade(int pathIndex)
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
