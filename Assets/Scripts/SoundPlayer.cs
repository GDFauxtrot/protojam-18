using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour {

    AudioSource audio;

    void Awake() {
        audio = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip, float volume) {
        audio.clip = clip;
        audio.volume = volume;
        audio.Play();
        Destroy(gameObject, clip.length);
    }
}
