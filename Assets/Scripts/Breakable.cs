using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] private GameObject breakParticles;

    public void Break()
    {
        Instantiate(breakParticles, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
