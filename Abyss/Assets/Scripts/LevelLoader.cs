using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private GameObject player;
    private GameObject outputLocation;

    public Scene previousScene;
    
    public string sceneName;
    public string outputLocationName;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        Debug.Log("Found " + player);
        previousScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(previousScene);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Check");
        if (other.transform == player.transform) {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            previousScene = SceneManager.GetActiveScene();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        previousScene = SceneManager.GetActiveScene();
        outputLocation = GameObject.Find(outputLocationName);
        player.transform.position = outputLocation.transform.position;
        Debug.Log("Previous scene was: " + previousScene.name);
        SceneManager.UnloadSceneAsync(previousScene);
    }
}
