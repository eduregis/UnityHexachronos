using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterImageManager : MonoBehaviour {

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

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("Found more than one DialogueManager in the scene");
        }
        instance = this;
    }

    public static CharacterImageManager GetInstance() {
        return instance;
    }

    public Sprite ChangeCharacterImage(String imageText) {
        return imageText switch {
            "Billy" => Billy_Serious,
            "Billy_Broke" => Billy_Broke,
            "Billy_Confused" => Billy_Confused,
            "Billy_Happy" => Billy_Happy,
            "Billy_Sad" => Billy_Sad,
            "Billy_Serious" => Billy_Serious,
            "Billy_Speechless" => Billy_Speechless,
            "Billy_Struggle" => Billy_Struggle,
            "Billy_Surprised" => Billy_Surprised,
            "Billy_Tired" => Billy_Tired,
            "Hammer_Happy" => Hammer_Happy,
            "Hammer_Angry" => Hammer_Angry,
            "Hammer" => Hammer_Happy,
            "Hammer_Serious" => Hammer_Serious,
            "Hammer_Smile" => Hammer_Smile,
            "Hammer_Surprised" => Hammer_Surprised,
            "Hammer_Sweet" => Hammer_Sweet,
            "Morya" => Morya_Sigh,
            "Morya_Sad" => Morya_Sad,
            "Morya_Sigh" => Morya_Sigh,
            "Morya_Struggle" => Morya_Struggle,
            "Morya_Surprised" => Morya_Surprised,
            "Cap" => Cap_Neutral,
            "Cap_Angry" => Cap_Angry,
            "Cap_Happy" => Cap_Happy,
            "Cap_Neutral" => Cap_Neutral,
            "Cap_Sad" => Cap_Sad,
            "Cap_Serious" => Cap_Serious,
            "Cap_Shy" => Cap_Shy,
            "Cap_Surprised" => Cap_Surprised,
            "Cap_Sweet" => Cap_Sweet,
            "Sam" => Sam_Serious,
            "Sam_Angry" => Sam_Angry,
            "Sam_Cheeky" => Sam_Cheeky,
            "Sam_Defy" => Sam_Defy,
            "Sam_Focused" => Sam_Focused,
            "Sam_Happy" => Sam_Happy,
            "Sam_Sad" => Sam_Sad,
            "Sam_Serious" => Sam_Serious,
            "Sam_Shy" => Sam_Shy,
            "Sam_Surprised" => Sam_Surprised,
            "Thunder" => Thunder_Serious,
            "Thunder_Angry" => Thunder_Angry,
            "Thunder_Broke" => Thunder_Broke,
            "Thunder_Happy" => Thunder_Happy,
            "Thunder_Sad" => Thunder_Sad,
            "Thunder_Serious" => Thunder_Serious,
            "Thunder_Sigh" => Thunder_Sigh,
            "Thunder_Sweet" => Thunder_Sweet,
            _ => Thunder_Sigh,
        };
    }
}
