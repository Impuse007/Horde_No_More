using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class Results_Screen : MonoBehaviour
    {
        public GameManager gameManager;
        public WaveManager waveManager;
        public UIManager uiManager;
        PlayerController playerController;
        
        public TMP_Text killsText;
        public TMP_Text wavesCompletedText;
        public TMP_Text timeText;
        
        public GameObject resultsScreenBackground;
        
        public void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            playerController = FindObjectOfType<PlayerController>();
        }
        
        public void Update()
        {
            if (SceneManager.GetActiveScene().name == "Level 1")
            {
                waveManager = FindObjectOfType<WaveManager>();
            }
            
            killsText.text = "Kills: " + gameManager.kills;
            wavesCompletedText.text = "Wave: " + gameManager.waveNumber + 1 + "/30"; 
            timeText.text = "Time: " + gameManager.timeInGame.ToString("F2"); // 2 decimal places
        }

        public void ResultScreenTween()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Debug.LogError("RectTransform component is missing.");
                return;
            }

            Vector3 originalScale = rectTransform.localScale;
            Vector3 smallScale = originalScale * 0.5f; // 0.5x smaller than the original scale

            LeanTween.scale(rectTransform, smallScale, 1f)
                .setEase(LeanTweenType.easeOutElastic)
                .setOnComplete(() =>
                {
                    LeanTween.scale(rectTransform, originalScale, 1f)
                        .setEase(LeanTweenType.easeOutElastic);
                });

            CanvasGroup canvasGroup = resultsScreenBackground.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = resultsScreenBackground.AddComponent<CanvasGroup>();
            }

            LeanTween.alphaCanvas(canvasGroup, 0.5f, 1f)
                .setEase(LeanTweenType.easeOutElastic)
                .setOnComplete(() =>
                {
                    LeanTween.alphaCanvas(canvasGroup, 1f, 1f)
                        .setEase(LeanTweenType.easeOutElastic);
                });

            Debug.Log("Results Screen Tween");
        }
    }
}