using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class DragDir :ScrollRect
{
    public Vector3 dir;
    public override void OnDrag (PointerEventData eventData)
    {
        base.OnDrag (eventData);
        var contentPostion = this.content.anchoredPosition;
        dir = new Vector3(contentPostion.x, 0, contentPostion.y);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        dir = Vector3.zero;
    }
}
