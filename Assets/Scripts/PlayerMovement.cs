using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpPower = 10;
    [SerializeField] private float extraJumpPower = 5;
    [SerializeField] private float extraJumpLength = 0.5f;
    [SerializeField] private float movementSpeed = 12;
    [SerializeField] private float movementThreshold = 0.05f;
    [SerializeField] private float extraGroundTime = 0.05f;

    private Player player;
    private Rigidbody2D body;
    private bool isGrounded;
    private float extraJumpTimer = 0;
    private float groundTimer = 0;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
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

        if (groundTimer > 0)
        {
            groundTimer -= Time.deltaTime;
            if (groundTimer <= 0)
            {
                isGrounded = false;
            }
        }

        if (startJumping && isGrounded)
        {
            extraJumpTimer = extraJumpLength;
            verticalVel = jumpPower;
            isGrounded = false;
            player.jumpSound.Play();
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
            groundTimer = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            groundTimer = extraGroundTime;
            if (extraGroundTime <= 0)
            {
                isGrounded = false;
            }
        }
    }
}
