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

    public Sprite Arrow_Up_Highlighted;
    public Sprite Arrow_Down_Highlighted;
    public Sprite Arrow_Left_Highlighted;
    public Sprite Arrow_Right_Highlighted;

    public Sprite empty;

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
            case "arrow_up_highlighted":
                return Arrow_Up_Highlighted;
            case "arrow_down":
                return Arrow_Down;
            case "arrow_down_highlighted":
                return Arrow_Down_Highlighted;
            case "arrow_left":
                return Arrow_Left;
            case "arrow_left_highlighted":
                return Arrow_Left_Highlighted;
            case "arrow_right":
                return Arrow_Right;
            case "arrow_right_highlighted":
                return Arrow_Right_Highlighted;
            default:
                return empty;
        }
    }
}
