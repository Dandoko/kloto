using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject linkedPortal;

    private Camera playerCamera;
    private Camera myCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        myCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        var cameraPositionMatrix = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * playerCamera.transform.localToWorldMatrix;
        myCamera.transform.SetPositionAndRotation(cameraPositionMatrix.GetColumn(3), cameraPositionMatrix.rotation);
    }
}
