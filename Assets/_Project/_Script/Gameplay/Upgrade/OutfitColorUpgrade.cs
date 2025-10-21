using System;
using UnityEngine;

[Serializable]
public class OutfitColorUpgrade : IUpgradeStrategy
{
    [SerializeField] private Color outfitColor;

    public void ApplyUpgrade(Hero hero)
    {
        hero.GraphicController.ChangeOutfitColor(outfitColor);
    }
}