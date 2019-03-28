
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    private int level = 1;                                  //Current level number, expressed in game as "Day 1".
    private List<Player> players = new List<Player>();
    private List<Platform> platforms = new List<Platform>();

    public void Awake()
    {
        //Check if instance already exists
        if (Instance == null)
            Instance = this;

        //If instance already exists and it's not this:
        else if (Instance != this)
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterPlayer(Player player)
    {
        players.Add(player);
    }

    public void RegisterPlatform(Platform platform)
    {
        platforms.Add(platform);
    }

    public List<Platform> GetPlatforms()
    {
        return platforms;
    }

    public int GetPlayerCount()
    {
        return players.Count;
    }

    public void GoalReached()
    {
        players.Clear();
        platforms.Clear();
        level += 1;
        LevelManager.instance.LevelCompleted();
    }

    public void DeadZone()
    {
        players.Clear();
        platforms.Clear();
        LevelManager.instance.RestartLevel();
    }
}