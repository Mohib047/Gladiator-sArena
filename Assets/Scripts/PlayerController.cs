using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variable declaration
    private Rigidbody rb;
    public float speed = 10f;
    public float jumpForce = 100;
    private bool isOnGround = true;
    private bool doubleJump = false;
    public float gravityModifier;
    private float verticalInput;
    private float horizontalInput;

    private void OnCollisionEnter(Collision collision)
    {
        isOnGround = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement Inputs
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.forward * speed * verticalInput * Time.deltaTime);
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);
        /*if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }*/
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isOnGround || doubleJump)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse );
                isOnGround = false;
                doubleJump = !doubleJump;
            }

        }
        else if (isOnGround && !Input.GetKeyDown(KeyCode.Space))
        {
            doubleJump = false;
        }
    }
}
