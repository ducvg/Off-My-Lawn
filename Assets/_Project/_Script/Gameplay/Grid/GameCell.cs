using UnityEngine;

public class GameCell : MonoBehaviour
{
    public Hero Hero { get; private set; }

    public void PlaceHero(Hero hero, float offsetY = GameConstant.LAWN_ELEVATION_Y)
    {

        Hero = hero;
        hero.transform.parent = null;
        hero.OnCellPlaced(this);
    }

    public void OnEntityDespawn(Hero hero)
    {
        if (Hero == hero)
        {
            Hero = null;
        } else
        {
            Debug.LogError($"?? wrong entity despawned at cell", this);
        }
    }

    public bool CanPlace()
    {
        return Hero == null;
    }
}
