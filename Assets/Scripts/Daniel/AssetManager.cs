using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    private static AssetManager _i;

    public AudioClip playerRun;

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
}
