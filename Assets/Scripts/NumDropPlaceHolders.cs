using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NumDropPlaceHolders : MonoBehaviour, IDropHandler
{
    public DragableObject draggable;
    public int placeHolderId;
    public float valueOfGem = 21;
    public void OnDrop(PointerEventData eventData)
    {
        draggable = eventData.pointerDrag.GetComponent<DragableObject>();
        if (draggable != null)
        {
            draggable.transform.position = transform.position;
            draggable.droppedCorrectly = true;
            valueOfGem=draggable.GetComponent<NumberInEqScript>().value;
        }
        
    }


}
