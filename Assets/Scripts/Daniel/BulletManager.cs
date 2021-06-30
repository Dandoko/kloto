using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager
{
    //=========================================================================
    // General
    //=========================================================================
    private GunManager gunManager;
    private PortalManager portalManager;
    private Transform playerCamera;

    //=========================================================================
    // Bullet
    //=========================================================================
    private GameObject bulletGameObject;
    private const float speed = 50f;
    private RaycastHit bulletDest;
    private Material bulletMat;
    private int bulletType;
    private LayerMask bulletMask = LayerMask.NameToLayer("Bullet");

    //=========================================================================
    // Continuous Collision Detection
    //=========================================================================
    private const float skinWidth = 0.1f;  
    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;
    private Vector3 previousPosition;
    private Collider bulletCollider;

    public BulletManager(
        GunManager gunManager, PortalManager portalManager, GameObject bullet, Material bulletMat,
        Transform gunTip, RaycastHit bulletDest, Transform playerCamera, int bulletType)
    {
        this.gunManager = gunManager;
        this.portalManager = portalManager;
        this.bulletDest = bulletDest;
        this.bulletMat = bulletMat;
        this.playerCamera = playerCamera;
        this.bulletType = bulletType;

        // Creating the bullet
        bulletGameObject = bullet;
        bulletGameObject.GetComponent<MeshRenderer>().material = bulletMat;
        bulletGameObject.transform.position = gunTip.position;
        Vector3 bulletDir = (bulletDest.point - gunTip.position).normalized;
        bulletGameObject.transform.forward = bulletDir;
        bulletGameObject.AddComponent<Bullet>();

        bulletCollider = bulletGameObject.GetComponent<Collider>();
        previousPosition = bulletGameObject.transform.position;
        minimumExtent = Mathf.Min(Mathf.Min(bulletCollider.bounds.extents.x, bulletCollider.bounds.extents.y), bulletCollider.bounds.extents.z);
        partialExtent = minimumExtent * (1.0f - skinWidth);
        sqrMinimumExtent = minimumExtent * minimumExtent;
    }

    public void updateBullet()
    {
        if (null != bulletGameObject)
        {
            bulletGameObject.transform.position += bulletGameObject.transform.forward * speed * Time.deltaTime;
            continuousCollisionDetection();
        }
        else
        {
            gunManager.removeBullet(this);
            portalManager.createPortal(bulletDest, playerCamera, bulletMat, bulletType);
        }
    }

    private void continuousCollisionDetection()
    {
        Vector3 movementThisFrame = bulletGameObject.transform.position - previousPosition;
        float movementSqrMagnitude = movementThisFrame.sqrMagnitude;

        // Checking if the bullet moved more than the minimum extent
        if (movementSqrMagnitude > sqrMinimumExtent)
        {
            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
            RaycastHit hitObject;

            Vector3 dir = movementThisFrame.normalized;
            Debug.DrawRay(previousPosition, dir, Color.red, 10);


            // Check for objects that were missed 
            if (Physics.Raycast(previousPosition, movementThisFrame, out hitObject, movementMagnitude, bulletMask))
            {
                // If the raycast hit something, and the collided object is not an isTrigger object
                if (!hitObject.collider.isTrigger)
                {
                    // Move the bullet to where the collision occurred
                    bulletGameObject.transform.position = hitObject.point - (movementThisFrame / movementMagnitude) * partialExtent;
                }
            }
        }
        previousPosition = bulletGameObject.transform.position;
    }
}
