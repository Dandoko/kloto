using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // The bullet must travel fast enough to hit any surfaces before this time limit runs out
    private const int dyingTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        // Destroys the bullet using a timer
        Destroy(transform.parent.gameObject, dyingTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(transform.parent.gameObject);
    }
}
