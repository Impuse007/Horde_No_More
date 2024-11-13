using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CursorController : MonoBehaviour
{
    // Needs cursor to play the animation when on click and not automatically. 
    
    [SerializeField] private Texture2D[] cursorTextureArray;

    [SerializeField] private int  frameCount;
    [SerializeField] private float frameRate;

    private int currentFrame;
    private float frameTimer;

    [SerializeField] private Vector2 clickPosition = new Vector2(4, 4);

    void Start()
    {
        Cursor.SetCursor(cursorTextureArray[0], clickPosition, CursorMode.Auto);
    }

    private void Update()
    {
        if (frameRate <= 0)
        {
            return;
        }
        
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0)
        {
            frameTimer += frameRate;
            currentFrame = (currentFrame + 1) % frameCount;
            Cursor.SetCursor(cursorTextureArray[currentFrame], clickPosition, CursorMode.Auto);
        }
    }
}
