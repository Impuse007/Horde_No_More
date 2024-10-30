using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject Player;
    
    public void LoadLevel(string levelName)
    {
        Application.LoadLevel(levelName);
        Player.SetActive(true);
        Player.transform.position = new Vector3(0, 0, 0);
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Player.SetActive(false);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
