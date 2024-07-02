using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelSelect : MonoBehaviour
{
    public Button[] buttons; // level buttons
    // Start is called before the first frame update
    void Start()
    {
        buttons = GetComponentsInChildren<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var i in buttons) {
            if (PlayerPrefs.GetInt("Level " + i.GetComponentInChildren<Text>().text) == 1) { // checks if a button's corresponding level has been unlocked
                i.interactable = true;
            }
            if (PlayerPrefs.GetInt("Level " + i.GetComponentInChildren<Text>().text + " Fact") == 1) { // changes active sprite depending on if collectible for the level has been collected
                i.transform.Find("Fact Collected").gameObject.SetActive(true);
                i.transform.Find("Fact Uncollected").gameObject.SetActive(false);
            }
        }
    }
    public void ResetProgress() {
        PlayerPrefs.DeleteAll();
        foreach(var i in buttons) { // deactivate all buttons and lock collectibles
            i.interactable = false;
            i.transform.Find("Fact Collected").gameObject.SetActive(false);
            i.transform.Find("Fact Uncollected").gameObject.SetActive(true);
        }
    }
}
