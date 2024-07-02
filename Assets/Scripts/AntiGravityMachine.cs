using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiGravityMachine : MonoBehaviour
{
    public bool controlled; // determines if anti-gravity is automatic or controlled by player
    public AntiGravityButton onbutton,offbutton; // buttons that control anti-gravity
    AreaEffector2D areaEffector;
    public bool on; // is the machine on?
    // Start is called before the first frame update
    void Start()
    {
        areaEffector = GetComponent<AreaEffector2D>();
    }
    
    // Update is called once per frame
    void Update()
    {
        areaEffector.enabled = on;
        if (controlled && onbutton != null && offbutton != null) {
            //areaEffector.forceMagnitude = button.pressed ? defaultforce : 0;
            if (onbutton.pressed) {
                on = true;
            } 
            else if (offbutton.pressed) { 
                on = false;
            }
        } 
        else { // anti-gravity is always on if machine is not controlled by buttons
            on = true;
        }
    }
}
