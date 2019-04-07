using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnZone : MonoBehaviour
{
    public float maxVelocity = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float x = (float)GetRandomNumber(-8d, 8d);
        collision.gameObject.transform.position = new Vector3(x,7,0);

        Rigidbody2D rigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rigidbody2D != null)
        {
            if (Mathf.Abs(rigidbody2D.velocity.x) > maxVelocity || Mathf.Abs(rigidbody2D.velocity.y) > maxVelocity)
                rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, maxVelocity);
        }

        BasePlatform platform = collision.gameObject.GetComponent<BasePlatform>();

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
