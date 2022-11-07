using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private ConstantForce2D gravity;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed; // Allows us to set speed within Unity while keeping it a private variable, for security reasons.

    // Awake is called even if the script is disabled.
    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        gravity = GetComponent<ConstantForce2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate() {

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(3, 3, 3);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-3, 3, 3);

        // Only allows movement only if bug is grounded. If in the air, normal downward gravity is applied but if grounded then relative gravity is applied so the bug sticks to the walls.
        if (isGrounded()) {
            body.AddForce(transform.TransformDirection(new Vector2(horizontalInput*speed, 0)));
            gravity.force = Vector2.zero;
            gravity.relativeForce = new Vector2(0, -9.8f);
            // body.velocity = (Vector2) transform.right*horizontalInput*speed;
        }
        else {
            gravity.relativeForce = Vector2.zero;
            gravity.force = new Vector2(0, -9.8f);
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded()) {
            // body.velocity += (Vector2) transform.up.normalized * speed;
            // TODO: Multiply this speed so that as the bug's rotation is farther from 0 the force of the jump is lower (if the bug jumps sideways then gravity isn't acting against it and it flies off into oblivion)
            body.AddForce(transform.TransformDirection(new Vector2(0, speed, ForceMode2D.Impulse);
            // body.velocity = transform.TransformDirection(new Vector2(body.velocity.x, speed));
            // body.velocity += (Vector2) transform.TransformDirection(new Vector2(0, speed * (transform.rotation.eulerAngles.z != 0 ? 1f/Mathf.Abs(transform.rotation.eulerAngles.z) : 1)));
            
        }

        // Helps rotate the player upright mid-air if it is rotating unsafely (off for now)
        // if (!isGrounded()) {
        //     if (transform.rotation.eulerAngles.z > 30 && transform.rotation.eulerAngles.z < 180) {
        //         Debug.Log("Rotation Detected");
        //         transform.Rotate(0, 0, -20f);
        //     } 
        //     else if (transform.rotation.eulerAngles.z < 330 && transform.rotation.eulerAngles.z >= 180) {
        //         transform.Rotate(0, 0, 20f);
        //     }
        // }
    }

    // Returns boolean based on if the player is grounded
    private bool isGrounded() {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
}
