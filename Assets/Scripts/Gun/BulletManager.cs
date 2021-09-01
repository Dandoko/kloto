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

    //=========================================================================
    // Bullet
    //=========================================================================
    private GameObject bulletGameObject;
    private const float speed = 60f;
    private int bulletType;
    // Note: Different from [SerializedField] LayerMask bulletMask
    private LayerMask bulletMask = LayerMask.NameToLayer("Bullet"); // Decimal value

    //=========================================================================
    // Continuous Collision Detection
    //=========================================================================
    private const float skinWidth = 0.1f;  
    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;
    private Vector3 previousPosition;
    private Rigidbody bulletRigidbody;
    private Collider bulletCollider;
    private int allLayersExceptBullet;

    public BulletManager (
        GunManager gunManager, PortalManager portalManager, GameObject bullet,
        Transform gunTip, RaycastHit bulletDest, int bulletType)
    {
        this.gunManager = gunManager;
        this.portalManager = portalManager;
        this.bulletType = bulletType;

        // Creating the bullet
        bulletGameObject = bullet;
        bulletGameObject.transform.position = gunTip.position;
        Vector3 bulletDir = (bulletDest.point - gunTip.position).normalized;
        bulletGameObject.transform.forward = bulletDir;
        bulletGameObject.transform.GetChild(0).gameObject.AddComponent<Bullet>();

        bulletRigidbody = bulletGameObject.transform.GetChild(0).GetComponent<Rigidbody>();
        bulletCollider = bulletGameObject.transform.GetChild(0).GetComponent<Collider>();
        previousPosition = bulletRigidbody.position;
        minimumExtent = Mathf.Min(Mathf.Min(bulletCollider.bounds.extents.x, bulletCollider.bounds.extents.y), bulletCollider.bounds.extents.z);
        partialExtent = minimumExtent * (1.0f - skinWidth);
        sqrMinimumExtent = minimumExtent * minimumExtent;
        int bulletLayerMask = 1 << bulletMask.value;
        allLayersExceptBullet =~ bulletLayerMask;
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
            gunManager.removeBullet();
            portalManager.instatiatePortal(bulletType);
        }
    }

    private void continuousCollisionDetection()
    {
        Vector3 movementThisFrame = bulletRigidbody.position - previousPosition;
        float movementSqrMagnitude = movementThisFrame.sqrMagnitude;

        // Checking if the bullet moved more than the minimum extent
        if (movementSqrMagnitude > sqrMinimumExtent)
        {
            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
            RaycastHit hitObject;

            // Check for objects that were missed 
            if (Physics.Raycast(previousPosition, movementThisFrame, out hitObject, movementMagnitude, allLayersExceptBullet))
            {
                // If the raycast hit something, and the collided object is not an isTrigger object
                if (!hitObject.collider.isTrigger)
                {
                    // Move the bullet to where the collision occurred
                    bulletGameObject.transform.position = hitObject.point - (movementThisFrame / movementMagnitude) * partialExtent;
                }
            }
        }

        previousPosition = bulletRigidbody.position;
    }
}
