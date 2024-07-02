using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target; // target to follow
    public float smoothing = 1f; // speed of camera movement
    public bool followtarget;
    // Start is called before the first frame update
    void Start() {
        followtarget = true;
    }

    // Update is called once per frame
    void Update() {
        if (followtarget) {
            Vector3 targetposition = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetposition, Time.deltaTime * smoothing);
        }
    }
}
