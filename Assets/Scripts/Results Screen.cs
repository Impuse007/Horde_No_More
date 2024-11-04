using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;

namespace DefaultNamespace
{
    public class Results_Screen : MonoBehaviour
    {
        public GameManager gameManager;
        PlayerController playerController;
        
        public TMP_Text killsText;
        public TMP_Text playerMoneyText;
        public TMP_Text timeText;
        
        public void Update()
        {
            killsText.text = "Kills: " + gameManager.kills;
            //playerMoneyText.text = "Money: " + gameManager.playerController.playerMoney;
            timeText.text = "Time: " + gameManager.timeInGame;
        }
    }
}