using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : SceneLoader
{
    public int leveltoload; // next level
    public bool levelcomplete;
    public GameObject completionscreen,completiontext;
    public Collectible fact;
    public Button factbutton; // button that displays tip at end of level
    public GameObject[] facttexts; // text to show if player collected tip
    public AudioClip voiceover; // voice over for the level
    TutorialDiagram diagram; // tutorial diagram
    bool tutorial; // is current level is a tutorial level?
    [SerializeField] string[] factkeys; // player pref keys for tips
    // Start is called before the first frame update
    void Start()
    {
        fact = FindObjectOfType<Collectible>();
        completionscreen.SetActive(false);
        levelcomplete = false;
        Time.timeScale = 1;
        tutorial = SceneManager.GetActiveScene().buildIndex >= 3 && SceneManager.GetActiveScene().buildIndex <= 5;
        if (tutorial) { // tutorial levels
            StartCoroutine(SwitchMusic(AudioManager.instance.levelmusic,true));
        } 
        else {
            StartCoroutine(SwitchMusic(AudioManager.instance.levelmusic)); // switch to level music
        }
        if(!PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + " Played")) {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + " Played", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fact.collected) {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + " Fact", 1);
            factbutton.interactable = true;
        }
        if (levelcomplete) {
            completionscreen.SetActive(true); 
            Time.timeScale = 0; // freeze game to improve performance
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 1); // set the current level to completed
        }
        if(SceneManager.GetActiveScene().buildIndex == 8) { // level 6
            int factcount = 0; // has the player collected all tips?
            for(int i = 0; i < factkeys.Length; i++) {
                factcount += PlayerPrefs.GetInt(factkeys[i]); // tip has been collected for that level, add to total
            }
            if (factcount == 6) { // tip has been collected in all levels, load lore scene
                leveltoload = 9;
            } 
            else { // else load main menu.
                leveltoload = 0;
            }
        }
    }
    public void NextLevel() {
        LoadScene(leveltoload);
    }
    public void RestartLevel() {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu() {
        LoadScene(0);
        Time.timeScale = 1;
    }
    public void ShowFact() {
        factbutton.gameObject.SetActive(false);
        completiontext.SetActive(false);
        foreach (var i in facttexts) {
            i.SetActive(true);
        }
    }
    IEnumerator SwitchMusic(AudioClip music,bool tutorial = false) {
        AudioManager.instance.PauseMusic();
        if (!PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + " Played")) { // ensures voiceover is only played once
            if (tutorial) { // tutorial levels
                diagram = FindObjectOfType<TutorialDiagram>(true);
                AudioManager.instance.PlaySFX(voiceover);
                diagram.gameObject.SetActive(true);
                yield return new WaitForSeconds(voiceover.length);
                AudioManager.instance.PlayMusic(music);
            } 
            else if (voiceover != null) { // non-tutorial, contains voice over
                AudioManager.instance.PlaySFX(voiceover);
                yield return new WaitForSeconds(voiceover.length);
                AudioManager.instance.PlayMusic(music);
            }
        }
        else { // level doesn't have voice over or voiceover has already played
            yield return new WaitForEndOfFrame();
            AudioManager.instance.PlayMusic(music);
        }
    }
}
