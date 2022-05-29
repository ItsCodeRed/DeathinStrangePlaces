using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpPower = 10;
    [SerializeField] private float extraJumpPower = 5;
    [SerializeField] private float extraJumpLength = 0.5f;
    [SerializeField] private float movementSpeed = 12;
    [SerializeField] private float movementThreshold = 0.05f;

    private Rigidbody2D body;
    private bool isGrounded;
    private float extraJumpTimer = 0;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool IsMoving()
    {
        return Mathf.Abs(Input.GetAxisRaw("Horizontal")) > movementThreshold;
    }

    private void Update()
    {
        float verticalVel = body.velocity.y;

        bool startJumping = Input.GetKeyDown(KeyCode.Space);
        bool isJumping = Input.GetKey(KeyCode.Space);
        bool releaseJump = Input.GetKeyUp(KeyCode.Space);

        if (startJumping && isGrounded)
        {
            extraJumpTimer = extraJumpLength;
            verticalVel = jumpPower;
            isGrounded = false;
        }
        if (isJumping)
        {
            if (extraJumpTimer > 0)
            {
                verticalVel += extraJumpPower * Time.deltaTime;
                extraJumpTimer -= Time.deltaTime;
            }
            else
            {
                extraJumpTimer = 0;
            }
        }
        if (releaseJump)
        {
            if (verticalVel > 0 && extraJumpTimer > 0)
            {
                verticalVel *= 0.5f;
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
        return Input.GetAxisRaw("Horizontal") * movementSpeed;
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
