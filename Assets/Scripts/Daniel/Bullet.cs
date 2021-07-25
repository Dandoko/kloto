using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const int dyingTime = 3;

    // Start is called before the first frame update
    void Start()
    {
        // Destroys the bullet using a timer
        Destroy(gameObject, dyingTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(transform.parent.gameObject);
    }
}
