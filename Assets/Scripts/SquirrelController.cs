using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.Events;

public class SquirrelController : MonoBehaviour
{ 
    private DialogueRunner dialogueRunner;
    //public UnityEvent dialogueEvent; See comment below
    

    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        Debug.Log("triggered");
        dialogueRunner.StartDialogue("First");
        //dialogueEvent.Invoke(); 
        //Eventually, this event can be hooked up to a method freezing the player's movement until the
        //dialogue has finished.

    }
}
