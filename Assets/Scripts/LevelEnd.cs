using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    LevelManager levelManager;
    CameraController cam;
    Player player;
    Animator anim;
    bool move;
    public AudioClip levelclearsound,endvoiceover;
    public GameObject computerclear, computerdefault; // computer sprite indicating if box is on elevator
    public Collider2D[] ignoreplayer;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        cam = FindObjectOfType<CameraController>();
        player = FindObjectOfType<Player>();
        anim = GetComponent<Animator>();
        move = false;
        computerclear.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Move", move);
        foreach(var i in ignoreplayer) {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), i); // ensures player doesn't go up with elevator
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Goal Box")) { // check if goal box is on elevator
            StartCoroutine(CamPan());
            computerclear.SetActive(true);
            computerdefault.SetActive(false);
        }
    }
    IEnumerator CamPan() {
        cam.followtarget = false;
        player.canMove = false; // freeze player
        player.rb.velocity = Vector2.zero;
        AudioManager.instance.PauseMusic();
        AudioManager.instance.StopSFX();
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlaySFX(levelclearsound);
        move = true;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // wait for animation to finish
        levelManager.levelcomplete = true;
        AudioManager.instance.PlaySFX(endvoiceover);
    }
}
