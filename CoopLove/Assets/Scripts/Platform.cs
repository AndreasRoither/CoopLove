﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private int playerAssignment = -1;
    private Color baseColor;
    private Renderer platformRenderer;
    private int baseLayer;
    public Player preAssignedPlayer = null;
    public Platform linkedPlatform = null;

    public void Awake()
    {
        this.platformRenderer = this.GetComponent<Renderer>();
        this.baseColor = platformRenderer.material.color;
        baseLayer = this.gameObject.layer;

        if (preAssignedPlayer != null)
        {
            playerAssignment = preAssignedPlayer.playerId;
            if (platformRenderer != null)
                platformRenderer.material.color = preAssignedPlayer.playerColor;

            if (preAssignedPlayer.gameObject.layer == 9)
                this.gameObject.layer += 1;
            else
                this.gameObject.layer += 2;
        }
    }

    public void Start()
    {
        GameManager.instance.RegisterPlatform(this);
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

    public void SetAssignment(Player player)
    {
        if (player.gameObject.layer == 9)
            this.gameObject.layer += 1;
        else
            this.gameObject.layer += 2;

        if (platformRenderer != null)
            platformRenderer.material.color = player.playerColor;
        playerAssignment = player.playerId;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerAssignment == -1)
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                SetAssignment(player);

                if (linkedPlatform != null)
                    linkedPlatform.SetAssignment(player);
            }
        }
    }
}
