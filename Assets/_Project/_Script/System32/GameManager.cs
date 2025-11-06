using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdate
{
    void OnUpdate();
}
public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField] private Light directionalLight;
    HashSet<IUpdate> updates = new();
    HashSet<IUpdate> updatesToAdd = new();
    HashSet<IUpdate> updatesToRemove = new();
    private bool isPaused;

    public void SetGamePause(bool isPaused)
    {
        this.isPaused = isPaused;
    }

    void Update()
    {
        if (isPaused) return;
        var value = 8000 + Mathf.PingPong(Time.time * 150, 7000f); //575 secs a day
        directionalLight.colorTemperature = value;
        foreach (var u in updates)
        {
            u.OnUpdate();
        }
        ProjectileManager.Instance.OnUpdate();
    }

    void LateUpdate()
    {
        if (isPaused) return;

        foreach (var u in updatesToAdd)
        {
            updates.Add(u);
        }
        updatesToAdd.Clear();
            
        foreach (var u in updatesToRemove)
        {
            updates.Remove(u);
        }
        updatesToRemove.Clear();
    }

    public bool TryRegisterUpdate(IUpdate entity)
    {
        return updatesToAdd.Add(entity);
    }
    public bool TryUnregisterUpdate(IUpdate entity)
    {
        return updatesToRemove.Add(entity);
    }
}
