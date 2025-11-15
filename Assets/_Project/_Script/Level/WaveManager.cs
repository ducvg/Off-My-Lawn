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
        else if (waveTimer >= levelWaves[currentWaveIndex].WaveTime)
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
        List<EntityConfigSO> entities = GetOrderedWaveMonsters(currentWaveIndex);
        // SpawnAtGraves(entities);
        SpawnRandomRows(entities);

        currentWaveIndex++;
        waveTimer = 0f;
    }

    public List<EntityConfigSO> GetOrderedWaveMonsters(int waveIndex)
    {
        WaveData wave = levelWaves[waveIndex];
        var configsToSpawn = new List<EntityConfigSO>();
        var monsterSpawnCount = wave.MonsterSpawnData.Length;
        var weights = new List<float>();

        for(int i = 0; i < monsterSpawnCount; i++) //spawning forced picks first
        {
            weights.Add(wave.MonsterSpawnData[i].PickWeight);
            var monster = wave.MonsterSpawnData[i];

            for (int j = 0; j < monster.ForcedPickCount; j++)
            {
                var config = EntityFactory.Instance.GetEntityConfig(monster.EntityID);
                configsToSpawn.Add(config);
                wave.WaveSpawnPoint -= config.SpawnCost;
            }
        }

        int safe = 0;
        while (wave.WaveSpawnPoint > 0)
        {
            if(safe++ > 1000)
            {
                Debug.LogError("Infinite loop in GetOrderedWaveMonsters");
                break;
            }

            int index = weights.GetRandomWeightedIndex();
            if (index == -1) continue;
            var monsterToSpawn = wave.MonsterSpawnData[index];
            if (monsterToSpawn.MaxInWave <= 0)
            {
                weights.RemoveAt(index);
                continue;
            }

            var config = EntityFactory.Instance.GetEntityConfig(monsterToSpawn.EntityID);
            configsToSpawn.Add(config);
            wave.WaveSpawnPoint -= config.SpawnCost;
            --monsterToSpawn.MaxInWave;
        }
        return configsToSpawn;
    }

    public void SpawnRandomRows(List<EntityConfigSO> spawnList)
    {
        foreach (var monster in spawnList)
        {
            var pos = new Vector3(
                GameConstant.GRID_BOUND_X_MAX + 0.5f + Random.Range(0, GameConstant.MONSTER_SPAWN_RANGE_X),
                GameConstant.LAWN_ELEVATION_Y,
                0.5f + GameGrid.Instance.GetRandomRowIndex()
            );
            Entity m = EntityFactory.Instance.Spawn(monster.Id, pos);
            m.transform.rotation = Quaternion.Euler(0f, -90f, 0f);

            waveMonsters.Add(m);
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
