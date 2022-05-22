using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement movement;

    private bool hasDied = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death") && !hasDied)
        {
            hasDied = true;
            PlayerManager.singleton.Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Death") && !hasDied)
        {
            hasDied = true;
            PlayerManager.singleton.Die();
        }
    }
}
