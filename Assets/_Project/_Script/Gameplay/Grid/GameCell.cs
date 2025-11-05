using System.Runtime.CompilerServices;
using UnityEngine;

public class GameCell : MonoBehaviour
{
    public Entity Entity { get; private set; }

    public void Place(Entity entity, float offsetY = GameConstant.LAWN_ELEVATION_Y)
    {
        Entity = entity;
        entity.transform.parent = null;
        entity.OnCellPlaced(this);
    }

    public void OnEntityDespawn(Entity entity)
    {
        if (Entity == entity)
        {
            Entity = null;
        } else
        {
            Debug.LogError($"?? wrong entity despawned at cell", this);
        }
    }

    public bool CanPlace()
    {
        return Entity == null;
    }
}
