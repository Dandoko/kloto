using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{

    public List<GameObject> teleporters;

    // Start is called before the first frame update
    void Start()
    {
        teleporters = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            foreach (var x in teleporters)
            {
                Debug.Log(x.gameObject.name);
            }
        }

    }

}
