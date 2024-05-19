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
    public AudioSource music;
    public AudioSource SFX;

    public float sensitivityY;
    public float sensitivityX;

    public float exp = 0;

    public bool isPostProcessing;
    public float renderDis;
    public int graphicsValue;
    public bool isFullscreen;
    public float FOV;
    //-----------------------------------------------------------------

    public GameObject canvas;
    public GameObject mainCamera;

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

    public List<Spells> allSpells;

    public Terrain terrain;

    public List<AudioClip> soundEffects;

    /*
     * 0-Use Spell
     * 1-Get Hit
     * 2-Hit Smth
     * 3-Button Click
     * 4-Unlock Spell
     * 5-Walk
     * 6-Dash
     * 7-Death
     */

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        player = (MonoBehaviour)GameObject.FindObjectOfType(typeof(PlayerController));
        LoadPlayerPrefs();
    }

    private void Start()
    {
        
        SetUpSpellIcons();

        InvokeRepeating("SpawnManaGems", 0f, 5f);
        UpdateEqNums();
    }

    private void Update()
    {
        SpellsVisualUpdate();
        OpenSettingsPanel();

        CheckAmmountOfNumsInEq();
    }

    void CheckAmmountOfNumsInEq()
    {
        if (numbersEq.Count >15)
        {
            numbersEq.RemoveAt(15);
            UpdateEqNums();
        }
    }
    void LoadPlayerPrefs()
    {
        chosenSpells = new List<Spells>();
        unlockedSpells = new List<Spells>();
        numbersEq = new List<float>();

        if (PlayerPrefs.HasKey("unlockedSpells_count"))
        {
            for(int i=0; i < PlayerPrefs.GetInt("unlockedSpells_count"); i++)
            {
                foreach(var spell in allSpells)
                {
                    if(spell.id == PlayerPrefs.GetInt("unlockedSpellsId_" + i))
                    {
                        unlockedSpells.Add(spell);
                        break;
                    }
                }
            }
        }
        else
        {
            foreach (var spell in allSpells)
            {
                spell.lvl = 0;
            }
            unlockedSpells.Add(allSpells[0]);
        }

        if (PlayerPrefs.HasKey("chosenSpells_count"))
        {
            for (int i = 0; i < 5; i++)
            {
                foreach (var spell in allSpells)
                {
                    if (spell.id == PlayerPrefs.GetInt("chosenSpellsId_" + i))
                    {
                        chosenSpells.Add(spell);
                        break;
                    }
                    else if (PlayerPrefs.GetInt("chosenSpellsId_" + i) == 10)
                    {
                        chosenSpells.Add(null);
                        break;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                chosenSpells.Add(null);
            }
        }

        if (PlayerPrefs.HasKey("numbersEq_count"))
        {
            for (int i = 0; i < PlayerPrefs.GetFloat("numbersEq_count"); i++)
            {
                numbersEq.Add(PlayerPrefs.GetFloat("numbersEq_" + i));
            }
        }

        if (PlayerPrefs.HasKey("musicVol"))
        {
            music.volume=PlayerPrefs.GetFloat("musicVol");
        }
        else
        {
            music.volume = 1;
        }
        if (PlayerPrefs.HasKey("sfxVol"))
        {
            SFX.volume = PlayerPrefs.GetFloat("sfxVol");
        }
        else
        {
            SFX.volume = 1;
        }

        if (PlayerPrefs.HasKey("sensitivityX"))
        {
            sensitivityX = PlayerPrefs.GetFloat("sensitivityX");
        }
        else
        {
            sensitivityX = 400;
        }
        if (PlayerPrefs.HasKey("sensitivityY"))
        {
            sensitivityY = PlayerPrefs.GetFloat("sensitivityY");
        }
        else
        {
            sensitivityY = 400;
        }

        if (PlayerPrefs.HasKey("exp"))
        {
           exp=PlayerPrefs.GetFloat("exp");
        }
        else
        {
            exp = 0;
        }

        if (PlayerPrefs.HasKey("postProcessing"))
        {
            if (PlayerPrefs.GetInt("postProcessing") == 0)
            {
                isPostProcessing = false;
            }
            else
            {
                isPostProcessing= true;
            }
        }
        else
        {
            isPostProcessing = true;
        }

        if (PlayerPrefs.HasKey("renderDistance"))
        {
            renderDis = PlayerPrefs.GetFloat("renderDistance");
        }
        else
        {
            renderDis = 1000;
        }

        if (PlayerPrefs.HasKey("graphicsValue"))
        {
            graphicsValue = PlayerPrefs.GetInt("graphicsValue");
        }
        else
        {
            graphicsValue = 2;
        }

        if (PlayerPrefs.HasKey("fullscreen"))
        {
            if (PlayerPrefs.GetInt("fullscreen") == 0)
            {
                isFullscreen = false;
            }
            else
            {
                isFullscreen = true;
            }
        }
        else
        {
            isFullscreen= true;
        }

        if (PlayerPrefs.HasKey("fov"))
        {
            FOV = PlayerPrefs.GetFloat("fov");
        }
        else
        {
            FOV = 80;
        }

        if (PlayerPrefs.HasKey("posX")&& PlayerPrefs.HasKey("posY") && PlayerPrefs.HasKey("posZ"))
        {
            player.transform.position = new Vector3(PlayerPrefs.GetFloat("posX"), PlayerPrefs.GetFloat("posY"), PlayerPrefs.GetFloat("posZ"));
        }

        if (PlayerPrefs.HasKey("rotX") && PlayerPrefs.HasKey("rotY") && PlayerPrefs.HasKey("rotZ"))
        {
            player.transform.eulerAngles = new Vector3(PlayerPrefs.GetFloat("rotX"), PlayerPrefs.GetFloat("rotY"), PlayerPrefs.GetFloat("rotZ"));
        }

        if (PlayerPrefs.HasKey("playerHp"))
        {
            player.GetComponent<PlayerController>().hp=PlayerPrefs.GetFloat("playerHp");
        }

        SavePlayerPrefs();
    }

    public void SavePlayerPrefs()
    {
        //unlockedSpells
        PlayerPrefs.SetInt("unlockedSpells_count", unlockedSpells.Count);
        for (int i = 0; i < unlockedSpells.Count; i++)
        {
            PlayerPrefs.SetInt("unlockedSpellsId_" + i, unlockedSpells[i].id);
            PlayerPrefs.SetInt("unlockedSpellsLvl_" + i, unlockedSpells[i].lvl);
        }

        //chosenSpells
        PlayerPrefs.SetInt("chosenSpells_count", chosenSpells.Count);
        for (int i = 0; i < chosenSpells.Count; i++)
        {
            if (chosenSpells[i] == null)
            {
                PlayerPrefs.SetInt("chosenSpellsId_" + i, 10);
            }
            else
            {
                PlayerPrefs.SetInt("chosenSpellsId_" + i, chosenSpells[i].id);
            }
        }

        //nums in eq
        PlayerPrefs.SetFloat("numbersEq_count", numbersEq.Count);
        for (int i = 0; i < numbersEq.Count; i++)
        {
            PlayerPrefs.SetFloat("numbersEq_" + i, numbersEq[i]);
        }

        //volume
        PlayerPrefs.SetFloat("musicVol", music.volume);
        PlayerPrefs.SetFloat("sfxVol", SFX.volume);

        //sensitivity
        PlayerPrefs.SetFloat("sensitivityX", sensitivityX);
        PlayerPrefs.SetFloat("sensitivityY", sensitivityY);

        //EXP
        PlayerPrefs.SetFloat("exp", exp);

        //postProcessing
        if(isPostProcessing)
        {
            PlayerPrefs.SetInt("postProcessing", 1);
        }
        else
        {
            PlayerPrefs.SetInt("postProcessing", 0);
        }

        //render distance
        PlayerPrefs.SetFloat("renderDistance", renderDis);

        //graphicsValue
        PlayerPrefs.SetInt("graphicsValue", graphicsValue);

        //fullscreen
        if (isFullscreen)
        {
            PlayerPrefs.SetInt("fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("fullscreen", 0);
        }

        //FOV
        PlayerPrefs.SetFloat("fov", FOV);

        //player
            //pos
            PlayerPrefs.SetFloat("posX",player.transform.position.x);
            PlayerPrefs.SetFloat("posY",player.transform.position.y);
            PlayerPrefs.SetFloat("posZ",player.transform.position.z);

            //rotation
            PlayerPrefs.SetFloat("rotX", player.transform.rotation.x);
            PlayerPrefs.SetFloat("rotY", player.transform.rotation.y);
            PlayerPrefs.SetFloat("rotZ", player.transform.rotation.z);

            //hp
            PlayerPrefs.SetFloat("playerHp", player.GetComponent<PlayerController>().hp);
    }

    void SpawnManaGems()
    {
        var manaG = Instantiate(manaGem, new Vector3(Random.Range(player.transform.position.x - 20, player.transform.position.x + 20), player.transform.position.y+13, Random.Range(player.transform.position.z - 20, player.transform.position.z + 20)), Quaternion.identity);

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

    public void GoToMenu()
    {
        SavePlayerPrefs();
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOverResetLvl()
    {
        SavePlayerPrefs();
        PlayerPrefs.DeleteKey("posX");
        PlayerPrefs.DeleteKey("posY");
        PlayerPrefs.DeleteKey("posZ");

        PlayerPrefs.DeleteKey("rotX");
        PlayerPrefs.DeleteKey("rotY");
        PlayerPrefs.DeleteKey("rotZ");

        PlayerPrefs.DeleteKey("unlockedSpells_count");
        for (int id=0; id<allSpells.Count;id++)
        {
            if (PlayerPrefs.HasKey("unlockedSpellsId_" + id.ToString()))
                PlayerPrefs.DeleteKey("unlockedSpellsId_" + id.ToString());
        }

        PlayerPrefs.DeleteKey("chosenSpells_count");
        for (int id = 0; id < allSpells.Count; id++)
        {
            if(PlayerPrefs.HasKey("chosenSpellsId_" + id.ToString()))
                PlayerPrefs.DeleteKey("chosenSpellsId_" + id.ToString());
        }

        PlayerPrefs.DeleteKey("exp");

        for (int i = 0; i < PlayerPrefs.GetFloat("numbersEq_count"); i++)
        {
            PlayerPrefs.DeleteKey("numbersEq_" + i);
        }
        PlayerPrefs.DeleteKey("numbersEq_count");

        PlayerPrefs.DeleteKey("playerHp");

        SceneManager.LoadScene("Level1");
    }

    public void ExitGame()
    {
        SavePlayerPrefs();
        Application.Quit();
    }

    public void BttnSound()
    {
        SFX.clip = soundEffects[3];
        SFX.Play();
    }

}
