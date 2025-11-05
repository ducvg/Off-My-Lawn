using System;
using Cysharp.Threading.Tasks;
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
}