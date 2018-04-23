using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] public float speed = 8.0f;
    [SerializeField] float airSpeed = 6.0f;
    [SerializeField] float accel = 20f;
    [SerializeField] float airAccel = 10f;
    [SerializeField] float decel = 15f;
    [SerializeField] float airDecel = 2f;
    [SerializeField] float gravity = 8;
    [SerializeField] float gravityMin = 16;
    [SerializeField] float landGravity = 18;

    Vector3 curMove, curMoveX;
    float xVel, yVel, xIn, yIn;
	float runDir;
    bool moving;
	Animator anim;
    CharacterController ctrl;

    void Start()
    {
        ctrl = GetComponent<CharacterController>();
		anim = GetComponentInChildren<Animator> ();
        curMove = Vector3.zero;
        xVel = 0;
        yVel = -gravity;
    }

    void Update()
    {
        
        xIn = Input.GetAxis("Horizontal");
        yIn = Input.GetAxis("Vertical");
        moving = Mathf.Abs(xIn) > 0.1f || Mathf.Abs(yIn) > 0.1f;
		anim.SetBool ("moving", moving);
		runDir = Vector3.SignedAngle (curMoveX, transform.forward, Vector3.up);
		if (runDir < 0) {
			runDir = (360 + runDir);
		}
		anim.SetFloat ("runDir", runDir / 360.0f);
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
            yVel = -gravity * 0.5f;
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
            yVel -= landGravity * Time.deltaTime;
        }
        if (moving)
        {
            curMoveX = Vector3.ClampMagnitude(new Vector3(xIn, 0, yIn), 1f);
        }
        curMove = (curMoveX * xVel) + (Vector3.up * yVel);
        ctrl.Move(curMove * Time.fixedDeltaTime);
    }

}
