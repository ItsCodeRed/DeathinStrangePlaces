using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement movement;
    public Animator animator;
    public Rigidbody2D body;

    private bool hasDied = false;
    private bool ascending = false;

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
        animator.SetBool("InAir", !movement.IsGrounded());
        animator.SetBool("IsRunning", movement.IsMoving());

        float input = Input.GetAxisRaw("Horizontal");
        float scaleX = input < 0 ? -1 : input > 0 ? 1 : transform.localScale.x;
        transform.localScale = new Vector3(scaleX, 1, 1);
    }

    public void Ascend()
    {
        ascending = true;
        animator.Play("Ascend");
        movement.enabled = false;
        body.isKinematic = true;
        body.gravityScale = 0;
        body.velocity = Vector2.zero;
    }
}
