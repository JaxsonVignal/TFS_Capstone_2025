using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    [Header ("Keybinds")]

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Player Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    [Header("Crouching Settings")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;



    public Transform orientation;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool CanJump;

    public float groundDrag;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    public enum MoveState
    {
        walking, 
        sprinting,
        crouching,
        air
    }

    public MoveState state;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask WhatIsGround;
    bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        CanJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, WhatIsGround);
        MyInput();
        SpeedControl();
        StateHandler();

        //groundDrag settings 
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && CanJump && grounded){

            CanJump = false;
            Jump();

            Invoke(nameof(resetJump), jumpCooldown);
        }

        //start crouch

        //change to  modify height instead 
        if (Input.GetKeyDown(crouchKey) && grounded)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        //stop crouch 
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        

        //change to moveplayer
        //colide and slide for slope detection
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        else if(!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f *airMultiplier, ForceMode.Force);
        }

    }

    private void SpeedControl()
    {
        Vector3 vel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

       
        if (vel.magnitude > moveSpeed)
        {
             Vector3 velLimit = vel.normalized * moveSpeed;
             rb.velocity = new Vector3(velLimit.x, rb.velocity.y, velLimit.z);
        } 
        
    }

    private void Jump()
    {
        //look into kin for jumping 

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void resetJump()
    {
        CanJump = true;
    }

    private void StateHandler()
    {

        //crouch state
        if(Input.GetKey(crouchKey))
        {
            state = MoveState.crouching;
            moveSpeed = crouchSpeed;
        }

        // sprint state
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MoveState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // walking state
        else if (grounded)
        {
            state = MoveState.walking;
            moveSpeed = walkSpeed;
        }

        //air state
        else
        {
            state = MoveState.air;
        }
    }
}
