using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public List<Sound> sounds = new List<Sound>();
    public List<AudioSource> UISounds = new List<AudioSource>();
    public List<AudioSource> effectsSounds = new List<AudioSource>();
    public List<AudioSource> musicSounds = new List<AudioSource>();
    public float musicVolume = 0.5f;
    public float effectsVolume = 0.5f;
    public float UIVolume = 0.5f;
    private float masterVolume = 0.5f;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();

            sound.Initialize(audioSource);

            switch (sound.type)
            {
                case "UI":
                    UISounds.Add(audioSource);

                    break;
                case "Effects":
                    effectsSounds.Add(audioSource);

                    break;
                case "Music":
                    audioSource.loop = true;

                    musicSounds.Add(audioSource);

                    break;
                default:
                    break;
            }
        }

        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetEffectsVolume(effectsVolume);
        SetUIVolume(UIVolume);

        PlayMusic();
    }

    private void Start()
    {
        SetMasterVolume(1);
    }

    public void Play(int id)
    {
        Sound sound = sounds.Find(sound => sound.id == id);
        sound.audioSource.Play();
    }

    public void Play(int id, float pitch)
    {
        Sound sound = sounds.Find(sound => sound.id == id);
        sound.audioSource.pitch = pitch;
        sound.audioSource.Play();
    }

    public void Play(int id, float minPitchVariance, float maxPitchVariance)
    {
        Sound sound = sounds.Find(sound => sound.id == id);
        float pitchVariance = Random.Range(minPitchVariance, maxPitchVariance);
        sound.audioSource.pitch = sound.initialPitch;
        sound.audioSource.pitch += pitchVariance;
        sound.audioSource.Play();
    }

    public void PlayMusic()
    {
        Debug.Log("PlayingMusc");

        Play(-1);
    }

    public void ChangePitch(int id, float pitch)
    {
        Sound sound = sounds.Find(sound => sound.id == id);
        sound.audioSource.pitch = pitch;
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;

        SetUIVolume(UIVolume);
        SetEffectsVolume(effectsVolume);
        SetMusicVolume(musicVolume);
    }

    public void SetUIVolume(float volume)
    {
        UIVolume = volume;

        foreach (AudioSource audioSource in UISounds)
        {
            audioSource.volume = (volume * 0.5f) * masterVolume;
        }
    }

    public void SetEffectsVolume(float volume)
    {
        effectsVolume = volume;

        foreach (AudioSource audioSource in effectsSounds)
        {
            audioSource.volume = (volume * 0.5f) * masterVolume;
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;

        foreach (AudioSource audioSource in musicSounds)
        {
            audioSource.volume = (volume * 0.5f) * masterVolume;
        }
    }
}
