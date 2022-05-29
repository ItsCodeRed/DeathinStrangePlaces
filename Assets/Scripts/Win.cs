using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    [SerializeField] private float ascendAccel = 5f;
    [SerializeField] private float ascendSpeed = 5f;

    private bool hasWon = false;
    private bool playingAnimation = false;
    private float speed = 0;
    private Transform player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasWon && collision.gameObject.CompareTag("Player"))
        {
            playingAnimation = true;
            PlayerManager.singleton.player.Ascend();
            player = PlayerManager.singleton.player.transform;
        }
    }

    private void Update()
    {
        if (playingAnimation && player != null)
        {
            float distance = ((Vector2)(player.position - transform.position)).magnitude;

            if (distance > speed * Time.deltaTime)
            {
                speed = Mathf.Min(speed + ascendAccel * Time.deltaTime, ascendSpeed);

                player.position += speed * (transform.position - player.position).normalized * Time.deltaTime;
            }
            else
            {
                playingAnimation = false;
                GameManager.singleton.Win();
            }
        }
    }
}
