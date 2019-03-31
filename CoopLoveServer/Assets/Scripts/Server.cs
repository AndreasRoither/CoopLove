using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    private bool isRunning = false;
    private byte reliableChanel;
    private int hostId;
    private int webHostId;

    private const int MAX_USER = 100;
    private int WEB_PORT = 26001;
    private const int PORT = 26000;


    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }

    /// <summary>
    /// Initialize the Server
    /// Set Channels and topology
    /// Add Hosts, WebSocket
    /// </summary>
    private void Init()
    {
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        reliableChanel = cc.AddChannel(QosType.Reliable);

        HostTopology topology = new HostTopology(cc, MAX_USER);

        hostId = NetworkTransport.AddHost(topology, PORT, null);
        webHostId = NetworkTransport.AddWebsocketHost(topology, WEB_PORT, null);

        Debug.Log($"> Started Server\nOpened Connection on port {PORT} and webport {WEB_PORT}");
        isRunning = true;
    }

    /// <summary>
    /// Shutdown server
    /// </summary>
    private void Shutdown()
    {
        isRunning = false;
        NetworkTransport.Shutdown();
    }
}
