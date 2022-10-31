using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;


public class SquirrelController : MonoBehaviour
{ 
    private DialogueRunner dialogueRunner;
    

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
    }
}
