using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
    // The single instance of the class
    private static DontDestroyObject _instance;

    // Public property to access the instance
    public static DontDestroyObject Instance {
        get {
            // If the instance doesn't exist, find or create it
            if (_instance == null) {
                _instance = FindObjectOfType<DontDestroyObject>();

                // If no instance is found in the scene, create a new one
                if (_instance == null) {
                    GameObject singletonObject = new GameObject(typeof(DontDestroyObject).Name);
                    _instance = singletonObject.AddComponent<DontDestroyObject>();
                }
            }

            return _instance;
        }
    }

    private void Awake() {
        // Ensure there is only one instance in the scene
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private GameObject player;
    private GameObject virtualCam;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;
    }

    private void Update() {
        if (virtualCam == null) {
            virtualCam = GameObject.Find("Virtual Camera");
            virtualCam.GetComponent<CinemachineVirtualCamera>().Follow = GameObject.Find("Player").transform;
        }
    }
}
