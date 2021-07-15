using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementDaniel : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    private float speed = 5f;
    private float gravity = -19.62f;
    private Vector3 velocity;

    private float groundCheckRadius = 0.4f;
    private bool isGrounded = false;
    private bool prevIsGrounded = false;
    private float jumpHeight = 2f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Resetting the y velocity after landing
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        if (isGrounded && velocity.y < 0)
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

        prevIsGrounded = isGrounded;
    }

}
