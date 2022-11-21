using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* Note: Prefabs do not store the method that the deathZoneEvent calls. Make sure to specify the ResetPlayer method in the
inspector! */ 

public class DeathzoneController : MonoBehaviour
{

    private BoxCollider2D deathZone;
    public UnityEvent deathZoneEvent;

    // Start is called before the first frame update
    void Start()
    {
        deathZone = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D (Collider2D other){
        if(other.CompareTag("Bug")){
            deathZoneEvent.Invoke();
            Debug.Log("entered death zone");
        }
    }


}
