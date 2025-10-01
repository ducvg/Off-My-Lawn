using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class AudioConfig<T> where T : Enum
{
    [field: SerializeField] public T AudioKey { get; private set; }
    [field: SerializeField] public AudioClip[] AudioClip { get; private set; }

    public AudioClip GetRandomClip()
    {
        return AudioClip[Random.Range(0, AudioClip.Length)];
    }
}