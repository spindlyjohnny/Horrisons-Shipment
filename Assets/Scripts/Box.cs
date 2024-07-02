using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    protected Player player;
    public float defaultgravity, defaultmass;
    protected Rigidbody2D rb;
    protected Vector2 force; // force currently exerted on box
    public GameObject breakvfx;
    public AudioClip breaksound;
    [SerializeField]bool breakable; // for breakable boxes
    public float breakforce; // force at which box breaks 
    protected bool impactpoint; // ensures that boxes don't break when nothing is touching them
    public float detectionlength;
    // Start is called before the first frame update
    protected void Start()
    {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!breakable) return; // don't run these lines on non-breakable boxes
        impactpoint = CheckForImpact();
        // destroy box if not being held and force exerted is over limit
        if (impactpoint && !player.holding && force.magnitude >= breakforce) {
            Destruct();
        }
    }
    protected bool CheckForImpact() {
        // box only breaks when touching ground or other boxes
        var downcast = Physics2D.Raycast(transform.position - new Vector3(0, .25f, 0), Vector2.down, detectionlength, LayerMask.GetMask("Ground", "Pickups", "Explosives"));
        var upcast = Physics2D.Raycast(transform.position + new Vector3(0, .25f, 0), Vector2.up, detectionlength, LayerMask.GetMask("Ground", "Pickups", "Explosives"));
        if ((downcast && downcast.collider != GetComponent<Collider2D>()) || (upcast && upcast.collider != GetComponent<Collider2D>())) return true;
        else return false;
    }
    protected void FixedUpdate() {
        if(breakable)force = CalculateForce(Vector2.zero, rb.velocity); // only calculate force if box can break
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<AntiGravityMachine>() && collision.GetComponent<AntiGravityMachine>().on) { // improve anti-gravity
            rb.gravityScale = Mathf.Epsilon;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetComponent<AntiGravityMachine>()) {
            rb.gravityScale = defaultgravity; // reset gravity
        }
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.GetComponent<AntiGravityMachine>() && !collision.GetComponent<AntiGravityMachine>().on) {
            rb.gravityScale = defaultgravity; // reset gravity
        }
    }
    protected Vector2 CalculateForce(Vector2 finalvelocity, Vector2 initialvelocity) { // for the purpose of creating breakable boxes
        Vector2 acceleration = (finalvelocity - initialvelocity) / Time.fixedDeltaTime;
        Vector2 force = acceleration * defaultmass;
        return force;
    }
    protected virtual void Destruct() {
        Destroy(Instantiate(breakvfx, transform.position, transform.rotation), .5f);
        Destroy(gameObject,.1f);
        //AudioManager.instance.PlaySFX(breaksound);
    }
    private void OnDrawGizmos() {
        if (!breakable) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position - new Vector3(0, .25f, 0), Vector3.down * detectionlength);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + new Vector3(0, .25f, 0), Vector3.up * detectionlength);
    }
}
