using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sounds
    {
        PlayerRun,
        PlayerJump,
        ShootGun,
        BulletTravel,
        CreatePortal
    }

    public static void PlaySound()
    {
        GameObject soundGameOjbect = new GameObject("Sound");
        AudioSource audioSource = soundGameOjbect.AddComponent<AudioSource>();
        audioSource.PlayOneShot(AssetManager.instance.playerRun);
    }
}
