using System.Collections;
using System.Collections.Generic;

public class GameManager : PersistentSingleton<GameManager>
{
    public static GameState GameState { get; private set; }

    void Start()
    {
        CameraManager.Instance.Init();
        LevelManager.Instance.Init(1);
    }

    public void SetGameState(GameState newState)
    {
        GameState = newState;
    }   
}

public enum GameState
{
    SelectCard,
    Playing,
    Paused,
}