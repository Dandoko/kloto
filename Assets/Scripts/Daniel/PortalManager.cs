using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] private GameObject portalPrefab;
    [SerializeField] private LayerMask portalMask;
    [SerializeField] private LayerMask playerMask;

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
        fixOverhangs(ref tempPortalCentre);

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

    private void fixOverhangs(ref Transform tempPortalCentre)
    {
        // Hard coded points at the centre of the rectangular portal hitbox
        // 0.5f seems to work with the game object's scale to align at the outside 
        List<Vector3> portalEdgePoints = new List<Vector3>
        {
            new Vector3(-0.5f,  0.0f, 0.05f),
            new Vector3( 0.5f,  0.0f, 0.05f),
            new Vector3( 0.0f, -0.5f, 0.05f),
            new Vector3( 0.0f,  0.5f, 0.05f)
        };

        List<Vector3> testDirs = new List<Vector3>
        {
             Vector3.right,
            -Vector3.right,
             Vector3.up,
            -Vector3.up
        };

        int interferingLayerMasks = portalMask.value | playerMask.value;
        int layerMasksToIgnore =~ interferingLayerMasks;

        for (int i = 0; i < 4; ++i) {
            RaycastHit hit;
            Vector3 raycastPos = tempPortalCentre.TransformPoint(portalEdgePoints[i]);
            Vector3 raycastDir = tempPortalCentre.TransformDirection(testDirs[i]);

            Collider[] edgeColliders = Physics.OverlapSphere(raycastPos, 0.1f, layerMasksToIgnore);
            if (edgeColliders.Length == 0)
            {
                // Overhang occurs

                Debug.DrawRay(raycastPos, raycastDir * 1.3f, Color.red, 20);

                if (Physics.Raycast(raycastPos, raycastDir, out hit, 1.1f, portalMask))
                {
                    var offset = hit.point - raycastPos;
                    tempPortalCentre.Translate(offset, Space.World);
                    Debug.Log(i + " | " + offset + " | " + hit.collider.name);
                }
            }

            // Start - Debugging
            GameObject tempSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            tempSphere.transform.position = raycastPos;
            tempSphere.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            tempSphere.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            tempSphere.GetComponent<Collider>().enabled = false;
            // End - Debugging
        }
        Debug.Log("========");
    }
}
