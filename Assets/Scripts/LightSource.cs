using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LightSource : MonoBehaviour
{
    [SerializeField] private float defaultIntensity = 1f;
    [SerializeField] private float hiddenIntensity = 0.1f;

    private Light2D light2D;

    private void Awake()
    {
        light2D = GetComponent<Light2D>();
    }

    private void Update()
    {
        light2D.intensity = Physics2D.OverlapPoint(transform.position) == null ? defaultIntensity : hiddenIntensity;
    }
}
