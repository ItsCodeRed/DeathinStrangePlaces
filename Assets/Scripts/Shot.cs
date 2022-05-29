using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shot : Breakable
{
    private Rigidbody2D body;

    [SerializeField] private float speed = 8;
    [SerializeField] private float maxSpeed = 4;
    [SerializeField] private float rotationSpeed = 1000;
    [SerializeField] private float checkDist = 10f;
    [SerializeField] private float lifeTime = 8f;

    private float timer = 0;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > lifeTime)
        {
            Break();
        }

        if (PlayerManager.singleton.player != null)
        {
            Vector2 direction = PlayerManager.singleton.player.transform.position - transform.position;
            RaycastHit2D hit = ShootRay(direction);

            if (hit.collider != null && direction.magnitude > 3)
            {
                Vector2 leftDirection = Vector2.Perpendicular(direction);
                RaycastHit2D leftHit = ShootRay(leftDirection);

                if (leftHit.collider == null)
                {
                    direction = leftDirection;
                }
                else
                {
                    Vector2 rightDirection = -leftDirection;
                    RaycastHit2D rightHit = ShootRay(rightDirection);

                    if (rightHit.collider == null || ((hit.distance < rightHit.distance && leftHit.distance < rightHit.distance)))
                    {
                        direction = rightDirection;
                    }
                    else if (hit.distance < leftHit.distance && leftHit.distance > rightHit.distance)
                    {
                        direction = leftDirection;
                    }
                }
            }

            body.velocity += Time.deltaTime * direction.normalized * speed;
        }

        if (body.velocity.magnitude > maxSpeed)
        {
            body.velocity = body.velocity.normalized * maxSpeed;
        }

        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
    }

    private RaycastHit2D ShootRay(Vector2 dir)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir.normalized, checkDist);

        return hits.Where(x => !x.collider.CompareTag("Player") && !x.collider.CompareTag("Enemy") && !x.collider.isTrigger).FirstOrDefault();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Break();
    }
}
