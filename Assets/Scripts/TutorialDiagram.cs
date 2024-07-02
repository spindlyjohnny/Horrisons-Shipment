using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TutorialDiagram : MonoBehaviour
{
    public Image[] tutimgs; // diagram images
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tutimgs[SceneManager.GetActiveScene().buildIndex].gameObject.SetActive(true);
    }
    public void Close() {
        gameObject.SetActive(false);
    }
}
