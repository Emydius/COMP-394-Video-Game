using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.Events;

public class SquirrelController : MonoBehaviour
{ 
    private DialogueRunner dialogueRunner;
    private bool conversationStarted;

    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        conversationStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if(!conversationStarted){
            dialogueRunner.StartDialogue("First");
            conversationStarted = true;
        }
    }
}
