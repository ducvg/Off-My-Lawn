using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

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

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] private AudioConfig<MusicID>[] musicAus;
    [SerializeField] private AudioConfig<FxID>[] soundAus;

    [SerializeField] private AudioSource musicSource;
    private AudioSource[] soundSource = new AudioSource[Enum.GetNames(typeof(FxID)).Length];

    public void Init()
    {
        musicSource.loop = true;
        musicSource.volume = Data.User.Setting.IsMusicOn ? 1 : 0;
        if (musicAus.Length > 0)
        {
            PlayMusic(MusicID.BgMainMenu);
        }
    }

    public void PlayFx(FxID ID)
    {
        if (Data.User.Setting.IsSoundOn)
        {
            if (soundSource[(int)ID] == null)
            {
                soundSource[(int)ID] = new GameObject().AddComponent<AudioSource>();
                soundSource[(int)ID].loop = false;
                soundSource[(int)ID].transform.SetParent(transform);
            }
            soundSource[(int)ID].volume = 1;
            soundSource[(int)ID].PlayOneShot(soundAus[(int)ID].GetRandomClip());
        }
    }

    public void UpdateSoundVolume()
    {
        musicSource.volume = Data.User.Setting.IsMusicOn ? 1 : 0;
    }

    private void PlayMusic(MusicID ID)
    {
        AudioConfig<MusicID> audioConfig = musicAus[(int)ID];
        if (audioConfig == null) return;
        AudioClip newMusicAudioClip = audioConfig.GetRandomClip();
        musicSource.clip = newMusicAudioClip;
        musicSource.Play();
    }

    public async UniTaskVoid ChangeMusic(MusicID ID, float time)
    {
        musicSource.Play();

        float startVolume = Data.User.Setting.IsMusicOn ? 1 : 0;
        float elapsed = 0f;

        // Fade out
        while (elapsed < time)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / time);
            elapsed += Time.deltaTime;
            await UniTask.Yield(destroyCancellationToken);
        }

        musicSource.volume = 0f;
        PlayMusic(ID);

        // Fade in
        elapsed = 0f;
        while (elapsed < time)
        {
            musicSource.volume = Mathf.Lerp(0f, startVolume, elapsed / time);
            elapsed += Time.deltaTime;
            await UniTask.Yield(destroyCancellationToken);
        }

        musicSource.volume = startVolume;
    }

    public async UniTaskVoid StopMusic(float duration = 1f)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            musicSource.volume = Mathf.Lerp(musicSource.volume, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            await UniTask.Yield(destroyCancellationToken);
        }
        musicSource.Stop();
    }
    
}