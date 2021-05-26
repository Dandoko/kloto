using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraMovement : MonoBehaviour
{

    [SerializeField] Transform playerCamera;
    [SerializeField] Transform lookingAtPortal;
    [SerializeField] Transform renderingOnPortal;
    Vector3 playerToPortal1;
    Vector3 cameraToPortal2;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerToPortal1 = playerCamera.position - renderingOnPortal.position;
        cameraToPortal2 = playerToPortal1;
        transform.position = cameraToPortal2 + lookingAtPortal.position;


        transform.rotation = playerCamera.rotation;
    }
}
