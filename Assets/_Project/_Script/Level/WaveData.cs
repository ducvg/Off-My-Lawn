using System;

[Serializable]
public class WaveData
{
    public bool IsFlag = false;
    public float WaveTime = 15;
    public int WaveSpawnPoint = 10;
    public SpawnData[] MonsterSpawnData;
}

[Serializable]
public class SpawnData
{
    public EntityID EntityID;
    public float PickWeight = 1f;
    public int ForcedPickCount;
    public int MaxInWave = 999;
}