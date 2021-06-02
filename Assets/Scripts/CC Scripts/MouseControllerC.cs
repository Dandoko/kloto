using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControllerC : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;


    private float mouseXSensitivity = 300f;
    private float mouseYSensitivity = 300f;
    private Vector2 rotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {


        rotation.y += Input.GetAxis("Mouse X") * mouseXSensitivity * Time.deltaTime;
        rotation.x -= Input.GetAxis("Mouse Y") * mouseYSensitivity * Time.deltaTime;

        //Limiting rotation about x-axis so you can't flip the camera
        rotation.x = Mathf.Clamp(rotation.x, -90, 90);


        //Rotating the entire player group about the y-axis only
        transform.eulerAngles = new Vector3(0, rotation.y, 0);
        //Rotating the camera independently to match the mouse inputs
        playerCamera.eulerAngles = new Vector3 (rotation.x, rotation.y, 0);
    }
}
