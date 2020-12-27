using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffector : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip jumpSound, coinSound, loseSound, winSound;
   
    public void Play_jumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }

    public void Play_coinSound()
    {
        audioSource.PlayOneShot(coinSound);
    }

    public void Play_loseSound()
    {
        audioSource.PlayOneShot(loseSound);
    }

    public void Play_winSound()
    {
        audioSource.PlayOneShot(winSound);
    }
}
