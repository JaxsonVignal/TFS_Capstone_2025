using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Keybinds")]

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Player Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float wallrunSpeed;
    private float desiredSpeed;
    private float lastDesiredSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    [Header("Crouching Settings")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Slope Settings")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;


    [Header("Jump Settings")]
    public Transform orientation;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public float coyoteTime;
    public float gravityMult;
    private float coyoteTimeCounter;
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
        sliding,
        wallrunning,
        slippy,
        air
    }

    public bool sliding;
    public bool wallrunning;
    public bool hitJumpPad;
    public MoveState state;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask WhatIsGround;
    public bool grounded;
    bool isSlippyBoy;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        CanJump = true;
        isSlippyBoy = false;
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
        {
            rb.drag = groundDrag;
            coyoteTimeCounter = coyoteTime;
            hitJumpPad = false;
        }
        else
        {
            rb.drag = 0;
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (state == MoveState.air && rb.velocity.y <= 0)
        {


            rb.AddForce(Physics.gravity * Mathf.Lerp(1f, gravityMult, 2f));
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && CanJump && coyoteTimeCounter > 0f && !hitJumpPad)
        {

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

        //on slope

        if (onSlope())
        {
            Vector3 slopeDir = GetSlopeDirection(moveDirection);
            rb.velocity = new Vector3(slopeDir.x * moveSpeed, rb.velocity.y, slopeDir.z * moveSpeed);  // Apply movement on slope with modified velocity
        }


        //change to moveplayer
        //colide and slide for slope detection
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
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
        coyoteTimeCounter = 0f;
    }

    private void resetJump()
    {
        CanJump = true;
    }

    private void StateHandler()
    {
        //wallrunning state

        if (wallrunning)
        {
            state = MoveState.wallrunning;
            desiredSpeed = wallrunSpeed;
        }


        // slide state
        else if (sliding)
        {
            state = MoveState.sliding;

            if (onSlope() && rb.velocity.y < 0.1f)
            {
                desiredSpeed = slideSpeed;
            }

            else
            {
                desiredSpeed = sprintSpeed;
            }
        }

        //crouch state
        else if (Input.GetKey(crouchKey))
        {
            state = MoveState.crouching;
            desiredSpeed = crouchSpeed;
        }

        // sprint state (make sure the player cant enter sprint state while crouching, causes acceleration issues)
        else if (grounded && Input.GetKey(sprintKey) && state != MoveState.crouching)
        {
            state = MoveState.sprinting;
            desiredSpeed = sprintSpeed;
        }

        // walking state
        else if (grounded)
        {
            state = MoveState.walking;
            desiredSpeed = walkSpeed;
        }

        else if (grounded && isSlippyBoy)
        {
            desiredSpeed = 40f;
        }




        //air state
        else
        {
            state = MoveState.air;
            isSlippyBoy = false;
        }


        //acceleration hndling 
        if (Mathf.Abs(desiredSpeed - lastDesiredSpeed) > 4f && moveSpeed != 0 && state != MoveState.crouching && !isSlippyBoy)
        {
            StopAllCoroutines();
            StartCoroutine(acceleration());
        }
        else
        {
            moveSpeed = desiredSpeed;
        }

        lastDesiredSpeed = desiredSpeed;
    }

    public bool onSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * .5f + .3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    private IEnumerator acceleration()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredSpeed, time / difference);

            if (onSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }

            else
            {
                time += Time.deltaTime * speedIncreaseMultiplier;
            }

            yield return null;
        }

        moveSpeed = desiredSpeed;

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "jumpPad")
        {
            hitJumpPad = true;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.velocity = new Vector3(rb.velocity.x, 20f, rb.velocity.z);

        }

        else if (other.gameObject.tag == "superJumpPad")
        {
            hitJumpPad = true;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.velocity = new Vector3(rb.velocity.x, 40f, rb.velocity.z);

        }

        else if (other.gameObject.tag == "speedyBoy")
        {
            isSlippyBoy = true;

        }
    }
}
