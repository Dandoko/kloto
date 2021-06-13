using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDaniel : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    
    private Camera playerCamera;
    private Transform gunTip;

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
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitObject))
        {
            
        }

        // Creating a bullet a few units in front of the camera with the same forward direction as the camera
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = playerCamera.transform.position + playerCamera.transform.forward * 1.5f;
        bullet.transform.forward = playerCamera.transform.forward;
    }
}
