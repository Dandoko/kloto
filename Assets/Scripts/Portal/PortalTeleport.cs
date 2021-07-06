using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{


    //Change thisPortal field to be parent
    private GameObject thisPortal;
    [SerializeField] GameObject otherPortal;
    [SerializeField] GameObject playerCam;

    private List<PortalTraveller> trackedTravellers;

    // Start is called before the first frame update
    void Start()
    {
        thisPortal = transform.parent.gameObject;
        trackedTravellers = new List<PortalTraveller>();



    }

    // Update is called once per frame
    void Update()
    {


        float halfHeight = Camera.main.nearClipPlane * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float halfWidth = halfHeight * Camera.main.aspect;

        float distToNearClipCorner = new Vector3(halfWidth, halfHeight, Camera.main.nearClipPlane).magnitude;
        bool camFacingSameDirAsPortal = Vector3.Dot(transform.parent.transform.forward, transform.parent.transform.position - Camera.main.transform.position) > 0;

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, distToNearClipCorner);
        transform.localPosition = Vector3.forward * distToNearClipCorner * ((camFacingSameDirAsPortal) ? 0.5f : -0.5f) + Vector3.up * 2.5f;
        
    }

    private void LateUpdate()
    {
        for (int i = 0; i < trackedTravellers.Count; i++)
        {
            
            PortalTraveller traveller = trackedTravellers[i];
            
            Vector3 curRelPortalPos = traveller.transform.position - transform.parent.transform.position;

            int prevPortalSide = System.Math.Sign(Vector3.Dot(traveller.prevRelPortalPos, transform.parent.transform.forward));
            int curPortalSide = System.Math.Sign(Vector3.Dot(curRelPortalPos, transform.parent.transform.forward));

            if (curPortalSide != prevPortalSide)
            {
                traveller.Teleport(thisPortal, otherPortal);
                trackedTravellers.RemoveAt(i);
                i--;
            }else
            {
                traveller.prevRelPortalPos = curRelPortalPos;
            }
        }
    }

    private void OnTriggerEnter(Collider collided)
    {

        GameObject hitObject = collided.gameObject;
        var traveller = hitObject.GetComponent<PortalTraveller>();
        if (traveller)
        {
            
            if (!trackedTravellers.Contains(traveller))
            {
                traveller.prevRelPortalPos = traveller.transform.position - transform.parent.transform.position;
                
                trackedTravellers.Add(traveller);
            }
        }



    }

    private void OnTriggerExit(Collider collided)
    {
        GameObject hitObject = collided.gameObject;
        var traveller = hitObject.GetComponent<PortalTraveller>();

        if (traveller) {
            if (trackedTravellers.Contains(traveller))
            {
                trackedTravellers.Remove(traveller);
            }
        }
    }
}