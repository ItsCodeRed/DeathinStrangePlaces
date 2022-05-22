using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float jumpPower = 10;
    public float extraJumpPower = 5;
    public float extraJumpLength = 0.5f;
    public float walkSpeed = 8;
    public float sprintSpeed = 15;

    private Rigidbody2D body;
    private bool isGrounded;
    private float extraJumpTimer = 0;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float verticalVel = body.velocity.y;

        bool isJumping = Input.GetKey(KeyCode.Space);

        if (isJumping)
        {
            if (isGrounded)
            {
                extraJumpTimer = extraJumpLength;
                verticalVel = jumpPower + extraJumpPower;
                isGrounded = false;
            }
            else if (extraJumpTimer > 0)
            {
                verticalVel += extraJumpPower * Time.deltaTime;
                extraJumpTimer -= Time.deltaTime;
            }
            else
            {
                extraJumpTimer = 0;
            }
        }

        body.velocity = new Vector2(body.velocity.x, verticalVel);
    }

    private void FixedUpdate()
    {
        float horizontalVel = HorizontalMovement();

        body.velocity = new Vector2(horizontalVel, body.velocity.y);
    }

    private float HorizontalMovement()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        return horizontalInput * (isSprinting ? sprintSpeed : walkSpeed);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            isGrounded = false;
        }
    }
}
