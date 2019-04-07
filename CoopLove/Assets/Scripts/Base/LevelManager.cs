using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private int currentScene = 0;

    public void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void RestartLevel()
    {
        
        SceneManager.LoadScene(currentScene);
    }

    public void LevelCompleted()
    {
        SceneManager.LoadScene(currentScene + 1);
        currentScene++;
    }
}
