using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Song : MonoBehaviour
{
    public static Song instance = null;

    [SerializeField] private float menuVolume = 0.1f;
    [SerializeField] private float levelVolume = 0.1f;

    private AudioSource song;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        song = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        song.enabled = true;
    }

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name.Contains("Level"))
        {
            song.volume = levelVolume;
        }
        else
        {
            song.volume = menuVolume;
        }
    }

    public void Disable()
    {
        song.enabled = false;
    }

    public void Enable()
    {
        song.enabled = true;
    }
}
