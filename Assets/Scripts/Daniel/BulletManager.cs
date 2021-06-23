using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager
{
    private GameObject bulletGameObject;
    private float speed = 30f;
    private GunManager gunManager;
    private PortalManager portalManager;
    private RaycastHit hitObject;
    private Material bulletMat;
    private Transform playerCamera;

    public BulletManager(
        GunManager gunManager, PortalManager portalManager, GameObject bullet, Material bulletMat,
        Transform gunTip, RaycastHit hitObject, Transform playerCamera)
    {
        this.gunManager = gunManager;
        this.portalManager = portalManager;
        this.hitObject = hitObject;
        this.bulletMat = bulletMat;
        this.playerCamera = playerCamera;

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
            portalManager.createPortal(hitObject, playerCamera, bulletMat);
        }
    }
}
