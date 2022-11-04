using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextHighlight : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    [SerializeField] public GameObject _textForHighlight;
    public void OnPointerEnter(PointerEventData eventData)
    {
        _textForHighlight.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _textForHighlight.SetActive(false);
        Debug.Log(_textForHighlight);
    }
}
