
using UnityEngine;
using UnityEngine.Networking;

using System.Collections.Generic;

using System.Text;

/*
This class will act as a server or a client
depends of init parameter
*/
public class netSynchManager : MonoBehaviour
{
    /*
    Common attributes for server and client
    */
    // connexion parameters
    public string _ipServer = "192.168.95.10";
    public int _port = 8888;

    // isRoot = true on server, false else
    bool _isRoot = false;

    // true if init and started
    bool _isStarted = false;
    
    // The network id
    int _hostId = 0;

    // Channels for communication available
    int _reliableChannel;
    int _unreliableChannel;

    const int MAX_CONNECTION = 20;

    public List<networkAgent> _netAgent = new List<networkAgent>();

    int _mpiId = 0;

    /*
    server attributes
    */
    List<int> _connectedClients = new List<int>();

    List<string> _messagesTosend = new List<string>();

    /*
    client attributes
    */
    bool _isConnected = false;

    int _serverId = 0;

    /*
     * Monos
     */

    void Awake()
    {
        //NetworkServer.Reset();
    }

    void Update()
    {
        if (_isStarted)
        {
            if (_isRoot)
            {
                updateServer();
            }
            else
            {
                updateClient();
            }
        }
    }

    void OnApplicationquit()
    {
        NetworkTransport.Shutdown();
    }

    /*
     * Interfaces
     */

    public void init(bool server, int mpiId)
    {
        _isRoot = server;
        _mpiId = mpiId;

        networkInit();

        if (_isRoot)
        {
            initServer();
        }
        else
        {
            initClient();
        }

        _isStarted = true;
    }

    public void addMessage(string mes)
    {
        _messagesTosend.Add(mes);
    }

    public void findNetAgent()
    {
        _netAgent.Clear();
        networkAgent[] agents = GameObject.FindObjectsOfType<networkAgent>();

        for (int i = 0; i < agents.Length; i++)
        {
            agents[i]._idNetwork = i;
            _netAgent.Add(agents[i]);
        }

        Debug.Log("Found and init " + _netAgent.Count + " agents" );
    }

    /*
     * Internals
     */
    void networkInit()
    {
        NetworkTransport.Init();
    }

    void initServer()
    {
        ConnectionConfig config = new ConnectionConfig();

        _reliableChannel = config.AddChannel(QosType.Reliable);
        _unreliableChannel = config.AddChannel(QosType.Unreliable);

        HostTopology topology = new HostTopology(config, MAX_CONNECTION);

        _hostId = NetworkTransport.AddHost(topology, _port);

        Debug.Log("Server id = " + _hostId);
    }

    void initClient()
    {
        ConnectionConfig config = new ConnectionConfig();

        _reliableChannel = config.AddChannel(QosType.Reliable);
        _unreliableChannel = config.AddChannel(QosType.Unreliable);

        HostTopology topology = new HostTopology(config, 4);

        int portClient = _port + ( _mpiId * 2 );
        _hostId = NetworkTransport.AddHost(topology, 0);

        Debug.Log("Client id = " + _hostId);
    }

    void updateServer()
    {
        // Wait for connection event
        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType networkEvent = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

        switch (networkEvent)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.ConnectEvent:
                OnConnection(connectionId);
                break;
            case NetworkEventType.DisconnectEvent:
                OnDisconnection(connectionId);
                break;
        }

        for (int i = 0; i < _netAgent.Count; i++)
        {
            string mes = _netAgent[i].getMessage();
            if (mes != "")
            {
                _messagesTosend.Add(mes);
            }
        }

        // Send messages if needed
        if (_messagesTosend.Count > 0)
        {
            //Debug.Log("Client send a message");

            string globalMessage = _messagesTosend[0];
            for (int i = 1; i < _messagesTosend.Count; i++)
            {
                globalMessage += "|" + _messagesTosend[i];
            }

            byte[] msg = Encoding.Unicode.GetBytes(globalMessage);
            for (int i = 0; i < _connectedClients.Count; i++)
            {
                NetworkTransport.Send(_hostId, _connectedClients[i], _unreliableChannel, msg, globalMessage.Length * sizeof(char), out error);
            }

            _messagesTosend.Clear();
        }
    }

    void updateClient()
    {
        if (!_isConnected)
        {
            byte error;
            _serverId = NetworkTransport.Connect(_hostId, _ipServer, _port, 0, out error);

            NetworkError netError = (NetworkError)error;
            if (netError == NetworkError.Ok)
            {
                _isConnected = true;
                Debug.Log("Client connected with success");
            }
            else
            {
                Debug.LogError(netError.ToString());
            }
        }
        else
        {
            // Wait for messages
            int recHostId;
            int connectionId;
            int channelId;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            byte error;
            NetworkEventType networkEvent = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

            if (networkEvent == NetworkEventType.DataEvent)
            {
                // We receive a message from server, parse it
                string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                Debug.LogError("Receive message : " + msg);
                string[] messages = msg.Split('|');
                for (int i = 0; i < messages.Length; i++)
                {
                    // Read messages
                    for (int j = 0; j < _netAgent.Count; j++)
                    {
                        _netAgent[j].readMessage(messages[i]);
                    }
                }
            }
        }
    }

    void OnConnection(int cnnId)
    {
        _connectedClients.Add(cnnId);
        //Debug.Log("Connect with " + cnnId);
    }

    void OnDisconnection(int cnnId)
    {
        // Remove this player from our client list
        _connectedClients.Remove( cnnId);
        //Debug.Log("Disconnect with " + cnnId);
    }
}
