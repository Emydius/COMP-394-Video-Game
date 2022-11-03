using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenAcorn : MonoBehaviour
{
     
    Rigidbody2D acorn;
    public int timetoStart;
    
    // Start is called before the first frame update
    void Start()
    {
      acorn = GetComponent<Rigidbody2D>();
     Invoke("LaunchAcorn", timetoStart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     

    void OnCollisionEnter2D (Collision2D coll){
        if ( coll.collider.CompareTag("Branch"))
        {
        Vector2 nvec= new Vector2(acorn.velocity.x, 0.5f* acorn.velocity.y+0.5f*coll.collider.attachedRigidbody.velocity.y);
        acorn.velocity = nvec;

        }
        if( coll.collider.CompareTag("Bug")){
            Destroy( coll.collider.gameObject);
        }
        

    }

    void ResetBall()
    { 
        Vector2 nvect= new Vector2(0,0);
        acorn.velocity = nvect;
        transform.position = Vector2.zero;//?
        Invoke("LaunchAcorn", 1);
    }

    void LaunchAcorn()
    {
       
            acorn.gravityScale=1;
       

    }
}
