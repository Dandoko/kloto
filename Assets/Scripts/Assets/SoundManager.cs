using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    private const float playerRunTimeDelay = 0.45f;
    private static Dictionary<Sounds, float> soundTimer = new Dictionary<Sounds, float>();

    public enum Sounds
    {
        PlayerRun,
        PlayerJump,
        PlayerLand,
        ShootGun,
        Portal
    }

    public static void playSound(Sounds sound)
    {
        if (canPlaySound(sound))
        {
            GameObject soundGameOjbect = new GameObject("Sound");
            AudioSource audioSource = soundGameOjbect.AddComponent<AudioSource>();
            AudioClip audioClip = getAudioClip(sound);

            if (Sounds.Portal == sound)
            {
                audioSource.clip = audioClip;
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                audioSource.PlayOneShot(audioClip);
                Object.Destroy(soundGameOjbect, audioClip.length + 0.2f);
            }

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

        if (Sounds.PlayerRun == sound)
        {
            if (soundTimer.ContainsKey(sound))
            {
                float lastTimePlayed = soundTimer[sound];
                if (lastTimePlayed + playerRunTimeDelay < Time.time)
                {
                    soundTimer[sound] = Time.time;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                soundTimer[sound] = Time.time;
                return true;
            }
        }
        else
        {
            return true;
        }
    }
}
