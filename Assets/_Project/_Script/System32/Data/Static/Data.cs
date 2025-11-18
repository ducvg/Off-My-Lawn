using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class Data
{
    public static UserProfile User;
    private static bool isCorrupted = false;

    public static async UniTask SaveAsync(bool encrypt = false)
    {
        if (isCorrupted)
        {
            Debug.LogError("Game data is corrupted, save disable.");
            return;
        }

        var path = Application.persistentDataPath + "/save.sav";
        await UniTask.RunOnThreadPool(() => SaveService.SaveLocal(path, User, encrypt));
    }

    public static void Save(bool encrypt = false)
    {
        if (isCorrupted)
        {
            Debug.LogError("Game data is corrupted, save disable.");
            return;
        }

        var path = Application.persistentDataPath + "/save.sav";
        SaveService.SaveLocal(path, User, encrypt);
    }

    public static async UniTask LoadAsync(bool encrypt = false)
    {
        try
        {
            var path = Application.persistentDataPath + "/save.sav";
            User = await UniTask.RunOnThreadPool(() => SaveService.LoadLocal<UserProfile>(path, encrypt, ref isCorrupted));
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load game data: " + e.Message);
        }
    }

#if UNITY_EDITOR
    const string levelPath = "Assets/_Project/Resources/Levels";
    public static LevelData LoadLevel(int levelIndex)
    {
        string fileName = $"Level_{levelIndex}.json";
        var path = Path.Combine(levelPath, fileName);
        string json = File.ReadAllText(path);
        var loadedLevel = JsonUtility.FromJson<LevelData>(json);
        Debug.Log($"Loaded level {loadedLevel.LevelIndex} at: {path}");
        return loadedLevel;
    }

    public static void SaveLevel(LevelData saveLevel)
    {
        string fileName = $"Level_{saveLevel.LevelIndex}.json";
        var path = Path.Combine(levelPath, fileName);
        string json = JsonUtility.ToJson(saveLevel);
        File.WriteAllText(path, json);
        AssetDatabase.ImportAsset(path);
        Debug.Log($"Saved Level {saveLevel.LevelIndex} to: " + path);
    }
#endif
}