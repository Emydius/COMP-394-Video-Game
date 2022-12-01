using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.Events;

public class OpeningController : MonoBehaviour
{


    private DialogueRunner dialogueRunner;


    // Start is called before the first frame update
    void Start()
    { dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
    dialogueRunner.StartDialogue("Opening_Scene");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
