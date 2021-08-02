using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunManager : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefabBlue;
    [SerializeField] private GameObject bulletPrefabRed;
    [SerializeField] private GameObject muzzleFlashBlue;
    [SerializeField] private GameObject muzzleFlashRed;
    [SerializeField] private Image crosshair;
    [SerializeField] private PortalManager portalManager;
    
    private Camera playerCamera;
    private Transform gunTip;
    private const float gunRange = 100f;
    private float shootingTime;
    private const float shootingInterval = 0.7f;
    private Color canShootColor;
    private Color cannotShootColor;
    private BulletManager bulletManager;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        gunTip = transform.GetChild(0);

        shootingTime = 0f;

        canShootColor = new Color(1, 0, 0, 1);
        cannotShootColor = new Color(1, 0.3f, 0.6f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        bool canShoot = false;


        // Checking if the raycast hit an object that is not a portal
        RaycastHit hitObject;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitObject, gunRange))
        {
            // Checking if the shooting delay is over
            if (shootingTime <= Time.time)
            {
                if (canCreatePortal(hitObject))
                {
                    canShoot = true;

                    if (Input.GetButtonDown("Fire1"))
                    {
                        shootGun(hitObject, 1, bulletPrefabBlue, muzzleFlashBlue);
                    }
                    else if (Input.GetButtonDown("Fire2"))
                    {
                        shootGun(hitObject, 2, bulletPrefabRed, muzzleFlashRed);
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

        if (null != bulletManager)
        {
            bulletManager.updateBullet();
        }
    }

    private void shootGun(RaycastHit hitObject, int bulletType, GameObject bulletPrefab, GameObject muzzleFlash)
    {
        GameObject muzzleFlashObject = Instantiate(muzzleFlash, gunTip.position, gunTip.rotation);
        Destroy(muzzleFlashObject, 2.0f);

        shootingTime = Time.time + shootingInterval;

        // Creating the bullet
        GameObject newBulletObject = Instantiate(bulletPrefab);
        bulletManager = new BulletManager(this, portalManager, newBulletObject, gunTip, hitObject, bulletType);

        SoundManager.playSound(SoundManager.Sounds.ShootGun, null);
    }

    // Checking if a portal can be created on the surface the raycast hit
    private bool canCreatePortal(RaycastHit hitObject)
    {
        // Check if bullet is firing because you can't create a portal when a bullet hasn't been destroyed yet
        if (null != bulletManager)
        {
            return false;
        }

        // Check if the surface is a portal
        if (1 << hitObject.collider.gameObject.layer == portalManager.getPortalLayerMask())
        {
            return false;
        }

        // Check if a portal can be created
        return portalManager.checkPortalCreation(hitObject, playerCamera.transform);
    }

    public void removeBullet()
    {
        bulletManager = null;
    }
}
