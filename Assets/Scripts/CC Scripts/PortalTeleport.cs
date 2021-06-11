using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{


    //Change thisPortal field to be parent
    private GameObject thisPortal;
    [SerializeField] GameObject otherPortal;
    private GameObject playerCamera;


    private Vector3 objToPortal1;
    private Vector3 objToPortal2;
    private Vector3 rotObjToPortal1;
    private Vector3 rotObjToPortal2;
    private Quaternion portalRotDif;
    private Matrix4x4 rotDifMatrix;

    private List<GameObject> teleporters;

    // Start is called before the first frame update
    void Start()
    {
        thisPortal = transform.parent.gameObject;
        teleporters = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            foreach (var x in teleporters)
            {
                Debug.Log(x.gameObject.name);
            }
            
        }

    }
    //A teleport controller that has a public list that both screens are able to modify. When something enters a screen, check if it is already on the list, if it isn't add to list then teleport. When something exits a screen, if it is on the list, remove it from the list.

    private void OnTriggerEnter(Collider collided)
    {

        GameObject hitObject = collided.gameObject;
        teleporters.Add(hitObject);

        /*
        if (hitObject.name == "Player")
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

            
        }*/
    }

    private void OnTriggerExit(Collider collided)
    {
        GameObject hitObject = collided.gameObject;

        if (teleporters.Contains(hitObject))
        {
            teleporters.Remove(hitObject);
            Debug.Log("bruh");
        }
    }
}