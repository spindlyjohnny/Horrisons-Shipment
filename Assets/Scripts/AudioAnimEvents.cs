using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class AudioAnimEvents : MonoBehaviour
{
    // this script is purely for instances where sound has to be played during an event.
    public void PlaySound(AudioClip clip) {
        AudioManager.instance.PlaySFX(clip);
    }
}
