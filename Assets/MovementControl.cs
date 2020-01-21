﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControl : MonoBehaviour
{
    public float speed = 1f;

    Rigidbody rb;
    bool isOnTheGround;

    float h;
    float v;

    Vector3 movDir;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isOnTheGround = false;

        h = 0f;
        v = 0f;

        movDir = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        // movDir 결정하기

        movDir = (Vector3.forward * v) + (Vector3.right * h);

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
        transform.position += movDir.normalized * speed * Time.deltaTime;
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
