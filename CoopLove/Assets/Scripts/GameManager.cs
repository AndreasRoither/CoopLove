
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    private int level = 1;                                  //Current level number, expressed in game as "Day 1".
    private List<Player> players = new List<Player>();

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

    public void AddPlayer(Player player)
    {
        players.Add(player);
    }

    public int GetPlayerCount()
    {
        return players.Count;
    }

    void InitGame()
    {

    }

    public void GoalReached()
    {
        LevelManager.instance.LevelCompleted();
    }

    public void DeadZone()
    {
        Debug.Log("Dead");
        LevelManager.instance.RestartLevel();
    }
}