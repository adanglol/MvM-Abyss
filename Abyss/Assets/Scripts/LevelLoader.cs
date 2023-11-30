using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private GameObject player;

    public int Scene;

    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        Debug.Log("Found " + player);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Check");
        if (other.transform == player.transform) {
            SceneManager.LoadScene(Scene);
        }
    }
}
