using System.Collections.Generic;
using UnityEngine;

public class BasePlatform : MonoBehaviour
{
    private int playerAssignment = -1;
    private Color baseColor;
    private Renderer platformRenderer;
    private int baseLayer;
    private Collider2D collider2d;

    public int basePlatformLayer;

    public Player preAssignedPlayer = null;
    public List<BasePlatform> linkedPlatforms = new List<BasePlatform>();

    public void Awake()
    {
        this.collider2d = this.GetComponent<Collider2D>();
        this.platformRenderer = this.GetComponent<Renderer>();
        this.baseColor = platformRenderer.material.color;
        baseLayer = this.gameObject.layer;
    }

    public void Start()
    {
        if (GameManager.Instance != null)
        {
            // setup layer if platform is preassigned
            if (preAssignedPlayer != null)
            {
                playerAssignment = preAssignedPlayer.playerId;
                if (platformRenderer != null)
                    platformRenderer.material.color = preAssignedPlayer.playerColor;

                // TODO: improve logic, get rid of magic numbers
                if (preAssignedPlayer.gameObject.layer == 9)
                    this.gameObject.layer += 1;
                else
                    this.gameObject.layer += 2;
            }

            GameManager.Instance.RegisterPlatform(this);
        }
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (playerAssignment == -1)
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                if (collider2d != null)
                {
                    Physics2D.IgnoreCollision(collision.collider, this.collider2d);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerAssignment == -1)
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                SetAssignment(player);

                ParticleSystem s = ObjectPooler.Instance.SpawnFromPool(
                    "PlatformParticleSystem", this.gameObject.transform.position, Quaternion.identity)
                    .GetComponent<ParticleSystem>();

                ParticleSystem.MainModule settings = s.main;
                settings.startColor = player.playerColor;
                s.transform.Rotate(new Vector3(-180, 0, 0));
                s.Play();

                if (linkedPlatforms != null)
                    linkedPlatforms.ForEach(p => p?.SetAssignment(player));
            }
        }
    }
}