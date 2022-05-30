using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : Breakable
{
    [Header("Eye Animation")]
    [SerializeField] private Transform eye;
    [SerializeField] private EyeState eyeState;
    [SerializeField] private float shakeAmount;
    [SerializeField] private float lookOffset;
    [SerializeField] private float maxTimeBetweenShakes;
    [SerializeField] private float minTimeBetweenShakes;
    [SerializeField] private float maxShakeTime;
    [SerializeField] private float minShakeTime;
    [Header("Agro")]
    [SerializeField] private bool agroLineOfSight = true;
    [SerializeField] private float deagroRange;

    private float shakeTimer = 0;
    private SpriteRenderer sprite;

    internal bool isShaking = false;
    internal bool isAgro = false;

    public virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public virtual void Update()
    {
        if (!isAgro && sprite.isVisible && (!agroLineOfSight || InLineOfSight().Item1))
        {
            isAgro = true;
            isShaking = eyeState == EyeState.Agro ? true : isShaking;
        }
        else if (isAgro && !sprite.isVisible)
        {
            (bool, float) lineOfSight = InLineOfSight();

            if (!lineOfSight.Item1 || lineOfSight.Item2 > deagroRange)
            {
                isAgro = false;
                isShaking = eyeState == EyeState.Agro ? false : isShaking;
            }
        }

        if (eyeState == EyeState.Random)
        {
            shakeTimer -= Time.deltaTime;

            if (shakeTimer < 0)
            {
                isShaking = !isShaking;
                shakeTimer = isShaking ? Random.Range(minShakeTime, maxShakeTime) : Random.Range(minTimeBetweenShakes, maxTimeBetweenShakes);
            }
        }

        Vector2 playerPos = PlayerManager.singleton.player != null ? PlayerManager.singleton.player.transform.position : transform.position;
        eye.localPosition = (playerPos - (Vector2)transform.position).normalized * lookOffset;

        if (isShaking)
        {
            eye.localPosition += (Vector3)Random.insideUnitCircle * shakeAmount;
        }
    }

    private (bool, float) InLineOfSight()
    {
        if (PlayerManager.singleton.player == null) return (false, 0);

        Vector3 direction = (PlayerManager.singleton.player.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);

        return hit.transform == null ? (false, 0) : (hit.transform.CompareTag("Player"), hit.distance);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            Break();
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger && collision.gameObject.CompareTag("Death"))
        {
            Break();
        }
    }
}

public enum EyeState
{
    Inactive = 0,
    Random,
    Agro,
}
