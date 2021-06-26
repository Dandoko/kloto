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
    private float angleAdjustIncrement = 1f;

    private bool charIsGrounded;
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LateUpdate()
    {



        if (Mathf.Repeat(transform.eulerAngles.z, 360) != 0f)
        {

            if ((transform.eulerAngles.z % 360) > 180)
            {
                if ((transform.eulerAngles.z % 360) < (360f - angleAdjustIncrement))
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + angleAdjustIncrement);
                }
                else if (Mathf.Repeat(transform.eulerAngles.z, 360) > (360f - angleAdjustIncrement))
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
                }
            }
            else if (Mathf.Repeat(transform.eulerAngles.z, 360) <= 180)
            {
                if (Mathf.Repeat(transform.eulerAngles.z, 360) > angleAdjustIncrement)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - angleAdjustIncrement);
                }
                else if (Mathf.Repeat(transform.eulerAngles.z, 360) < angleAdjustIncrement)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
                }
            }

        }
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


        if ((transform.eulerAngles.x % 360) != 0f)
        {
            
           
            if ((transform.eulerAngles.x % 360) > 180)
            {
                if ((transform.eulerAngles.x % 360) < (360f - angleAdjustIncrement))
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x + angleAdjustIncrement, transform.eulerAngles.y, transform.eulerAngles.z);
                }
                else if ((transform.eulerAngles.x % 360) > (360f - angleAdjustIncrement))
                {
                    transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
                }
            }
            else if ((transform.eulerAngles.x % 360) <= 180)
            {
                if ((transform.eulerAngles.x % 360) > angleAdjustIncrement)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x - angleAdjustIncrement, transform.eulerAngles.y, transform.eulerAngles.z);
                }
                else if ((transform.eulerAngles.x % 360) < angleAdjustIncrement)
                {
                    transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
                }
            }

        }
 



        float movementX = Input.GetAxis("Horizontal");
        float movementZ = Input.GetAxis("Vertical");

        //Set left-right and forward-back movement relative to player view
        Vector3 movement = transform.right * movementX + transform.forward * movementZ + Vector3.up*yVel;

        controller.Move(movement * speed * Time.deltaTime);
    }
}
