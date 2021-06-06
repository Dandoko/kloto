using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal linkedPortal;
    [SerializeField] private Transform player;
    
    private Camera playerCamera;
    private Camera portalCamera;
    private RenderTexture cameraTexture;
    private MeshRenderer screenFront;
    private MeshRenderer screenBack;

    private bool isTeleporting;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        portalCamera = GetComponentInChildren<Camera>();

        // Must disable the portal camera to render the other portal camera onto the portal screen manually
        portalCamera.enabled = false;

        screenFront = transform.GetChild(1).gameObject.GetComponent<MeshRenderer>();
        screenBack = transform.GetChild(2).gameObject.GetComponent<MeshRenderer>();

        isTeleporting = false;
    }

    void Update()
    {
        // Don't update the linked portal screen if the player camera cannot see the linked portal
        if (!isVisibleOnPlayerCamera())
        {
            return;
        }

        // Hide the portal render screens while the render texture is being created
        screenFront.enabled = false;
        screenBack.enabled = false;

        // If the render texture has not been created yet, or if the dimensions of the render texture have changed
        if (null == cameraTexture || cameraTexture.width != Screen.width || cameraTexture.height != Screen.height)
        {
            if (cameraTexture != null)
            {
                // Release the hardware resources used by the existing render texture
                cameraTexture.Release();
            }
            cameraTexture = new RenderTexture(Screen.width, Screen.height, 24);
            portalCamera.targetTexture = cameraTexture;
            linkedPortal.getFrontScreen().material.mainTexture = cameraTexture;
            linkedPortal.getBackScreen().material.mainTexture = cameraTexture;
        }

        // Calculate the position and rotation of the portal camera using the world space
        var cameraPositionMatrix = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * playerCamera.transform.localToWorldMatrix;
        portalCamera.transform.SetPositionAndRotation(cameraPositionMatrix.GetColumn(3), cameraPositionMatrix.rotation);

        // Renders the camera manually each update frame
        portalCamera.Render();

        screenFront.enabled = true;
        screenBack.enabled = true;
    }

    private void LateUpdate()
    {
        if (isTeleporting)
        {
            Vector3 offsetFromPortal = player.position - transform.TransformPoint(transform.position);
            float dotProduct = Vector3.Dot(transform.up, offsetFromPortal);

            if (dotProduct < 0f)
            {
                var playerPositionMatrix = linkedPortal.transform.worldToLocalMatrix * transform.localToWorldMatrix * player.localToWorldMatrix;
                //player.SetPositionAndRotation(playerPositionMatrix.GetColumn(3), playerPositionMatrix.rotation);

                isTeleporting = false;
            }
        }
    }

    public MeshRenderer getFrontScreen()
    {
        return screenFront;
    }

    public MeshRenderer getBackScreen()
    {
        return screenBack;
    }

    // Returns true if the player camera can see the linked portal
    // @see https://wiki.unity3d.com/index.php/IsVisibleFrom
    private bool isVisibleOnPlayerCamera()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCamera);
        return GeometryUtility.TestPlanesAABB(planes, linkedPortal.getFrontScreen().bounds) || GeometryUtility.TestPlanesAABB(planes, linkedPortal.getBackScreen().bounds);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isTeleporting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isTeleporting = false;
        }
    }
}
