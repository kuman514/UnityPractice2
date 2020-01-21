using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewControl : MonoBehaviour
{
    Rigidbody rb;

    public Camera cam;

    public float rotateSpeed = 5f;

    public float cameraRotationLimit = 80f;

    private Vector3 rotation = Vector3.zero;
    private float cameraRotation = 0f;
    private float currentCameraRotation = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        if (cam == null)
        {
            print("Error: No Camera\n");
        }
    }

    void Update()
    {
        float yRot = Input.GetAxisRaw("Mouse X");
        float xRot = Input.GetAxisRaw("Mouse Y");

        rotation = new Vector3(0f, yRot, 0f) * rotateSpeed;
        cameraRotation = xRot * rotateSpeed;
    }

    void FixedUpdate()
    {
        PreformRotation();
    }

    void PreformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (cam != null)
        {
            currentCameraRotation -= cameraRotation;
            currentCameraRotation = Mathf.Clamp(currentCameraRotation, -cameraRotationLimit, cameraRotationLimit);
            cam.transform.localEulerAngles = new Vector3(currentCameraRotation, 0f, 0f);
        }
    }
}
