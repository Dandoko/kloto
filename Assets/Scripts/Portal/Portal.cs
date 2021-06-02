using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject linkedPortal;
    [SerializeField] private Material linkedPortalCameraMat;

    private Camera playerCamera;
    private Camera myCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        myCamera = GetComponentInChildren<Camera>();

        // If the portal camera already has a render texture
        if (null != myCamera.targetTexture)
        {
            // Remove the texture
            myCamera.targetTexture.Release();
        }
        myCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        linkedPortalCameraMat.mainTexture = myCamera.targetTexture;
    }

    // Update is called once per frame
    void Update()
    {
        // Moves the portal camera relative to the player
        var cameraPositionMatrix = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * playerCamera.transform.localToWorldMatrix;
        myCamera.transform.SetPositionAndRotation(cameraPositionMatrix.GetColumn(3), cameraPositionMatrix.rotation);
    }
}
