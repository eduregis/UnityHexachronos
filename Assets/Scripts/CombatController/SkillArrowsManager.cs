using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillArrowsManager : MonoBehaviour
{
    [Header("Skill Menu UI")]
    [SerializeField] private Image[] skill1Arrows;
    [SerializeField] private Image[] skill2Arrows;
    [SerializeField] private Image[] skill3Arrows;

    private static SkillArrowsManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one SkillArrowsManager in the scene");
        }
        instance = this;
    }
    public static SkillArrowsManager GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var skill in skill1Arrows) { skill.enabled = false; }
        foreach (var skill in skill2Arrows) { skill.enabled = false; }
        foreach (var skill in skill3Arrows) { skill.enabled = false; }
    }

    public void ShowSkillCombo(List<int> sequences, int techniqueCode)
    {
        switch (techniqueCode)
        {
            case 1:
                for (int i = 0; i < sequences.Count; i++) {
                    skill1Arrows[i].enabled = true;
                    skill1Arrows[i].sprite = GetSkillArrowSprite(sequences[i]);
                }
                for(int i = sequences.Count; i < skill1Arrows.Length; i++)
                {
                    skill1Arrows[i].sprite = ArrowSpriteManager.GetInstance().ChangeArrowSprite("");
                    skill1Arrows[i].enabled = false;
                }
                break;
            case 2:
                for (int i = 0; i < sequences.Count; i++)
                {
                    skill2Arrows[i].enabled = true;
                    skill2Arrows[i].sprite = GetSkillArrowSprite(sequences[i]);
                }
                for (int i = sequences.Count; i < skill1Arrows.Length; i++)
                {
                    skill2Arrows[i].sprite = ArrowSpriteManager.GetInstance().ChangeArrowSprite("");
                    skill2Arrows[i].enabled = false;
                }
                break;
            case 3:
                for (int i = 0; i < sequences.Count; i++)
                {
                    skill3Arrows[i].enabled = true;
                    skill3Arrows[i].sprite = GetSkillArrowSprite(sequences[i]);
                }
                for (int i = sequences.Count; i < skill1Arrows.Length; i++)
                {
                    skill3Arrows[i].sprite = ArrowSpriteManager.GetInstance().ChangeArrowSprite("");
                    skill3Arrows[i].enabled = false;
                }
                break;
            default:
                break;
        }
    }

    private Sprite GetSkillArrowSprite(int code)
    {
        switch (code)
        {
            case 1:
                return ArrowSpriteManager.GetInstance().ChangeArrowSprite("arrow_up");
            case 2:
                return ArrowSpriteManager.GetInstance().ChangeArrowSprite("arrow_left");
            case 3:
                return ArrowSpriteManager.GetInstance().ChangeArrowSprite("arrow_right");
            case 4:
                return ArrowSpriteManager.GetInstance().ChangeArrowSprite("arrow_down");
            default:
                return ArrowSpriteManager.GetInstance().ChangeArrowSprite("");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
