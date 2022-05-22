using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager singleton;

    [SerializeField] private Transform spawn;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Gravestone gravestone;

    public Player player;
    public CamFollow cam;

    private int tries = 0;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            return;
        }
        Debug.LogWarning("A PlayerManager already exists! Deleting this one....");
        Destroy(gameObject);
    }

    public void Die()
    {
        tries++;

        StartCoroutine(GraveFallRoutine());
    }

    private IEnumerator GraveFallRoutine()
    {
        Vector2 graveSpawnPos = Vector3.right * player.transform.position.x + Vector3.up * (cam.transform.position.y + cam.GetCameraSize().y + 2);
        Gravestone stone = Instantiate(gravestone, graveSpawnPos, Quaternion.identity);
        stone.Initialize(player.transform.position.y, tries);
        Destroy(player.gameObject);

        yield return new WaitUntil(() => !stone.isFalling);

        player = Instantiate(playerPrefab, spawn.position, spawn.rotation);
    }
}
