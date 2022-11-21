using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D body;
    private BoxCollider2D groundCollider;
    private BoxCollider2D ceilingCollider;
    private ConstantForce2D gravity;
    [SerializeField] private float gravConstant = -9.8f;
    private float jumpBuffer = 0.3f;
    public AudioSource jumpSound;
    private bool potentiallyFlipped;
    private float flipTimer = 1;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed; // Allows us to set speed within Unity while keeping it a private variable, for security reasons.

    [SerializeField] private float velocity; // This is so I can see the velocity in real time in the inspector

    [SerializeField] private Sprite groundedBug;
    [SerializeField] private Sprite bug;
    [SerializeField] private Sprite flippedBug;
    
    // Awake is called even if the script is disabled.
    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        groundCollider = transform.GetChild(1).GetComponent<BoxCollider2D>();
        ceilingCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        gravity = GetComponent<ConstantForce2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate() {
        velocity = body.velocity.magnitude;

        // Limits velocity (while jumping at an angle the bug might accidentally reach unsafe velocities)
        if (body.velocity.magnitude > 10f)
            body.velocity = body.velocity.normalized * 10;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(3, 3, 3);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-3, 3, 3);

        // This actually moves the character, exerting a force left or right of the character depending on horizontal input.



        if (isGrounded()) {
            GetComponent<SpriteRenderer>().sprite = groundedBug;
            
            gravity.force = Vector2.zero;
            gravity.relativeForce = new Vector2(0, gravConstant);
            body.AddRelativeForce(new Vector2(horizontalInput*speed, 0));
            if (jumpBuffer > 0) {
                jumpBuffer -= Time.deltaTime;
            } else {
                if (Input.GetKey(KeyCode.Space)) {
                    // body.velocity = body.velocity + (Vector2)transform.up*speed*0.3f;
                    body.AddRelativeForce(new Vector2(0, 10f), ForceMode2D.Impulse);
                    jumpSound.Play();
                    jumpBuffer = 0.3f;
                }
            }
        }
        else {
            jumpBuffer = 0.3f;
            GetComponent<SpriteRenderer>().sprite = bug;
            gravity.relativeForce = Vector2.zero;
            gravity.force = new Vector2(0, gravConstant);
            
            // Helps rotate the player upright mid-air if it has rotated unsafely (turn off if glitchy)
            if (Mathf.Abs(transform.rotation.eulerAngles.z) > 20 ) {
                // transform.rotation = Quaternion.RotateTowards(transform.rotation,)
                transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z/1.1f, transform.rotation.w);
            }
            if (isFlipped()) {
                GetComponent<SpriteRenderer>().sprite = flippedBug;
            }
            else {
                // If bug is flipped, no movement
                body.AddForce(new Vector2(horizontalInput*speed, 0));
            }
            
        }
    }

    // Returns boolean based on if the player is grounded
    private bool isGrounded() {
        RaycastHit2D raycastHit = Physics2D.BoxCast(groundCollider.bounds.center, groundCollider.bounds.size, 0, transform.TransformDirection(Vector2.down), 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    // Checks if bug's back is touching ground, runs a timer for 3 seconds and if continuously flipped over for 3 seconds it negates the bug's Z rotation
    // I think we should play test this with and without the flip detection later on. I feel like the bug flipping mid air is good enough, and this seems like a lot of computing, but there's always the what if the player manages to flip so I don't know.
    private bool isFlipped() {

        RaycastHit2D raycastHit = Physics2D.BoxCast(ceilingCollider.bounds.center, ceilingCollider.bounds.size, 0, transform.TransformDirection(Vector2.up), 0.1f, groundLayer);
        
        // Runs timer if bug's back is touching ground, otherwise resets timer
        if (raycastHit.collider != null) {
            if (potentiallyFlipped) {
                flipTimer -= Time.deltaTime;
            } else {
                potentiallyFlipped = true;
            }
        } else {
            if (potentiallyFlipped) {
                potentiallyFlipped = false;
                flipTimer = 1;
            } 
        }

        if (flipTimer <= 0) {
            transform.Rotate(0, 0, 180);
            flipTimer = 1;
            potentiallyFlipped = false;
        }

        return raycastHit.collider != null;
    }

    void OnCollisionEnter2D (Collision2D coll){
        if ( coll.collider.CompareTag("Water"))
        {
        speed=speed*0.7f;
         Invoke("NormalSpeed",10);
        
      
        }
    }

    void NormalSpeed()
    {
       
         speed=speed/0.7f;
       

    }

    public void ResetPlayer(){
        Vector2 resetPosition = new Vector2(-6.0f, -3.0f); 
        body.transform.position = resetPosition;
    }
}
