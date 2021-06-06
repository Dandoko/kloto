using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraMovement : MonoBehaviour
{

    private GameObject playerCamera;
    private GameObject lookingAtPortal;
    [SerializeField] GameObject renderingOnPortal;
    private Vector3 playerToPortal1;
    private Vector3 cameraToPortal2;
    private Quaternion rotPlayerToPortal1;
    private Quaternion rotCameraToPortal2;
    private Quaternion portalRotDif;
    private Matrix4x4 rotMatrix; 

    private Vector4 nearClipPlane;
    private Vector4 nearClipPlaneWorld;
    private Vector3[] portalCoords = new Vector3[3];
    private Plane portalScreenPlane;
    private Camera camComponent;
    private int sign;


    // Start is called before the first frame update
    void Start()
    {
        lookingAtPortal = transform.parent.gameObject;
        camComponent = GetComponent<Camera>();
        playerCamera = Camera.main.gameObject;

        portalCoords[0] = lookingAtPortal.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.max;
        portalCoords[1] = lookingAtPortal.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.min;
        portalCoords[2] = lookingAtPortal.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.center;
        portalScreenPlane = new Plane(-lookingAtPortal.transform.forward, lookingAtPortal.transform.position);

    }

    // Update is called once per frame
    void Update()
    {



        //Mimics the player camera rotations relative to portal 1
        rotPlayerToPortal1 = Quaternion.Inverse(renderingOnPortal.transform.rotation) * playerCamera.transform.rotation;
        rotCameraToPortal2 = rotPlayerToPortal1;
        transform.rotation = lookingAtPortal.transform.rotation * rotCameraToPortal2;


        //Gets the difference in rotations between the two portals
        portalRotDif = lookingAtPortal.transform.rotation * Quaternion.Inverse(renderingOnPortal.transform.rotation);
        //Calculates a transformation matrix that would rotate any vector by the difference in rotation of the two portals
        rotMatrix = Matrix4x4.Rotate(portalRotDif);

        //Positions the camera the same distance from portal 2 as the player is from portal 1
        playerToPortal1 = playerCamera.transform.position - renderingOnPortal.transform.position;
        cameraToPortal2 = rotMatrix.MultiplyPoint(playerToPortal1);
        transform.position = cameraToPortal2 + lookingAtPortal.transform.position;


        //Changes the normal vector of the plane so that the near clip plane is always facing the right direction, lowering the possibility of optical glitches
        sign = System.Math.Sign(Vector3.Dot(lookingAtPortal.transform.forward, transform.position - lookingAtPortal.transform.position));

        //Creates a near clip plane and transforms it to world coordinates
        nearClipPlane = new Vector4(portalScreenPlane.normal.x * sign, portalScreenPlane.normal.y * sign, portalScreenPlane.normal.z * sign, sign * portalScreenPlane.distance);
        nearClipPlaneWorld = Matrix4x4.Transpose(Matrix4x4.Inverse(camComponent.worldToCameraMatrix)) * nearClipPlane;

        camComponent.projectionMatrix = camComponent.CalculateObliqueMatrix(nearClipPlaneWorld);
    }
}
