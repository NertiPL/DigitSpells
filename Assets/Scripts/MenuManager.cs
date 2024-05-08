using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public GameObject Credits;

    private void Start()
    {
        MainMenu.SetActive(true);
        SettingsMenu.SetActive(false);
        Credits.SetActive(false);
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

}
