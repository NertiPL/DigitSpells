using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine.PostFX;
using UnityEngine.Rendering;

public class InGameSettings : MonoBehaviour
{

    //video
    [Header("Video:")]
    public CinemachineVirtualCamera virtualCamera;
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

    //language
    private void Start()
    {
        SensitivityYSlider.value = GameManager.instance.sensitivityY/100;
        SensitivityXSlider.value = GameManager.instance.sensitivityX/100;
        musicVolSlider.value = GameManager.instance.music.volume;
        SFXVolSlider.value = GameManager.instance.SFX.volume;
        postprocessingToggle.isOn = GameManager.instance.isPostProcessing;
        RenderDisSlider.value = GameManager.instance.renderDis / 100;
        fullscreenToggle.isOn = GameManager.instance.isFullscreen;
        FOVSlider.value = GameManager.instance.FOV / 160;
        dropdownQuality.value = QualitySettings.GetQualityLevel();
        ChangeGraphicsLevel(GameManager.instance.graphicsValue);
    }

    private void Update()
    {
        ChangeAudio();
        ChangeVideo();
        ChangeSensitivity();
    }

    public void Open()
    {
        Time.timeScale = 0f;
    }
    public void OpenSettingsPanels(int index)
    {
        foreach (Transform child in transform.GetChild(2))
        {
            child.gameObject.SetActive(false);
        }
        transform.GetChild(2).GetChild(index).gameObject.SetActive(true);
    }

    public void GoBack()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
    void ChangeAudio()
    {
        music.volume = musicVolSlider.value;
        musicValueTxt.text = Mathf.RoundToInt(musicVolSlider.value*100).ToString() + "%";
        SFX.volume = SFXVolSlider.value;
        SFXValueTxt.text = Mathf.RoundToInt(SFXVolSlider.value * 100).ToString() + "%";
    }

    void ChangeVideo()
    {
        virtualCamera.m_Lens.FieldOfView = FOVSlider.value * 160;
        GameManager.instance.FOV = FOVSlider.value * 160;
        FOVValueTxt.text = Mathf.RoundToInt(FOVSlider.value * 160).ToString();

        virtualCamera.gameObject.GetComponent<CinemachinePostProcessing>().enabled = postprocessingToggle.isOn;
        GameManager.instance.isPostProcessing = postprocessingToggle.isOn;

        Screen.fullScreen = fullscreenToggle.isOn;
        GameManager.instance.isFullscreen = fullscreenToggle.isOn;

        GameManager.instance.terrain.basemapDistance = RenderDisSlider.value * 100;
        GameManager.instance.renderDis = RenderDisSlider.value * 100;
        RenderDisValueTxt.text = Mathf.RoundToInt(RenderDisSlider.value * 100).ToString();

    }

    void ChangeSensitivity()
    {
        GameManager.instance.sensitivityX = SensitivityXSlider.value * 100;
        SensitivityXValueTxt.text = Mathf.RoundToInt(SensitivityXSlider.value * 100).ToString();
        GameManager.instance.sensitivityY = SensitivityYSlider.value * 100;
        SensitivityYValueTxt.text = Mathf.RoundToInt(SensitivityYSlider.value * 100).ToString();
    }

    public void ChangeGraphicsLevel(int value)
    {
        GameManager.instance.graphicsValue = value;
        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = qualityLevels[value];
    }
}
