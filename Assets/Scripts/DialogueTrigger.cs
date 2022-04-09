using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(inkJSON.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
