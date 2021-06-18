using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunDaniel : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Material bulletBlueMat;
    [SerializeField] private Material bulletRedMat;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Image crosshair;
    [SerializeField] private GameObject portalPrefab;
    
    private Camera playerCamera;
    private Transform gunTip;
    private float gunRange = 100f;
    private float shootingTime = 0f;
    private float shootingInterval = 0.7f;
    private Color canShootColor;
    private Color cannotShootColor;
    private GameObject portalScreenPrefab;
    private float portalColliderRadius = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        gunTip = transform.GetChild(0);

        canShootColor = new Color(1, 0, 0, 1);
        cannotShootColor = new Color(1, 0.3f, 0.6f, 0.5f);

        portalScreenPrefab = portalPrefab.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitObject;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitObject, gunRange))
        {
            crosshair.color = canShootColor;
        }
        else
        {
            crosshair.color = cannotShootColor;
        }

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
            if (canCreatePortal(hitObject))
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

    private bool canCreatePortal(RaycastHit hitObject)
    {
        // Make the centre of the portal the position of the raycast
        Vector3 portalCentre = hitObject.point;

        // Create a temporary portal mold with sphere colliders at the corners of the mold
        Vector3 portalSize = portalScreenPrefab.GetComponent<MeshRenderer>().bounds.size;
        Vector3 topLeftPos = new Vector3(portalCentre.x, portalCentre.y + portalSize.z / 2, portalCentre.z + portalSize.y / 2);

        //=====================================================================
        // Start - Portal Creation
        //=====================================================================
        GameObject portal = Instantiate(portalPrefab);
        GameObject portalScreen = portal.transform.GetChild(0).gameObject;
        portalScreen.GetComponent<MeshRenderer>().material = bulletBlueMat;
        portal.transform.position = portalCentre;

        portal.transform.up = hitObject.normal;

        //Vector3 upwards = Vector3.Cross(hitObject.normal, Vector3.up);
        //portal.transform.rotation = Quaternion.LookRotation(hitObject.normal, upwards);
        ////portal.transform.up = Quaternion.LookRotation(hitObject.normal, upwards) * Vector3.forward;
        //=====================================================================
        // End - Portal Creation
        //=====================================================================

        //=====================================================================
        // Start - Debugging
        //=====================================================================
        Debug.DrawRay(portalCentre, hitObject.normal * 3, Color.red, 100);

        //GameObject testSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //testSphere.transform.position = topLeftPos;
        //testSphere.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        //=====================================================================
        // End - Debugging
        //=====================================================================

        Debug.Log("portalCentre: " + portalCentre);
        Debug.Log("portalSize: " + portalSize);
        Debug.Log("portalSize.y: " + portalSize.y);
        Debug.Log("================");

        // Check if all sphere colliders colide with the same object

        return true;
    }
}
