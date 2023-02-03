using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    private static GameObject soundObject;
    private static AudioSource audioSource;

    public static void PlaySound(AudioClip sound, float volume){
        if (soundObject == null){
            soundObject = new GameObject("SoundObject");
            audioSource = soundObject.AddComponent<AudioSource>();
        }
        audioSource.PlayOneShot(sound, volume);
    }

    public static void PlayMusic(AudioClip music){
        GameObject musicObject = new GameObject("MusicObject");
        AudioSource musicSource = musicObject.AddComponent<AudioSource>();
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.volume = 1f;
        musicSource.Play();
    }
}
