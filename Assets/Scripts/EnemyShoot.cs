using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : Enemy
{
    private Rigidbody2D body;

    [SerializeField] private AudioSource shootSound;
    [SerializeField] private GameObject shot;
    [SerializeField] private float wanderInterval = 1;
    [SerializeField] private float wanderDist = 0.5f;
    [SerializeField] private float wanderSpeed = 3;
    [SerializeField] private float timeBetweenShots = 3;

    private bool runningRoutine = false;
    private Vector2 nextTarget = Vector2.zero;
    private Vector2 startPos = Vector2.zero;
    private float timer = 0;

    public override void Awake()
    {
        base.Awake();
        body = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        nextTarget = Random.insideUnitCircle * wanderDist;
    }

    public override void Update()
    {
        base.Update();
        timer += Time.deltaTime;

        if (isAgro && !runningRoutine)
        {
            StartCoroutine(ShootRoutine());
        }

        if (timer > wanderInterval)
        {
            timer = 0;
            nextTarget = Random.insideUnitCircle * wanderDist;
        }
        
        body.velocity += Time.deltaTime * (startPos + nextTarget - (Vector2)transform.position).normalized * wanderSpeed;
    }

    private IEnumerator ShootRoutine()
    {
        runningRoutine = true;

        yield return new WaitForSeconds(timeBetweenShots);
        shootSound.Play();
        Instantiate(shot, transform.position, Quaternion.identity);

        runningRoutine = false;
    }
}
