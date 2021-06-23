using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager
{
    private GameObject bulletGameObject;
    private float speed = 30f;
    private GunManager gunManager;

    public BulletManager(GunManager gunManager, GameObject bullet, Material bulletMat, Transform gunTip, RaycastHit hitObject)
    {
        this.gunManager = gunManager;

        // Creating the bullet
        bulletGameObject = bullet;
        bulletGameObject.GetComponent<MeshRenderer>().material = bulletMat;
        bulletGameObject.transform.position = gunTip.position;
        Vector3 bulletDir = (hitObject.point - gunTip.position).normalized;
        bulletGameObject.transform.forward = bulletDir;
        bulletGameObject.AddComponent<Bullet>();
    }

    public void updateBullet()
    {
        if (null != bulletGameObject)
        {
            bulletGameObject.transform.position += bulletGameObject.transform.forward * speed * Time.deltaTime;
        }
        else
        {
            gunManager.removeBullet(this);
        }
    }
}
