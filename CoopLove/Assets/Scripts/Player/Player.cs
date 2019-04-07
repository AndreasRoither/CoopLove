using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Color playerColor;
    public int playerId;

    void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RegisterPlayer(this);
    }
}
