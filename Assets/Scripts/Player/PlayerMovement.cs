using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    private float speed = 8.5f;
    private float jumpHeight = 3f;
    private float gravity = -9.81f;
    private float yVel;


    private bool charIsGrounded;
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        charIsGrounded = controller.isGrounded;



        if (charIsGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                yVel = jumpHeight;
            }
            else
            {
                yVel = -2f;
            }
        }
        else
        {
            //Applying gravity
            yVel += gravity * Time.deltaTime;
        }



        float movementX = Input.GetAxis("Horizontal");
        float movementZ = Input.GetAxis("Vertical");

        //Set left-right and forward-back movement relative to player view
        Vector3 movement = transform.right * movementX + transform.forward * movementZ + Vector3.up*yVel;

        controller.Move(movement * speed * Time.deltaTime);
    }
}
