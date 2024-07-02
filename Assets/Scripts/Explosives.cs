using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosives : Box
{
    public float explosiveforce;
    public float explosionradius;
    Collider2D[] inradius; // colliders within explosionradius
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        inradius = Physics2D.OverlapCircleAll(transform.position, explosionradius);
        impactpoint = CheckForImpact();
        if (impactpoint && !player.holding && force.magnitude >= breakforce) {
            Explode(explosiveforce);
        }
    }
    void Explode(float explosiveforce) {
        foreach (var i in inradius) {
            Rigidbody2D rb2D = i.GetComponent<Rigidbody2D>();
            if (rb2D != null) {
                Vector2 dist = i.transform.position - transform.position;
                if (dist.magnitude > 0) { // avoids division by 0
                    explosiveforce /= dist.magnitude; // decreases force based on distance
                    rb2D.AddForce(explosiveforce * dist.normalized);
                }
            }
        }
        Destruct();
    }
    protected override void Destruct() {
        GameObject go = Instantiate(breakvfx, transform.position, transform.rotation);
        Animator goanim = go.GetComponent<Animator>();
        // wait for animation to finish first
        Destroy(go, goanim.GetCurrentAnimatorStateInfo(0).length + goanim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        Destroy(gameObject, .1f);
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, explosionradius);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position - new Vector3(0, .25f, 0), Vector3.down * detectionlength);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + new Vector3(0, .25f, 0), Vector3.up * detectionlength);
    }
}
