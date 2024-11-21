using System;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class HoveringOverSkills : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color hoverColor = Color.white;
    private Color originalColor;
    private Image image;
    public bool skillBrought = false;
    
    private void Start()
    {
        image = GetComponent<Image>();
        if (image != null && !skillBrought)
        {
            originalColor = image.color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (image != null && !skillBrought)
        {
            image.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (image != null && !skillBrought)
        {
            image.color = originalColor;
        }
    }
}
