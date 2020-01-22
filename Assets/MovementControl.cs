using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControl : MonoBehaviour
{
    public float speed = 1f;

    Rigidbody rb;
    bool isOnTheGround;

    float h;
    float v;

    Vector3 Forward;
    Vector3 Right;

    Vector3 movDir;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isOnTheGround = false;

        h = 0f;
        v = 0f;

        Forward = Vector3.zero;
        Right = Vector3.zero;

        movDir = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        // movDir 결정하기
        float theta = transform.rotation.y * Mathf.PI;
        Forward = new Vector3(Mathf.Sin(theta), 0f, Mathf.Cos(theta));
        Right = new Vector3(Mathf.Cos(theta), 0f, -Mathf.Sin(theta));
        movDir = (Forward * v) + (Right * h);

        if (Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d"))
        {
            Move();
        }

        if (Input.GetKey("space"))
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Accelerate();
        }
        else
        {
            Deaccelerate();
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        // if the cube is on the floor...
        if (col.gameObject.CompareTag("Floor"))
        {
            isOnTheGround = true;
        }
    }

    void Move()
    {
        transform.position += movDir * speed * Time.deltaTime;
    }

    void Jump()
    {
        if(isOnTheGround)
        {
            rb.AddForce(0f, 300f, 0f);
            isOnTheGround = false;
        }
    }

    void Accelerate()
    {
        speed = 3f;
    }

    void Deaccelerate()
    {
        speed = 1f;
    }
}
