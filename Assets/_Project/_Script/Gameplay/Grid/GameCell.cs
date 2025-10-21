using System.Runtime.CompilerServices;
using UnityEngine;

public class GameCell : MonoBehaviour
{
    public Entity Entity { get; private set; }

    public void Place(Entity entity, float offsetY = 1.15f)
    {
        Entity = entity;
        Entity.transform.position = GameGrid.Instance.GetCellCenterPosition(this).WithY(offsetY + transform.position.y);
        Entity.Init();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool CanPlace()
    {
        return Entity == null;
    }
}
