using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private int playerAssignment = -1;
    private Color baseColor;
    private Renderer platformRenderer;

    public void Awake()
    {
        this.platformRenderer = this.GetComponent<Renderer>();
        this.baseColor = platformRenderer.material.color;
    }

    public int PlayerAssignment
    {
        get { return playerAssignment; }
        set { playerAssignment = value; }
    }

    public void ResetPlatform()
    {
        platformRenderer.material.color = baseColor;
        playerAssignment = -1;
    }

    public void DestroyPlatform(int time)
    {
        // TODO: implement
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null && playerAssignment == -1)
        {
            Color playerColor = player.playerColor;
            platformRenderer.material.color = playerColor;
            PlayerAssignment = player.playerId;
        }
    }
}
