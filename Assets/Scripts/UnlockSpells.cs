using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockSpells : MonoBehaviour
{
    public Spells spell;
    public GameObject spellCardsPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!GameManager.instance.unlockedSpells.Contains(spell))
            {
                GameManager.instance.unlockedSpells.Add(spell);
                GameManager.instance.SFX.clip = GameManager.instance.soundEffects[4];
                GameManager.instance.SFX.Play();
                spellCardsPanel.GetComponent<SpellCardPanel>().UpdateCards();
                Destroy(gameObject);
            }

        }
    }
}
