using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = nameof(SoundEffect))]
public class SoundEffect : ScriptableObject
{
    public AudioClip[] Sounds;

    public AudioClip GetRandomClip()
    {
        if (Sounds.Length == 0 || Sounds is null)
            return null;

        return Sounds[Random.Range(0, Sounds.Length)];
    }
}
