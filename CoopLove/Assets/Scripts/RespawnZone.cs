using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnZone : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        float x = (float)GetRandomNumber(-8d, 8d);
        collision.gameObject.transform.position = new Vector3(x,7,0);

        Platform platform = collision.gameObject.GetComponent<Platform>();

        if (platform != null)
        {
            platform.ResetPlatform();
        }
    }

    public double GetRandomNumber(double minimum, double maximum)
    {
        System.Random random = new System.Random();
        return random.NextDouble() * (maximum - minimum) + minimum;
    }
}
