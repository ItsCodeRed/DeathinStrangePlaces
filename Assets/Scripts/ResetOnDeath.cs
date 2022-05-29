using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnDeath : MonoBehaviour
{
    private Vector2 startPos;

    public virtual void Awake()
    {
        startPos = transform.position;
    }

    public virtual void ResetObject()
    {
        transform.position = startPos;
    }
}
