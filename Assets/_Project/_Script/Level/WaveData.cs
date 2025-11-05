using System;

[Serializable]
public class WaveData
{
    public bool IsFlag = false;
    public float WaveTime = 15;  //seconds, next wave when defeat all or time up
    public int WaveSpawnPoint = 10;
    public SpawnData[] SpawnDatas;
}

[Serializable]
public class SpawnData
{
    public EntityID EntityID;
    public float PickWeight;
    public int ForcedPickCount;
    public int MaxInWave = 999;
}