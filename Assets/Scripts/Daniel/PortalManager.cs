using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    // Portal configuration (x, y, z)
    // Rotation (0, 90, 0)
    // Scale (1.4, 2,2, 0.005)

    [SerializeField] private GameObject portalPrefab;
    [SerializeField] private LayerMask portalMask;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Material portalMat;

    private Transform tempPortal;
    private GameObject portal1;
    private GameObject portal2;

    public bool checkPortalCreation(RaycastHit hitObject, Transform playerCamera)
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

        return canPortalBeCreated(hitObject, portalRotation);
    }

    private bool canPortalBeCreated(RaycastHit hitObject, Quaternion portalRotation)
    {
        GameObject portalScreen = portalPrefab.transform.GetChild(0).gameObject;

        Transform tempPortalCentre = portalScreen.transform;
        tempPortalCentre.position = hitObject.point;
        tempPortalCentre.rotation = portalRotation;
        tempPortalCentre.position -= tempPortalCentre.forward * 0.001f;

        fixOverhangs(ref tempPortalCentre);
        fixIntersections(ref tempPortalCentre);

        if (fixesAreSuccessful(tempPortalCentre))
        {
            tempPortal = portalScreen.transform;
            tempPortal.position = tempPortalCentre.position;
            tempPortal.rotation = tempPortalCentre.rotation;

            return true;
        }

        return false;
    }

    public void instatiatePortal(int bulletType, Material bulletMat)
    {
        if (bulletType == 1)
        {
            instantiatePortalHelper(ref portal1, ref portal2, bulletMat);
        }
        else
        {
            instantiatePortalHelper(ref portal2, ref portal1, bulletMat);
        }
    }

    private void instantiatePortalHelper(ref GameObject portal, ref GameObject linkedPortal, Material bulletMat)
    {
        if (null != portal)
        {
            destroyPortalScript(ref linkedPortal);
            Destroy(portal);
        }

        portal = Instantiate(portalPrefab);


        if (bothPortalsExist())
        {
            destroyPortalScript(ref linkedPortal);
            destroyPortalScript(ref portal);

            portal.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = portalMat;
            Destroy(linkedPortal.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material);
            linkedPortal.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = portalMat;

            var portalComponent = portal.AddComponent<OneSidedPortal>();
            var linkedPortalComponent = linkedPortal.AddComponent<OneSidedPortal>();

            portalComponent.setPortal(linkedPortalComponent, null);
            linkedPortalComponent.setPortal(portalComponent, null);
        }
        else
        {
            portal.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = bulletMat;
        }


        portal.transform.position = tempPortal.position;
        portal.transform.rotation = tempPortal.rotation;

        // Screen
        portal.transform.GetChild(0).gameObject.transform.position = tempPortal.position;
        portal.transform.GetChild(0).gameObject.transform.rotation = tempPortal.rotation;
        // Frame
        portal.transform.GetChild(2).gameObject.transform.position = tempPortal.position;
        portal.transform.GetChild(2).gameObject.transform.rotation = tempPortal.rotation;

    }

    public int getPortalLayerMask()
    {
        return portalMask;
    }

    // Check for overhangs and fix the issues
    // Returns true if all overhangs have been fixed
    private void fixOverhangs(ref Transform tempPortalCentre)
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

        int layerMasksToIgnore = portalMask.value | playerMask.value;
        int allMasksWithoutMasksToIgnore =~ layerMasksToIgnore;

        for (int i = 0; i < portalEdgePoints.Count; ++i) {
            RaycastHit hit;
            Vector3 raycastPos = tempPortalCentre.TransformPoint(portalEdgePoints[i]);
            Vector3 raycastDir = tempPortalCentre.TransformDirection(testDirs[i]);

            Collider[] edgeColliders = Physics.OverlapSphere(raycastPos, 0.1f, allMasksWithoutMasksToIgnore);
            // Overhang occurs because the edge collider didn't collide with anything
            if (edgeColliders.Length == 0)
            {
                if (Physics.Raycast(raycastPos, raycastDir, out hit, 1.1f, allMasksWithoutMasksToIgnore))
                {
                    var offset = hit.point - raycastPos;
                    tempPortalCentre.Translate(offset, Space.World);
                }
            }
        }
    }

    // Checks for intersections between multiple adjacent surfaces (including portals) and fixes the issues
    // Returns true if all intersections have been fixed
    private void fixIntersections(ref Transform tempPortalCentre)
    {
        List<Vector3> testDirs = new List<Vector3>
        {
             Vector3.right,
            -Vector3.right,
             Vector3.up,
            -Vector3.up
        };

        int layerMasksToIgnore = playerMask.value;
        int allMasksWithoutMasksToIgnore =~ layerMasksToIgnore;

        // Distances from the centre of the portal
        List<float> testDists = new List<float> { 0.7f, 0.7f, 1.1f, 1.1f };

        for (int i = 0; i < testDirs.Count; i++)
        {
            RaycastHit hit;
            Vector3 raycastPos = tempPortalCentre.TransformPoint(0.0f, 0.0f, -0.5f);
            Vector3 raycastDir = tempPortalCentre.TransformDirection(testDirs[i]);

            if (Physics.Raycast(raycastPos, raycastDir, out hit, testDists[i], allMasksWithoutMasksToIgnore))
            {
                var offset = (hit.point - raycastPos);
                var newOffset = -raycastDir * (testDists[i] - offset.magnitude);
                tempPortalCentre.Translate(newOffset, Space.World);
            }
        }
    }

    private bool fixesAreSuccessful(Transform tempPortalCentre)
    {
        //=====================================================================
        // Checking intersection fixes
        //=====================================================================
        int layerMasksToIgnore = playerMask.value;
        int allMasksWithoutMasksToIgnore = ~layerMasksToIgnore;

        // Slightly shorter than the testDists and z is shorter than the width of the portal
        var rectColliderExtentDist = new Vector3(0.65f, 1.05f, 0.0001f);

        Collider[] intersections = Physics.OverlapBox(tempPortalCentre.position, rectColliderExtentDist, tempPortalCentre.rotation, allMasksWithoutMasksToIgnore);
        if (intersections.Length > 0)
        {
            return false;
        }

        //=====================================================================
        // Checking overhang fixes
        //=====================================================================
        layerMasksToIgnore = portalMask.value | playerMask.value;
        allMasksWithoutMasksToIgnore = ~layerMasksToIgnore;

        List<Vector3> vertexPositions = new List<Vector3>
        {
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f,  0.5f, -0.5f),
            new Vector3( 0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f,  0.5f, -0.5f)
        };

        for (int i = 0; i < vertexPositions.Count; i++)
        {
            Vector3 spherePos = tempPortalCentre.TransformPoint(vertexPositions[i]);
            Collider[] vertexColliders = Physics.OverlapSphere(spherePos, 0.1f, allMasksWithoutMasksToIgnore);

            if (vertexColliders.Length == 0)
            {
                return false;
            }
        }

        return true;
    }

    private bool bothPortalsExist()
    {
        return portal1 != null && portal2 != null;
    }

    private void destroyPortalScript(ref GameObject portal)
    {
        if (null != portal)
        {
            if (null != portal.GetComponent<OneSidedPortal>())
            {
                Destroy(portal.GetComponent<OneSidedPortal>());
            }

        }
    }
}
