using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSidedPortal : MonoBehaviour
{

    [SerializeField] private OneSidedPortal linkedPortal;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject tempConnectedWall;


    private GameObject thisPortal;
    private Camera playerCamera;
    private Camera portalCamera;
    private RenderTexture cameraTexture;
    private MeshRenderer portalScreen;
    private List<PortalTraveller> trackedTravellers;


    private Transform clipPlane;
    private Vector4 nearClipPlane;

    // Start is called before the first frame update
    void Start()
    {

        playerCamera = Camera.main;
        portalCamera = GetComponentInChildren<Camera>();
        thisPortal = transform.gameObject;
        trackedTravellers = new List<PortalTraveller>();

        // Must disable the portal camera to render the other portal camera onto the portal screen manually
        portalCamera.enabled = false;

        portalScreen = transform.GetChild(0).GetComponent<MeshRenderer>();
        portalScreen.material.SetInt("displayMask", 1);
    }

    void Update()
    {
        // Don't update the linked portal screen if the player camera cannot see the linked portal
        if (!isVisibleOnPlayerCamera())
        {
            return;
        }

        // Hide the portal render screens while the render texture is being created
        portalScreen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

        // If the render texture has not been created yet, or if the dimensions of the render texture have changed
        if (cameraTexture == null || cameraTexture.width != Screen.width || cameraTexture.height != Screen.height)
        {
            if (cameraTexture != null)
            {
                // Release the hardware resources used by the existing render texture
                cameraTexture.Release();
            }
            cameraTexture = new RenderTexture(Screen.width, Screen.height, 24);
            portalCamera.targetTexture = cameraTexture;
            linkedPortal.portalScreen.material.mainTexture = cameraTexture;
        }


        // Calculate the position and rotation of the portal camera using the world space
        var cameraPositionMatrix = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * playerCamera.transform.localToWorldMatrix;
        portalCamera.transform.SetPositionAndRotation(cameraPositionMatrix.GetColumn(3), cameraPositionMatrix.rotation);


        //Sets the screen's to the appropriate width so that screen flickering is minimized
        //setScreenWidth();


        //Calculates and sets the clip plane
        clipPlane = transform;
        int sign = System.Math.Sign(Vector3.Dot(clipPlane.forward, transform.position - portalCamera.transform.position));

        Vector3 camSpacePos = portalCamera.worldToCameraMatrix.MultiplyPoint(clipPlane.position);
        Vector3 camSpaceNormal = portalCamera.worldToCameraMatrix.MultiplyVector(clipPlane.forward) * sign;
        float camSpaceDst = -Vector3.Dot(camSpacePos, camSpaceNormal);

        //Creates a near clip plane based on the portal screen's position
        if (Mathf.Abs(camSpaceDst) > 0.2f)
        {
            nearClipPlane = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);
            portalCamera.projectionMatrix = playerCamera.CalculateObliqueMatrix(nearClipPlane);
            Debug.Log(transform.name + "CLIPPING");
        }
        else
        {

            portalCamera.projectionMatrix = playerCamera.projectionMatrix;
        }






        // Renders the camera manually each update frame
        portalCamera.Render();


        portalScreen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }



    private void LateUpdate()
    {
        //Handles and teleports all travellers if they move through the screen
        for (int i = 0; i < trackedTravellers.Count; i++)
        {

            PortalTraveller traveller = trackedTravellers[i];

            

            Vector3 curRelPortalPos = traveller.transform.position - transform.position;

            int prevPortalSide = System.Math.Sign(Vector3.Dot(traveller.prevRelPortalPos, transform.forward));
            int curPortalSide = System.Math.Sign(Vector3.Dot(curRelPortalPos, transform.forward));


            if (curPortalSide != prevPortalSide)
            {
                traveller.Teleport(thisPortal, linkedPortal.gameObject, curPortalSide);
                trackedTravellers.RemoveAt(i);
                i--;
            }
            else
            {
                traveller.prevRelPortalPos = curRelPortalPos;
            }
        }
    }


    //Sets the width of the portal screen to avoid clipping with the player camera's near clip plane
    private void setScreenWidth()
    {
        Transform screenTrans = portalScreen.transform;

        float halfHeight = playerCamera.nearClipPlane * Mathf.Tan(playerCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float halfWidth = halfHeight * playerCamera.aspect;
        float distToNearClipCorner = new Vector3(halfWidth, halfHeight, playerCamera.nearClipPlane).magnitude;
        bool camFacingSameDirAsPortal = Vector3.Dot(transform.forward, transform.position - playerCamera.transform.position) > 0;

        screenTrans.localScale = new Vector3(screenTrans.localScale.x, screenTrans.localScale.y, distToNearClipCorner);
/*        screenTrans.localPosition = Vector3.forward * distToNearClipCorner * ((camFacingSameDirAsPortal) ? 0.5f : -0.5f);*/
    }


    // Returns true if the player camera can see the linked portal
    // @see https://wiki.unity3d.com/index.php/IsVisibleFrom
    private bool isVisibleOnPlayerCamera()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCamera);
        return GeometryUtility.TestPlanesAABB(planes, linkedPortal.portalScreen.bounds);
    }



    //
    private void OnTriggerEnter(Collider collided)
    {
        GameObject hitObject = collided.gameObject;
        var traveller = hitObject.GetComponent<PortalTraveller>();
        if (traveller)
        {
            if (!trackedTravellers.Contains(traveller))
            {
                InsidePortal(traveller);
                traveller.prevRelPortalPos = traveller.transform.position - transform.position;
                trackedTravellers.Add(traveller);
            }
        }

    }


    private void OnTriggerExit(Collider collided)
    {
        GameObject hitObject = collided.gameObject;
        var traveller = hitObject.GetComponent<PortalTraveller>();

        if (traveller)
        {
            if (traveller.tag == "Player")
            {
                Physics.IgnoreCollision(traveller.gameObject.GetComponent<CharacterController>(), tempConnectedWall.GetComponent<BoxCollider>(), false);
            }
            if (trackedTravellers.Contains(traveller))
            {
                trackedTravellers.Remove(traveller);
            }
        }
    }


    void InsidePortal(PortalTraveller traveller) { 
    
        if (traveller.tag == "Player")
        {
            Physics.IgnoreCollision(traveller.gameObject.GetComponent<CharacterController>(), tempConnectedWall.GetComponent<BoxCollider>(), true);
        }
    
    }

}
