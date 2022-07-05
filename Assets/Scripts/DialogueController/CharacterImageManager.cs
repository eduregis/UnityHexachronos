using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterImageManager : MonoBehaviour
{
    private static CharacterImageManager instance;

    [Header("Billy")]
    public Sprite Billy_Broke;
    public Sprite Billy_Confused;
    public Sprite Billy_Happy;
    public Sprite Billy_Sad;
    public Sprite Billy_Serious;
    public Sprite Billy_Speechless;
    public Sprite Billy_Struggle;
    public Sprite Billy_Surprised;
    public Sprite Billy_Tired;

    [Header("Hammer")]
    public Sprite Hammer_Angry;
    public Sprite Hammer_Happy;
    public Sprite Hammer_Serious;
    public Sprite Hammer_Smile;
    public Sprite Hammer_Surprised;
    public Sprite Hammer_Sweet;

    [Header("Morya")]
    public Sprite Morya_Sad;
    public Sprite Morya_Sigh;
    public Sprite Morya_Struggle;
    public Sprite Morya_Surprised;

    [Header("Cap")]
    public Sprite Cap_Angry;
    public Sprite Cap_Happy;
    public Sprite Cap_Neutral;
    public Sprite Cap_Sad;
    public Sprite Cap_Serious;
    public Sprite Cap_Shy;
    public Sprite Cap_Surprised;
    public Sprite Cap_Sweet;

    [Header("Sam")]
    public Sprite Sam_Angry;
    public Sprite Sam_Cheeky;
    public Sprite Sam_Defy;
    public Sprite Sam_Focused;
    public Sprite Sam_Happy;
    public Sprite Sam_Sad;
    public Sprite Sam_Serious;
    public Sprite Sam_Shy;
    public Sprite Sam_Surprised;

    [Header("Thunder")]
    public Sprite Thunder_Angry;
    public Sprite Thunder_Broke;
    public Sprite Thunder_Happy;
    public Sprite Thunder_Sad;
    public Sprite Thunder_Serious;
    public Sprite Thunder_Sigh;
    public Sprite Thunder_Sweet;

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
            case "Billy_Broke":
                return Billy_Broke;
            case "Billy_Confused":
                return Billy_Confused;
            case "Billy_Happy":
                return Billy_Happy;
            case "Billy_Sad":
                return Billy_Sad;
            case "Billy_Serious":
                return Billy_Serious;
            case "Billy_Speechless":
                return Billy_Speechless;
            case "Billy_Struggle":
                return Billy_Struggle;
            case "Billy_Surprised":
                return Billy_Surprised;
            case "Billy_Tired":
                return Billy_Tired;
            case "Hammer_Angry":
                return Hammer_Angry;
            case "Hammer_Happy":
                return Hammer_Happy;
            case "Hammer_Serious":
                return Hammer_Serious;
            case "Hammer_Smile":
                return Hammer_Smile;
            case "Hammer_Surprised":
                return Hammer_Surprised;
            case "Hammer_Sweet":
                return Hammer_Sweet;
            case "Morya_Sad":
                return Morya_Sad;
            case "Morya_Sigh":
                return Morya_Sigh;
            case "Morya_Struggle":
                return Morya_Struggle;
            case "Morya_Surprised":
                return Morya_Surprised;
            case "Cap_Angry":
                return Cap_Angry;
            case "Cap_Happy":
                return Cap_Happy;
            case "Cap_Neutral":
                return Cap_Neutral;
            case "Cap_Sad":
                return Cap_Sad;
            case "Cap_Serious":
                return Cap_Serious;
            case "Cap_Shy":
                return Cap_Shy;
            case "Cap_Surprised":
                return Cap_Surprised;
            case "Sam_Angry":
                return Sam_Angry;
            case "Sam_Cheeky":
                return Sam_Cheeky;
            case "Sam_Defy":
                return Sam_Defy;
            case "Sam_Focused":
                return Sam_Focused;
            case "Sam_Happy":
                return Sam_Happy;
            case "Sam_Sad":
                return Sam_Sad;
            case "Sam_Serious":
                return Sam_Serious;
            case "Sam_Shy":
                return Sam_Shy;
            case "Sam_Surprised":
                return Sam_Surprised;
            case "Cap_Sweet":
                return Cap_Sweet;
            case "Thunder_Angry":
                return Thunder_Angry;
            case "Thunder_Broke":
                return Thunder_Broke;
            case "Thunder_Happy":
                return Thunder_Happy;
            case "Thunder_Sad":
                return Thunder_Sad;
            case "Thunder_Serious":
                return Thunder_Serious;
            case "Thunder_Sigh":
                return Thunder_Sigh;
            case "Thunder_Sweet":
                return Thunder_Sweet;
            default:
                return Thunder_Sigh;
        }
    }
}
