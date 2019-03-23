using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    public List<Player> players = new List<Player>();
    public Color baseColor = Color.black;
    public Color goalColor = Color.magenta;
    public Color onePlayerStandingColor = Color.blue;

    private Renderer goalRenderer;

    public void Awake()
    {
        goalRenderer = this.GetComponent<Renderer>();

        if (goalRenderer != null)
            baseColor = goalRenderer.material.color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            // make sure player is above the goal
            if (player.gameObject.transform.position.y > this.transform.position.y)
            {
                if (!players.Contains(player))
                    players.Add(player);

                if (players.Count == GameManager.instance.GetPlayerCount())
                {
                    SetFinish();
                }
                else
                {
                    if (goalRenderer != null)
                    {
                        goalRenderer.material.color = onePlayerStandingColor;
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // remove player from player list
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            players.Remove(player);

            if (goalRenderer != null && players.Count == 0)
                goalRenderer.material.color = baseColor;
        }
    }

    public void SetFinish()
    {
        // all players are on top of the goal
        if (goalRenderer != null)
        {
            goalRenderer.material.color = goalColor;
            GameManager.instance.GoalReached();
        }
    }
}
