using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiGravityButton : MonoBehaviour {
    Animator anim; // button animator
    public GameObject prompt; // input prompt
    public bool pressed;
    public AntiGravityButton otherbutton; // reference other button so that can control its pressed bool
    public PlatformEffector2D[] shelves; // shelves whose platform effectors have to be reversed while antigravity is on
    public AudioClip onsound, offsound;
    private void Start() {
        anim = GetComponent<Animator>();
        prompt.SetActive(false);
        pressed = false;
    }
    private void Update() {
        if (prompt.activeSelf && Input.GetKeyDown(KeyCode.E) /*&& !pressed && gameObject.CompareTag("On Button")*/) { // checks if button hasn't been pressed to turn on anti-gravity 
            prompt.SetActive(false);
            pressed = true;
            otherbutton.pressed = false;
            anim.SetTrigger("Pressed");
            if(gameObject.CompareTag("On Button")) {
                foreach(var i in shelves) {
                    i.useOneWay = false;
                }
                AudioManager.instance.PlaySFX(onsound);
            }
            else if (gameObject.CompareTag("Off Button")) {
                foreach(var i in shelves) {
                    i.useOneWay = true;
                }
                AudioManager.instance.PlaySFX(offsound);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Player>()) {
            prompt.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetComponent<Player>()) {
            prompt.SetActive(false);
        }
    }
}
