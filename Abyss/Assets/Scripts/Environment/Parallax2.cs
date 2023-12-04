using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax2 : MonoBehaviour
{
    public Transform cam;
    public float relativeMove = .3f;

    private void Awake() {
        if (cam == null) {
            cam = GameObject.Find("Virtual Camera").transform;
        }
    }

    void Update()
    {
        if (cam == null) {
            cam = GameObject.Find("Virtual Camera").transform;
        }
        
        transform.position = new Vector2(cam.position.x * relativeMove, cam.position.y * relativeMove);
    }
}
