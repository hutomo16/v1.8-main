using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Animate")]
    public Animator Anim;

    [Header("Movement")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private float JumpHeight = 12f;
    [SerializeField] private Rigidbody2D rb;
    private float horizontal;
    private bool isFacingRight = true;

    [Header("GroundCheck")]
    public Transform GroundCheck;
    public Vector2 GroundCheckSize = new Vector2(0.7f, 0.1f);
    public LayerMask GroundLayer;

    [Header("Dash")]
    private bool candash = true; //dash
    private bool isdash;
    [SerializeField] private float dashpower = 24f;
    [SerializeField] private float dashtime = 0.2f;
    [SerializeField] private float dashcooldown = 1f;

    [Header("WallCheck")]
    public Transform WallCheck;
    public Vector2 WallCheckSize = new Vector2(0.7f, 0.1f);
    public LayerMask WallLayer;

    [Header("WallMovement")]
    private bool isWallJump;
    private float WallJumpDirection;
    [SerializeField] private float WallJumpTime = 0.2f;
    private float WallJumpCounter;
    [SerializeField] private float WallJumpDuration = 0.4f;
    [SerializeField] private Vector2 WallJumpPower = new Vector2(6f, 12f);
    private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed = 2f;

    [Header("Audio")]
    [SerializeField] private AudioSource jumpSoundEffect;

    void Start()
    {
        Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isdash) //dash
        {
            return;
        }
        horizontal = Input.GetAxisRaw("Horizontal");
        Movement();//fungsi Berjalan
        Jump();//fungsi loncat
        Flip();//fungsi Berputar
        IsGrounded();
        IsWalled();
        WallSlide();
        Walljump();

        if (Input.GetKeyDown(KeyCode.LeftShift) && candash) //dash
        {
            float dashDirection = Input.GetAxisRaw("Horizontal");
            StartCoroutine(Dash(dashDirection));
        }

        if (!isWallJump)
        {
            Flip();
        }
    }

    private void Movement()
    {
        if (isdash)
        {
            return;
        }

        if (!isWallJump)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            Anim.SetFloat("xVelocity", Math.Abs(rb.velocity.x));
            Anim.SetFloat("yVelocity", rb.velocity.y);
            Flip();
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool IsGrounded()
    {
        if(Physics2D.OverlapBox(GroundCheck.position, GroundCheckSize, 0, GroundLayer))
        {
            return true;
        }
        else
        {
            Anim.SetBool("isJumping", true);
            return false;
        }

    }

    private void Jump()
    {
        if (IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AudioManager.instance.PlaySFX("Jump");
                rb.velocity = new Vector2(rb.velocity.x, JumpHeight);
                Anim.SetBool("isJumping", true);

            }else
            {
                Anim.SetBool("isJumping", false);
            }
        }
    }

    private bool IsWalled()
    {
        if (Physics2D.OverlapBox(WallCheck.position, WallCheckSize, 0, WallLayer))
        {

            Debug.Log("is walled");
            return true;
        }
        else
        {
            Debug.Log("is not near wall");
            return false;
        }

    }

    private void WallSlide()
    {
        if(IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void Walljump()
    {
        if (isWallSliding)
        {
            isWallJump = false;
            WallJumpDirection = -transform.localScale.x;
            WallJumpCounter = WallJumpTime;

            CancelInvoke(nameof(StopWallJump));
        }
        else
        {
            WallJumpCounter -= Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.Space) && WallJumpCounter > 0f)
        {
            AudioManager.instance.PlaySFX("Jump");
            isWallJump = true;
            rb.velocity = new Vector2(WallJumpDirection * WallJumpPower.x, WallJumpPower.y);
            WallJumpCounter = 0f;

            if (transform.localScale.x != WallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJump), WallJumpDuration);
        }
    }

    private void StopWallJump()
    {
        isWallJump = false;
    }

    private IEnumerator Dash(float direction)
    {
        candash = false; //dash
        isdash = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(direction * dashpower, 0f);
        yield return new WaitForSeconds(dashtime);
        rb.gravityScale = originalGravity;
        isdash = false;
        yield return new WaitForSeconds(dashcooldown);
        candash = true;  //Yoga
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(GroundCheck.position, GroundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(WallCheck.position, WallCheckSize);
    }
}
