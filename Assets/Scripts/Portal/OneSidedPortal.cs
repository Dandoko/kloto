using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSidedPortal : MonoBehaviour
{
    private const float scaleChangeVal = 0.1f;
    private Vector3 scaleChange = new Vector3(scaleChangeVal, scaleChangeVal, scaleChangeVal);

    private OneSidedPortal linkedPortal;
    [SerializeField] GameObject connectedSurface;

    private int portalDirFacing = 0;
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
            transform.GetChild(3).GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;


            // If the render texture has not been created yet, or if the dimensions of the render texture have changed
            if (cameraTexture == null || cameraTexture.width != Screen.width || cameraTexture.height != Screen.height)
            {
                if (cameraTexture != null)
                {
                    // Release the hardware resources used by the existing render texture
                    cameraTexture.Release();
                }
                //Create a new render texture and render the child's camera to the texture
                cameraTexture = new RenderTexture(Screen.width, Screen.height, 24);
                portalCamera.targetTexture = cameraTexture;
                //Set the circular portal screen and the hidden portal screen of the linked portal to this child camera's render texture
                linkedPortal.portalScreen.material.mainTexture = cameraTexture;
                linkedPortal.transform.GetChild(3).GetComponent<MeshRenderer>().material = linkedPortal.portalScreen.material;

            }


            // Calculate the position and rotation of the portal camera using the world space
            var cameraPositionMatrix = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * playerCamera.transform.localToWorldMatrix;
            //Move the camera into the proper position relative to this portal
            portalCamera.transform.SetPositionAndRotation(cameraPositionMatrix.GetColumn(3), cameraPositionMatrix.rotation);



            //Sets the clip plane (the plane where the camera stops rendering things that are behind it) to be the same as the plane that the portal is sitting on (i.e. the wall that the portal is on)
            clipPlane = portalScreen.transform;
            //Calculates which side of the portal the player is on
            int sign = System.Math.Sign(Vector3.Dot(clipPlane.forward, portalScreen.transform.position - portalCamera.transform.position));

            //Variables to create the near clip plane
            Vector3 camSpacePos = portalCamera.worldToCameraMatrix.MultiplyPoint(clipPlane.position);
            Vector3 camSpaceNormal = portalCamera.worldToCameraMatrix.MultiplyVector(clipPlane.forward) * sign;
            float camSpaceDst = -Vector3.Dot(camSpacePos, camSpaceNormal);

            //If the camera is greater than a certain, set distance activate the near clip plane and don't render anything before the plane
            if (Mathf.Abs(camSpaceDst) > 0.2f)
            {
                //Creates the plane using a vector4
                nearClipPlane = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);
                portalCamera.projectionMatrix = playerCamera.CalculateObliqueMatrix(nearClipPlane);
            }
            else
            {
                //If the camera is close to the portal, don't render the near clip plane, render everything. The clip plane is needed if you're close because nothing would be blocking your view
                portalCamera.projectionMatrix = playerCamera.projectionMatrix;
            }



            // Renders the camera manually each update frame
            portalCamera.Render();

            //Show the screens again
            portalScreen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            transform.GetChild(3).GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
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


                //If you're on the backside of the portal (i.e. your relative z is less than the portal's, teleport)
                if (transform.InverseTransformPoint(traveller.transform.position).z * portalDirFacing < 0f)
                {
                    traveller.OneSidedTeleport(thisPortal, linkedPortal.gameObject);
                    linkedPortal.trackedTravellers.Remove(traveller);
                    trackedTravellers.RemoveAt(i);
                    i--;
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
                    //Disable collisions with the wall while the player is inside the portal
                    InsidePortal(traveller);
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
                //Reenable all the collisions if the traveller leaves the portal
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
                //If the traveller is tracked, remove it from the list
                if (trackedTravellers.Contains(traveller))
                {
                    trackedTravellers.Remove(traveller);
                }
            }
        }
    }


    void InsidePortal(PortalTraveller traveller) { 
    
        //If the traveller is inside the portal, disable all collisions with the surface the portal is on
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


            if (portalDirFacing == 0)
            {
                if (transform.InverseTransformPoint(traveller.transform.position).z < 0f)
                {
                    portalDirFacing = -1;
                }
                else if (transform.InverseTransformPoint(traveller.transform.position).z > 0f)
                {
                    portalDirFacing = 1;
                }

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
