using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class SpellCardPanel : MonoBehaviour
{
    public Slider slider;
    public float center;
    public float difference;

    public List<Spells> spellsInGame;

    public GameObject cardPrefab;

    GameObject showStatsPanel;
    /* 0-Name
     * 1-Description
     * 2-Lvl
     * 3-Type
     * 4-Dif
     * 5-Cd
     */

    Spells cardSpellComp;
    private void Start()
    {
        Debug.Log(transform.position);
        showStatsPanel = transform.parent.GetChild(2).gameObject;
        int id = 0;
        foreach (Spells spell in spellsInGame)
        {
            var card=Instantiate(cardPrefab, transform);
            card.transform.GetChild(0).GetComponent<TMP_Text>().text = spell.name;
            card.transform.GetChild(1).GetComponent<TMP_Text>().text = spell.description;
            card.GetComponent<SpellCard>().id = id;
            card.GetComponent<SpellCard>().unlocked = true;

            if(!GameManager.instance.unlockedSpells.Contains(spell))
            {
                card.GetComponent<Image>().color = new Color(card.GetComponent<Image>().color.r, card.GetComponent<Image>().color.g, card.GetComponent<Image>().color.b, 0.2f);
                card.GetComponent<SpellCard>().unlocked = false;
            }
            id++;
        }
    }
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, center + (difference * slider.value), transform.position.z);
    }

    public void ShowStatsAndLvlUp(int id)
    {
        slider.value = 0f;
        slider.gameObject.SetActive(false);
        foreach (Transform card in transform)
        {
            if(card.GetComponent<SpellCard>().id == id)
            {
                cardSpellComp = spellsInGame[id];
                showStatsPanel.SetActive(true);
                showStatsPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = cardSpellComp.name;
                showStatsPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = cardSpellComp.description;
                showStatsPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = "Level: " + cardSpellComp.lvl.ToString();
                showStatsPanel.transform.GetChild(3).GetComponent<TMP_Text>().text = "Spell Type: " + cardSpellComp.spellType.ToString();
                showStatsPanel.transform.GetChild(4).GetComponent<TMP_Text>().text = "Difficulty: " + cardSpellComp.difficulty.ToString();
                showStatsPanel.transform.GetChild(5).GetComponent<TMP_Text>().text = "Cooldown: " + cardSpellComp.cd.ToString() + "s";

                if (cardSpellComp.lvl < 5)
                {
                    showStatsPanel.transform.GetChild(7).gameObject.SetActive(true);
                }
                else
                {
                    showStatsPanel.transform.GetChild(7).gameObject.SetActive(false);
                }

            }
        }
    }

    public void GoBack()
    {
        slider.gameObject.SetActive(true);
        showStatsPanel.SetActive(false);
        foreach (Transform card in transform)
        {
            card.gameObject.SetActive(true);
            card.GetChild(0).gameObject.SetActive(true);
            card.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void LvlUp()
    {
        if(cardSpellComp != null)
        {
            cardSpellComp.lvl++;
            GoBack();
        }
    }

    public void BackToGame()
    {
        transform.parent.gameObject.SetActive(false);
        GameManager.instance.canvas.transform.GetChild(0).gameObject.SetActive(true);
        GameManager.instance.miniGamePanel.SetActive(true);
        Time.timeScale = 1f;
    }
}
