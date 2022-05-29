using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Animation levelCompleteAnimation;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private string nextLevelUnlock;

    private float timer = 0;
    private bool paused = false;
    private bool won = false;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            return;
        }
        Debug.LogWarning("A GameManager already exists! Deleting this one....");
        Destroy(gameObject);
    }

    private void Update()
    {
        if (won) return;

        if (PlayerManager.singleton.player != null)
        {
            timer += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Win()
    {
        paused = false;
        won = true;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);

        if (PlayerManager.singleton.tries == 1)
        {
            timeText.text = $"{PlayerManager.singleton.tries} death\n{Mathf.Round(timer * 100f) / 100f} seconds";
        }
        else
        {
            timeText.text = $"{PlayerManager.singleton.tries} deaths\n{Mathf.Round(timer * 100f) / 100f} seconds";
        }
        Destroy(PlayerManager.singleton.player.gameObject);
        levelCompleteAnimation.Play();
        PlayerPrefs.SetInt(nextLevelUnlock, 1);
    }

    public void Pause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        pauseMenu.SetActive(paused);
    }
}
