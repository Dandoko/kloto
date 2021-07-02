using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] private GameObject portalPrefab;

    private GameObject portal1;
    private GameObject portal2;

    public void createPortal(RaycastHit hitObject, Transform playerCamera, Material bulletMat, int bulletType)
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

        Vector3 portalForward = -hitObject.normal;
        Vector3 portalUp = -Vector3.Cross(portalRight, portalForward);
        Quaternion portalRotation = Quaternion.LookRotation(portalForward, portalUp);

        if (bulletType == 1)
        {
            createPortalHelper(ref portal1, bulletMat, hitObject, portalRotation);
        }
        else
        {
            createPortalHelper(ref portal2, bulletMat, hitObject, portalRotation);
        }
    }

    private void createPortalHelper(ref GameObject portal, Material bulletMat, RaycastHit hitObject, Quaternion portalRotation)
    {
        if (null != portal)
        {
            Destroy(portal);
        }

        portal = Instantiate(portalPrefab);
        portal.GetComponent<MeshRenderer>().material = bulletMat;

        Transform tempPortalCentre = portal.transform;
        tempPortalCentre.position = hitObject.point;
        tempPortalCentre.rotation = portalRotation;
        tempPortalCentre.position -= tempPortalCentre.forward * 0.001f;

        // Check for overhangs and fix the issues

        // Check for intersections from the centre of the portal and fix the issues

        // Sanity check to see if the fixes worked
        if (true)
        {
            portal.transform.position = tempPortalCentre.position;
            portal.transform.rotation = tempPortalCentre.rotation;
        }
    }

    public bool isPortal(GameObject comparingObj)
    {
        if (null != portal1)
        {
            if (comparingObj == portal1)
            {
                return true;
            }
        }

        if (null != portal2)
        {
            if (comparingObj == portal2)
            {
                return true;
            }
        }

        return false;
    }

}
