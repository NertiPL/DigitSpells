using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using Cinemachine.PostFX;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public GameObject Credits;


    //video
    [Header("Video:")]
    public Slider FOVSlider;
    public TMP_Text FOVValueTxt;

    public Toggle postprocessingToggle;
    public Toggle fullscreenToggle;

    public Slider RenderDisSlider;
    public TMP_Text RenderDisValueTxt;

    public RenderPipelineAsset[] qualityLevels;
    public TMP_Dropdown dropdownQuality;

    //audio
    [Header("Audio:")]
    public AudioSource music;
    public AudioSource SFX;
    public Slider musicVolSlider;
    public Slider SFXVolSlider;
    public TMP_Text SFXValueTxt;
    public TMP_Text musicValueTxt;

    //controls
    [Header("Controls:")]
    public Slider SensitivityYSlider;
    public TMP_Text SensitivityYValueTxt;

    public Slider SensitivityXSlider;
    public TMP_Text SensitivityXValueTxt;
    private void Start()
    {
        MainMenu.SetActive(true);
        SettingsMenu.SetActive(false);
        Credits.SetActive(false);
    }

    private void Update()
    {
        ChangeAudio();
        ChangeSensitivity();
        ChangeVideo();
    }

    public void OpenSettings()
    {
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
        Credits.SetActive(false);
    }

    public void OpenMainMenu()
    {
        MainMenu.SetActive(true);
        SettingsMenu.SetActive(false);
        Credits.SetActive(false);
        SavePlayerPrefs();
    }

    public void OpenCredits()
    {
        Credits.SetActive(true);
        SettingsMenu.SetActive(false);
        MainMenu.SetActive(false);
    }


    public void OpenSettingsPanels(int index)
    {
        foreach(Transform child in SettingsMenu.transform.GetChild(2))
        {
            child.gameObject.SetActive(false);
        }
        SettingsMenu.transform.GetChild(2).GetChild(index).gameObject.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetFloat("musicVol", music.volume);
        PlayerPrefs.SetFloat("sfxVol", SFX.volume);

        PlayerPrefs.SetFloat("fov", FOVSlider.value * 40);

        if (postprocessingToggle.isOn)
        {
            PlayerPrefs.SetInt("postProcessing", 1);
        }
        else
        {
            PlayerPrefs.SetInt("postProcessing", 0);
        }
        if (fullscreenToggle.isOn)
        {
            PlayerPrefs.SetInt("fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("fullscreen", 0);
        }

        PlayerPrefs.SetFloat("renderDistance", RenderDisSlider.value * 100);

        PlayerPrefs.SetFloat("sensitivityX", SensitivityXSlider.value * 100);
        PlayerPrefs.SetFloat("sensitivityY", SensitivityYSlider.value * 100);
    }

    void ChangeAudio()
    {
        music.volume = musicVolSlider.value;
        musicValueTxt.text = Mathf.RoundToInt(musicVolSlider.value * 100).ToString() + "%";
        SFX.volume = SFXVolSlider.value;
        SFXValueTxt.text = Mathf.RoundToInt(SFXVolSlider.value * 100).ToString() + "%";
    }

    void ChangeVideo()
    {
        FOVValueTxt.text = Mathf.RoundToInt(FOVSlider.value * 40).ToString();


        Screen.fullScreen = fullscreenToggle.isOn;

        RenderDisValueTxt.text = Mathf.RoundToInt(RenderDisSlider.value * 100).ToString();

    }

    void ChangeSensitivity()
    {
        SensitivityXValueTxt.text = Mathf.RoundToInt(SensitivityXSlider.value * 100).ToString();
        SensitivityYValueTxt.text = Mathf.RoundToInt(SensitivityYSlider.value * 100).ToString();
    }

    public void ChangeGraphicsLevel(int value)
    {
        PlayerPrefs.SetInt("graphicsValue", value);
        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = qualityLevels[value];
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
