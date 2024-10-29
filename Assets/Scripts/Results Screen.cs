using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;

namespace DefaultNamespace
{
    public class Results_Screen
    {
        GameManager gameManager;
        PlayerController playerController;
        
        public TMP_Text scoreText;
        public TMP_Text highScoreText;
        public TMP_Text killsText;
        public TMP_Text playerMoneyText;
        public TMP_Text waveNumberText;
        public TMP_Text timeText;
        
        public void Update()
        {
            scoreText.text = "Score: " + gameManager.playerScore;
            highScoreText.text = "High Score: " + gameManager.highScore;
            killsText.text = "Kills: " + gameManager.kills;
            playerMoneyText.text = "Money: " + gameManager.playerController.playerMoney;
            waveNumberText.text = "Wave: " + gameManager.waveNumber;
            timeText.text = "Time: " + gameManager.timeInGame;
        }
    }
}