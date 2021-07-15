using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTouch : MonoBehaviour
{
    // Start is called before the first frame updates
    void Start()
    {
        Physics.IgnoreCollision(transform.GetComponent<MeshCollider>(), Camera.main.transform.parent.GetComponent<CharacterController>(), true);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider collided)
    {

        if (collided.tag == "MainCamera")
        {
            int sideOfPlayer = System.Math.Sign(Vector3.Dot(transform.forward, transform.position - collided.transform.position));
            Debug.Log("Inside");

            collided.transform.parent.transform.position -= transform.forward * 0.05f * sideOfPlayer;



        }
    }

}
