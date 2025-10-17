using System.Runtime.CompilerServices;
using UnityEngine;

public class GameCell : MonoBehaviour
{
    public Hero Hero { get; private set; }

    public void PlaceHero(Hero hero, float positionY = 1.15f)
    {
        Hero = hero;
        Hero.transform.position = GameGrid.Instance.GetCellCenterPosition(this).WithY(positionY + transform.position.y);
        Hero.Init();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool CanPlaceHero()
    {
        return Hero == null;
    }
}
