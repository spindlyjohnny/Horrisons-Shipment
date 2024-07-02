using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    public GameObject feedbacktext; // text indicating collectible has been collected
    public Text fact;
    public string facttext;
    public GameObject prompt; // input prompt
    public bool collected;
    public AudioClip collectedsound;
    // Start is called before the first frame update
    void Start()
    {
        feedbacktext.SetActive(false);
        fact.text = facttext;
        prompt.SetActive(false);
        collected = false;
        GetComponentInChildren<Canvas>().worldCamera = FindObjectOfType<Camera>(); // set event camera of world space canvas
    }

    // Update is called once per frame
    void Update()
    {
        if (prompt.activeSelf && Input.GetKeyDown(KeyCode.E)) {
            feedbacktext.SetActive(true);
            prompt.SetActive(false);
            collected = true;
            AudioManager.instance.PlaySFX(collectedsound);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Player>() && !collected) prompt.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetComponent<Player>() && feedbacktext.activeSelf) {
            feedbacktext.SetActive(false);
        }
        else if (collision.GetComponent<Player>()) {
            prompt.SetActive(false);
        }
    }
}
