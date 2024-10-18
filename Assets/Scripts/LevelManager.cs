using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void LoadLevel(string levelName)
    {
        Application.LoadLevel(levelName);
        Player.SetActive(true);
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Player.SetActive(false);
    }
    
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Player.SetActive(true);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
