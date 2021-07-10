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

    private const float vertDistFromCentre = 1.1f;
    private const float horzDistFromCentre = 0.7f;

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

        Transform tempPortalCentre = portal.transform;
        tempPortalCentre.position = hitObject.point;
        tempPortalCentre.rotation = portalRotation;
        tempPortalCentre.position -= tempPortalCentre.forward * 0.001f;

        int layerMasksToIgnore = portalMask.value | playerMask.value;
        int allMasksWithoutMasksToIgnore = ~layerMasksToIgnore;

        if (fixOverhangs(ref tempPortalCentre, allMasksWithoutMasksToIgnore) &&
            fixIntersections(ref tempPortalCentre, allMasksWithoutMasksToIgnore))
        {
            portal.GetComponent<MeshRenderer>().material = bulletMat;
            portal.transform.position = tempPortalCentre.position;
            portal.transform.rotation = tempPortalCentre.rotation;
        }
        else
        {
            Destroy(portal);
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

    // Check for overhangs and fix the issues
    // Returns true if all overhangs have been fixed
    private bool fixOverhangs(ref Transform tempPortalCentre, int allMasksWithoutMasksToIgnore)
    {
        // Hard coded points at the centre of the rectangular portal hitbox
        // The z point goes "into" the portal which is used later by the raycast 
        List<Vector3> portalEdgePoints = new List<Vector3>
        {
            new Vector3(-0.5f,  0.0f, 0.5f),
            new Vector3( 0.5f,  0.0f, 0.5f),
            new Vector3( 0.0f, -0.5f, 0.5f),
            new Vector3( 0.0f,  0.5f, 0.5f)
        };

        List<Vector3> testDirs = new List<Vector3>
        {
             Vector3.right,
            -Vector3.right,
             Vector3.up,
            -Vector3.up
        };

        for (int i = 0; i < portalEdgePoints.Count; ++i) {
            RaycastHit hit;
            Vector3 raycastPos = tempPortalCentre.TransformPoint(portalEdgePoints[i]);
            Vector3 raycastDir = tempPortalCentre.TransformDirection(testDirs[i]);

            Collider[] edgeColliders = Physics.OverlapSphere(raycastPos, 0.1f, allMasksWithoutMasksToIgnore);
            // Overhang occurs because the edge collider didn't collide with anything
            if (edgeColliders.Length == 0)
            {
                //Debug.DrawRay(raycastPos, raycastDir * 1.3f, Color.red, 20);

                if (Physics.Raycast(raycastPos, raycastDir, out hit, 1.1f, allMasksWithoutMasksToIgnore))
                {
                    var offset = hit.point - raycastPos;
                    tempPortalCentre.Translate(offset, Space.World);
                    //Debug.Log(i + " | " + offset + " | " + hit.collider.name);
                }
            }

            // Start - Debugging
            //GameObject tempSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //tempSphere.transform.position = raycastPos;
            //tempSphere.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            //tempSphere.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            //tempSphere.GetComponent<Collider>().enabled = false;
            // End - Debugging
        }
        //Debug.Log("========");

        List<Vector3> vertexPositions = new List<Vector3>
        {
            tempPortalCentre.position + tempPortalCentre.TransformVector(new Vector3(-0.5f, -0.5f, -0.5f)),
            tempPortalCentre.position + tempPortalCentre.TransformVector(new Vector3(-0.5f,  0.5f, -0.5f)),
            tempPortalCentre.position + tempPortalCentre.TransformVector(new Vector3( 0.5f, -0.5f, -0.5f)),
            tempPortalCentre.position + tempPortalCentre.TransformVector(new Vector3( 0.5f,  0.5f, -0.5f)),
        };

        for (int i = 0; i < vertexPositions.Count; i++)
        {
            Collider[] vertexColliders = Physics.OverlapSphere(vertexPositions[i], 0.1f, allMasksWithoutMasksToIgnore);

            if (vertexColliders.Length == 0)
            {
                return false;
            }
        }

        return true;
    }

    // Checks for intersections between multiple adjacent surfaces and fixes the issues
    // Returns true if all intersections have been fixed
    private bool fixIntersections(ref Transform tempPortalCentre, int allMasksWithoutMasksToIgnore)
    {
        List<Vector3> testDirs = new List<Vector3>
        {
             Vector3.right,
            -Vector3.right,
             Vector3.up,
            -Vector3.up
        };

        // Distances from the centre of the portal
        List<float> testDists = new List<float> { horzDistFromCentre, horzDistFromCentre, vertDistFromCentre, vertDistFromCentre };

        for (int i = 0; i < testDirs.Count; i++)
        {
            RaycastHit hit;
            Vector3 raycastPos = tempPortalCentre.TransformPoint(0.0f, 0.0f, -0.5f);
            Vector3 raycastDir = tempPortalCentre.TransformDirection(testDirs[i]);

            //Debug.DrawRay(raycastPos, raycastDir * testDists[i], Color.black, 20);

            if (Physics.Raycast(raycastPos, raycastDir, out hit, testDists[i], allMasksWithoutMasksToIgnore))
            {
                var offset = (hit.point - raycastPos);
                var newOffset = -raycastDir * (testDists[i] - offset.magnitude);
                tempPortalCentre.Translate(newOffset, Space.World);
            }
        }

        return true;
    }
}
