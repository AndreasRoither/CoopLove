using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlatform : MonoBehaviour
{
    public bool endlessTriggerCount = true;
    public int maxTriggerCount = 5;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        // reset all platforms if player entity found
        if (player != null)
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

    private void ResetAll()
    {
        foreach (Platform platform in GameManager.instance.GetPlatforms())
        {
            platform.ResetPlatform();
        }
    }
}
