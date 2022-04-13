using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArrowSpriteManager : MonoBehaviour
{
    private static ArrowSpriteManager instance;

    public Sprite Arrow_Up;
    public Sprite Arrow_Down;
    public Sprite Arrow_Left;
    public Sprite Arrow_Right;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one DialogueManager in the scene");
        }
        instance = this;
    }
    public static ArrowSpriteManager GetInstance()
    {
        return instance;
    }

    public Sprite ChangeArrowSprite(String imageText)
    {
        switch (imageText)
        {
            case "arrow_up":
                return Arrow_Up;
            case "arrow_down":
                return Arrow_Down;
            case "arrow_left":
                return Arrow_Left;
            case "arrow_right":
                return Arrow_Right;
            default:
                return Arrow_Up;
        }
    }
}
