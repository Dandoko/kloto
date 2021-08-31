using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float resetYLevel = -50f;
    private Vector3 resetPos = new Vector3(23f, 3f, 17f);

    [SerializeField] Rigidbody rigidbodyChar;
    [SerializeField] PortalManager portalManager;
    [SerializeField] LayerMask portalLayer;
    [SerializeField] LayerMask portalScreenLayer;
    [SerializeField] GunManager gunManager;
    public Vector3 portalVel = new Vector3(0f, 0f, 0f);
    private Vector3 movement;

    private Vector3 desiredVel = new Vector3(0f, 0f, 0f);
    private float movementX, movementZ;
    private float speed = 5f;
    private bool isGrounded = false;
    private bool prevIsGrounded = false;
    private float jumpHeight = 10f;
    private float angleAdjustIncrement = 15f;
    // The time it takes to walk this distance should be less than footstep sound delay
    private const float footstepMaxDist = 0.4f;
    private float footstepDist;
    private Vector3 prevPos;

    // Start is called before the first frame update
    void Start()
    {
        footstepDist = 0f;
        prevPos = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        float distanceToGround = GetComponent<CapsuleCollider>().bounds.extents.y;

        LayerMask portalLayer = portalManager.getPortalLayerMask();
        //A raycast is cast downward to determine if the player is on solid ground. The portal layer is ignored.
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f, ~portalLayer);

        movementX = Input.GetAxis("Horizontal");
        movementZ = Input.GetAxis("Vertical");

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rigidbodyChar.AddForce(jumpHeight * transform.up, ForceMode.VelocityChange);
        }

        if (!isGrounded && prevIsGrounded)
        {
            footstepDist = 0f;
            SoundManager.playSound(SoundManager.Sounds.PlayerJump, null);
        }
        else if (isGrounded && !prevIsGrounded)
        {
            footstepDist = 0f;
            SoundManager.playSound(SoundManager.Sounds.PlayerLand, null);
        }

        // Using GetButton because GetAxis preserves momentum
        else if ((Input.GetButton("Vertical") || Input.GetButton("Horizontal")) && isGrounded)
        {
            footstepDist += Vector3.Distance(transform.position, prevPos);

            if (footstepDist >= footstepMaxDist)
            {
                footstepDist = 0f;
                SoundManager.playSound(SoundManager.Sounds.PlayerRun, null);
            }
        }
        else
        {
            footstepDist = 0f;
        }

        //If the portal velocity is not zero, decrease it quickly. It decreases faster if the player is on the ground
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

            //The velocity will never be exactly zero, so if it's close enough, set it to zero
            if (portalVel.magnitude < 0.1f)
            {
                portalVel = Vector3.zero;
            }
        }

        //Reset the player if they fall out of the map
        if (transform.position.y < resetYLevel) {
            transform.position = resetPos;
            rigidbodyChar.velocity = Vector3.zero;
        }

        ResetCameraUpright();

        prevIsGrounded = isGrounded;
        prevPos = transform.position;
    }

    private void FixedUpdate()
    {

        //If there is vertical velocity from the portal teleportation, add it to the player ONCE but max it out at 30
        if (portalVel.y != 0)
        {
            rigidbodyChar.AddForce(Mathf.Clamp(portalVel.y, -30f, 30f) * Vector3.up, ForceMode.VelocityChange);
            portalVel.y = 0;
        }


        movement = transform.right * movementX * speed + transform.forward * movementZ * speed;
        //Do not set the y vel. The y vel is controlled by unity's default physics engine and gravity
        desiredVel = new Vector3(movement.x + portalVel.x - rigidbodyChar.velocity.x, 0f, movement.z + portalVel.z - rigidbodyChar.velocity.z);

        //Set a max limit on movement speed, no greater than 30 
        desiredVel = new Vector3(Mathf.Clamp(desiredVel.x, -30f, 30f), 0f, Mathf.Clamp(desiredVel.z, -30f, 30f));
        rigidbodyChar.AddForce(desiredVel, ForceMode.VelocityChange);

    }

    private void ResetCameraUpright()
    {
        var targetRot = new Vector3(0f, transform.eulerAngles.y, 0f);

        transform.eulerAngles = new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, targetRot.x, Time.deltaTime * angleAdjustIncrement), Mathf.LerpAngle(transform.eulerAngles.y, targetRot.y, Time.deltaTime * angleAdjustIncrement), Mathf.LerpAngle(transform.eulerAngles.z, targetRot.z, Time.deltaTime * angleAdjustIncrement));

    }

    private void OnTriggerEnter(Collider other)
    {
        int collidingLayer = 1 << other.gameObject.layer;
        if (collidingLayer == portalLayer.value || collidingLayer == portalScreenLayer.value)
        {
            gunManager.setCollidingWithPortal(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int collidingLayer = 1 << other.gameObject.layer;
        if (collidingLayer == portalLayer.value || collidingLayer == portalScreenLayer.value)
        {
            gunManager.setCollidingWithPortal(false);
        }
    }
}