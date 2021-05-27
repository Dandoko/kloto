using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    [SerializeField] private GameObject portal1;
    [SerializeField] private GameObject portal2;
    [SerializeField] private Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerOffsetFromPortal = playerCamera.transform.position - portal1.transform.position;
        transform.position = portal2.transform.position + playerOffsetFromPortal;


    }
}
