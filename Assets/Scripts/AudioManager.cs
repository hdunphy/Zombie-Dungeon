using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] Sounds;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in Sounds)
        {
            s.AudioSource = gameObject.AddComponent<AudioSource>();
            s.AudioSource.clip = s.Clip;
            s.AudioSource.volume = s.Volume;
            s.AudioSource.pitch = s.Pitch;
            s.AudioSource.loop = s.Loop;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Sound s in Sounds)
        {
            if (s.PlayOnStart)
                s.AudioSource.Play();
        }
    }

    public void PlaySound(string name)
    {
        Sound _sound = Array.Find(Sounds, s => s.name == name);

        if (_sound != null)
        {
            _sound.AudioSource.Play();
        }
    }

    public IEnumerator PlayAndWaitForSound(string name, Action playAfterSound)
    {
        Sound _sound = Array.Find(Sounds, s => s.name == name);
        if (_sound != null)
        {
            _sound.AudioSource.Play();

            while (_sound.AudioSource.isPlaying)
            {
                yield return null;
            }
        }

        playAfterSound?.Invoke();
    }

    public void ToggleSound(string name, bool play)
    {
        Sound _sound = Array.Find(Sounds, s => s.name == name);

        if (_sound != null)
        {
            if (play)
            {
                _sound.AudioSource.Play();
            }
            else
            {
                _sound.AudioSource.Stop();
            }
        }
    }
}
