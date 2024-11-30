using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sliding : MonoBehaviour
{


    [Header("References")]

    public Transform orientation;
    public Transform Player;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Keybind")]
    public KeyCode slideKey = KeyCode.LeftControl;

    private float horizontalInput;
    private float verticalInput;
   

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        startYScale = Player.localScale.y;
    }

    private void Update()
    {
        // A and D keys
        horizontalInput = Input.GetAxisRaw("Horizontal");

        //W and S keys 
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0)) 
        {
            startSlide();
        }

        if (Input.GetKeyUp(slideKey) && pm.sliding) 
        {
            stopSlide();
        }
    }

    private void FixedUpdate()
    {
        if (pm.sliding)
            slideMovement();
    }


    private void startSlide()
    {
        pm.sliding = true;

        //this may need to be changed to adj height instead of Y scale will see once assets are in
        Player.localScale = new Vector3(Player.localScale.x, slideYScale, Player.localScale.z);
        
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void slideMovement()
    {
        //directional slide
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;


        //if not on slope
        if(!pm.onSlope() || rb.velocity.y > -.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }

        //if on slope
        else
        {
            rb.AddForce(pm.GetSlopeDirection(inputDirection) * slideForce, ForceMode.Force);
        }
        
        

        if (slideTimer <= 0) 
        {
            stopSlide();
        }
    }

    

    private void stopSlide()
    {
        pm.sliding = false;
        Player.localScale = new Vector3(Player.localScale.x, startYScale, Player.localScale.z);
    }
}
