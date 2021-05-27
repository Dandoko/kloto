using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal otherPortal;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera otherCamera;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 playerOffsetFromPortal = gameObject.transform.position - otherPortal.gameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
