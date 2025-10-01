using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public static class SaveService
{
    private static string GetKey() => "rLmiasAMPNLCQ21TqeBd/BtkmsokhVy/3YCHpCXthTM=";
    private static string GetIV() => "4/BnWPU2Xcv7yHiBiHtgCA==";

    public static bool SaveLocal<T>(string path, T Data, bool isEncrypted)
    {
        var backupPath = Path.ChangeExtension(path, ".bak");
        try
        {
            if (File.Exists(path))
            {
                Debug.Log("Save file existed, creating a backup...");
                if (File.Exists(backupPath)) //delete previous backup
                {
                    File.Delete(backupPath);
                }
                File.Copy(path, backupPath); // Backup the existing save
            }
            else
            {
                Debug.Log("Creating save file...");
            }
            using FileStream stream = File.Create(path);
            if (isEncrypted)
            {
                WriteEncryptedData(Data, stream);
            }
            else
            {
                stream.Close();
                File.WriteAllText(path, JsonConvert.SerializeObject(Data, Formatting.Indented));
            }

            Debug.Log(path);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Save failed: {e.Message}\n{e.StackTrace}");
            return false;
        }
    }

    private static void WriteEncryptedData<T>(T data, FileStream stream)
    {
        using Aes aesProvider = Aes.Create();

        // Debug.Log("Mode: " + aesProvider.Mode);
        // Debug.Log("Padding: " + aesProvider.Padding);
        // Debug.Log($"IV: {Convert.ToBase64String(aesProvider.IV)}");
        // Debug.Log($"Key: {Convert.ToBase64String(aesProvider.Key)}");

        aesProvider.Key = Convert.FromBase64String(GetKey());
        aesProvider.IV = Convert.FromBase64String(GetIV());
        using ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor(aesProvider.Key, aesProvider.IV);
        using CryptoStream cryptoStream = new(stream, cryptoTransform, CryptoStreamMode.Write);

        cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));
    }

    public static T LoadLocal<T>(string path, bool isEncrypted, ref bool isCorrupted) where T : class
    {
        try
        {
            if (!File.Exists(path))
            {
                Debug.LogWarning($"No save file at {path}");
                return null;
            }

            T data;
            if (isEncrypted)
            {
                data = ReadEncryptedData<T>(path);
            }
            else
            {
                data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            }
            if (data == null)
            {
                Debug.LogError($"data null, maybe failed to deserialize data from {path}");
                return null;
            }
            Debug.Log("Load success");
            return data;
        }
        catch (Exception e)
        {
            File.Delete(path); //delete the corrupted file
            isCorrupted = true;
            throw new Exception($"Failed to load game data: {e.Message}, \n {e.StackTrace}", e);
        }
    }

    private static T ReadEncryptedData<T>(string relativePath)
    {
        byte[] bytes = File.ReadAllBytes(relativePath);
        using Aes aesProvider = Aes.Create();

        aesProvider.Key = Convert.FromBase64String(GetKey());
        aesProvider.IV = Convert.FromBase64String(GetIV());

        using ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(aesProvider.Key, aesProvider.IV);
        using MemoryStream memoryStream = new(bytes);
        using CryptoStream cryptoStream = new(memoryStream, cryptoTransform, CryptoStreamMode.Read);
        using StreamReader streamReader = new(cryptoStream);

        string result = streamReader.ReadToEnd();
        Debug.Log(result);
        return JsonConvert.DeserializeObject<T>(result);
    }
}