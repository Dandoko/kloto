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
    private float angleAdjustIncrement = 15f;

    // Start is called before the first frame update
    void Start()
    {
    }

    void LateUpdate()
    {
        
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

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Applying gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime + movement*speed*Time.deltaTime);

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