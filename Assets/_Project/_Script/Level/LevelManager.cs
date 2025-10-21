using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LevelData levelData;
    public GameplayCanvas gameplayCanvas;

    void Start()
    {
        // GameplayCanvas gameplayCanvas = UIManager.Instance.GetCanvas<GameplayCanvas>();
        gameplayCanvas.SetMoneyText(levelData.StartMoney);
        foreach (var heroConfig in levelData.ForcedHeroes)
        {
            gameplayCanvas.AddCard(heroConfig);
        }
    }
}
