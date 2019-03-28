using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Color playerColor;
    public int playerId;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RegisterPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
