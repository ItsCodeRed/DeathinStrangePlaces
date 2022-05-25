using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gravestone : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private float slowDist = 5f;
    [SerializeField] private float minSlowSpeed = 0.2f;

    public bool isFalling = true;
    private float targetY = 0;
    private Rigidbody2D body;

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
        if (!isFalling) return;

        if (transform.position.y > targetY)
        {
            float additionalSlow = Mathf.Max(fallSpeed * Mathf.Max(1 - (transform.position.y - targetY) / slowDist, 0), minSlowSpeed);
            transform.Translate(0, (-fallSpeed + additionalSlow) * Time.fixedDeltaTime, 0);
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
        isFalling = false;
        StartCoroutine(FreezeRoutine());
    }

    IEnumerator FreezeRoutine()
    {
        yield return new WaitForFixedUpdate();

        body.isKinematic = true;
    }
}