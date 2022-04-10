using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterImageManager : MonoBehaviour
{
    private static CharacterImageManager instance;

    public Sprite Cap_Sad;
    public Sprite Thunder_Sigh;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one DialogueManager in the scene");
        }
        instance = this;
    }
    public static CharacterImageManager GetInstance()
    {
        return instance;
    }

    public Sprite ChangeCharacterImage(String imageText)
    {
        switch (imageText)
        {
            case "Thunder_Sigh":
                return Thunder_Sigh;
            case "Cap_Sad":
                return Cap_Sad;
            case "Cap":
                return Cap_Sad;
            default:
                return Thunder_Sigh;
        }
    }
}
