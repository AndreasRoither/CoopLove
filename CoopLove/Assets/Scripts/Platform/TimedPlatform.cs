using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedPlatform : MonoBehaviour
{
    public float TimeToLive = 2f;
    public float BlinkTime = 0.5f;
    public float BlinkOffTime = 0.5f;
    public float DestroyAfterTime = 2f;
    public bool IgnoreCollisionsOnFall = false;
    public bool FreezeZAxis = false;
    public float GravityScale = 0.75f;

    private Collider2D m_Collider2d;
    private MeshRenderer m_MeshRenderer;
    private bool m_IsAlive = true;
    private bool m_TimerStarted = false;

    public void Start()
    {
        m_Collider2d = this.GetComponent<Collider2D>();
        m_MeshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null && !m_TimerStarted)
        {
            m_TimerStarted = true;
            StartCoroutine(Blink());
            StartCoroutine(TimedDestroy());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!m_IsAlive && this.m_Collider2d != null && IgnoreCollisionsOnFall)
        {
            Physics2D.IgnoreCollision(this.m_Collider2d, collision.collider);
        }
    }

    IEnumerator TimedDestroy()
    {
        yield return new WaitForSeconds(TimeToLive);
        m_IsAlive = false;

        // make it fall
        Rigidbody2D r = gameObject.AddComponent<Rigidbody2D>();
        if (FreezeZAxis)
            r.constraints = RigidbodyConstraints2D.FreezeRotation;

        r.gravityScale = GravityScale;

        Destroy(this.gameObject, DestroyAfterTime);
    }

    IEnumerator Blink()
    {
        Color color = new Color();
        if (m_MeshRenderer != null)
            color = m_MeshRenderer.material.color;

        while(m_IsAlive)
        {
            if (m_MeshRenderer != null)
            {
                color.a = 0.5f;
                m_MeshRenderer.material.color = color;
            }
            yield return new WaitForSeconds(BlinkTime);
            if (m_MeshRenderer != null)
            {
                color.a = 1f;
                m_MeshRenderer.material.color = color;
            }
            yield return new WaitForSeconds(BlinkOffTime);
        }

        // make sure platform is transparent
        if (m_MeshRenderer != null)
        {
            color.a = 1f;
            m_MeshRenderer.material.color = color;
        }
    }
}
