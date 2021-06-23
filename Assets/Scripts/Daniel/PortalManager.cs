using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] private GameObject portalPrefab;

    public void createPortal(RaycastHit hitObject, Transform playerCamera, Material bulletMat)
    {
        // Find direction and rotation of portal
        Quaternion cameraRotation = playerCamera.rotation;
        Vector3 portalRight = cameraRotation * Vector3.right;

        if (Mathf.Abs(portalRight.x) >= Mathf.Abs(portalRight.z))
        {
            portalRight = (portalRight.x >= 0) ? Vector3.right : -Vector3.right;
        }
        else
        {
            portalRight = (portalRight.z >= 0) ? Vector3.forward : -Vector3.forward;
        }

        var portalForward = -hitObject.normal;
        var portalUp = -Vector3.Cross(portalRight, portalForward);
        var portalRotation = Quaternion.LookRotation(portalForward, portalUp);

        // Placing the portal
        GameObject portal = Instantiate(portalPrefab);
        portal.GetComponent<MeshRenderer>().material = bulletMat;
        portal.transform.position = hitObject.point;
        portal.transform.rotation = portalRotation;
        portal.transform.position -= portal.transform.forward * 0.001f;
    }
}
