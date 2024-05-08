using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
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

    //controls
    [Header("Controls:")]
    public Slider SensitivityYSlider;
    public TMP_Text SensitivityYValueTxt;

    public Slider SensitivityXSlider;
    public TMP_Text SensitivityXValueTxt;

    //language
    private void Start()
    {
        SensitivityYSlider.value = GameManager.instance.sensitivityY;
        SensitivityXSlider.value = GameManager.instance.sensitivityX;
        dropdownQuality.value = QualitySettings.GetQualityLevel();
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
        SFX.volume = SFXVolSlider.value;
    }

    void ChangeVideo()
    {
        virtualCamera.m_Lens.FieldOfView = FOVSlider.value * 160;
        FOVValueTxt.text = Mathf.RoundToInt(FOVSlider.value * 160).ToString();

        virtualCamera.gameObject.GetComponent<CinemachinePostProcessing>().enabled = postprocessingToggle.isOn;

        Screen.fullScreen = fullscreenToggle.isOn;

        GameManager.instance.terrain.basemapDistance = RenderDisSlider.value * 100;
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
        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = qualityLevels[value];
    }
}
