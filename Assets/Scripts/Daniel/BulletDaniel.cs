using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDaniel : MonoBehaviour
{
    private const int dyingTime = 2;
    private float speed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        // Temporary patch to destroy bullets after 5 seconds if they pass through walls without colliding
        // because there isn't continuous collision detection
        Destroy(gameObject, dyingTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
