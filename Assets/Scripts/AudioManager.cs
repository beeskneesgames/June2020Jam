using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    private static AudioManager instance;

    public bool IntroPlaying { get; private set; }

    public static AudioManager Instance {
        get {
            return instance;
        }
    }

    public AudioMixerGroup mixerGroup;

    public Sound[] sounds;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.loop = sound.loop;

            sound.source.outputAudioMixerGroup = mixerGroup;
        }
    }

    public void PlayBtnHover() {
        Play("BtnHover");
    }

    public void PlayBtnClick() {
        Play("BtnClick");
    }

    public void Mute(string soundName) {
        Sound sound = Array.Find(sounds, item => item.name == soundName);
        CheckForSound(sound, soundName);

        sound.source.volume = 0.0f;
    }

    public void Unmute(string soundName) {
        Sound sound = Array.Find(sounds, item => item.name == soundName);
        CheckForSound(sound, soundName);

        sound.source.volume = 1.0f;
    }

    public void Play(string soundName) {
        Sound sound = Array.Find(sounds, item => item.name == soundName);
        CheckForSound(sound, soundName);

        if (soundName == "Intro") {
            IntroPlaying = true;
        }

        sound.source.volume = sound.volume * (1.0f + UnityEngine.Random.Range(-sound.volumeVariance * 0.5f, sound.volumeVariance * 0.5f));
        sound.source.pitch = sound.pitch * (1.0f + UnityEngine.Random.Range(-sound.pitchVariance * 0.5f, sound.pitchVariance * 0.5f));

        sound.source.Play();
    }

    public void Stop(string soundName) {
        Sound sound = Array.Find(sounds, item => item.name == soundName);
        CheckForSound(sound, soundName);

        if (soundName == "Intro") {
            IntroPlaying = false;
        }

        if (sound.source) {
            sound.source.Stop();
        }
    }

    private void CheckForSound(Sound sound, string soundName) {
        if (sound == null) {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
    }
}
