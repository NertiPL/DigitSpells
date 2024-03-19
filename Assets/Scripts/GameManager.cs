using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TMP_Text numholder;
    public Image staminaBar;

    public Image healthBar;

    public MonoBehaviour player;

    public GameObject gameOverPanel;

    public List<Spells> chosenSpells;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        numholder.text = "";

        player = (MonoBehaviour)GameObject.FindObjectOfType(typeof(PlayerController));
    }

    private void Update()
    {
        NumholderStuff();
    }

    void NumholderStuff()
    {
        CheckWhatNum();
    }

    void CheckWhatNum()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (numholder.text.Length > 0)
            {
                numholder.text = numholder.text.Remove(numholder.text.Length - 1, 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown("[0]"))
        {
            numholder.text += "0";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown("[1]"))
        {
            numholder.text += "1";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown("[2]"))
        {
            numholder.text += "2";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown("[3]"))
        {
            numholder.text += "3";
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown("[4]"))
        {
            numholder.text += "4";
        }
        else if(Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown("[5]"))
        {
            numholder.text += "5";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown("[6]"))
        {
            numholder.text += "6";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown("[7]"))
        {
            numholder.text += "7";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown("[8]"))
        {
            numholder.text += "8";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown("[9]"))
        {
            numholder.text += "9";
        }
        else if (Input.GetKeyDown(KeyCode.Minus))
        {
            numholder.text += "-";
        }


    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }


}
