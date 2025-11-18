using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using ZLinq;

public class WaveManager : Singleton<WaveManager>
{
    HashSet<Entity> waveMonsters = new(); 
    List<WaveData> levelWaves;
    public int currentWaveIndex;
    public float waveTimer = 0f;

    public void Init(List<WaveData> levelWaves)
    {
        this.levelWaves = levelWaves;
        currentWaveIndex = 0;
        SpawnNextWave();
    }

    public void Update()
    {
        if(GameManager.GameState != GameState.Playing) return;

        waveTimer += Time.deltaTime;
        if (waveMonsters.Count == 0)
        {
            OnWaveCleared();
        }
        else if (currentWaveIndex < levelWaves.Count && waveTimer >= levelWaves[currentWaveIndex].WaveTime)
        {
            OnWaveTimeUp();
        }
    }

    void OnWaveTimeUp()
    {
        Debug.Log("Wave time up!");
        if (currentWaveIndex >= levelWaves.Count) return;
        SpawnNextWave();
    }

    void OnWaveCleared()
    {
        Debug.Log("Wave cleared!");
        if (currentWaveIndex >= levelWaves.Count)
        {
            LevelManager.Instance.OnLevelComplete();
            return;
        }
        
        float leftoverTime = levelWaves[currentWaveIndex].WaveTime - waveTimer;
        if (leftoverTime > 0)
        {
            LevelManager.Instance.SkipLevelTime(leftoverTime);
        }

        SpawnNextWave();
    }

    void SpawnNextWave()
    {
        SpawnWave(currentWaveIndex);
        currentWaveIndex++;
        waveTimer = 0f;
    }

    void SpawnWave(int waveIndex)
    {
        WaveData wave = levelWaves[waveIndex];
        var monsterSpawnCount = wave.SpawnDataList.Count;
        var weights = new List<int>();

        for(int i = 0; i < monsterSpawnCount; i++) //spawning forced spawn first
        {
            weights.Add(wave.SpawnDataList[i].PickWeight);
            var spawnData = wave.SpawnDataList[i];

            for (int j = 0; j < spawnData.MinSpawn; j++) //ignore spawn points limit
            {
                Entity spawnedEntity = EntityFactory.Instance.SpawnRandomRow(spawnData.EntityID);
                wave.WaveSpawnPoint -= spawnedEntity.Config.SpawnCost;
                waveMonsters.Add(spawnedEntity);
            }
        }

        int safe = 0;
        while (wave.WaveSpawnPoint > 0) //spawn by pick weight
        {
            if(safe++ > 1000)
            {
                Debug.LogError("Infinite loop in GetOrderedWaveMonsters");
                break;
            }

            int index = weights.GetRandomWeightedIndex();
            if (index == -1) continue;
            var spawnData = wave.SpawnDataList[index];
            if (spawnData.MaxSpawn <= 0)
            {
                weights.RemoveAt(index);
                continue;
            }

            Entity spawnedEntity = EntityFactory.Instance.SpawnRandomRow(spawnData.EntityID);
            wave.WaveSpawnPoint -= spawnedEntity.Config.SpawnCost;
            --spawnData.MaxSpawn;
            waveMonsters.Add(spawnedEntity);
        }
    }

    public void OnWaveMonsterDespawn(Entity monster)
    {
        waveMonsters.Remove(monster);
    }

    public void ClearAllMonsters()
    {
        foreach (var monster in waveMonsters)
        {
            EntityFactory.Instance.Release(monster);
        }
        waveMonsters.Clear();
    }
}
