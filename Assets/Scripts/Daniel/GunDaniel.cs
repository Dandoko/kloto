using UnityEngine;

public class GunDaniel : MonoBehaviour
{
    private Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
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
            Debug.Log(hitObject.transform.name);
        }
    }
}
