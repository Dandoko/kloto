using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    // Portal configuration (x, y, z)
    // Rotation (0, 90, 0)
    // Scale (1.4, 2,2, 0.005)

    public static List<Vector3> portalOffsets = new List<Vector3>
    {
        new Vector3( 0.0f,  0.0f, 0f), // Centre
        new Vector3(-0.5f, -0.5f, 0f), // Bottom Left
        new Vector3(-0.5f,  0.5f, 0f), // Top Left
        new Vector3( 0.5f, -0.5f, 0f), // Bottom Right
        new Vector3( 0.5f,  0.5f, 0f)  // Top Right
    };
    // The two maps must be farther apart than this distance
    public static float mapDistance = 50f;

    [SerializeField] private GameObject portalPrefab;
    [SerializeField] private LayerMask portalMask;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Material portalMat;
    [SerializeField] private Material singlePortalMat1;
    [SerializeField] private Material singlePortalMat2;
    [SerializeField] private GameObject twoSidedPortal1;
    [SerializeField] private GameObject twoSidedPortal2;
    [SerializeField] private Camera playerCamera;

    private Transform tempPortal;
    private Quaternion tempBackwardsPortalRotation;
    private GameObject connectingSurface;
    private GameObject connectedSurface1;
    private GameObject connectedSurface2;
    private GameObject portal1;
    private GameObject portal2;
    private string outlineName1 = "OutlineBlue";
    private string outlineName2 = "OutlineRed";
    private Vector3 connectedSurfaceNormal;

    // 0.3f acts as the depth of the screen corners
    private List<Vector3> playerCameraNearClipPlaneCorners = new List<Vector3>
    {
        new Vector3(0, 0, 0.3f),
        new Vector3(0, 1, 0.3f),
        new Vector3(1, 0, 0.3f),
        new Vector3(1, 1, 0.3f)
    };

    void Update()
    {
        oneWayPortalsSeeTwoWayPortals();

        twoWayPortalsSeeOneWayPortals();
    }

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
        Quaternion backwardsPortalRotation = Quaternion.LookRotation(-portalForward, portalUp);

        return canPortalBeCreated(hitObject, portalRotation, backwardsPortalRotation);
    }

    private bool canPortalBeCreated(RaycastHit hitObject, Quaternion portalRotation, Quaternion backwardsPortalRotation)
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
            tempBackwardsPortalRotation = backwardsPortalRotation;
            connectingSurface = hitObject.collider.gameObject;
            connectedSurfaceNormal = hitObject.normal;

            return true;
        }

        return false;
    }

    public void instatiatePortal(int bulletType)
    {
        if (bulletType == 1)
        {
            instantiatePortalHelper(ref portal1, ref portal2, singlePortalMat1, true, connectingSurface, connectedSurface2);
            connectedSurface1 = connectingSurface;
        }
        else
        {
            instantiatePortalHelper(ref portal2, ref portal1, singlePortalMat2, false, connectingSurface, connectedSurface1);
            connectedSurface2 = connectingSurface;
        }
    }

    private void instantiatePortalHelper(ref GameObject portal, ref GameObject linkedPortal, Material bulletMat, bool isPortal1,
                                         GameObject portalSurface, GameObject linkedPortalSurface)
    {
        if (null != portal)
        {
            destroyPortalScript(ref linkedPortal);
            Destroy(portal);
        }

        portal = Instantiate(portalPrefab);
        portal.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        var portalComponent = portal.AddComponent<OneSidedPortal>();

        if (bothPortalsExist())
        {
            portal.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = portalMat;
            Destroy(linkedPortal.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material);
            destroyPortalScript(ref linkedPortal);
            linkedPortal.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = portalMat;

            var linkedPortalComponent = linkedPortal.AddComponent<OneSidedPortal>();

            portalComponent.setPortal(linkedPortalComponent, portalSurface, this, playerMask);
            linkedPortalComponent.setPortal(portalComponent, linkedPortalSurface, this, playerMask);
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

        // TESTING
        portal.transform.GetChild(3).gameObject.transform.position = portal.transform.position;
        portal.transform.GetChild(3).gameObject.transform.localPosition = new Vector3(0f, 0f, portal.transform.GetChild(3).gameObject.transform.localScale.z/2f + 0.02f);
        portal.transform.GetChild(3).gameObject.transform.rotation = portal.transform.rotation;

        // Blue
        if (isPortal1)
        {
            portal.transform.rotation = tempBackwardsPortalRotation;
            portal.transform.GetChild(3).gameObject.transform.localPosition = new Vector3(0f, 0f, -(portal.transform.GetChild(3).gameObject.transform.localScale.z / 2f + 0.02f));

            portal.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(outlineName1);

            if (connectedSurfaceNormal.z == -1)
            {
                portal.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }

            portal.name = "BluePortal";
        }
        // Red
        else /* if (!isPortal1) */
        {
            portal.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(outlineName2);

            if (connectedSurfaceNormal.z == 1)
            {
                portal.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }

            portal.name = "RedPortal";
        }

        SoundManager.playSound(SoundManager.Sounds.Portal, portal);
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

            Collider[] edgeColliders = Physics.OverlapSphere(raycastPos, 0.05f, allMasksWithoutMasksToIgnore);
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

    public bool bothPortalsExist()
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

    public GameObject getTwoWayPortal(int portalNum)
    {
        if (1 == portalNum)
        {
            return twoSidedPortal1;
        }
        else /* if (2 == portalNum) */
        {
            return twoSidedPortal2;
        }
    }

    public GameObject getOneWayPortal(int portalNum)
    {
        if (1 == portalNum)
        {
            return portal1;
        }
        else /* if (2 == portalNum) */
        {
            return portal2;
        }
    }

    private void oneWayPortalsSeeTwoWayPortals()
    {
        if (!bothPortalsExist())
        {
            return;
        }

        //=====================================================================
        // Using camera bounding boxes and raycasts to check if the player can
        // see either two-way portals
        //=====================================================================

        twoSidedPortal1.GetComponent<TwoSidedPortal>().setIsBeingLookedThroughOneWayPortal(false);
        twoSidedPortal2.GetComponent<TwoSidedPortal>().setIsBeingLookedThroughOneWayPortal(false);

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCamera);

        if (GeometryUtility.TestPlanesAABB(planes, twoSidedPortal1.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds))
        {
            for (int i = 0; i < portalOffsets.Count; i++)
            {
                for (int j = 0; j < playerCameraNearClipPlaneCorners.Count; j++)
                {
                    RaycastHit hit;
                    Vector3 startPos = playerCamera.ViewportToWorldPoint(playerCameraNearClipPlaneCorners[j]);
                    Vector3 dir = twoSidedPortal1.transform.GetChild(0).gameObject.transform.TransformPoint(portalOffsets[i]) - startPos;
                    if (Physics.Raycast(startPos, dir, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.gameObject == twoSidedPortal1 || hit.collider.gameObject == twoSidedPortal1.transform.GetChild(0).gameObject ||
                            hit.collider.gameObject == twoSidedPortal1.transform.GetChild(1).gameObject)
                        {
                            projectBothOneWayPortalsThroughSameTwoWayPortal(twoSidedPortal2, ref twoSidedPortal1);
                            return;
                        }
                    }
                }
            }
        }

        // Need to check the two-way portals separatey because the raycasts are not using the
        // direction the camera is facing but just the position of the camera
        if (GeometryUtility.TestPlanesAABB(planes, twoSidedPortal2.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().bounds))
        {
            for (int i = 0; i < portalOffsets.Count; i++)
            {
                for (int j = 0; j < playerCameraNearClipPlaneCorners.Count; j++)
                {
                    RaycastHit hit;
                    Vector3 startPos = playerCamera.ViewportToWorldPoint(playerCameraNearClipPlaneCorners[j]);
                    Vector3 dir = twoSidedPortal2.transform.GetChild(0).gameObject.transform.TransformPoint(portalOffsets[i]) - startPos;
                    if (Physics.Raycast(startPos, dir, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.gameObject == twoSidedPortal2 || hit.collider.gameObject == twoSidedPortal2.transform.GetChild(0).gameObject ||
                            hit.collider.gameObject == twoSidedPortal2.transform.GetChild(1).gameObject)
                        {
                            projectBothOneWayPortalsThroughSameTwoWayPortal(twoSidedPortal1, ref twoSidedPortal2);
                            return;
                        }
                    }
                }
            }
        }

        //=====================================================================
        // Preparing the camera orientation of the two-way portal
        //=====================================================================
        var oneWayPortalScreen1 = portal1.GetComponent<OneSidedPortal>();
        var oneWayPortalScreen2 = portal2.GetComponent<OneSidedPortal>();

        bool oneWayPortalScreen1SeesTwoWay = oneWayPortalScreen2.getSeesTwoWayPortal();
        bool oneWayPortalScreen2SeesTwoWay = oneWayPortalScreen1.getSeesTwoWayPortal();

        float twoWayPortaldist1 = Vector3.Distance(playerCamera.transform.position, twoSidedPortal1.transform.position);
        float twoWayPortaldist2 = Vector3.Distance(playerCamera.transform.position, twoSidedPortal2.transform.position);
        GameObject twoSidedPortal = twoWayPortaldist1 < twoWayPortaldist2 ? twoSidedPortal2 : twoSidedPortal1;

        // Check if the one-way portals are on different maps
        if (Vector3.Distance(portal1.transform.position, portal2.transform.position) > mapDistance)
        {
            twoSidedPortal = twoWayPortaldist1 > twoWayPortaldist2 ? twoSidedPortal2 : twoSidedPortal1;
        }

        // If both one-way portals can see a two-way portal
        if (oneWayPortalScreen1SeesTwoWay && oneWayPortalScreen2SeesTwoWay)
        {
            // Assign the perspective of the one-way portal closest to the player to the two-way portal
            float oneWayPortaldist1 = Vector3.Distance(playerCamera.transform.position, portal1.transform.position);
            float oneWayPortaldist2 = Vector3.Distance(playerCamera.transform.position, portal2.transform.position);
            Camera oneWayPortalCamera = oneWayPortaldist1 < oneWayPortaldist2 ? portal2.GetComponentInChildren<Camera>() : portal1.GetComponentInChildren<Camera>();

            twoSidedPortal.GetComponent<TwoSidedPortal>().setOneWayPortalPos(oneWayPortalCamera);
            twoSidedPortal.GetComponent<TwoSidedPortal>().setIsBeingLookedThroughOneWayPortal(true);
        }
        else if (oneWayPortalScreen1SeesTwoWay)
        {
            twoSidedPortal.GetComponent<TwoSidedPortal>().setOneWayPortalPos(portal2.GetComponentInChildren<Camera>());
            twoSidedPortal.GetComponent<TwoSidedPortal>().setIsBeingLookedThroughOneWayPortal(true);

        }
        else if (oneWayPortalScreen2SeesTwoWay)
        {
            twoSidedPortal.GetComponent<TwoSidedPortal>().setOneWayPortalPos(portal1.GetComponentInChildren<Camera>());
            twoSidedPortal.GetComponent<TwoSidedPortal>().setIsBeingLookedThroughOneWayPortal(true);
        }
    }

    private void projectBothOneWayPortalsThroughSameTwoWayPortal(GameObject twoWayPortalViewedByOneWayPortals, ref GameObject twoWayPortalViewedByPlayer)
    {
        var oneWayPortalScreen1 = portal1.GetComponent<OneSidedPortal>();
        var oneWayPortalScreen2 = portal2.GetComponent<OneSidedPortal>();

        bool oneWayPortalScreen1SeesTwoWay = oneWayPortalScreen2.getSeesTwoWayPortal();
        bool oneWayPortalScreen2SeesTwoWay = oneWayPortalScreen1.getSeesTwoWayPortal();

        if (oneWayPortalScreen1SeesTwoWay && oneWayPortalScreen2SeesTwoWay)
        {
            GameObject twoWayPortalSeenByOneWayPortal1 = oneWayPortalScreen1.getLookingatTwoWayPortal();
            GameObject twoWayPortalSeenByOneWayPortal2 = oneWayPortalScreen2.getLookingatTwoWayPortal();

            if (twoWayPortalSeenByOneWayPortal1 == twoWayPortalSeenByOneWayPortal2)
            {
                if (twoWayPortalSeenByOneWayPortal1 == twoWayPortalViewedByOneWayPortals)
                {
                    // Uisng the camera of the one-way portal closer to the two-way portal
                    float oneWayPortalDist1 = Vector3.Distance(twoWayPortalViewedByOneWayPortals.transform.position, portal1.transform.position);
                    float oneWayPortalDist2 = Vector3.Distance(twoWayPortalViewedByOneWayPortals.transform.position, portal2.transform.position);
                    Camera oneWayPortalCamera = oneWayPortalDist1 < oneWayPortalDist2 ? portal2.GetComponentInChildren<Camera>() : portal1.GetComponentInChildren<Camera>();

                    twoWayPortalViewedByPlayer.GetComponent<TwoSidedPortal>().setOneWayPortalPos(oneWayPortalCamera);
                    twoWayPortalViewedByPlayer.GetComponent<TwoSidedPortal>().setIsBeingLookedThroughOneWayPortal(true);
                }
            }
        }
    }

    private void twoWayPortalsSeeOneWayPortals()
    {
        if (!bothPortalsExist())
        {
            return;
        }

        // Only checking the two-way portal that is on the other map of the player
        // which means it's the farther away portal
        GameObject twoWayPortal = Vector3.Distance(twoSidedPortal1.transform.position, playerCamera.transform.position) >
                                  Vector3.Distance(twoSidedPortal2.transform.position, playerCamera.transform.position) ?
                                  twoSidedPortal1 : twoSidedPortal2;

        twoWayPortalsSeeOneWayPortalsHelper(ref twoWayPortal, ref portal1, 1);
        twoWayPortalsSeeOneWayPortalsHelper(ref twoWayPortal, ref portal2, 2);
    }

    private void twoWayPortalsSeeOneWayPortalsHelper(ref GameObject twoWayPortal, ref GameObject oneWayPortal, int portalNum)
    {
        oneWayPortal.GetComponent<OneSidedPortal>().setIsBeingLookedThroughTwoWayPortal(false);

        var twoWayPortalScreen = twoWayPortal.GetComponent<TwoSidedPortal>();
        bool twoWayPortalScreenSeesOneWay = twoWayPortalScreen.getSeesOneWayPortal(portalNum);

        if (twoWayPortalScreenSeesOneWay)
        {
            oneWayPortal.GetComponent<OneSidedPortal>().setTwoWayPortalPos(twoWayPortal.GetComponentInChildren<Camera>());
            oneWayPortal.GetComponent<OneSidedPortal>().setIsBeingLookedThroughTwoWayPortal(true);
        }
    }
}
