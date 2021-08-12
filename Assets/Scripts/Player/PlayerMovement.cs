using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbodyChar;
    public Vector3 portalVel = new Vector3(0f, 0f, 0f);
    public Vector3 movement;

    private Vector3 desiredVel = new Vector3(0f, 0f, 0f);
    private float gravity = 9.81f;
    private float movementX, movementZ;
    private float speed = 5f;
    private bool isGrounded = false;
    private bool prevIsGrounded = false;
    private float jumpHeight = 5f;
    private float angleAdjustIncrement = 15f;
    private bool jumped = false;

    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        float distanceToGround = GetComponent<CapsuleCollider>().bounds.extents.y;
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);

        movementX = Input.GetAxis("Horizontal");
        movementZ = Input.GetAxis("Vertical");

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rigidbodyChar.AddForce(jumpHeight * transform.up, ForceMode.VelocityChange);
        }


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




        if (portalVel != Vector3.zero) {
            if (isGrounded)
            {
                portalVel.x = Mathf.Lerp(portalVel.x, 0f, 0.1f);
                portalVel.z = Mathf.Lerp(portalVel.z, 0f, 0.1f);
            }
            else if (!isGrounded)
            {
                portalVel.x = Mathf.Lerp(portalVel.x, 0f, 0.01f);
                portalVel.z = Mathf.Lerp(portalVel.z, 0f, 0.01f);
            }

            if (portalVel.magnitude < 0.1f)
            {
                portalVel = Vector3.zero;
            }
        }

        if (transform.position.y < -50f) {
            transform.position = Vector3.zero + Vector3.up*5f;
        }



        ResetCameraUpright();


        prevIsGrounded = isGrounded;
    }

    private void FixedUpdate()
    {

        if (portalVel.y != 0)
        {
            rigidbodyChar.AddForce(portalVel.y * Vector3.up, ForceMode.VelocityChange);
            portalVel.y = 0;
        }

        movement = transform.right * movementX * speed + transform.forward * movementZ * speed;
        desiredVel = new Vector3(movement.x + portalVel.x - rigidbodyChar.velocity.x, 0f, movement.z + portalVel.z - rigidbodyChar.velocity.z);

        rigidbodyChar.AddForce(desiredVel, ForceMode.VelocityChange);

    }

    private void ResetCameraUpright()
    {
        var targetRot = new Vector3(0f, transform.eulerAngles.y, 0f);

        transform.eulerAngles = new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, targetRot.x, Time.deltaTime * angleAdjustIncrement), Mathf.LerpAngle(transform.eulerAngles.y, targetRot.y, Time.deltaTime * angleAdjustIncrement), Mathf.LerpAngle(transform.eulerAngles.z, targetRot.z, Time.deltaTime * angleAdjustIncrement));
    
    }

}