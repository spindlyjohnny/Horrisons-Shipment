using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : SceneLoader
{
    public GameObject pausescreen;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            if (Time.timeScale == 0) { // if paused unpause else pause
                Resume();
            } 
            else {
                Pause();
            }
        }
    }
    public void Pause() {
        pausescreen.SetActive(true); // activate pause screen
        FindObjectOfType<Player>().canMove = false;
        AudioManager.instance.PauseMusic();
        AudioManager.instance.PauseSFX();
        Time.timeScale = 0; // freeze game
    }
    public void Resume() {
        pausescreen.SetActive(false);
        FindObjectOfType<Player>().canMove = true;
        AudioManager.instance.ResumeMusic();
        AudioManager.instance.ResumeSFX();
        Time.timeScale = 1;
    }
    public override void LoadScene(int scene) {
        Time.timeScale = 1; // unfreeze time
        base.LoadScene(scene);
    }
}
