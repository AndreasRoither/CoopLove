using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    private bool isRunning = false;
    private byte reliableChanel;
    private int hostId;
    private const string SERVER_IP = "127.0.0.1";

    private const int MAX_USER = 100;
    private int WEB_PORT = 26001;
    private const int PORT = 26000;
    private byte error;

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

        hostId = NetworkTransport.AddHost(topology, 0);

#if UNITY_WEBGL && !UNITY_EDITOR
        
        // web client
        NetworkTransport.Connect(hostId, SERVER_IP, WEB_PORT, 0, out error);  
#else
        // standalone client
        NetworkTransport.Connect(hostId, SERVER_IP, PORT, 0, out error);
#endif

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
