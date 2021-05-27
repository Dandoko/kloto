using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraMovement : MonoBehaviour
{

    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject lookingAtPortal;
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
        camComponent = GetComponent<Camera>();

        portalCoords[0] = lookingAtPortal.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.max;
        portalCoords[1] = lookingAtPortal.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.min;
        portalCoords[2] = lookingAtPortal.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.center;
        //portalScreenPlane = new Plane(-Vector3.Cross(portalCoords[0], portalCoords[1]), portalCoords[2]);
        portalScreenPlane = new Plane(-lookingAtPortal.transform.forward, lookingAtPortal.transform.position);

        DrawPlane(portalCoords[2], portalScreenPlane.normal);
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
        nearClipPlane = new Vector4(portalScreenPlane.normal.x * sign, portalScreenPlane.normal.y * sign, portalScreenPlane.normal.z * sign, portalScreenPlane.distance * sign);
        nearClipPlaneWorld = Matrix4x4.Transpose(Matrix4x4.Inverse(camComponent.worldToCameraMatrix)) * nearClipPlane;

        camComponent.projectionMatrix = camComponent.CalculateObliqueMatrix(nearClipPlaneWorld);
    }


    //FOR DEBUGGING vv
    void DrawPlane(Vector3 position,Vector3 normal )
    {

        Vector3 v3;

        if (normal.normalized != Vector3.forward)
            v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
        else
            v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude; ;

        var corner0 = position + v3;
        var corner2 = position - v3;
        var q = Quaternion.AngleAxis(90.0f, normal);
        v3 = q * v3;
        var corner1 = position + v3;
        var corner3 = position - v3;

        Debug.DrawLine(corner0, corner2, Color.green, 100);
        Debug.DrawLine(corner1, corner3, Color.green, 100);
        Debug.DrawLine(corner0, corner1, Color.green, 100);
        Debug.DrawLine(corner1, corner2, Color.green, 100);
        Debug.DrawLine(corner2, corner3, Color.green, 100);
        Debug.DrawLine(corner3, corner0, Color.green, 100);
        Debug.DrawRay(position, normal, Color.red, 100);
    }
}
