using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDaniel : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Material bulletBlueMat;
    [SerializeField] private Material bulletRedMat;
    
    private Camera playerCamera;
    private Transform gunTip;
    private const float gunRange = 100f;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        gunTip = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse Click
        if (Input.GetButtonDown("Fire1"))
        {
            shootGun("Fire1");
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            shootGun("Fire2");
        }
    }

    private void shootGun(string fireMouseClick)
    {
        // Checking if the raycast hit an object
        RaycastHit hitObject;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitObject, gunRange))
        {
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
        }
    }
}
