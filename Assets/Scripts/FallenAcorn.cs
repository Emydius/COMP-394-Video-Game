using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallenAcorn : MonoBehaviour
{
     
    Rigidbody2D acorn;
    public int timetoStart;
    float speed=5f;
    
    // Start is called before the first frame update
    void Start()
    {
      //acorn = GetComponent<Rigidbody2D>();
     Invoke("LaunchAcorn", timetoStart);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position-= new Vector3(0,speed*Time.deltaTime);
    }

     

    void OnCollisionEnter2D (Collision2D coll){
        // if ( coll.collider.CompareTag("Branch"))
        // {
        // Vector2 nvec= new Vector2(acorn.velocity.x, 5f* acorn.velocity.y);//change the acorn and let it collide a bit and fall through
        
        // acorn.velocity = nvec;

        // }
        if( coll.collider.CompareTag("Bug")){
            Destroy( coll.collider.gameObject);
        }
        

    }


    void LaunchAcorn()
    {
       
            acorn.gravityScale=1;
       

    }
}
