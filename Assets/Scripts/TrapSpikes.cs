using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikes : MonoBehaviour
{
    [SerializeField] private Animation anim;
    [SerializeField] private Collider2D deathCollider;
    [SerializeField] private AudioSource trapSound;
    private bool trapTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!trapTriggered && !collision.isTrigger && collision.CompareTag("Player"))
        {
            StartCoroutine(TriggerTrap());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Breakable breakable = collision.gameObject.GetComponent<Breakable>();
        if (breakable != null)
        {
            breakable.Break();
        }
    }

    private IEnumerator TriggerTrap()
    {
        trapTriggered = true;
        anim.Play();
        deathCollider.enabled = true;

        yield return new WaitForSeconds(0.1f);
        trapSound.Play();

        yield return new WaitForSeconds(1.4f);

        deathCollider.enabled = false;

        yield return new WaitWhile(() => anim.isPlaying);

        trapTriggered = false;
    }
}
