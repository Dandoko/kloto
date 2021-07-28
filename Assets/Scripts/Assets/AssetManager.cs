using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    private static AssetManager _i;

    public SoundAudioClip[] soundAudioClips;

    public static AssetManager instance
    {
        get
        {
            if (null == _i)
            {
                _i = Instantiate(Resources.Load<AssetManager>("AssetManager"));
            }
            return _i;
        }
    }

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sounds sound;
        public AudioClip audioClip;
    }
}
