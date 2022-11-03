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

    // Vector2 gravity = new Vector2(0, -9.8f);

    // Awake is called even if the script is disabled.
    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        gravity = GetComponent<ConstantForce2D>();
        // body.useGravity = false;
        // LeftGravity();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update() {

        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(3, 3, 3);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-3, 3, 3);

        if (Input.GetKey(KeyCode.Space) && isGrounded()) {
            body.velocity = new Vector2(body.velocity.x, speed);
        }

        // Helps rotate the player upright mid-air if it is rotating unsafely
        // if (!isGrounded()) {
            if (transform.rotation.eulerAngles.z > 30 && transform.rotation.eulerAngles.z < 180) {
                Debug.Log("Rotation Detected");
                transform.Rotate(0, 0, -0.4f);
            } 
            else if (transform.rotation.eulerAngles.z < 330 && transform.rotation.eulerAngles.z >= 180) {
                transform.Rotate(0, 0, 0.4f);
            }
        // }
    }

    // Returns boolean based on if the player is grounded
    private bool isGrounded() {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private void LeftGravity() {
        // gravity = new Vector2(-9.8f, 0);
    }
}
