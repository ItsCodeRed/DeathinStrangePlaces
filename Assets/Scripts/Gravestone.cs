using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Gravestone : Breakable
{
    [SerializeField] private GameObject crack;
    [SerializeField] private GameObject crackParticles;
    [SerializeField] private GameObject impactParticles;
    [SerializeField] private Transform stoneVisuals;
    [SerializeField] private TMP_Text text;
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private float slowDist = 5f;
    [SerializeField] private float minFallSpeed = 0.2f;
    [SerializeField] private float timeToCrack = 1f;
    [SerializeField] private float timeToBreak = 2f;
    [SerializeField] private float shakeAmount = 0.4f;

    public bool isFalling = true;
    public bool impacted = false;

    private float targetY = 0;
    private Rigidbody2D body;
    private float breakTimer = 0;
    private bool hasCracked = false;
    private bool isStanding = false;
    private bool hasSpawned = false;
    private int ticks = 0;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void Initialize(float target, int deathNum)
    {
        targetY = target;
        text.text = deathNum.ToString();
        isFalling = true;
    }

    private void FixedUpdate()
    {
        ticks = ticks < 4 ? ticks + 1 : ticks;
        hasSpawned = ticks >= 4;

        if (isStanding)
        {
            breakTimer += Time.fixedDeltaTime;
            if (breakTimer > timeToBreak)
            {
                Break();
            }
            else if (breakTimer > timeToCrack)
            {
                if (!hasCracked)
                {
                    Crack();
                }
                stoneVisuals.localPosition = Random.insideUnitCircle * shakeAmount;
            }
        }

        if (!isFalling) return;

        if (transform.position.y > targetY)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll((Vector2)transform.position, Vector2.down);
            RaycastHit2D hit = hits.Where(x => !x.collider.isTrigger).FirstOrDefault();
            float speed = -fallSpeed;
            if (hit.collider == null || hit.distance - 1f > transform.position.y - targetY)
            {
                speed += fallSpeed * Mathf.Max(1 - (transform.position.y - targetY) / slowDist, 0);
            }
            transform.Translate(0, Mathf.Min(speed, -minFallSpeed) * Time.fixedDeltaTime, 0);
        }
        else if (transform.position.y < targetY)
        {
            isFalling = false;
            body.isKinematic = true;
            transform.position = new Vector2(transform.position.x, targetY);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasSpawned)
        {
            Break();
        }

        impacted = true;

        if (!isFalling) return;

        Breakable breakable = collision.gameObject.GetComponent<Breakable>();
        if (breakable != null)
        {
            breakable.Break();
            if (!collision.gameObject.CompareTag("Enemy"))
            {
                Break();
            }
        }

        if (!collision.gameObject.CompareTag("Enemy"))
        {
            isFalling = false;
            StartCoroutine(FreezeRoutine());
        }
        body.velocity = Vector2.zero;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.isTrigger && collision.gameObject.CompareTag("Player"))
        {
            isStanding = true;
        }
        else
        {
            if (!isFalling) return;

            Breakable breakable = collision.gameObject.GetComponent<Breakable>();
            if (breakable != null && !collision.gameObject.CompareTag("Gravestone"))
            {
                breakable.Break();
                if (!collision.gameObject.CompareTag("Enemy"))
                {
                    Break();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isStanding = false;
        }
    }

    private void Crack()
    {
        hasCracked = true;
        crack.SetActive(true);
        Instantiate(crackParticles, transform.position, Quaternion.identity);
    }

    private void Impact()
    {
        Instantiate(impactParticles, transform.position + Vector3.down * 0.75f, Quaternion.identity);
    }

    IEnumerator FreezeRoutine()
    {
        yield return new WaitForFixedUpdate();

        body.isKinematic = true;
        Impact();
    }
}