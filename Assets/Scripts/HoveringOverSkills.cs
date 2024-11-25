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
        SFXManager.instance.PlayEnvironmentSFX(1); // Play the hover sound effect
        if (image != null)
        {
            Color color = skillBrought ? Color.green : hoverColor;
            color.a = 1f; // Fully opaque
            image.color = color;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (image != null)
        {
            Color color = skillBrought ? Color.green : originalColor;
            color.a = 0.5f; // Semi-transparent
            image.color = color;
        }
    }
}
