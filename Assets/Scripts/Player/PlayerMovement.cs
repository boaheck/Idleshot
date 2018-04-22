using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float speed = 8.0f;
    [SerializeField] float airSpeed = 6.0f;
    [SerializeField] float accel = 20f;
    [SerializeField] float airAccel = 10f;
    [SerializeField] float decel = 15f;
    [SerializeField] float airDecel = 2f;
    [SerializeField] float jumpVel = 6;
    [SerializeField] float gravity = 8;
    [SerializeField] float gravityMin = 16;
    [SerializeField] float landGravity = 18;
    [SerializeField] float jumpBufferTime = 0.1f;

    Vector3 curMove, curMoveX;
    float xVel, yVel, xIn, yIn, jumpBuffer;
    bool jump, jumping, jumpHeld, moving;

    CharacterController ctrl;

    void Start()
    {
        ctrl = GetComponent<CharacterController>();
        curMove = Vector3.zero;
        jumpBuffer = 0;
        xVel = 0;
        yVel = -gravity;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            jumpBuffer = jumpBufferTime;
        }
        jumpHeld = Input.GetButton("Jump") && jumping;
        xIn = Input.GetAxis("Horizontal");
        yIn = Input.GetAxis("Vertical");
        moving = Mathf.Abs(xIn) > 0.1f || Mathf.Abs(yIn) > 0.1f;
        if (jump)
        {
            if (jumpBuffer > 0)
            {
                jumpBuffer -= Time.deltaTime;
            }
            else
            {
                jump = false;
            }
        }
        else if (jumpBuffer > 0)
        {
            jumpBuffer = 0;
        }
    }

    void FixedUpdate()
    {
        if (ctrl.isGrounded)
        {
            if (moving)
            {
                xVel = Mathf.MoveTowards(xVel, speed, accel * Time.fixedDeltaTime);
            }
            else
            {
                xVel = Mathf.MoveTowards(xVel, 0, decel * Time.fixedDeltaTime);
            }
            if (jump)
            {
                jumping = true;
                yVel = jumpVel;
            }
            else
            {
                yVel = -gravity * 0.5f;
            }
        }
        else
        {
            if (moving)
            {
                if (xVel > airSpeed)
                {
                    xVel = Mathf.MoveTowards(xVel, airSpeed, airDecel * Time.fixedDeltaTime);
                }
                else
                {
                    xVel = Mathf.MoveTowards(xVel, airSpeed, airAccel * Time.fixedDeltaTime);
                }
            }
            else
            {
                xVel = Mathf.MoveTowards(xVel, 0, airDecel * Time.fixedDeltaTime);
            }
            if (yVel > 0)
            {
                if (jumpHeld)
                {
                    yVel -= gravity * Time.deltaTime;
                }
                else
                {
                    yVel -= gravityMin * Time.deltaTime;
                    jumping = false;
                }
            }
            else
            {
                yVel -= landGravity * Time.deltaTime;
            }
        }
        if (moving)
        {
            curMoveX = Vector3.ClampMagnitude(new Vector3(xIn, 0, yIn), 1f);
        }
        curMove = (curMoveX * xVel) + (Vector3.up * yVel);
        ctrl.Move(curMove * Time.fixedDeltaTime);
    }

}
