using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCanvas : BaseCanvas
{
    [SerializeField] private Transform cardParent;
    [SerializeField] private CardFactory cardFactory;

    public void AddCard(HeroConfigSO heroConfig)
    {
        cardFactory.CreateCard(heroConfig, cardParent);
    }
}
