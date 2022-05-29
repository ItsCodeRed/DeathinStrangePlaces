using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class GraveButton : MonoBehaviour
{
    public UnityEvent onClick;

    [SerializeField] private float lightFadeSpeed = 10;
    [SerializeField] private Light2D light2d;
    [SerializeField] private ShadowCaster2D shadow;
    [SerializeField] private string levelLock;

    public float defaultBrightness;
    public float clickBrightness;
    public float hoverBrightness;

    private bool isHovered = false;
    private bool isClicked = false;
    private float targetBrightness = 0;

    private void Awake()
    {
        if (levelLock != "" && PlayerPrefs.GetInt(levelLock) != 1)
        {
            Disable();
        }
    }

    private void Update()
    {
        targetBrightness = isClicked ? clickBrightness : isHovered ? hoverBrightness : defaultBrightness;

        if (Mathf.Abs(targetBrightness - light2d.intensity) > lightFadeSpeed * Time.deltaTime)
        {
            light2d.intensity += Mathf.Sign(targetBrightness - light2d.intensity) * lightFadeSpeed * Time.deltaTime;
        }
    }

    private void Disable()
    {
        light2d.intensity = defaultBrightness;
        shadow.selfShadows = true;
        enabled = false;
    }

    private void OnMouseEnter()
    {
        isHovered = true;
    }

    private void OnMouseExit()
    {
        isHovered = false;
    }

    private void OnMouseDown()
    {
        if (!enabled) return;

        onClick.Invoke();
        isClicked = true;
    }

    private void OnMouseUp()
    {
        isClicked = false;
    }
}
