using System.Collections.Generic;
using UnityEngine;
using ZLinq;

public class WaveManager : Singleton<WaveManager>, IUpdate
{
    public List<Entity> WaveMonsters { get; private set; }
    List<WaveData> levelWaves;
    public int currentWaveIndex;
    public float waveTimer = 0f;

    public void Init(List<WaveData> levelWaves)
    {
        GameManager.Instance.TryRegisterUpdate(this);
        WaveMonsters = new();
        this.levelWaves = levelWaves;

        SpawnNextWave();
    }

    public void OnUpdate()
    {
        waveTimer += Time.deltaTime;
        if (waveTimer >= levelWaves[currentWaveIndex].WaveTime)
        {
            OnWaveTimeUp();
        }
        else if (WaveMonsters.Count == 0)
        {
            OnWaveCleared();
        }
    }

    void OnWaveTimeUp()
    {
        if (currentWaveIndex + 1 >= levelWaves.Count) return;
        SpawnNextWave();
    }

    void OnWaveCleared()
    {
        if(currentWaveIndex + 1 >= levelWaves.Count)
        {
            GameManager.Instance.TryUnregisterUpdate(this);
            LevelManager.Instance.OnLevelComplete();
            return;
        }
        SpawnNextWave();
        float leftoverTime = waveTimer - levelWaves[currentWaveIndex - 1].WaveTime;
        if(leftoverTime > 0)
        {
            LevelManager.Instance.AddLevelTime(-leftoverTime);
        }
    }

    void SpawnNextWave()
    {
        
        List<EntityConfigSO> entities = GetWaveMonsters(currentWaveIndex);
        // SpawnAtGraves(entities);
        SpawnRandomRows(entities);

        currentWaveIndex++;
        waveTimer = 0f;
    }

    public List<EntityConfigSO> GetWaveMonsters(int waveIndex)
    {
        WaveData wave = levelWaves[waveIndex];
        var configsToSpawn = new List<EntityConfigSO>();

        foreach (var monster in wave.MonsterSpawnData) //spawn forced pick first
        {
            for (int i = 0; i < monster.ForcedPickCount; i++)
            {
                var config = EntityFactory.Instance.GetEntityConfig(monster.EntityID);
                configsToSpawn.Add(config);
                wave.WaveSpawnPoint -= config.SpawnCost;
            }
        }

        int safeLock = 10_000;
        var weights = wave.MonsterSpawnData.AsValueEnumerable()
            .Select(m => m.PickWeight).ToList();
        while (wave.WaveSpawnPoint > 0 && --safeLock > 0)
        {
            int index = weights.GetRandomWeightedIndex();
            if (index == -1) continue;
            if (wave.MonsterSpawnData[index].MaxInWave <= 0)
            {
                weights.RemoveAt(index);
                continue;
            }

            var config = EntityFactory.Instance.GetEntityConfig(wave.MonsterSpawnData[index].EntityID);
            configsToSpawn.Add(config);
            wave.WaveSpawnPoint -= config.SpawnCost;
            --wave.MonsterSpawnData[index].MaxInWave;
        }
        return configsToSpawn;
    }

    private void SpawnRandomRows(List<EntityConfigSO> waveMonsters)
    {
        foreach (var monster in waveMonsters)
        {
            var pos = new Vector3(
                GameConstant.GRID_BOUND_X_MAX + 0.5f + Random.Range(0, GameConstant.MONSTER_SPAWN_RANGE_X),
                GameConstant.LAWN_ELEVATION_Y,
                0.5f + GameGrid.Instance.GetRandomRowIndex()
            );
            Entity m = EntityFactory.Instance.Spawn(monster.Id, pos);
            m.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            m.ChangeState(new WalkState());

            WaveMonsters.Add(m);
        }
    }

    public void OnWaveMonsterDespawn(Entity monster)
    {
        WaveMonsters.Remove(monster);
    }

    void OnDestroy()
    {
        if(!GameManager.Instance) return;
        GameManager.Instance.TryUnregisterUpdate(this);
    }
}
