using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform groundCheckTransform = null;
    [SerializeField] private LayerMask playerMask;

    private bool spacebarPressed;
    private float horizontalInput;
    private Rigidbody rigidbodyComponent;
    private int superJumpsRemaining;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
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
        rigidbodyComponent.velocity = new Vector3(horizontalInput * 2, rigidbodyComponent.velocity.y, 0);

        // If the groundCheck collider is not colliding with anything (using a LayerMask to avoid colliding with the player itself)
        if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0)
        {
            return;
        }

        if (spacebarPressed)
        {
            float jumpPower = 5f;
            if (superJumpsRemaining > 0)
            {
                jumpPower *= 1.3f;
                superJumpsRemaining--;
            }

            rigidbodyComponent.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            spacebarPressed = false;
        }
    }

    // Gets called when collided with the Is Trigger field enabled
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            Destroy(other.gameObject);
            superJumpsRemaining++;
        }
    }
}
