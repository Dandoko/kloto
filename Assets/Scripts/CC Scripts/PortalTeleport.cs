using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{


    //Change thisPortal field to be parent
    private GameObject thisPortal;
    [SerializeField] GameObject otherPortal;
    private GameObject playerCamera;
    private GameObject teleController;

    private Vector3 objToPortal1;
    private Vector3 objToPortal2;
    private Vector3 rotObjToPortal1;
    private Vector3 rotObjToPortal2;
    private Quaternion portalRotDif;
    private Matrix4x4 rotDifMatrix;
    private int oldSidePortal;

    // Start is called before the first frame update
    void Start()
    {
        thisPortal = transform.parent.gameObject;
        teleController = GameObject.Find("Teleport Controller");
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void OnTriggerEnter(Collider collided)
    {

        GameObject hitObject = collided.gameObject;


        if (!teleController.GetComponent<TeleportController>().teleporters.Contains(hitObject))
        {
            teleController.GetComponent<TeleportController>().teleporters.Add(hitObject);
            

            if (hitObject.tag == "Player")
            {
                playerCamera = hitObject.GetComponentInChildren<Camera>().gameObject;


                rotObjToPortal1 = thisPortal.transform.eulerAngles - playerCamera.transform.eulerAngles;
                rotObjToPortal2 = rotObjToPortal1;
                playerCamera.transform.eulerAngles = otherPortal.transform.eulerAngles - rotObjToPortal2;


                //Gets the difference in rotations between the two portals
                portalRotDif = otherPortal.transform.rotation * Quaternion.Inverse(thisPortal.transform.rotation);
                //Calculates a transformation matrix that would rotate any vector by the difference in rotation of the two portals
                rotDifMatrix = Matrix4x4.Rotate(portalRotDif);


                objToPortal1 = hitObject.transform.position - thisPortal.transform.position;
                objToPortal2 = rotDifMatrix.MultiplyPoint(objToPortal1);
                hitObject.transform.position = objToPortal2 + otherPortal.transform.position;


            }
        }

    }

    private void OnTriggerExit(Collider collided)
    {
        GameObject hitObject = collided.gameObject;

        if (teleController.GetComponent<TeleportController>().teleporters.Contains(hitObject))
        {
            //teleController.GetComponent<TeleportController>().teleporters.Remove(hitObject);
        }
    }
}