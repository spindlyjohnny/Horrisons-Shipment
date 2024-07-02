using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] AudioSource audio, sfxaudio;
    public AudioClip staticnoise;
    public AudioClip levelmusic;
    public AudioClip intromessage;
    // Start is called before the first frame update
    private void Awake() {
        if(instance == null) { // create singleton.
            instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else {
            Destroy(gameObject);
        }
    }
    public void StopMusic() {
        audio.Stop();
    }
    public void StopSFX() {
        sfxaudio.Stop();
    }
    public void PlayMusic(AudioClip music) {
        if (audio.isPlaying) return;
        audio.clip = music;
        audio.Play();
    }
    public void PlaySFX(AudioClip clip) {
        sfxaudio.PlayOneShot(clip);
    }
    public void ResumeMusic() {
        audio.UnPause();
    }
    public void PauseMusic() {
        audio.Pause();
    }
    public void PauseSFX() {
        sfxaudio.Pause();
    }
    public void ResumeSFX() {
        sfxaudio.UnPause();
    }
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex <= 2) { 
            PlayMusic(staticnoise);// plays static noise in menu screens
        }
        if(SceneManager.GetActiveScene().buildIndex == 0 && !PlayerPrefs.HasKey("Played")) { // play intro message on player's first ever playthrough
            PlaySFX(intromessage);
            PlayerPrefs.SetInt("Played", 1);
        }
    }
    
}
