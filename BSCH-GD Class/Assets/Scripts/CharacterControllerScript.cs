using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public Rigidbody2D myRb;
    public float jumpForce;
    public bool isGrounded;
    public float secondaryJumpForce;
    public float secondaryJumpTime;
    private bool secondaryJump;
    public Animator anim;

    private float horizontalInput;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        anim.SetFloat("speed", Mathf.Abs(myRb.velocity.x));

        // Animation flip code
        if (horizontalInput > 0.1f)
        {
            anim.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput < -0.1f)
        {
            anim.transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Mathf.Abs(horizontalInput) > 0.1f && Mathf.Abs(myRb.velocity.x) < maxSpeed)
        {
            myRb.AddForce(new Vector2(horizontalInput * acceleration, 0), ForceMode2D.Force);
        }

        // Jump code
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            myRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            if (!secondaryJump) StartCoroutine(SecondaryJump());
        }

        if (!isGrounded && Input.GetButton("Jump") && secondaryJump)
        {
            myRb.AddForce(new Vector2(0, secondaryJumpForce), ForceMode2D.Force);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isGrounded = false;
    }

    IEnumerator SecondaryJump()
    {
        secondaryJump = true;
        yield return new WaitForSeconds(secondaryJumpTime);
        secondaryJump = false;
    }
}
