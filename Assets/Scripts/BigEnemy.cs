using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemy : ResetOnDeath
{
    [SerializeField] private GameObject deathParticles;

    private Rigidbody2D body;
    private float speed = 0;

    [SerializeField] private Animation bossAnim;
    [SerializeField] private GameObject bossTrigger;
    [SerializeField] private GameObject[] escapePlatforms;

    private float lastX = 0;

    public override void Awake()
    {
        base.Awake();
        body = GetComponent<Rigidbody2D>();
        lastX = transform.position.x;
    }

    private void Update()
    {
        body.velocity = new Vector2(speed, body.velocity.y);

        if (speed == 0)
        {
            transform.position = new Vector2(lastX, transform.position.y);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        lastX = transform.position.x;
    }

    public void Jump(float height)
    {
        body.velocity = new Vector2(body.velocity.x, height);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Breakable breakable = collision.gameObject.GetComponent<Breakable>();
        if (breakable != null)
        {
            breakable.Break();
        }

        if (collision.gameObject.CompareTag("Crusher"))
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            foreach (GameObject platform in escapePlatforms)
            {
                platform.SetActive(true);
            }
            bossTrigger.SetActive(false);
            Destroy(gameObject);
        }
    }

    public void EnablePlayerControls()
    {
        if (PlayerManager.singleton.player != null)
        {
            PlayerManager.singleton.player.movement.enabled = true;
            PlayerManager.singleton.player.enabled = true;
        }
    }

    public void DisablePlayerControls()
    {
        if (PlayerManager.singleton.player != null)
        {
            PlayerManager.singleton.player.movement.enabled = false;
            PlayerManager.singleton.player.body.velocity = Vector2.zero;
            PlayerManager.singleton.player.animator.SetBool("IsRunning", false);
            PlayerManager.singleton.player.animator.SetBool("InAir", false);
            PlayerManager.singleton.player.enabled = false;
        }
    }

    public override void ResetObject()
    {
        base.ResetObject();
        bossAnim.Stop();
        SetSpeed(0);
    }
}
