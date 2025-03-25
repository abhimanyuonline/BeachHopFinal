using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public Sound[] musicSounds, sfxSounds;

    public void PlayMusic(string name, bool loop = false)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not found: " + name);
        }
        else
        {
            s.source.clip = s.clip;
            s.source.loop = loop;
            s.source.volume = s.volume;
            s.source.Play();
        }

    }
    public void StopMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not found: " + name);
        }
        else
        {
            s.source.Stop();
        }
    }
    public void PlaySfx(string name, bool loop = false)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("sfxSounds Not found: " + name);
        }
        else
        {
            s.source.clip = s.clip;
            s.source.loop = loop;
            s.source.volume = s.volume;
            s.source.Play();
        }
    }
    public void StopSfx(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("sfxSounds Not found: " + name);
        }
        else
        {
            s.source.Stop();
        }
    }

    public void MuteMusic(bool value){
        foreach (var item in musicSounds)
        {
            item.source.mute = value;
        }
    }
     public void MuteSfx(bool value){
        foreach (var item in sfxSounds)
        {
            item.source.mute = value;
        }
    }

    public void PauseMusicSfx() {
        foreach (var item in musicSounds) {
            item.source.Pause();
        }

        foreach (var items in sfxSounds) {
            items.source.Pause();
        }
    }
    public void PlayMusicSfx() {
        foreach (var item in musicSounds)
        {
            item.source.UnPause();
        }

        foreach (var items in sfxSounds) {
            items.source.UnPause();
        }
    }

}
