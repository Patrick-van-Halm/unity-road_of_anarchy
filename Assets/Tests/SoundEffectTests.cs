using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class SoundEffectTests
{
    [Test]
    public void GetRandomClipEmptyArrayReturnsNull()
    {
        int expectedSoundAmount = 0;

        SoundEffect effect = ScriptableObject.CreateInstance<SoundEffect>();
        effect.Sounds = new AudioClip[] { };

        AudioClip clip = effect.GetRandomClip();

        Assert.AreEqual(expectedSoundAmount, effect.Sounds.Length);
        Assert.IsNull(clip);
    }

    [Test]
    public void GetRandomClipWithOneSoundReturnsOneSound()
    {
        int expectedSoundAmount = 1;

        SoundEffect effect = ScriptableObject.CreateInstance<SoundEffect>();
        effect.Sounds = new AudioClip[] { AudioClip.Create("TestClip", 1, 1, 1000, false) };

        AudioClip clip = effect.GetRandomClip();

        Assert.AreEqual(expectedSoundAmount, effect.Sounds.Length);
        Assert.IsNotNull(clip);
    }
}
