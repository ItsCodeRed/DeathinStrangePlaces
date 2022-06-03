using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] private List<AnimationState> enterAnimations;
    [SerializeField] private List<AnimationState> exitAnimations;

    private List<AnimationState> enterAnims;
    private List<AnimationState> exitAnims;

    [SerializeField] private bool resetOnDeath = false;

    private List<Collider2D> colliders = new List<Collider2D>();

    private AudioSource sound;

    private void Awake()
    {
        sound = GetComponent<AudioSource>();
        SetupAnimations();
    }

    private void SetupAnimations()
    {
        enterAnims = new List<AnimationState>(enterAnimations);
        exitAnims = new List<AnimationState>(exitAnimations);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;

        colliders.Add(collision);

        if (enterAnims.Count == 0 || colliders.Count > 1) return;

        for (int i = enterAnims.Count - 1; i >= 0; i--)
        {
            AnimationState anim = enterAnims[i];
            if (anim.animation == null)
            {
                enterAnims.RemoveAt(i);
                continue;
            }

            if (!anim.important && anim.animation.isPlaying)
            {
                continue;
            }

            anim.animation.CrossFade(anim.name);

            if (anim.playOnce)
            {
                enterAnims.RemoveAt(i);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!colliders.Contains(collision)) return;

        colliders.Remove(collision);

        if (exitAnims.Count == 0 || colliders.Count > 0) return;

        for (int i = exitAnims.Count - 1; i >= 0; i--)
        {
            AnimationState anim = exitAnims[i];
            if (anim.animation == null)
            {
                exitAnims.RemoveAt(i);
                continue;
            }

            if (!anim.important && anim.animation.isPlaying)
            {
                continue;
            }

            anim.animation.CrossFade(anim.name);

            if (anim.playOnce)
            {
                exitAnims.RemoveAt(i);
            }
        }
    }

    private void FixedUpdate()
    {
        if (resetOnDeath && PlayerManager.singleton.player == null && !AreAnimationsComplete())
        {
            SetupAnimations();
        }
        if (colliders.Count > 0)
        {
            for (int i = colliders.Count - 1; i >= 0; i--)
            {
                if (colliders[i] == null)
                {
                    colliders.RemoveAt(i);
                }
            }
        }
    }

    public void PlaySound()
    {
        sound.Play();
    }

    private bool AreAnimationsComplete()
    {
        return enterAnims.Count == enterAnimations.Count && exitAnims.Count == exitAnimations.Count;
    }
}

[Serializable]
public struct AnimationState
{
    public Animation animation;
    public string name;
    public bool important;
    public bool playOnce;
}
