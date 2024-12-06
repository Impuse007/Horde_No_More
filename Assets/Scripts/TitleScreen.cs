using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
namespace DentedPixel.LTExamples
{
    public class TitleScreen : MonoBehaviour
    {
        private TextMeshProUGUI titleText;
        public GameObject mainmenuSelection;
        
        // Start is called before the first frame update
        void Start()
        {
            titleText = this.GetComponent<TextMeshProUGUI>();
            TitleScreenTween();
            MainMenuButtons();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void TitleScreenTween()
        {
            Vector3 originalScale = titleText.transform.localScale;
            Vector3 smallScale = originalScale * 0.5f; // 0.5x smaller than the original scale
            Vector3 expandedScale = originalScale * 1.4f; // 0.4x bigger than the original scale

            LeanTween.scale(titleText.gameObject, smallScale, 1f) 
                .setEase(LeanTweenType.easeOutElastic) // Ease out elastic
                .setOnComplete(() =>
                {
                    LeanTween.scale(titleText.gameObject, expandedScale, 1f)
                        .setEase(LeanTweenType.easeOutElastic) // Ease out elastic
                        .setOnComplete(() =>
                        {
                            LeanTween.scale(titleText.gameObject, originalScale, 1f)
                                .setEase(LeanTweenType.easeInOutQuad); // Smooth transition back to original size
                        });
                });
        }

        public void MainMenuButtons()
        {
            Vector3 originalScale = mainmenuSelection.transform.localScale;
            Vector3 smallScale = originalScale * 0.1f; // 0.5x smaller than the original scale
            
            LeanTween.scale(mainmenuSelection.gameObject, smallScale, 0.2f) // 0.2 seconds
                .setEase(LeanTweenType.easeOutElastic) // Ease out elastic
                .setOnComplete(() =>
                {
                    LeanTween.scale(mainmenuSelection.gameObject, originalScale, 1f) // 1 second
                        .setEase(LeanTweenType.easeOutElastic); // Ease out elastic
                });
        }
    }
}
