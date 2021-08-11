using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbodyChar;
    public Vector3 portalVel = new Vector3(0f, 0f, 0f);

    private float speed = 5f;

    private float yVel;
    private bool isGrounded = false;
    private bool prevIsGrounded = false;
    private float jumpHeight = 7f;
    private float angleAdjustIncrement = 15f;


    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        float distanceToGround = GetComponent<CapsuleCollider>().bounds.extents.y;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.1f);


        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rigidbodyChar.AddForce(transform.up * jumpHeight, ForceMode.VelocityChange);
        }



        float movementX = Input.GetAxis("Horizontal");
        float movementZ = Input.GetAxis("Vertical");

        Vector3 movement = portalVel + transform.right * movementX * speed + transform.forward * movementZ * speed + transform.up * rigidbodyChar.velocity.y;
        movement = new Vector3(Mathf.Clamp(movement.x, -30f, 30f), Mathf.Clamp(movement.y, -30f, 30f), Mathf.Clamp(movement.z, -30f, 30f));
        rigidbodyChar.velocity = movement;


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
                portalVel = Vector3.Lerp(portalVel, Vector3.zero, 0.1f);
            }
            else if (!isGrounded)
            {
                portalVel = Vector3.Lerp(portalVel, Vector3.zero, 0.01f);
            }
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