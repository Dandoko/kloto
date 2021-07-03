using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunManager : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Material bulletBlueMat;
    [SerializeField] private Material bulletRedMat;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Image crosshair;
    [SerializeField] private PortalManager portalManager;
    
    private Camera playerCamera;
    private Transform gunTip;
    private const float gunRange = 100f;
    private float shootingTime;
    private const float shootingInterval = 0.7f;
    private Color canShootColor;
    private Color cannotShootColor;

    private List<BulletManager> bullets;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        gunTip = transform.GetChild(0);

        shootingTime = 0f;

        canShootColor = new Color(1, 0, 0, 1);
        cannotShootColor = new Color(1, 0.3f, 0.6f, 0.5f);

        bullets = new List<BulletManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool canShoot = false;

       // Checking if the raycast hit an object
        RaycastHit hitObject;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitObject, gunRange))
        {
            crosshair.color = canShootColor;

            // Checking if the shooting delay is over
            if (shootingTime <= Time.time)
            {
                // Checking if a portal can be created on the surface the raycast hit
                if (canCreatePortal(hitObject))
                {
                    canShoot = true;

                    if (Input.GetButtonDown("Fire1"))
                    {
                        shootGun("Fire1", hitObject, bulletBlueMat, 1);
                    }
                    else if (Input.GetButtonDown("Fire2"))
                    {
                        shootGun("Fire2", hitObject, bulletRedMat, 2);
                    }
                }
            }
        }

        if (canShoot)
        {
            crosshair.color = canShootColor;
        }
        else
        {
            crosshair.color = cannotShootColor;
        }

        // Shallow copying the bullets list to fix the error: "Collection was modified, enumeration operation may not execute"
        List<BulletManager> bulletCopy = bullets.GetRange(0, bullets.Count);
        foreach (var bullet in bulletCopy)
        {
            bullet.updateBullet();
        }
    }

    private void shootGun(string fireMouseClick, RaycastHit hitObject, Material bulletMat, int bulletType)
    {
        muzzleFlash.Play();
        shootingTime = Time.time + shootingInterval;

        // Creating the bullet
        GameObject newBulletObject = Instantiate(bulletPrefab);
        BulletManager bullet = new BulletManager(this, portalManager, newBulletObject, bulletMat, gunTip, hitObject, playerCamera.transform, bulletType);
        bullets.Add(bullet);
    }

    private bool canCreatePortal(RaycastHit hitObject)
    {
        if (isAimingAtPortal(hitObject))
        {
            return false;
        }

        return true;
    }

    public void removeBullet(BulletManager bullet)
    {
        bullets.Remove(bullet);
    }

    private bool isAimingAtPortal(RaycastHit hitObject)
    {
        if (portalManager.isPortal(hitObject.collider.gameObject))
        {
            return true;
        }

        return false;
    }
}
