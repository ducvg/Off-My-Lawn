using System;

[Serializable]
public class UserProfile
{
    public GameProfile Game = new();
    public ResourceProfile Resource = new();
    public SettingProfile Setting = new();
    public StatisticProfile Statistic = new();
}

[Serializable]
public class GameProfile
{
    public int CurrentLevel = 1;
}

[Serializable]
public class StatisticProfile
{
    // public DateTime LastPlayTime = new();
}

[Serializable]
public class ResourceProfile
{
    public int Heart = 3;
    public DateTime LastHeartRegenTime = new();
    public int Gold = 420;
}

[Serializable]
public class SettingProfile
{
    public bool IsMusicOn = true;
    public bool IsSoundOn = true;
    public bool IsVibrationOn = true;

    public string CurrentLanguageCode = "en";
}