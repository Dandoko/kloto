using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDaniel : MonoBehaviour
{
    private const float speed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        // Temporary patch to destroy bullets after 5 seconds if they pass through walls without colliding
        // because there isn't continuous collision detection
        Destroy(gameObject, 5);
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
