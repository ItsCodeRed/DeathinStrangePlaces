using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHop : Enemy
{
    private Rigidbody2D body;

    [SerializeField] private float supriseSpeed = 15;
    [SerializeField] private float supriseHeight = 10;
    [SerializeField] private float supriseLength = 0.5f;
    [SerializeField] private float chargeLength = 1f;
    [SerializeField] private float chargeShakeAmount = 1f;
    [SerializeField] private float hopLength = 1f;
    [SerializeField] private float hopDist = 1f;
    [SerializeField] private float hopHeight = 10f;

    private bool previouslyAgroed = false;
    private bool runningRoutine = false;

    public override void Awake()
    {
        base.Awake();
        body = GetComponent<Rigidbody2D>();
    }

    public override void Update()
    {
        base.Update();
        if (isAgro && !runningRoutine) 
        {
            if (!previouslyAgroed)
            {
                StartCoroutine(SupriseRoutine());
            }
            else
            {
                StartCoroutine(HopRoutine());
            }
        }
        previouslyAgroed = isAgro;
    }

    private IEnumerator SupriseRoutine()
    {
        runningRoutine = true;
        Vector2 pos = PlayerManager.singleton.player != null ? PlayerManager.singleton.player.transform.position : transform.position;
        float horizontalDirection = Mathf.Sign(transform.position.x - pos.x);
        body.velocity = new Vector2(horizontalDirection * supriseSpeed, supriseHeight);

        yield return new WaitForSeconds(supriseLength);

        runningRoutine = false;
    }
    
    private IEnumerator HopRoutine()
    {
        runningRoutine = true;
        float timer = 0;
        float posX = transform.position.x;

        while (timer < chargeLength)
        {
            timer += Time.deltaTime;

            transform.position = new Vector2(posX + Random.Range(-chargeShakeAmount, chargeShakeAmount), transform.position.y);

            yield return null;
        }

        Vector2 pos = PlayerManager.singleton.player != null ? PlayerManager.singleton.player.transform.position : transform.position;
        float horizontalDirection = Mathf.Sign(pos.x - transform.position.x);
        body.velocity = new Vector2(horizontalDirection * hopDist, hopHeight);

        yield return new WaitForSeconds(hopLength);

        runningRoutine = false;
    }
}
