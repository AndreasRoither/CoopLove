﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlatform : MonoBehaviour
{
    public bool endlessTriggerCount = true;
    public int maxTriggerCount = 5;
    public Color triggerColor;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        // reset all platforms if player entity found
        if (player != null)
        {
            // check if player is above platform
            if (player.gameObject.transform.position.y > this.transform.position.y)
            {
                if (endlessTriggerCount)
                {
                    ResetAll();
                }
                else if (maxTriggerCount > 0)
                {
                    ResetAll();
                    maxTriggerCount--;
                }
            }
        }
    }

    private void ResetAll()
    {
        foreach (BasePlatform platform in GameManager.Instance.GetPlatforms())
        {
            platform.ResetPlatform();
        }
    }
}
