using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControllerC : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;


    private float mouseXSensitivity = 400f;
    private float mouseYSensitivity = 400f;
    private float xRotation = 0f;
    private float yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseXSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseYSensitivity * Time.deltaTime;

        // Rotating the camera only about the x and y axes
        xRotation -= mouseY;
        yRotation -= mouseX;

        //Limiting rotation about x-axis so you can't flip the camera
        xRotation = Mathf.Clamp(xRotation, -90, 90);


        //Rotating the entire player group about the y-axis only
        transform.localRotation = Quaternion.Euler(0, -yRotation, 0);
        //Rotating the camera independently to match the mouse inputs
        playerCamera.eulerAngles = new Vector3(xRotation, -yRotation, 0);

    }
}
