using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("SFX Clips")]
    public AudioClip playerShoot;
    public AudioClip bulletImpact;
    public AudioClip tankDestroyed;
    public AudioClip powerUpPickup;
    public AudioClip powerUpAppear;
    public AudioClip eagleDestroyed;
    public AudioClip stageClear;
    public AudioClip gameOver;
    public AudioClip buttonClick;
    public AudioClip pauseToggle;

    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;
    public AudioClip gameCompleteJingle;

    private AudioSource sfxSource;
    private AudioSource musicSource;

    private float sfxVolume = 1f;
    private float musicVolume = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        sfxSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;

        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        ApplyVolumes();
    }

    void ApplyVolumes()
    {
        sfxSource.volume = sfxVolume;
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        sfxSource.volume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        musicSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public float GetSFXVolume() => sfxVolume;
    public float GetMusicVolume() => musicVolume;

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayButtonClick()
    {
        PlaySFX(buttonClick);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource.clip == clip) return;
        musicSource.loop = true;
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayJingle(AudioClip clip)
    {
        musicSource.loop = false;
        musicSource.clip = clip;
        musicSource.Play();
    }
}