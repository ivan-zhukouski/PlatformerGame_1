using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffector : MonoBehaviour
{
    public AudioSource soundsAudioSource, musicAudioSource;
    public AudioClip jumpSound, coinSound, loseSound, winSound, starSound;

    void Start()
    {
        soundsAudioSource.volume = (float)PlayerPrefs.GetInt("sVolume") / 9;
        musicAudioSource.volume = (float)PlayerPrefs.GetInt("mVolume") / 9;
    }

    public void Play_jumpSound()
    {
        soundsAudioSource.PlayOneShot(jumpSound);
    }

    public void Play_coinSound()
    {
        soundsAudioSource.PlayOneShot(coinSound);
    }

    public void Play_loseSound()
    {
        soundsAudioSource.PlayOneShot(loseSound);
    }

    public void Play_winSound()
    {
        soundsAudioSource.PlayOneShot(winSound);
    }
    public void Play_starSound()
    {
        soundsAudioSource.PlayOneShot(starSound);
    }
}
