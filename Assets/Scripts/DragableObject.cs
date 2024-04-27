using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class DragableObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform parent;
    public CanvasGroup canvasGroup;
    public bool droppedCorrectly  = false;

    private void Start()
    {
        parent = transform.parent;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(GameManager.instance.canvas.transform);
        droppedCorrectly = false;
        GameManager.instance.draggedObject = gameObject;
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        if (!droppedCorrectly)
        {
            transform.SetParent(parent);
        }

    }
}
