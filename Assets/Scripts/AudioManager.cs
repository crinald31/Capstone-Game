using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource level;
    public AudioSource gameOver;
    public AudioSource victory;

    public AudioSource[] sounds;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    public void PlayGameOver()
    {
        level.Stop();
        gameOver.Play();
    }

    public void PlayVictory()
    {
        level.Stop();
        victory.Play();
    }

    public void PlaySFX(int sfx)
    {
        sounds[sfx].Stop();
        sounds[sfx].Play();
    }
}
