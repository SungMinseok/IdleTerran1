using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBlocker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static UIBlocker instance;
    void Awake(){
        instance = this;
    }

    public void OnPointerEnter (PointerEventData eventData) 
    {
        UIManager.instance.uiBlocked = true;
    }
    public void OnPointerExit (PointerEventData eventData) 
    {

        UIManager.instance.uiBlocked = false;
    }
}
