using UnityEngine;
using UnityEngine.EventSystems;

public class SpellCard : MonoBehaviour,IPointerClickHandler
{
    public int id;
    public bool unlocked;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (unlocked)
        {
            foreach (Transform card in transform.parent)
            {
                if (card.GetComponent<SpellCard>().id != id)
                {
                    card.gameObject.SetActive(false);
                }
            }
            transform.parent.GetComponent<SpellCardPanel>().ShowStatsAndLvlUp(id);
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
