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

    private void OnCollisionExit2D(Collision2D collision)
    {
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
        if (goalRenderer != null)
        {
            goalRenderer.material.color = goalColor;
            GameManager.instance.GoalReached();
        }
    }
}
