using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const int dyingTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        // Temporary patch to destroy bullets after 5 seconds if they pass through
        // walls without because there isn't continuous collision detection
        Destroy(gameObject, dyingTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
