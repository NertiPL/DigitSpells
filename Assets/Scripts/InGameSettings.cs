using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine.PostFX;

public class InGameSettings : MonoBehaviour
{

    //video
    [Header("Video:")]
    public CinemachineVirtualCamera virtualCamera;
    public Slider FOVSlider;
    public TMP_Text FOVValueTxt;
    public Toggle postprocessingToggle;

    //audio
    [Header("Audio:")]
    public AudioSource music;
    public AudioSource SFX;
    public Slider musicVolSlider;
    public Slider SFXVolSlider;


    //language

    private void Update()
    {
        ChangeAudio();
        ChangeVideo();
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
    }
}
