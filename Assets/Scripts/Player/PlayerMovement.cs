using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    private float speed = 5f;
    private float gravity = -9.81f;
    private float yVel;
    public Vector3 velocity;
    

    private bool isGrounded = false;
    private bool prevIsGrounded = false;
    private float jumpHeight = 3f;
    private float angleAdjustIncrement = 15f;


    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            yVel = jumpHeight;
        }

        if (!isGrounded)
        {
            //Applying gravity
            //Added because the gravity coefficient is negative
            yVel += gravity * Time.deltaTime;
        }

        float movementX = Input.GetAxis("Horizontal");
        float movementZ = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * movementX + transform.forward * movementZ + Vector3.up * yVel;
        velocity = movement * speed;


        // Applying gravity
        controller.Move(velocity * Time.deltaTime);

        if (!isGrounded && prevIsGrounded)
        {
            SoundManager.playSound(SoundManager.Sounds.PlayerJump, null);
        }
        else if (isGrounded && !prevIsGrounded)
        {
            SoundManager.playSound(SoundManager.Sounds.PlayerLand, null);
        }
        else if ((Mathf.Abs(movementX) > 0 || Mathf.Abs(movementZ) > 0) && isGrounded)
        {
            SoundManager.playSound(SoundManager.Sounds.PlayerRun, null);
        }


        ResetCameraUpright();

        prevIsGrounded = isGrounded;
    }


    private void ResetCameraUpright()
    {
        var targetRot = new Vector3(0f, transform.eulerAngles.y, 0f);

        transform.eulerAngles = new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, targetRot.x, Time.deltaTime * angleAdjustIncrement), Mathf.LerpAngle(transform.eulerAngles.y, targetRot.y, Time.deltaTime * angleAdjustIncrement), Mathf.LerpAngle(transform.eulerAngles.z, targetRot.z, Time.deltaTime * angleAdjustIncrement));
    
    }

}