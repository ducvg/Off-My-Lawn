using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private FloatValueSO crystalValue;
    [SerializeField] private FloatValueSO levelProgressValue;
    [SerializeField] private LevelData levelData;
    private ColliderMap<Entity> entityColliderMap = new();
    public float levelTotalTime;
    public float levelTimer;

    public void Init()
    {
        crystalValue.Value = levelData.StartCrystal;
        levelTotalTime = 0;

        PreloadEntities();
        SetupLevelProgress();
        SetupDeckCards();

        WaveManager.Instance.Init(levelData.Waves);
    }

    public void Update()
    {
        if(GameManager.IsPause) return;
        levelTimer = Mathf.MoveTowards(levelTimer, levelTotalTime, Time.deltaTime);
        levelProgressValue.Value = Mathf.Lerp(0f, 1f, levelTimer / levelTotalTime);
    }

    public void OnLevelComplete()
    {
        
    }

    private void PreloadEntities()
    {
        HashSet<EntityID> uniqueEntityIDs = new();
        foreach (var wave in levelData.Waves)
        {
            foreach (var spawn in wave.MonsterSpawnData)
            {
                if (!uniqueEntityIDs.Add(spawn.EntityID)) continue;
                EntityFactory.Instance.PreloadEntity(spawn.EntityID);
            }
        }
    }

    void SetupLevelProgress()
    {
        var gameplayCanvas = UIManager.Instance.GetCanvas<GameplayCanvas>();
        gameplayCanvas.ClearFlags();
        int waveCount = levelData.Waves.Count;
        for (int i = 0; i < waveCount; i++)
        {
            levelTotalTime += levelData.Waves[i].WaveTime;
            if (!levelData.Waves[i].IsFlag) continue;

            float lerpFactor = (float)i / (waveCount - 1);
            gameplayCanvas.AddProgressFlag(lerpFactor);
        }
    }

    void SetupDeckCards()
    {
        var gameplayCanvas = UIManager.Instance.Open<GameplayCanvas>();
        foreach (var id in levelData.ForcedEntities)
        {
            var config = EntityFactory.Instance.GetEntityConfig(id);
            gameplayCanvas.AddCard(config);
        }
    }

    public void AddLevelTime(float additionalTime)
    {
        levelTotalTime += additionalTime;
    }

    public void RegisterEntityCollider(Collider collider, Entity entity)
    {
        entityColliderMap.Add(collider, entity);
    }
    public void UnregisterEntityCollider(Collider collider)
    {
        entityColliderMap.Remove(collider);
    }
    public bool TryGetEntityByCollider(Collider collider, out Entity entity)
    {
        return entityColliderMap.TryGetEntity(collider, out entity);
    }
}
