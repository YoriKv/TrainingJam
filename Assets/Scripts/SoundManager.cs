using System.Collections;
using UnityEngine;

public class SoundManager:MonoBehaviour {
    // Audio source
    private AudioSource musicSrc;
    private AudioSource effectSrc;
    // Instance variable
    private static SoundManager instance;

    // Instance
    public static SoundManager I {
        get {
            if(instance == null && Application.isPlaying) {
                // Create sound manager
                instance = Instantiate(Resources.Load<SoundManager>("SoundManager"));
            }
            return instance;
        }
    }

    // Sound library
    public AudioClip[] coinSnds;

    // Volume
    public float MusicVolume {
        get { return musicSrc.volume; }
        set { musicSrc.volume = value; }
    }

    public float EffectVolume {
        get { return effectSrc.volume; }
        set { effectSrc.volume = value; }
    }

    // Init audio sources
    public void Awake() {
        if(instance != null) {
            Destroy(instance.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject); // Persist
        // Add audio sources
        musicSrc = gameObject.AddComponent<AudioSource>();
        effectSrc = gameObject.AddComponent<AudioSource>();
        // Set volumes
        EffectVolume = MusicVolume = 1f;
    }

    /*********************** EFFECTS & MUSIC ****************************/

    // Single Effect
    public void PlayEffectOnce(AudioClip snd, float vol = 1f) {
        effectSrc.PlayOneShot(snd, vol);
    }

    // Random Effect from list
    public void PlayEffectOnce(AudioClip[] snds, float vol = 1f) {
        if(snds.Length > 0)
            PlayEffectOnce(snds[Random.Range(0, snds.Length)], vol);
    }

    public void PlayedDelayedEffect(float delay, AudioClip snd, float vol = 1f) {
        StartCoroutine(PlayedDelayedEffectRoutine(delay, snd, vol));
    }

    public void PlayedDelayedEffect(float delay, AudioClip[] snd, float vol = 1f) {
        StartCoroutine(PlayedDelayedEffectRoutine(delay, snd, vol));
    }

    private IEnumerator PlayedDelayedEffectRoutine(float delay, AudioClip snd, float vol = 1f) {
        yield return new WaitForSeconds(delay);
        PlayEffectOnce(snd);
    }

    private IEnumerator PlayedDelayedEffectRoutine(float delay, AudioClip[] snd, float vol = 1f) {
        yield return new WaitForSeconds(delay);
        PlayEffectOnce(snd);
    }


    // Music
    public void PlayMusic(AudioClip snd, bool loop) {
        if(musicSrc.clip != snd || !musicSrc.isPlaying) {
            musicSrc.volume = 0.6f;
            musicSrc.clip = snd;
            musicSrc.loop = loop;
            musicSrc.Play();
        }
    }

    public void StopMusic() {
        musicSrc.Stop();
    }
}
