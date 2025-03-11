using UnityEngine;
using System;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public AudioSource source;
    public bool isOnloop = false;
    [Range(0.0f, 1.0f)] public float volume = 1.0f;
}
