using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField] private Light directionalLight;
    public static bool IsPause { get; private set; }

    void Start()
    {
        LoadLevel(0);
    }

    void Update()
    {
        var value = 8000 + Mathf.PingPong(Time.time * 150, 7000f); //575 secs a day
        directionalLight.colorTemperature = value;
    }

    public void SetGamePause(bool isPause)
    {
        IsPause = isPause;
    }

    void LoadLevel(int levelIndex)
    {

        LevelManager.Instance.Init();
    }
}
