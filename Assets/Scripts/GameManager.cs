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

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        numholder.text = "";
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (numholder.text.Length > 0)
            {
                numholder.text = numholder.text.Remove(numholder.text.Length - 1, 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            numholder.text += "0";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            numholder.text += "1";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            numholder.text += "2";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            numholder.text += "3";
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            numholder.text += "4";
        }
        else if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            numholder.text += "5";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            numholder.text += "6";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            numholder.text += "7";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            numholder.text += "8";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            numholder.text += "9";
        }


    }
}
