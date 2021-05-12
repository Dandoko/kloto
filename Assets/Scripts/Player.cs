using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool spacebarPressed;
    private float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        // Check if space key is pressed down
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spacebarPressed = true;
        }

        // All horizontal axis inputs
        horizontalInput = Input.GetAxis("Horizontal");
    }

    // Called once every physics update (deault is 100/second)
    private void FixedUpdate()
    {
        if (spacebarPressed)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.VelocityChange);
            spacebarPressed = false;
        }

        GetComponent<Rigidbody>().velocity = new Vector3(horizontalInput, GetComponent<Rigidbody>().velocity.y, 0);
    }
}
