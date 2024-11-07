using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D[] cursorTextureArray;

    [SerializeField] private int  frameCount;
    [SerializeField] private float frameRate;

    private int currentFrame;
    private float frameTimer;

    [SerializeField] private Vector2 clickPostion = new Vector2(4, 4);

    void start()
    {
        Cursor.SetCursor(cursorTextureArray[0], clickPostion, CursorMode.Auto);
    }

    private void Update()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;
            currentFrame = (currentFrame + 1) % frameCount;
            Cursor.SetCursor(cursorTextureArray[currentFrame], clickPostion, CursorMode.Auto);
        }
    }
}
