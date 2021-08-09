using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTraveller : MonoBehaviour
{

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

        
        rotObjToPortal1 = Quaternion.Inverse(thisPortal.transform.rotation) * transform.rotation;
        rotObjToPortal2 = rotObjToPortal1;
        transform.rotation = otherPortal.transform.rotation * rotObjToPortal2;*/

    }

    public void OneSidedTeleport(GameObject thisPortal, GameObject otherPortal)
    {
        GameObject thisScreen = thisPortal.transform.GetChild(0).gameObject;
        GameObject otherScreen = otherPortal.transform.GetChild(0).gameObject;
        Quaternion portalRotDif = otherPortal.transform.rotation * Quaternion.Inverse(thisPortal.transform.rotation);
        Matrix4x4 rotDifMatrix = Matrix4x4.Rotate(portalRotDif);

        
        Vector3 relativePos = transform.position - thisScreen.transform.position;
        Vector3 rotatedRelPos = rotDifMatrix.MultiplyPoint(relativePos);
        transform.position = rotatedRelPos + otherScreen.transform.position;


        Quaternion relativeRot = Quaternion.Inverse(thisScreen.transform.rotation) * transform.rotation;
        transform.rotation = otherScreen.transform.rotation * relativeRot;

        if (transform.tag == "Player")
        {
            PlayerMovement playerScript = transform.GetComponent<PlayerMovement>();


            Vector3 transformedVel = rotDifMatrix.MultiplyVector(playerScript.velocity);
            transform.GetComponent<CharacterController>().Move(transformedVel);
        }



    }

}
