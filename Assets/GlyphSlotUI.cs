using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GlyphSlotUI : MonoBehaviour, IDropHandler
{
    public AddGlyphUI addGlyphUI;
    public void OnDrop(PointerEventData eventData)
    {
       if(eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = transform.parent.GetComponent<RectTransform>().anchoredPosition;
        }

        for (int i = 0; i < transform.parent.parent.childCount; i++)
        {
            if (transform.parent.parent.GetChild(i).GetChild(0) == transform)
            {
                addGlyphUI.PlacedNewGlyph(i);
                break;
            }
        }
    }
}
