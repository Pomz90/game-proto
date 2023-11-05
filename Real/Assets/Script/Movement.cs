using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{   
    private Collision coll;
    public Shadows ghost;
    [HideInInspector]
    public Rigidbody2D rb;
    private AnimationScript anim;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnVaultingEvent;
    private bool m_wasVault = false;

    [Space]
    [Header("Stats")]
    public float speed = 10;
    public float jumpForce = 15;
    public float wallJumpLerp = 10;
    private bool canMove= true;
public float slideSpeed = 5;
    [Space]
    [Header("Dash")]
    public bool isDashing;
    public bool canDash = true;
    private Vector2 dashDir;
    public float dashSpeed = 50;
    public float dashTime = 0.15f;
    private bool isOnCooldown = false;
    private float cooldownDuration = 1.0f; 
    [Space]
    [Header("Back Dash")]
    private Vector2 BackdashDir;
    public bool canBdash;
    public bool IsBdash;
    public float BdashSpeed = 30;
    private float BdashTime =0.1f;
    private Vector2 originalPosition;

    [Space]
    [Header("wall")]
    public bool canGrab = false;
    public bool wallGrab;
    public bool wallSlide;


    [Space]

    private bool groundTouch;

    public int side = 1;
    public bool canflip = true;

    [SerializeField] private Collider2D m_CrouchDisableCollider;
    private bool wallJumped;



    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<AnimationScript>();
    }

    // Update is called once per frame
    void Update()
    {

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);
        Walk(dir);
        anim.SetHorizontalMovement(x,y);

        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("Jump");
            if (coll.onGround)
                Jump(Vector2.up);
            if (coll.onWall && !coll.onGround)
                WallJump();

        }

        if (Input.GetButtonDown("Dash") && canDash)
        {
            if (xRaw != 0 || yRaw != 0)
                Dash(xRaw, yRaw);

        }

        if (Input.GetButtonDown("Back") )
        {
            if (coll.onGround || canBdash)
                BackStep();

        }

        if (!canBdash)
        {
            originalPosition = transform.position;
        }

        if (IsBdash) {

            StartCoroutine(DisableMovement(dashTime));
            if (canBdash)
            {
                if (y != 0)
                {

                    rb.velocity = BackdashDir.normalized * dashSpeed*0.7f;
                    return;

                }

                else rb.velocity = BackdashDir.normalized * dashSpeed*2;
                return;
            }
        }

        if (isDashing)
        {
            ghost.makeGhost = true;
            StopCoroutine(DisableMovement(0));
            StartCoroutine(DisableMovement(dashTime));
            if (y != 0)
            {

                rb.velocity = dashDir.normalized * dashSpeed * 0.7f;
                return;

            }

            else rb.velocity = dashDir.normalized * dashSpeed;
            return;
        }

        if (coll.onWall && !coll.onGround && !wallGrab)
        {
            wallGrab = true;
        }

        if (!coll.onWall || coll.onGround)
        {
            wallGrab = false;
        }

        if (coll.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }

        
        if (coll.onWall && !coll.onGround)
        {
            if (x != 0 && !canGrab)
            {
                wallSlide = true;
                WallSlide();
            }
        }

        if (coll.onGround && !isDashing)
        {
            wallJumped = false;
        }

        if (!coll.onWall || coll.onGround)
            wallSlide = false;



        if (wallSlide || !canMove)
            return;


        if (canflip) { 

        if (x > 0)
        {
            side = 1;
            anim.Flip(side);
        }
        if (x < 0)
        {
            side = -1;
            anim.Flip(side);
        }

        }

        if (canGrab) {
            canflip = false;
            if (Input.GetKey("down") )
                releseGrab();

            if (Input.GetButtonDown("Jump")) {
                releseGrab();
                Jump(Vector2.up); 
            }
        }

        


    }

    private void WallSlide()
    {
        if (!canGrab)
        {
            


            if (!canMove)
                return;

            bool pushingWall = false;
            if ((rb.velocity.x > 0 && coll.onRightWall) || (rb.velocity.x < 0 && coll.onLeftWall))
            {
                pushingWall = true;
            }
            float push = pushingWall ? 0 : rb.velocity.x;

            rb.velocity = new Vector2(push, -slideSpeed);
        }
    }

    public void Grab(Vector2 position,int side)
    {
        if (side != coll.wallSide)
            anim.Flip(side * -1);

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.position = position;
        rb.velocity = Vector2.zero;
        canGrab = true;
        
    }
    
    private void BackStep()
    {
        if (!isOnCooldown)
        {
            IsBdash = true;
            if (canBdash)
            {

                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                BackdashDir = originalPosition - (Vector2)transform.position;
                StartCoroutine(stopBdash2());


            }

            if (coll.onGround && !canBdash)
            {
                if (side == 1)
                {
                    BackdashDir = -transform.right.normalized;
                    rb.velocity = BackdashDir * BdashSpeed;
                }
                else
                {
                    BackdashDir = transform.right.normalized;
                    rb.velocity = BackdashDir * BdashSpeed;
                }

                StartCoroutine(stopBdash(BdashTime));
            }
        }
        StartCoroutine(StartCooldown());
    }

    private IEnumerator stopBdash(float time)
    {
        yield return new WaitForSeconds(time);
        IsBdash = false;
        isDashing = false;
        canDash = true;
        canBdash = false;


    }

    private IEnumerator stopBdash2()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        yield return new WaitForSeconds(dashTime);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.isKinematic = false;
        IsBdash = false;
        isDashing = false;
        canDash = true;
        canBdash = false;


    }

    private IEnumerator StartCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isOnCooldown = false;
    }

    private void Dash(float x, float y)
    {

        canBdash = true;
        isDashing = true;
        canDash = false;
        dashDir = new Vector2(x, y);
        if (dashDir == Vector2.zero)
        {
             dashDir = new Vector2(transform.localScale.x, transform.localScale.y);
        }

        
        StartCoroutine(StopDash());
    }

    private IEnumerator StopDash()
    {
        StartCoroutine(GroundDash());

        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        ghost.makeGhost = false;
        yield return new WaitForSeconds(0.5f);
        canBdash = false;
    }


    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    void RigidbodyDrag(float x)
    {
        rb.drag = x;
    }

    void GroundTouch()
    {
        canDash = true;
        isDashing = false;
        canGrab = false;
        
        side = anim.sr.flipX ? -1 : 1;
        
    }

   

    

    private void releseGrab()
    {
        anim.SetTrigger("Jump");
        canflip = true;
        canGrab = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(dashTime);
        if (coll.onGround)
            canDash = true;
    }
    

    private void Walk(Vector2 dir)
    {
        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (canGrab)
            return;

        if (!wallJumped)
        {
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }

    }

    private void Jump(Vector2 dir)
    {
        rb.velocity += dir * jumpForce;
        canGrab = false;

    }

    private void WallJump()
    {
        if ((side == 1 && coll.onRightWall) || side == -1 && !coll.onRightWall)
        {
            side *= -1;
            anim.Flip(side);
        }


        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.3f));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;

        Jump((Vector2.up / 1.5f + wallDir / 1.5f));

        wallJumped = true;

    }

}