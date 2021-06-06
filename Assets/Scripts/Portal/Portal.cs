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
    private List<Transform> teleporters;

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
        teleporters = new List<Transform>();
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
            linkedPortal.screenFront.material.mainTexture = cameraTexture;
            linkedPortal.screenBack.material.mainTexture = cameraTexture;
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
        //if (isTeleporting)
        //{
        //    Vector3 offsetFromPortal = player.position - transform.TransformPoint(transform.position);
        //    float dotProduct = Vector3.Dot(transform.up, offsetFromPortal);

        //    if (dotProduct < 0f)
        //    {
        //        Debug.Log("player before: " + player.TransformPoint(player.position));
        //        Debug.Log("linkedPortal: " + linkedPortal.transform.TransformPoint(linkedPortal.transform.position));
        //        Debug.Log("portal: " + transform.TransformPoint(transform.position));
        //        var playerPositionMatrix = linkedPortal.transform.worldToLocalMatrix * transform.localToWorldMatrix * player.localToWorldMatrix;
        //        player.SetPositionAndRotation(playerPositionMatrix.GetColumn(3), playerPositionMatrix.rotation);
        //        Debug.Log("player after: " + player.TransformPoint(player.position));

        //        isTeleporting = false;
        //    }
        //}

        for (int i = 0; i < teleporters.Count; i++)
        {
            Transform teleporter = teleporters[i];
            Vector3 offsetFromPortal = teleporter.position - transform.TransformPoint(transform.position);
            float dotProduct = Vector3.Dot(transform.forward, offsetFromPortal);

            if (dotProduct < 0f)
            {
                Debug.Log(this);

                //Debug.Log("i: " + i);
                Debug.Log("player before: " + teleporter.TransformPoint(teleporter.position));
                Debug.Log("player before: " + player.TransformPoint(player.position));
                Debug.Log("linkedPortal: " + linkedPortal.transform.TransformPoint(linkedPortal.transform.position));
                Debug.Log("portal: " + transform.TransformPoint(transform.position));
                var playerPositionMatrix = linkedPortal.transform.worldToLocalMatrix * transform.localToWorldMatrix * teleporter.localToWorldMatrix;
                teleporter.SetPositionAndRotation(playerPositionMatrix.GetColumn(3), playerPositionMatrix.rotation);
                //teleporter.SetPositionAndRotation(linkedPortal.transform.position, playerPositionMatrix.rotation);

                //float rotationDiff = -Quaternion.Angle(transform.rotation, linkedPortal.transform.rotation);
                //player.Rotate(Vector3.up, rotationDiff);
                //Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * offsetFromPortal;
                //player.position = linkedPortal.transform.position + offsetFromPortal;


                //Debug.Log("playerPositionMatrix: " + playerPositionMatrix.GetColumn(3));
                Debug.Log("player after: " + teleporter.TransformPoint(teleporter.position));
                Debug.Log("player after: " + player.TransformPoint(player.position));


                //Debug.Log("linkedPortal.teleporters: " + linkedPortal.teleporters.Count);
                //Debug.Log("teleporters: " + teleporters.Count);

                linkedPortal.teleporters.Add(teleporter);
                teleporters.RemoveAt(i);
                i--;

                //Debug.Log("linkedPortal.teleporters: " + linkedPortal.teleporters.Count);
                //Debug.Log("teleporters: " + teleporters.Count);

                Debug.Log("===================================");
            }
        }
    }

    // Returns true if the player camera can see the linked portal
    // @see https://wiki.unity3d.com/index.php/IsVisibleFrom
    private bool isVisibleOnPlayerCamera()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCamera);
        return GeometryUtility.TestPlanesAABB(planes, linkedPortal.screenFront.bounds) || GeometryUtility.TestPlanesAABB(planes, linkedPortal.screenBack.bounds);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isTeleporting = true;
            teleporters.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isTeleporting = false;

            if (teleporters.Contains(other.transform))
            {
                teleporters.Remove(other.transform);
                Debug.Log("bruh");
            }
        }
    }
}
