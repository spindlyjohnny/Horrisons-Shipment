using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoreScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForVideo());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator WaitForVideo() {
        yield return new WaitForSeconds(12f);
        Application.Quit();
    }
}
