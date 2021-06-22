using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDaniel : MonoBehaviour
{
    private GameObject bullet;
    private const int dyingTime = 2;
    private float speed = 30f;

    // Start is called before the first frame update
    //private void Start()
    //{
    //    // Temporary patch to destroy bullets after 5 seconds if they pass through walls without colliding
    //    // because there isn't continuous collision detection
    //    Destroy(gameObject, dyingTime);
    //}

    //public BulletDaniel(GameObject bulletPrefab, Material bulletMat, Transform gunTip, RaycastHit hitObject)
    //{
    //    // Creating the bullet
    //    bullet = Instantiate(bulletPrefab);
    //    bullet.GetComponent<MeshRenderer>().material = bulletMat;
    //    bullet.transform.position = gunTip.position;
    //    Vector3 bulletDir = (hitObject.point - gunTip.position).normalized;
    //    bullet.transform.forward = bulletDir;

    //    // Temporary patch to destroy bullets after 5 seconds if they pass through walls without colliding
    //    // because there isn't continuous collision detection
    //    Destroy(bullet, dyingTime);
    //}

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    public void updateItem()
    {
        
    }
}
