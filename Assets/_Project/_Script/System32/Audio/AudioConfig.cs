using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public enum MusicID
{
    BgMainMenu,
    BgGamePlay
}

[Serializable]
public enum FxID
{
    SfxDropSkewer,
    SfxClearSkewer,
    SfxPickSkewer,

    SfxUIClickBtn,
    SfxUIWinGame,
    SfxUILoseGame,
    SfxUIRewardClaim,
    SfxUICoinClaim,
    SfxGasTorch,
    SfxIceBreak
}

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