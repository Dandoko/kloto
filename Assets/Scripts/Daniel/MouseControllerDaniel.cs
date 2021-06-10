using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControllerDaniel : MonoBehaviour
{
    [SerializeField] private Transform playerBody;

    private float mouseXSensitivity = 300f;
    private float mouseYSensitivity = 200f;
    private float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseXSensitivity * Time.deltaTime;
        float mouseY= Input.GetAxis("Mouse Y") * mouseYSensitivity * Time.deltaTime;

        //Limiting rotation about x-axis so you can't flip the camera
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
