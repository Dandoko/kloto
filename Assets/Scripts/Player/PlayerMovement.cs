using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    private float speed = 5f;
    private float gravity = -19.62f;
    private Vector3 velocity;

    private bool isGrounded = false;
    private bool prevIsGrounded = false;
    private float jumpHeight = 2f;
    private float angleAdjustIncrement = 2f;

    // Start is called before the first frame update
    void Start()
    {
    }

    void LateUpdate()
    {
        adjustZAngle();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        // Resetting the y velocity after landing
        if (isGrounded)
        {
            velocity.y = -2f;
        }

        float movementX = Input.GetAxis("Horizontal");
        float movementZ = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * movementX + transform.forward * movementZ;
        controller.Move(movement * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Applying gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (!isGrounded && prevIsGrounded)
        {
            SoundManager.playSound(SoundManager.Sounds.PlayerJump);
        }
        else if (isGrounded && !prevIsGrounded)
        {
            SoundManager.playSound(SoundManager.Sounds.PlayerLand);
        }
        else if ((Mathf.Abs(movementX) > 0 || Mathf.Abs(movementZ) > 0) && isGrounded)
        {
            SoundManager.playSound(SoundManager.Sounds.PlayerRun);
        }

        adjustXAngle();

        prevIsGrounded = isGrounded;
    }

    void adjustXAngle()
    {
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
    }

    void adjustZAngle()
    {
        if ((transform.eulerAngles.z % 360) != 0f)
        {
            if ((transform.eulerAngles.z % 360) > 180)
            {
                if ((transform.eulerAngles.z % 360) < (360f - angleAdjustIncrement))
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + angleAdjustIncrement);
                }
                else if ((transform.eulerAngles.z % 360) > (360f - angleAdjustIncrement))
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
                }
            }
            else if ((transform.eulerAngles.z % 360) <= 180)
            {
                if ((transform.eulerAngles.z % 360) > angleAdjustIncrement)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - angleAdjustIncrement);
                }
                else if ((transform.eulerAngles.z % 360) < angleAdjustIncrement)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
                }
            }
        }
    }

}