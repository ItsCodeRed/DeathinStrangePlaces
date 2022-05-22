using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gravestone : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float fallSpeed = 5f;

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
            transform.Translate(0, -fallSpeed * Time.fixedDeltaTime, 0);
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