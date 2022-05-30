using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement movement;
    public Animator animator;
    public Rigidbody2D body;
    public AudioSource stepSounds;
    public AudioSource jumpSound;
    public AudioSource impactSound;

    private bool hasDied = false;
    private bool ascending = false;
    private bool previouslyAirborne = false;
    private float timeSinceLanding = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Death") || collision.gameObject.CompareTag("Crusher") || collision.gameObject.CompareTag("Enemy")) && !hasDied)
        {
            hasDied = true;
            PlayerManager.singleton.Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger && collision.gameObject.CompareTag("Death") && !hasDied)
        {
            hasDied = true;
            PlayerManager.singleton.Die();
        }
    }

    private void FixedUpdate()
    {
        if (ascending) return;

        HandleAnimations();
    }

    private void HandleAnimations()
    {
        bool onGround = movement.IsGrounded();
        bool isMoving = movement.IsMoving();

        animator.SetBool("InAir", !onGround);
        animator.SetBool("IsRunning", isMoving);
        if (isMoving && onGround && !stepSounds.isPlaying)
        {
            stepSounds.Play();
        }
        stepSounds.loop = isMoving && onGround;
        if (onGround && previouslyAirborne && timeSinceLanding > 0.25f)
        {
            impactSound.Play();
            timeSinceLanding = 0;
        }
        timeSinceLanding += Time.fixedDeltaTime;

        float input = Input.GetAxisRaw("Horizontal");
        float scaleX = input < 0 ? -1 : input > 0 ? 1 : transform.localScale.x;
        transform.localScale = new Vector3(scaleX, 1, 1);

        previouslyAirborne = !onGround;
    }

    public void Ascend()
    {
        ascending = true;
        animator.Play("Ascend");
        movement.enabled = false;
        body.isKinematic = true;
        body.gravityScale = 0;
        body.velocity = Vector2.zero;
        stepSounds.loop = false;
    }

    private void OnDisable()
    {
        stepSounds.loop = false;
    }
}
