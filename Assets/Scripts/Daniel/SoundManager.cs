using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    private const float playerRunTimeDelay = 0.45f;
    private static Dictionary<Sounds, float> soundTImer = new Dictionary<Sounds, float>();

    public enum Sounds
    {
        PlayerRun,
        PlayerJump,
        ShootGun,
        BulletTravel,
        CreatePortal
    }

    public static void playSound(Sounds sound)
    {
        if (canPlaySound(sound))
        {
            GameObject soundGameOjbect = new GameObject("Sound");
            AudioSource audioSource = soundGameOjbect.AddComponent<AudioSource>();
            AudioClip audioClip = getAudioClip(sound);
            audioSource.PlayOneShot(audioClip);
            Object.Destroy(soundGameOjbect, audioClip.length + 0.2f);
        }
    }

    private static AudioClip getAudioClip(Sounds sound)
    {
        foreach (AssetManager.SoundAudioClip soundAudioCLip in AssetManager.instance.soundAudioClips)
        {
            if (soundAudioCLip.sound == sound)
            {
                return soundAudioCLip.audioClip;
            }
        }

        Debug.LogError("Sound: " + sound + " not found");
        return null;
    }

    private static bool canPlaySound(Sounds sound)
    {
        switch (sound)
        {
            case Sounds.PlayerRun:
                if (soundTImer.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTImer[sound];
                    if (lastTimePlayed + playerRunTimeDelay < Time.time)
                    {
                        soundTImer[sound] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    soundTImer[sound] = Time.time;
                    return true;
                }
            default:
                return true;
        }
    }
}
