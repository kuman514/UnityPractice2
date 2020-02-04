using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // editable
    public float movSpeed;
    public float rotSpeed;
    public Camera cam;

    // undeitable
    float cameraRotationLimit = 0.5f;
    Rigidbody rb;
    bool isOnTheGround;
    float h;
    float v;
    float tmpMovSpd;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isOnTheGround = false;
        h = 0f;
        v = 0f;
        tmpMovSpd = movSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        MoveControl();
        RotationControl();
    }

    void RotationControl()
    {
        float yRot = Input.GetAxisRaw("Mouse X");
        float xRot = Input.GetAxisRaw("Mouse Y");

        this.transform.localRotation *= Quaternion.Euler(0, yRot * rotSpeed, 0);

        // Issue: If you turn the camera too much up, view is upside down vertically.
        cam.transform.localRotation *= Quaternion.Euler(-xRot * rotSpeed, 0, 0);
        /*
        if(-cameraRotationLimit <= cam.transform.localRotation.x && cam.transform.localRotation.x <= cameraRotationLimit)
        {
            cam.transform.localRotation *= Quaternion.Euler(-xRot * rotSpeed, 0, 0);
        }
        else
        {
            if(cam.transform.localRotation.x < -cameraRotationLimit)
            {
                cam.transform.localRotation = Quaternion.Euler(cameraRotationLimit, 0, 0);
            }
            else if(cam.transform.localRotation.x > cameraRotationLimit)
            {
                cam.transform.localRotation = Quaternion.Euler(cameraRotationLimit, 0, 0);
            }
        }
        */
    }

    void MoveControl()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d"))
        {
            transform.Translate(new Vector3(h, 0, v) * movSpeed * Time.deltaTime, Space.Self);
        }

        if (Input.GetKey("space"))
        {
            // Jump
            if (isOnTheGround)
            {
                rb.AddForce(0f, 300f, 0f);
                isOnTheGround = false;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            // Accerlate
            movSpeed = 2 * tmpMovSpd;
        }
        else
        {
            // Deaccerlate
            movSpeed = tmpMovSpd;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        // if the player is on the floor
        if (col.gameObject.CompareTag("Floor"))
        {
            isOnTheGround = true;
        }
    }
}
