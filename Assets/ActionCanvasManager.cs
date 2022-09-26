using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionCanvasManager : MonoBehaviour
{
    public GameObject actionCanvas;

    public GameObject overlay;

    public Transform hero1;
    public Transform hero2;
    public Transform hero3;
    public Transform enemy1;
    public Transform enemy2;
    public Transform enemy3;

    private static ActionCanvasManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one ActionCanvasManager in the scene");
        }
        instance = this;
    }
    public static ActionCanvasManager GetInstance()
    {
        return instance;
    }
    private void Start() {
        actionCanvas.SetActive(false);
    }
    public void TriggerAction() {
        actionCanvas.SetActive(true);
    }

    public void DismissAction() {
        actionCanvas.SetActive(false);
    }
}
