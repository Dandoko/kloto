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

    private Vector4 nearClipPlane;
    private Vector3[] portalCoords = new Vector3[3];
    private Plane portalScreenPlane;
    private Camera camComponent;


    // Start is called before the first frame update
    void Start()
    {
        portalCoords[0] = lookingAtPortal.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.max;
        portalCoords[1] = lookingAtPortal.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.min;
        portalCoords[2] = lookingAtPortal.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.center;
        portalScreenPlane = new Plane(portalCoords[0], portalCoords[1], portalCoords[2]);

        camComponent = GetComponent<Camera>();

        DrawPlane(portalCoords[2], Vector3.Cross(portalCoords[0], portalCoords[1]));
    }

    // Update is called once per frame
    void Update()
    {

        //Needs to be fixed so that angle of portal is accounted for
        playerToPortal1 = playerCamera.transform.position - renderingOnPortal.transform.position;
        cameraToPortal2 = playerToPortal1;
        transform.position = cameraToPortal2 + lookingAtPortal.transform.position;

        //Mimics the player camera rotations relative to portal 1
        rotPlayerToPortal1 = playerCamera.transform.rotation * Quaternion.Inverse(renderingOnPortal.transform.rotation);
        rotCameraToPortal2 = rotPlayerToPortal1;
        transform.rotation =  lookingAtPortal.transform.rotation * rotCameraToPortal2;


        //nearClipPlane = new Vector4(portalScreenPlane.normal.x, portalScreenPlane.normal.y, portalScreenPlane.normal.z, -Vector3.Dot(portalScreenPlane.normal, transform.position));


        //camComponent.projectionMatrix = camComponent.CalculateObliqueMatrix(nearClipPlane);
    }


    //FOR DEBUGGING
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
