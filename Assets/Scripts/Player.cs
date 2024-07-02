using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    Animator anim;
    public float movespeed;
    public float originaljumpforce; // normal jump force
    public float gravjumpforce; // jumpforce when anti-gravity is on
    float currentjumpforce;
    public float groundcheckradius;
    bool grounded;
    public bool canMove;
    public bool holding;
    Collider2D boxrange;
    public Transform box;
    public AudioClip jumpsound, pickupsound;
    public float defaultgravity;
    public Transform groundcheckpos; // position at which ground is checked for
    bool antigrav; // is anti-gravity on?
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canMove = true;
        antigrav = false;
        currentjumpforce = originaljumpforce;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed",Mathf.Abs(rb.velocity.x));
        anim.SetBool("Grounded", grounded);
        anim.SetBool("Holding", holding);
        if (canMove) {
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * movespeed, rb.velocity.y); // moves player with rigidbody and player input
            if (Input.GetButtonDown("Jump") && grounded) { // makes sure can only jump if on ground
                rb.velocity = new Vector2(rb.velocity.x, currentjumpforce);
                AudioManager.instance.PlaySFX(jumpsound);
            }
            if (Input.GetKeyDown(KeyCode.E)) {
                if (boxrange && !holding) { // pickup box if there's one in range and player isn't already holding one
                    Pickup();
                } 
                else if (holding) {
                    Release();
                }
            }
            if (holding) {
                box.GetComponentInChildren<Box>().transform.position = box.position; // ensures that box follows player
            }
            if (antigrav) { //orientation of player when anti-gravity is on
                if (Input.GetAxisRaw("Horizontal") > 0) { // change direction player is facing
                    transform.localScale = new Vector3(-1, 1, 1);
                } 
                else if (Input.GetAxisRaw("Horizontal") < 0) {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            } 
            else { //oritentation when anti-gravity is off
                if (Input.GetAxisRaw("Horizontal") > 0) {
                    transform.localScale = new Vector3(1, 1, 1);
                } 
                else if (Input.GetAxisRaw("Horizontal") < 0) {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }
           
        }
        grounded = Physics2D.OverlapCircle(groundcheckpos.position, groundcheckradius,LayerMask.GetMask("Ground","Pickups","Explosives")); // check for ground
        boxrange = Physics2D.OverlapBox(box.position, Vector2.one, 0, LayerMask.GetMask("Pickups", "Explosives"));// checks for boxes
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<AntiGravityMachine>() && collision.GetComponent<AntiGravityMachine>().on) { // improve anti-gravity
            rb.gravityScale = Mathf.Epsilon;
            antigrav = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetComponent<AntiGravityMachine>()) {
            rb.gravityScale = defaultgravity; // reset gravity
            currentjumpforce = originaljumpforce; // reset direction of jump
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0); // reset rotation
            antigrav = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.GetComponent<AntiGravityMachine>() && collision.GetComponent<AntiGravityMachine>().on) {
            // turn player upside down
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
            rb.gravityScale = Mathf.Epsilon;
            currentjumpforce = gravjumpforce; // reverse direction of jump
            antigrav = true;
        }
        if(collision.GetComponent<AntiGravityMachine>() && !collision.GetComponent<AntiGravityMachine>().on) {
            rb.gravityScale = defaultgravity; // reset gravity when machine is off
            currentjumpforce = originaljumpforce; // reset direction of jump
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0); // reset rotation
            antigrav = false;
        }
    }
    void Pickup() {
        if (holding) return; // ensures only one box can be held at a time
        AudioManager.instance.PlaySFX(pickupsound);
        boxrange.gameObject.SetActive(false);
        holding = true;
        boxrange.transform.parent = box;
        boxrange.GetComponent<Rigidbody2D>().gravityScale = 0; // set gravity to 0 so box doesn't fall while held
        boxrange.GetComponent<Rigidbody2D>().mass = 0; // set mass to 0 so no force is exerted on player
        boxrange.GetComponent<Collider2D>().enabled = false; // disable collider so box doesn't get detected by boxcast anymore
        boxrange.gameObject.SetActive(true);
    }
    void Release() {
        AudioManager.instance.PlaySFX(pickupsound);
        box.GetComponentInChildren<Rigidbody2D>().gravityScale = box.GetComponentInChildren<Box>().defaultgravity; // reset gravity
        box.GetComponentInChildren<Rigidbody2D>().mass = box.GetComponentInChildren<Box>().defaultmass; // reset mass
        holding = false;
        box.GetComponentInChildren<Collider2D>().enabled = true; // re-enable collider
        box.DetachChildren();
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundcheckpos.position, groundcheckradius);
        Gizmos.DrawWireCube(box.position, new Vector2(1, 1));
    }
}
