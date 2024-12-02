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
        PlayerController playerController;
        
        public TMP_Text killsText;
        public TMP_Text wavesCompletedText;
        public TMP_Text timeText;
        
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
    }
}