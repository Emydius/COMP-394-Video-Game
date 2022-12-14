using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D body;
    private BoxCollider2D groundCollider;
    private BoxCollider2D ceilingCollider;
    private ConstantForce2D gravity;
    [SerializeField] private float gravConstant = -29.4f;
    private float currentGravity;
    private float flipTimer = 1;
    private float jumpBuffer = 0.3f;
    private float maxSpeed;
    [SerializeField] private float jumpForce = 20f;
    public AudioSource jumpSound;
    private bool potentiallyFlipped;
    private bool dashing;
    [SerializeField] private bool dashEnabled = false; //should be enabled when dialogue with squirrel finishes

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 20f;
    private float dashingTime = 0.4f;
    private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;

    
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed; // Allows us to set speed within Unity while keeping it a private variable, for security reasons.

    [SerializeField] private float velocity; // This is so I can see the velocity in real time in the inspector

    [SerializeField] private Sprite groundedBug;
    [SerializeField] private Sprite bug;
    [SerializeField] private Sprite flippedBug;

    private bool frozen = false;
    
    // Awake is called even if the script is disabled.
    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        groundCollider = transform.GetChild(1).GetComponent<BoxCollider2D>();
        ceilingCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        gravity = GetComponent<ConstantForce2D>();
        currentGravity = gravConstant;
    }

    // Start is called before the first frame update
    private void Start() {

    }

    // Update is called once per frame
    private void FixedUpdate() {
        if(!frozen){
            if (Input.GetKey(KeyCode.Z) && canDash && dashEnabled) {
                    StartCoroutine(Dash());
                }

            velocity = body.velocity.magnitude;

            // Limits velocity (we require a big force to accelerate the bug quickly, but don't want it to actually go that fast)
            if (isDashing) {
                maxSpeed = 18f;
            } else maxSpeed = 10f;
            if (body.velocity.magnitude > maxSpeed)
                body.velocity = body.velocity.normalized * maxSpeed;
                // Saves the current horizontal input to call it more easily
            float horizontalInput = Input.GetAxisRaw("Horizontal");

            // Flips sprite depending on direction
            if (horizontalInput > 0.01f)
                transform.localScale = new Vector3(3, 3, 3);
            else if (horizontalInput < -0.01f)
                transform.localScale = new Vector3(-3, 3, 3);


            if (isGrounded()) {
                
                // If bug is grounded, regular gravity turns off and relative gravity turns on to make it stick to surfaces
                gravity.force = Vector2.zero;
                gravity.relativeForce = new Vector2(0, currentGravity);
                
                if (isDashing) {
                    return;
                }
                    
                // Actually moves character relative to its orientation, if grounded
                body.AddRelativeForce(new Vector2(horizontalInput*speed, 0));


                if (jumpBuffer > 0) {
                    // When grounded, starts a timer counting down before character is able to jump again
                    jumpBuffer -= Time.deltaTime;
                } else if (Input.GetKey(KeyCode.Space)) {
                    // Use space to jump by adding an impulse upward, reset jump buffer timer if jumping
                    body.AddRelativeForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    jumpSound.Play();
                    jumpBuffer = 0.3f;
                }
            }
            else {

                jumpBuffer = 0.3f;

                // If bug isn't grounded, regular gravity is applied to make it fall
                gravity.relativeForce = Vector2.zero;
                gravity.force = new Vector2(0, currentGravity);
                
                if (isDashing) {
                    return;
                }
                
                // Helps rotate the player upright mid-air if it has rotated unsafely, curved to be smooth
                if (Mathf.Abs(transform.rotation.eulerAngles.z) > 20 ) {
                    transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z/1.1f, transform.rotation.w);
                }
                // If bug is on it backs, starts a timer to flip and also makes it green for debugging purposes
                if (isFlipped()) {
                    // I forgor what goes here
                }
                else {
                    // This actually makes the bug move; only executes if bug isn't flipped
                    body.AddForce(new Vector2(horizontalInput*speed, 0));
                }
            }
        }
        
    }

    // Coroutine allowing player to dash. Turns off gravity and permanently sets velocity to specified direction. After waiting for dashingTime, reverts to normal.
    private IEnumerator Dash() {
        canDash = false;
        isDashing = true;
        currentGravity = 0;
        body.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))*dashingPower;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        currentGravity = gravConstant;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }

    // Returns boolean based on if the player is grounded
    private bool isGrounded() {
        // Box cast from bug's feet down, to check if grounded
        RaycastHit2D raycastHit = Physics2D.BoxCast(groundCollider.bounds.center, groundCollider.bounds.size, 0, transform.TransformDirection(Vector2.down), 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    // Checks if bug's back is touching ground, runs a timer for 3 seconds and if continuously flipped over for 3 seconds it negates the bug's Z rotation
    // Too computationally taxing?? Idk
    private bool isFlipped() {

        // Box cast from bug's head up, to check if flipped
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

        // Once timer is up, rotate bug 180 degrees
        if (flipTimer <= 0) {
            transform.Rotate(0, 0, 180);
            flipTimer = 1;
            potentiallyFlipped = false;
        }

        // Return if bug flipped or not
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
        Vector2 resetPosition = new Vector2(-660f, 264f); 
        Quaternion resetRotation = new Quaternion(0,0,0,0);
        transform.rotation = resetRotation;
        body.transform.position = resetPosition;
    }

    public void FreezePlayer (){
        frozen = true;
        Debug.Log("frozen");
    }

    public void UnfreezePlayer(){
        frozen = false;
        Debug.Log("unfrozen");
    }

    [YarnCommand("enable_dash")]
    void EnableDash(){
        dashEnabled = true;
        Debug.Log("dash now allowed");
    }
}
