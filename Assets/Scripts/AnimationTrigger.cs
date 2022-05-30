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

    private GameObject toucher;

    private bool enterAnimPlaying = false;
    private bool hasStarted = false;
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
        if (!hasStarted)
        {
            hasStarted = true;
            return;
        }

        if (enterAnims.Count == 0 || enterAnimPlaying || toucher != null || collision.isTrigger) return;

        toucher = collision.gameObject;

        for (int i = enterAnims.Count - 1; i >= 0; i--)
        {
            AnimationState anim = enterAnims[i];
            if (anim.animation == null)
            {
                enterAnims.RemoveAt(i);
                continue;
            }

            if (enterAnimPlaying || (!anim.important && anim.animation.isPlaying))
            {
                continue;
            }

            anim.animation.Blend(anim.name);

            if (anim.playOnce)
            {
                enterAnims.RemoveAt(i);
            }
        }
        enterAnimPlaying = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (exitAnims.Count == 0 || !enterAnimPlaying || toucher == null || collision.gameObject != toucher || collision.isTrigger) return;

        toucher = null;

        for (int i = exitAnims.Count - 1; i >= 0; i--)
        {
            AnimationState anim = exitAnims[i];
            if (anim.animation == null)
            {
                exitAnims.RemoveAt(i);
                continue;
            }

            if ((!anim.important && anim.animation.isPlaying))
            {
                continue;
            }

            anim.animation.Blend(anim.name);

            if (anim.playOnce)
            {
                exitAnims.RemoveAt(i);
            }
        }
        enterAnimPlaying = false;
    }

    private void FixedUpdate()
    {
        if (resetOnDeath && PlayerManager.singleton.player == null && !AreAnimationsComplete())
        {
            SetupAnimations();
            enterAnimPlaying = false;
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
