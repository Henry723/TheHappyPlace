﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceMixer : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] ambienceClips;
    private bool keepFadingIn;
    private bool keepFadingOut;

    void Start()
    {
        source = this.GetComponent<AudioSource>();
    }

    public void FadeIn(int track, float speed, float maxVolume)
    {
        StartCoroutine(FadeInRoutine(track, speed, maxVolume));
    }

    public void FadeOut(float speed)
    {
        StartCoroutine(FadeOutRoutine(speed));
    }

    public void ChangeMusic(int newTrack, float speed, float maxVolume)
    {
        StartCoroutine(ChangeMusicRoutine(newTrack, speed, maxVolume));
    }

    public void StartGameAmbience()
    {
        Loop();
        PlayClip(0, 0.1f);
    }

    public void PlayClip(int track, float volume)
    {
        source.volume = volume;
        source.clip = ambienceClips[track];
        source.Play();
    }

    public void PlayOneClip(int newTrack, float volume)
    {
        source.PlayOneShot(ambienceClips[newTrack], volume);
    }

    public void StopClip()
    {
        source.Stop();
    }

    public void Loop()
    {
        source.loop = true;
    }

    private IEnumerator FadeInRoutine(int track, float speed, float maxVolume)
    {
        keepFadingIn = true;
        keepFadingOut = false;
        source.volume = 0;
        source.clip = ambienceClips[track];
        source.Play();
        float audioVolume = source.volume;

        while (source.volume < maxVolume && keepFadingIn)
        {
            audioVolume += speed;
            source.volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }

        if(source.volume < speed && keepFadingOut)
        {
            source.Stop();
        }
    }
    
    private IEnumerator FadeOutRoutine(float speed)
    {
        keepFadingIn = false;
        keepFadingOut = true;
        float audioVolume = source.volume;

        while (source.volume >= speed && keepFadingOut)
        {
            audioVolume -= speed;
            source.volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator ChangeMusicRoutine(int newTrack, float speed, float maxVolume)
    {
        FadeOut(speed);

        while(source.volume >= speed)
        {
            yield return new WaitForSeconds(0.01f);
        }

        source.Stop();
        Debug.Log("Playing track: " + newTrack);
        if (ambienceClips != null)
        {
            FadeIn(newTrack, speed, maxVolume);
        }
    }
}
