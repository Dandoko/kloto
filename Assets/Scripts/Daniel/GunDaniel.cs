using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDaniel : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Material bulletBlueMat;
    [SerializeField] private Material bulletRedMat;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Image crosshair;
    
    private Camera playerCamera;
    private Transform gunTip;
    private float gunRange = 100f;
    private float shootingTime = 0f;
    private float shootingInterval = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        gunTip = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (shootingTime <= Time.time)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                shootGun("Fire1");
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                shootGun("Fire2");
            }
        }
    }

    private void shootGun(string fireMouseClick)
    {
        // Checking if the raycast hit an object
        RaycastHit hitObject;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitObject, gunRange))
        {
            muzzleFlash.Play();

            // Creating the bullet
            GameObject bullet = Instantiate(bulletPrefab);
            if ("Fire1" == fireMouseClick)
            {
                bullet.GetComponent<MeshRenderer>().material = bulletBlueMat;
            }
            else
            {
                bullet.GetComponent<MeshRenderer>().material = bulletRedMat;
            }
            bullet.transform.position = gunTip.position;
            Vector3 bulletDir = (hitObject.point - gunTip.position).normalized;
            bullet.transform.forward = bulletDir;

            shootingTime = Time.time + shootingInterval;
        }
    }
}
