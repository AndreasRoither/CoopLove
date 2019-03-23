using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private int playerAssignment = -1;
    private Color baseColor;
    private Renderer platformRenderer;
    private int baseLayer;

    public void Awake()
    {
        this.platformRenderer = this.GetComponent<Renderer>();
        this.baseColor = platformRenderer.material.color;
        baseLayer = this.gameObject.layer;
    }

    public void ResetPlatform()
    {
        this.gameObject.layer = baseLayer;
        platformRenderer.material.color = baseColor;
        playerAssignment = -1;
    }

    public void DestroyPlatform(int time)
    {
        // TODO: implement
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerAssignment == -1)
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                if (player.gameObject.layer == 9)
                    this.gameObject.layer += 1;
                else
                    this.gameObject.layer += 2;
                Color playerColor = player.playerColor;
                platformRenderer.material.color = playerColor;
                playerAssignment = player.playerId;
            }
        }
    }
}
