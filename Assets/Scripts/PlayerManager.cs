using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager singleton;

    [SerializeField] private Transform spawn;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private Gravestone gravestone;
    [SerializeField] private float graveDelay = 0.5f;
    [SerializeField] private float graveImpactTime = 0.5f;

    public Player player;
    public CamFollow cam;

    public int tries = 0;

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
        Vector2 playerPos = player.transform.position;
        Destroy(player.gameObject);
        Instantiate(deathParticles, playerPos, Quaternion.identity);

        yield return new WaitForSeconds(graveDelay);

        Vector2 graveSpawnPos = Vector3.right * playerPos.x + Vector3.up * (cam.transform.position.y + cam.GetCameraSize().y + 1);
        Gravestone stone = Instantiate(gravestone, graveSpawnPos, Quaternion.identity);
        stone.Initialize(playerPos.y, tries);

        yield return new WaitUntil(() => stone == null || !stone.isFalling);
        yield return new WaitForSeconds(stone == null || stone.impacted ? graveImpactTime : 0);

        player = Instantiate(playerPrefab, spawn.position, spawn.rotation);

        ResetOnDeath[] objs = FindObjectsOfType<ResetOnDeath>();

        if (objs.Length > 0)
        {
            foreach (ResetOnDeath obj in objs)
            {
                obj.ResetObject();
            }
        }
    }
}
