using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallenAcorn : MonoBehaviour
{
     public UnityEvent loseLifeEvent;
    Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
      rb = GetComponent<Rigidbody2D>();
     Invoke("LaunchBall", 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     

    void OnCollisionEnter2D (Collision2D coll){
        if ( coll.collider.CompareTag("Player"))
        {
        Vector2 nvec= new Vector2(rb.velocity.x, 0.5f* rb.velocity.y+0.5f*coll.collider.attachedRigidbody.velocity.y);
        rb.velocity = nvec;

        }
        if( coll.collider.CompareTag("Brick")){
            Destroy( coll.collider.gameObject);
        }
        if( coll.collider.CompareTag("BottomWall")){
            loseLifeEvent.Invoke();
            ResetBall();
        }

    }

    void ResetBall()
    { 
        Vector2 nvect= new Vector2(0,0);
        rb.velocity = nvect;
        transform.position = Vector2.zero;//?
        Invoke("LaunchBall", 1);
    }

    void LaunchBall()
    {
        // Get the ball's rigidbody
        float randomVal = Random.Range(0f, 2f);

        if (randomVal < 1) {
            // Launch the ball to the left
            rb.AddForce(new Vector2(-10, -20));
        } else {
            // Launch the ball to the right
            rb.AddForce(new Vector2(10, -20));
        }

    }
}
