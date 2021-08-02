using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTraveller : MonoBehaviour
{

    private Vector3 objToPortal1;
    private Vector3 objToPortal2;
    private Quaternion rotObjToPortal1;
    private Quaternion rotObjToPortal2;
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

    public void TwoSidedTeleport(GameObject thisPortal, GameObject otherPortal, Matrix4x4 transformMatrix, int portalSide)
    {

        transform.position = transformMatrix.GetColumn(3);
        transform.rotation = transformMatrix.rotation;
        transform.position += otherPortal.transform.forward * 0.03f * portalSide;

        /*//Gets the difference in rotations between the two portals
        portalRotDif = otherPortal.transform.rotation * Quaternion.Inverse(thisPortal.transform.rotation);
        //Calculates a transformation matrix that would rotate any vector by the difference in rotation of the two portals
        rotDifMatrix = Matrix4x4.Rotate(portalRotDif);


        objToPortal1 = transform.position - thisPortal.transform.position;
        objToPortal2 = rotDifMatrix.MultiplyPoint(objToPortal1);
        transform.position = objToPortal2 + otherPortal.transform.position + otherPortal.transform.forward * 0.03f * portalSide;
        //transform.position = objToPortal2 + otherPortal.transform.position;

        transform.rotation = otherPortal.transform.rotation * rotObjToPortal2;
        rotObjToPortal1 = Quaternion.Inverse(thisPortal.transform.rotation) * transform.rotation;
        rotObjToPortal2 = rotObjToPortal1;*/

    }

    public void OneSidedTeleport(GameObject thisPortal, GameObject otherPortal)
    {
        Vector3 relativePos = thisPortal.transform.InverseTransformPoint(transform.position);
        transform.position = otherPortal.transform.TransformPoint(relativePos);

        Quaternion relativeRot = Quaternion.Inverse(thisPortal.transform.rotation) * transform.rotation;
        transform.rotation = otherPortal.transform.rotation * relativeRot;

        if (transform.GetComponent<CharacterController>() != null)
        {
            Vector3 relativeVel = thisPortal.transform.InverseTransformDirection(transform.GetComponent<CharacterController>().velocity);
            //transform.GetComponent<CharacterController>().Move(otherPortal.transform.TransformDirection(relativeVel));
        }



    }

}
