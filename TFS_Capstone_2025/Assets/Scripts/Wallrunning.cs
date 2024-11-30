using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallrunning : MonoBehaviour
{
    [Header("Wallrunning Settings")]
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;

    public float wallrunForce;
    public float maxWallrunTime;
    private float wallrunTimer;

    [Header("inputs")]
    private float horizaontalInput;
    private float verticalInput;

    [Header("Wall Run Directional Settings")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit LeftWallHit;
    private RaycastHit RightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("References")]
    public Transform orientation;
    private PlayerMovement pm;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning) 
        {
            wallrunningMovement();
        }

    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out RightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out LeftWallHit, wallCheckDistance, whatIsWall);
    }

    private bool isOffGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        //get input 
        horizaontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //wallrunning
        if ((wallLeft || wallRight) && verticalInput > 0 && isOffGround())
        {
            if (!pm.wallrunning) 
            {
                startWallrun();
            }
           
        }

        else
        {
            if (pm.wallrunning)
            {
                stopWallrun();
            }
        }

    }

    private void startWallrun()
    {
        pm.wallrunning = true;
    }

    private void wallrunningMovement()
    {
        //turn off gravity while on wall and kill y momentum 
        rb.useGravity = false;

        //lerp the y velocity to sim gravity while on wall
        rb.velocity = new Vector3(rb.velocity.x, Mathf.Lerp(0f, -1f, 2f), rb.velocity.z);


        Vector3 wallNormal = wallRight ? RightWallHit.normal : LeftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        //flip normal orientation if needed 
        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude) 
        {
            wallForward = -wallForward;
        }

        //add wall run force
        rb.AddForce(wallForward * wallrunForce, ForceMode.Force);
    }

    private void stopWallrun()
    {
        pm.wallrunning = false;
        rb.useGravity = true; // Re-enable gravity when the wallrun stops.
    }
}
