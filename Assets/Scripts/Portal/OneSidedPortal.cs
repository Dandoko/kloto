using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSidedPortal : MonoBehaviour
{
    private const float scaleChangeVal = 0.1f;
    private Vector3 scaleChange = new Vector3(scaleChangeVal, scaleChangeVal, scaleChangeVal);

    private OneSidedPortal linkedPortal;
    [SerializeField] GameObject connectedSurface;

    private GameObject thisPortal;
    private Camera playerCamera;
    private Camera portalCamera;
    private RenderTexture cameraTexture;
    private MeshRenderer portalScreen;
    public List<PortalTraveller> trackedTravellers;


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

    public void setPortal(OneSidedPortal linkedPortal, GameObject connectedSurface)
    {
        this.linkedPortal = linkedPortal;
        this.connectedSurface = connectedSurface;
    }

    void Update()
    {
        changePortalSize();

        if (null != linkedPortal)
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



            //Calculates and sets the clip plane
            clipPlane = portalScreen.transform;
            int sign = System.Math.Sign(Vector3.Dot(clipPlane.forward, portalScreen.transform.position - portalCamera.transform.position));

            Vector3 camSpacePos = portalCamera.worldToCameraMatrix.MultiplyPoint(clipPlane.position);
            Vector3 camSpaceNormal = portalCamera.worldToCameraMatrix.MultiplyVector(clipPlane.forward) * sign;
            float camSpaceDst = -Vector3.Dot(camSpacePos, camSpaceNormal);

            //Creates a near clip plane based on the portal screen's position
            nearClipPlane = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst + 0.1f * sign);
            portalCamera.projectionMatrix = playerCamera.CalculateObliqueMatrix(nearClipPlane);



            // Renders the camera manually each update frame
            portalCamera.Render();


            portalScreen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }



    private void LateUpdate()
    {
        if (null != linkedPortal)
        {
            //Handles and teleports all travellers if they move through the screen
            for (int i = 0; i < trackedTravellers.Count; i++)
            {

                PortalTraveller traveller = trackedTravellers[i];



               Vector3 curRelPortalPos = traveller.transform.position - portalScreen.transform.position;

               int prevPortalSide = System.Math.Sign(Vector3.Dot(traveller.prevRelPortalPos, transform.forward));
               int curPortalSide = System.Math.Sign(Vector3.Dot(curRelPortalPos, transform.forward));

                if (curPortalSide != prevPortalSide)
                {
                    traveller.OneSidedTeleport(thisPortal, linkedPortal.gameObject);
                    linkedPortal.trackedTravellers.Remove(traveller);
                    trackedTravellers.RemoveAt(i);
                    i--;
                }
                else
                {
                    traveller.prevRelPortalPos = curRelPortalPos;
                }
            }
        }
    }




    // Returns true if the player camera can see the linked portal
    // @see https://wiki.unity3d.com/index.php/IsVisibleFrom
    private bool isVisibleOnPlayerCamera()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCamera);
        return GeometryUtility.TestPlanesAABB(planes, linkedPortal.portalScreen.bounds);
    }



    private void OnTriggerEnter(Collider collided)
    {
        if (null != linkedPortal)
        {
            GameObject hitObject = collided.gameObject;
            var traveller = hitObject.GetComponent<PortalTraveller>();
            if (traveller)
            {
                if (!trackedTravellers.Contains(traveller))
                {
                    InsidePortal(traveller);
                    traveller.prevRelPortalPos = traveller.transform.position - portalScreen.transform.position + 0.05f * portalScreen.transform.forward;
                    trackedTravellers.Add(traveller);
                }
            }
        }
    }


    private void OnTriggerExit(Collider collided)
    {
        if (null != linkedPortal)
        {
            GameObject hitObject = collided.gameObject;
            var traveller = hitObject.GetComponent<PortalTraveller>();

            if (traveller)
            {
                if (traveller.tag == "Player")
                {
                    if (null != connectedSurface.GetComponent<BoxCollider>())
                    {
                        Physics.IgnoreCollision(traveller.gameObject.GetComponent<CapsuleCollider>(), connectedSurface.GetComponent<BoxCollider>(), false);
                    }
                    else if (null != connectedSurface.GetComponent<MeshCollider>())
                    {
                        Physics.IgnoreCollision(traveller.gameObject.GetComponent<CapsuleCollider>(), connectedSurface.GetComponent<MeshCollider>(), false);
                    }
                    else
                    {
                        Debug.LogError("Error: " + connectedSurface + " doesn't have a collider");
                    }
                }
                if (trackedTravellers.Contains(traveller))
                {
                    trackedTravellers.Remove(traveller);
                }
            }
        }
    }


    void InsidePortal(PortalTraveller traveller) { 
    
        if (traveller.tag == "Player")
        {
            if (null != connectedSurface.GetComponent<BoxCollider>())
            {
                Physics.IgnoreCollision(traveller.gameObject.GetComponent<CapsuleCollider>(), connectedSurface.GetComponent<BoxCollider>(), true);
            }
            else if (null != connectedSurface.GetComponent<MeshCollider>())
            {
                Physics.IgnoreCollision(traveller.gameObject.GetComponent<CapsuleCollider>(), connectedSurface.GetComponent<MeshCollider>(), true);
            }
            else
            {
                Debug.LogError("Error: " + connectedSurface + " doesn't have a collider");
            }
        }
    
    }

    private void changePortalSize()
    {
        // Checking if portal size is at it's max size using one axis
        if (transform.localScale.x < 1.0f)
        {
            transform.localScale += scaleChange;

            // Capping the scale if it goes larger than its max
            if (transform.localScale.x > 1.0f)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
    }
}
