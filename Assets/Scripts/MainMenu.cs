using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider soundSlider;
    public Slider musicSlider;

    public AudioMixer musicMixer;
    public AudioMixer soundMixer;

    private void Awake()
    {
        float musicValue = PlayerPrefs.GetFloat("musicVolume") == 0 ? 0.5f : PlayerPrefs.GetFloat("musicVolume");
        float soundValue = PlayerPrefs.GetFloat("soundVolume") == 0 ? 0.5f : PlayerPrefs.GetFloat("soundVolume");
        musicSlider.value = musicValue;
        soundSlider.value = soundValue;
        SetMusicLevel(musicValue);
        SetSoundLevel(soundValue);
    }

    public void SetMusicLevel(float sliderValue)
    {
        musicMixer.SetFloat("volume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("musicVolume", sliderValue);
    }

    public void SetSoundLevel(float sliderValue)
    {
        soundMixer.SetFloat("volume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("soundVolume", sliderValue);
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
