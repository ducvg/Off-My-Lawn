using System.Collections;
using System.Collections.Generic;

public class GameManager : PersistentSingleton<GameManager>
{
    public static GameState GameState { get; private set; }

    void Start()
    {
        var levelData = LoadLevel(0);
        CameraManager.Instance.Init();
        LevelManager.Instance.Init(levelData);
    }

    public void SetGameState(GameState newState)
    {
        GameState = newState;
    }   

    LevelData LoadLevel(int levelIndex)
    {
        return null;
    }
}

public enum GameState
{
    SelectCard,
    Playing,
    Paused,
}