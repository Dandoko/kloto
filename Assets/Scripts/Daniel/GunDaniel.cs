using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDaniel : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    
    private Camera playerCamera;
    private Transform gunTip;
    private const float range = 100f;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        gunTip = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse Left Click
        if (Input.GetButtonDown("Fire1"))
        {
            shootGun();
        }
    }

    private void shootGun()
    {
        RaycastHit hitObject;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitObject, range))
        {
            // Creating a bullet a few units in front of the camera with the same forward direction as the camera
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = gunTip.position;
            Vector3 bulletDir = (hitObject.point - gunTip.position).normalized;
            bullet.transform.forward = bulletDir;
        }
    }
}
