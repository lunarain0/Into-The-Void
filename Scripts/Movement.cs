using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public float jumpHeight;
    public float speed;
    public float fallSpeed;
    public GameObject player;
    public Transform target;
    bool doubleJumpAvailable = false;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {

        Vector3 movement = new Vector3(Input.GetAxis("HorizontalMovement"), 0, -Input.GetAxis("VerticalMovement"));
        transform.Translate(movement * Time.deltaTime * speed, target);
    }

    void FixedUpdate()
    {
        rb.drag = 0;
        if (doubleJumpAvailable == true)
        {
            if (Input.GetButtonDown("Jump"))
            {
                doubleJumpAvailable = false;
                rb.AddForce(800 * Vector3.up);
            }
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 0.1F))
        {
            doubleJumpAvailable = false;

            if (Input.GetButtonDown("Jump"))
            {
                doubleJumpAvailable = true;
                rb.AddForce(800 * Vector3.up);

            }
        }

        if (transform.position.y >= jumpHeight)
        {
            rb.AddForce(fallSpeed * Vector3.down);

        }

        if (Input.GetButtonDown("Heavy"))
        {
            Vector3 destination = transform.position + (player.transform.TransformDirection(Vector3.forward) * 10);
            transform.position = Vector3.Lerp(transform.position, destination, 10);
        }

        if (Input.GetButtonDown("Light"))
        {
            Vector3 destination = transform.position + (player.transform.TransformDirection(Vector3.forward) * 5);
            transform.position = Vector3.Lerp(transform.position, destination, 5);
        }
    }
}
