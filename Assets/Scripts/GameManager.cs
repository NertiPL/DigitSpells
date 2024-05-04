using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public GameObject canvas;
    
    int m = 0;
    public static GameManager instance;

    public GameObject numholder;
    public GameObject manaGemInEqPrefab;

    public List<float> numbersEq;
    public GameObject miniGamePanel;

    public Image staminaBar;

    public Image healthBar;

    public MonoBehaviour player;
    public LayerMask enemies;

    public GameObject gameOverPanel;
    public GameObject spellsPanel;

    public GameObject manaGem;

    public List<Spells> chosenSpells;

    public GameObject draggedObject;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        player = (MonoBehaviour)GameObject.FindObjectOfType(typeof(PlayerController));
    }

    private void Start()
    {

        SetUpSpellIcons();

        InvokeRepeating("SpawnManaGems", 0f, 5f);
        UpdateEqNums();
    }

    private void Update()
    {
        NumholderStuff();
        SpellsVisualUpdate();

    }

    void SpawnManaGems()
    {
        var manaG = Instantiate(manaGem, new Vector3(Random.Range(player.transform.position.x - 20, player.transform.position.x + 20), 10, Random.Range(player.transform.position.z - 20, player.transform.position.z + 20)), Quaternion.identity);

        var numOnManaG = Random.Range(0, 20);

        manaG.transform.GetChild(0).GetComponent<TMP_Text>().text = numOnManaG.ToString();
        manaG.transform.GetChild(1).GetComponent<TMP_Text>().text = numOnManaG.ToString();

        manaG.GetComponent<ManaGemScript>().GiveNum(numOnManaG);

    }

    void NumholderStuff()
    {
        CheckWhatNum();
    }

    public void UpdateEqNums()
    {
        if (numholder.transform.childCount > 0)
        {
            foreach (Transform child in numholder.transform)
            {
                Destroy(child.gameObject);
            }
        }
        foreach (var num in numbersEq)
        {
            if (draggedObject != null)
            {
                if (num == draggedObject.GetComponent<NumberInEqScript>().value)
                {
                    draggedObject = null;
                    continue;
                }
            }
            var gem = Instantiate(manaGemInEqPrefab,numholder.transform);

            gem.GetComponent<NumberInEqScript>().value = num;
        }
    }
    void CheckWhatNum()
    {
        /*if (Input.GetKeyDown(KeyCode.Backspace))
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
        }*/


    }

    void SetUpSpellIcons()
    {
        foreach(Transform child in spellsPanel.transform)
        {
            child.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
            child.gameObject.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1f);
        }
    }

    void SpellsVisualUpdate()
    {
        if(chosenSpells.Count > 0)
        {
            m = 0;
            float a;
            foreach (var spell in chosenSpells)
            {
                if(spell != null)
                {
                    if (player.GetComponent<PlayerController>().btnsOnCooldown[m])
                    {
                        a = 0.25f;
                    }
                    else
                    {
                        a = 1f;
                    }

                    spellsPanel.transform.GetChild(m).GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                    spellsPanel.transform.GetChild(m).GetChild(0).gameObject.GetComponent<Image>().sprite = spell.sprite;
                    spellsPanel.transform.GetChild(m).GetChild(0).gameObject.GetComponent<Image>().color = new Color(spellsPanel.transform.GetChild(m).GetChild(0).gameObject.GetComponent<Image>().color.r, spellsPanel.transform.GetChild(m).GetChild(0).gameObject.GetComponent<Image>().color.g, spellsPanel.transform.GetChild(m).GetChild(0).gameObject.GetComponent<Image>().color.b, a);

                    switch (spell.spellType)
                    {
                        case SpellType.Fire:
                            spellsPanel.transform.GetChild(m).gameObject.GetComponent<Image>().color = new Color(1f, 0f, 0f, a);
                            break;

                        case SpellType.Water:
                            spellsPanel.transform.GetChild(m).gameObject.GetComponent<Image>().color = new Color(0f, 0.75f, 1f, a);
                            break;

                        case SpellType.Lightning:
                            spellsPanel.transform.GetChild(m).gameObject.GetComponent<Image>().color = new Color(1f, 1f, 0f, a);
                            break;

                        case SpellType.Earth:
                            spellsPanel.transform.GetChild(m).gameObject.GetComponent<Image>().color = new Color(0.6f, 0.4f, 0f, a);
                            break;
                        case SpellType.Healing:
                            spellsPanel.transform.GetChild(m).gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, a);
                            break;
                    }

                

                }
                else
                {
                    spellsPanel.transform.GetChild(m).GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                }
                m++;
            }

        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }


}
