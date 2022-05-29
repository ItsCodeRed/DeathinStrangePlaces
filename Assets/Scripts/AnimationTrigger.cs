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
    private bool enterAnimPlaying = false;

    private void Awake()
    {
        SetupAnimations();
    }

    private void SetupAnimations()
    {
        enterAnims = new List<AnimationState>(enterAnimations);
        exitAnims = new List<AnimationState>(exitAnimations);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (enterAnims.Count == 0) return;

        for (int i = enterAnims.Count - 1; i >= 0; i--)
        {
            AnimationState anim = enterAnims[i];
            if (anim.animation == null)
            {
                enterAnims.RemoveAt(i);
                continue;
            }

            if ((!anim.important && anim.animation.isPlaying) || enterAnimPlaying)
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
        if (exitAnims.Count == 0) return;

        for (int i = exitAnims.Count - 1; i >= 0; i--)
        {
            AnimationState anim = exitAnims[i];
            if (anim.animation == null)
            {
                exitAnims.RemoveAt(i);
                continue;
            }

            if ((!anim.important && anim.animation.isPlaying) || !enterAnimPlaying)
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

    private void Update()
    {
        if (resetOnDeath && PlayerManager.singleton.player == null && !AreAnimationsComplete())
        {
            SetupAnimations();
            enterAnimPlaying = false;
        }
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
