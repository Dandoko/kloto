using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] private Transform playerBody;

    private float sensitivity = 2.0f;
    private float smoothing = 2.0f;
    private float mouseSpeedFactor = 25.0f;
    private Vector2 mouseMovement;
    private Vector2 smoothV;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        mouseMovement = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        mouseMovement = sensitivity * smoothing * mouseMovement * mouseSpeedFactor * Time.deltaTime;
        smoothV.x = Mathf.Lerp(smoothV.x, mouseMovement.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, mouseMovement.y, 1f / smoothing);

        Vector3 modifiedEulers = transform.localEulerAngles + Vector3.left * smoothV.y * Time.deltaTime;

        //Transform euler angles from [0,360) to [-180,180) before clamp
        modifiedEulers.x = Mathf.Repeat(modifiedEulers.x + 180f, 360f) - 180f;
        modifiedEulers.x = Mathf.Clamp(modifiedEulers.x, -90f, 90f);

        //Rotate the camera about the x axis
        transform.localEulerAngles = modifiedEulers;
        //Rotate the body about the y axis
        playerBody.transform.Rotate(0f, smoothV.x, 0f);
    }
}
