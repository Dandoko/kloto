using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTraveller : MonoBehaviour
{

    private Vector3 objToPortal1;
    private Vector3 objToPortal2;
    private Vector3 rotObjToPortal1;
    private Vector3 rotObjToPortal2;
    private Quaternion portalRotDif;
    private Matrix4x4 rotDifMatrix;
    public Vector3 travelPos;
    public Vector3 prevRelPortalPos;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 travelPos = transform.position;
    }

    public void Teleport(GameObject thisPortal, GameObject otherPortal)
    {
        rotObjToPortal1 = thisPortal.transform.eulerAngles - transform.eulerAngles;
        rotObjToPortal2 = rotObjToPortal1;
        transform.eulerAngles = otherPortal.transform.eulerAngles - rotObjToPortal2;


        //Gets the difference in rotations between the two portals
        portalRotDif = otherPortal.transform.rotation * Quaternion.Inverse(thisPortal.transform.rotation);
        //Calculates a transformation matrix that would rotate any vector by the difference in rotation of the two portals
        rotDifMatrix = Matrix4x4.Rotate(portalRotDif);


        objToPortal1 = transform.position - thisPortal.transform.position;
        objToPortal2 = rotDifMatrix.MultiplyPoint(objToPortal1);
        transform.position = objToPortal2 + otherPortal.transform.position;
    }

}