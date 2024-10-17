using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelManager levelManager;
    public UIManager uiManager;
    public PlayerController playerController;
    
    public void Awake()
    {
        
    }

    public void Update()
    {
        //uiManager.ActivateUIBasedOnScene();
    }
}
