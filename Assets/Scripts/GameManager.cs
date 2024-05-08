using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //remember after saves

    public List<Spells> unlockedSpells;
    public List<Spells> chosenSpells;
    public List<float> numbersEq;

    //-----------------------------------------------------------------

    public GameObject canvas;
    
    int m = 0;
    public static GameManager instance;

    public GameObject numholder;
    public GameObject manaGemInEqPrefab;

    public GameObject miniGamePanel;

    public Image staminaBar;

    public Image healthBar;

    public MonoBehaviour player;
    public LayerMask enemies;

    public GameObject gameOverPanel;
    public GameObject spellsPanel;

    public GameObject manaGem;

    public GameObject draggedObject;

    public GameObject SpellLvlUpStation;

    public GameObject SettingsPanel;
    public AudioSource music;
    public AudioSource SFX;

    public float sensitivityY;
    public float sensitivityX;

    public Terrain terrain;

    public float exp=0;
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
        sensitivityX = 400;
        sensitivityY = 400;
        chosenSpells=new List<Spells>();
        for (int i = 0; i < 5; i++)
        {
            chosenSpells.Add(null);
        }
        SetUpSpellIcons();

        InvokeRepeating("SpawnManaGems", 0f, 5f);
        UpdateEqNums();
    }

    private void Update()
    {
        SpellsVisualUpdate();
        OpenSettingsPanel();

    }

    void SpawnManaGems()
    {
        var manaG = Instantiate(manaGem, new Vector3(Random.Range(player.transform.position.x - 20, player.transform.position.x + 20), 10, Random.Range(player.transform.position.z - 20, player.transform.position.z + 20)), Quaternion.identity);

        var numOnManaG = Random.Range(0, 20);

        manaG.transform.GetChild(0).GetComponent<TMP_Text>().text = numOnManaG.ToString();
        manaG.transform.GetChild(1).GetComponent<TMP_Text>().text = numOnManaG.ToString();

        manaG.GetComponent<ManaGemScript>().GiveNum(numOnManaG);

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
                    spellsPanel.transform.GetChild(m).gameObject.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1f);
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

    void OpenSettingsPanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SettingsPanel.SetActive(true);
            SettingsPanel.GetComponent<InGameSettings>().Open();
        }

    }

    public void GameOverMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOverResetLvl()
    {
        SceneManager.LoadScene("Level1");
    }

}
