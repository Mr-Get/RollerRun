using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

/** 
    Autor: Robert Strohmaier <br />
    Letzte Änderung: 30.03.2017
   
    Diese Klasse passt den bereits in Unity vorhandenen NetworkManager an.

    Funktion
    --------
    Erstellen eines angepassten Netzwerk Managers.
    
*/


public class NetworkManagerServerController : NetworkManager {

    [Header("Auswahl benötigter Sensor Werte")]
    public bool isGyroGravityRequested = true;
    public bool isGyroRotationRequested = true;
    public bool isGyroAccelerationRequested = true;
    public bool isMicrophoneRequested = true;
    public bool isAcceleratorRequested = true;
    public bool isCompassRequested = true;
    [Header("Debugging Feedback")]
    public Canvas canvasDebug;
    public Text textDebug;
    [Header("NICHT ÄNDERN - Message IDs")]
    //Message IDs - werden hier und im NetworkDataController benötigt für Verbindung zu Clients
    //Client kann DAten entgegen nehmen.  Clients --> Server       
    public short msgIdClientReady = 101; //Touchscreen
    //Message IDs | Buttons | für Komunikation mit Clients --> Server       
    public short msgIdButtonTouch01 = 111; //Touchscreen
    public short msgIdButtonBluetooth01 = 121; //Bluetooth Button

    //Gyro
    public short msgIDGyroControl = 130; //Server --> Client gibt an, welche Gyro Werte geschickt werden sollen
    public short msgIdGyroGravity = 131; //Client --> Server Datenübermittlung Sensor Daten
    public short msgIdGyroRotation = 132; //Client --> Server Datenübermittlung Sensor Daten
    public short msgIdGyroAcceleration = 133; //Client --> Server Datenübermittlung Sensor Daten
    //Accelerometer
    public short msgIdAcceleratorControl = 140; //Server --> Client gibt an, welche Accelerator Werte geschickt werden sollen
    public short msgIdAccelerator = 141; //Client --> Server Datenübermittlung Sensor Daten
    //Kompass
    public short msgIdCompassControl = 150; //Server --> Client gibt an, ob Compass Werte geschickt werden sollen
    public short msgIdCompass = 151; //Client --> Server Datenübermittlung Sensor Daten
    //Mic
    public short msgIDMicControl = 190; //Server --> Client gibt an, welche Sensor Werte geschickt werden sollen
    public short msgIdMicrophone = 191; //Client --> Server Datenübermittlung Sensor Daten

    //Vibra an trigger
    public short msgIdVibraMotor = 991; //Server --> Client == Steuerdaten z.B. Vibra

    private NetworkDataController networkDataController;

    // Start Server
    void Start() {

        /*
        Network.sendRate = 100;

        
        //ConnectionConfig
        ConnectionConfig myConfig = new ConnectionConfig();
        myConfig.AddChannel(QosType.ReliableSequenced);
        myConfig.AddChannel(QosType.Unreliable);
        myConfig.PacketSize = 1440;
        myConfig.FragmentSize = 900;
        myConfig.ResendTimeout = 1000;         //1 sec
        myConfig.DisconnectTimeout = 5000;          //5 seconds
        myConfig.ConnectTimeout = 1000;
        myConfig.MaxConnectionAttempt = 5;
        myConfig.PingTimeout = 500;
        myConfig.ReducedPingTimeout = 250;
        myConfig.AllCostTimeout = 10; // 10 ms
       // myConfig.AcksType = ConnectionAcksType.Acks32;
        NetworkServer.Configure(myConfig,4);
        */

        //Eigenschaften des Servers einstellen
        this.networkPort = 7777;
        this.autoCreatePlayer = false;
        //Server starten
        this.StartUpHost();
        //Handler der Handshake Daten vom Client registrieren
        NetworkServer.RegisterHandler(msgIdClientReady, ReceiveClientReady);
    }

    //Hier wird der Handshake vom Client entgegen genommen. 
    //Ab nun ist klar, dass dem Client mitgeteilt werden kann, welche Sensor Daten er senden soll
    private void ReceiveClientReady(NetworkMessage message) {
        message.reader.SeekZero();
        string text = message.ReadMessage<StringMessage>().value;
        Debug.Log("ReceiveClientReady");
        //Den Clients mitteilen, welche Sensorwerte sie senden sollen
        this.RequestGyroSensorValues();
        this.RequestMicrophoneValues();
        this.RequestAcceleratorSensorValues();
        this.RequestCompassSensorValues();
    }

    //Debugging Canvas mit ? einblenden
    void Update() {
        if (Input.anyKeyDown) {
            if (!string.IsNullOrEmpty(Input.inputString)) {
                //Empfangen wird in einem String Array, das abgearbeitet wird um auch gleichzeitig gedrückte Keys zu erhalten
                for (int c = 0; c < Input.inputString.Length; c++) {
                    char tmp = Input.inputString[c];
                    int asciiCode = (int)tmp;
                    //Debug Canvas mit ? einblenden
                    if (asciiCode == 63) {
                        //Debug.Log("? Pressed");                            
                        canvasDebug.enabled = !canvasDebug.enabled;
                    }
                }
            }
        }

    }


    //Server starten
    //---------------------------------------------
    public void StartUpHost() {
        NetworkManager.singleton.StartHost();
        Debug.LogWarning("StartUpHost IP:" + this.networkAddress);
    }

    //Senden welche Sensor Daten benötigt werden
    //---------------------------------------------
    public void RequestGyroSensorValues() { //Wenn mit den default Werten wie im Inspector eingestellt gearbeitet werden soll
        StringMessage msg = new StringMessage(); //Nachricht an alle Clients erstellen
        msg.value = isGyroGravityRequested.ToString() + ":" + isGyroRotationRequested.ToString() + ":" + isGyroAccelerationRequested.ToString();
        //Von Server an den Client senden
        NetworkServer.SendToAll(msgIDGyroControl, msg);
    }
    public void RequestGyroSensorValues(bool useGyroGravity = false, bool useGyroRotation = false, bool useGyroAcceleration = false) { //Wenn NICHT mit den default Werten wie im Inspector eingestellt gearbeitet werden soll
        isGyroGravityRequested = useGyroGravity;
        isGyroRotationRequested = useGyroRotation;
        isGyroAccelerationRequested = useGyroAcceleration;

        StringMessage msg = new StringMessage(); //Nachricht an alle Clients erstellen
        msg.value = isGyroGravityRequested.ToString() + ":" + isGyroRotationRequested.ToString() + ":" + isGyroAccelerationRequested.ToString();
        //Von Server an den Client senden
        NetworkServer.SendToAll(msgIDGyroControl, msg);
    }
    public void RequestAcceleratorSensorValues() { //Wenn mit den default Werten wie im Inspector eingestellt gearbeitet werden soll
        StringMessage msg = new StringMessage(); //Nachricht an alle Clients erstellen
        msg.value = isAcceleratorRequested.ToString();
        //Von Server an den Client senden
        NetworkServer.SendToAll(msgIdAcceleratorControl, msg);
    }
    public void RequestAcceleratorSensorValues(bool useAccelerometer = false) { //Wenn mit den default Werten wie im Inspector eingestellt gearbeitet werden soll
        isAcceleratorRequested = useAccelerometer;
        StringMessage msg = new StringMessage(); //Nachricht an alle Clients erstellen
        msg.value = isAcceleratorRequested.ToString();
        //Von Server an den Client senden
        NetworkServer.SendToAll(msgIdAcceleratorControl, msg);
    }
    public void RequestCompassSensorValues() { //Wenn mit den default Werten wie im Inspector eingestellt gearbeitet werden soll
        StringMessage msg = new StringMessage(); //Nachricht an alle Clients erstellen
        msg.value = isCompassRequested.ToString();
        //Von Server an den Client senden
        NetworkServer.SendToAll(msgIdCompassControl, msg);
    }
    public void RequestCompassSensorValues(bool useCompass = false) { //Wenn mit den default Werten wie im Inspector eingestellt gearbeitet werden soll
        isCompassRequested = useCompass;
        StringMessage msg = new StringMessage(); //Nachricht an alle Clients erstellen
        msg.value = isCompassRequested.ToString();
        //Von Server an den Client senden
        NetworkServer.SendToAll(msgIdCompassControl, msg);
    }
    public void RequestMicrophoneValues() {
        StringMessage msg = new StringMessage(); //Nachricht an alle Clients erstellen
        msg.value = isMicrophoneRequested.ToString();
        //Von Server an den Client senden
        NetworkServer.SendToAll(msgIDMicControl, msg);
    }
    public void RequestMicrophoneValues(bool useMicrophone = false) {
        isMicrophoneRequested = useMicrophone;
        StringMessage msg = new StringMessage(); //Nachricht an alle Clients erstellen        
        msg.value = isMicrophoneRequested.ToString();
        //Von Server an den Client senden
        NetworkServer.SendToAll(msgIDMicControl, msg);
    }




    //Server Handler
    //--------------------------------------
    //Wird am Server aufgefurfen, wenn sich ein Client (Smartphone) verbindet
    public override void OnStartHost() {
        Debug.LogWarning("OnStartHost ");
        textDebug.text = "OnStartHost on IP: " + this.networkAddress + " Port: " + this.networkPort + "\n" + textDebug.text;
    }
    public override void OnStopHost() {
        Debug.LogWarning("OnStopHost ");
        textDebug.text = "OnStopHost " + "\n" + textDebug.text;
    }
    public override void OnStartServer() {
        Debug.LogWarning("OnStartServer ");
        textDebug.text = "OnStartServer on IP: " + this.networkAddress + " Port: " + this.networkPort + "\n" + textDebug.text;
    }
    public override void OnStopServer() {
        Debug.LogWarning("OnStopServer ");
        textDebug.text = "OnStopServer " + "\n" + textDebug.text;
    }
    public override void OnServerConnect(NetworkConnection conn) {
        Debug.LogWarning("OnServerConnect" + conn.connectionId);
        textDebug.text = "OnServerConnect connID:" + conn.connectionId + " from IP: " + conn.address + "\n" + textDebug.text;
    }
    public override void OnServerDisconnect(NetworkConnection conn) {
        Debug.LogWarning("OnServerDisconnect " + conn.connectionId);
        textDebug.text = "OnServerDisconnect connID:" + conn.connectionId + " from IP: " + conn.address + "\n" + textDebug.text;
    }
    public override void OnServerReady(NetworkConnection conn) {
        Debug.LogWarning("OnServerReady " + conn.connectionId);
        textDebug.text = "OnServerReady " + conn.connectionId + "\n" + textDebug.text;
    }
    public override void OnServerSceneChanged(string sceneName) {
        Debug.LogWarning("OnServerSceneChanged " + sceneName);
        textDebug.text = "OnServerSceneChanged " + sceneName + "\n" + textDebug.text;
    }
    //Error Handling
    public override void OnServerError(NetworkConnection conn, int errorCode) {
        Debug.LogWarning("OnServerError " + conn.connectionId + " errorCode:" + errorCode + " Error:" + (NetworkConnectionError)errorCode);
        textDebug.text = "OnServerError " + conn.connectionId + " errorCode:" + errorCode + " Error:" + (NetworkConnectionError)errorCode + "\n" + textDebug.text;
    }


    //Client Handler 
    //--------------------------------------
    //Wird aufgerufen wenn sich Smartphone Client zu Server verbunden hat
    public override void OnStartClient(NetworkClient client) {
        Debug.LogWarning("OnStartClient serverIP:" + client.serverIp + " Server Port: " + client.serverPort);
        textDebug.text = "OnStartClient serverIP:" + client.serverIp + " Server Port: " + client.serverPort + "\n" + textDebug.text;
    }
    public override void OnStopClient() {
        Debug.LogWarning("OnStopClient ");
        textDebug.text = "OnStopClient " + "\n" + textDebug.text;
    }
    public override void OnClientConnect(NetworkConnection conn) {
        Debug.LogWarning("OnClientConnect" + conn.connectionId + " IP:" + conn.address);
        textDebug.text = "OnClientConnect connectionId:" + conn.connectionId + " IP:" + conn.address + "\n" + textDebug.text;
    }
    public override void OnClientDisconnect(NetworkConnection conn) {
        Debug.LogWarning("OnClientDisconnect " + conn.connectionId);
        textDebug.text = "OnClientDisconnect " + conn.connectionId + "\n" + textDebug.text;
    }
    public override void OnClientNotReady(NetworkConnection conn) {
        Debug.LogWarning("OnClientNotReady " + conn.connectionId);
        textDebug.text = "OnClientNotReady " + conn.connectionId + "\n" + textDebug.text;
    }
    public override void OnClientSceneChanged(NetworkConnection conn) {
        Debug.LogWarning("OnClientSceneChanged " + conn.connectionId);
        textDebug.text = "OnClientSceneChanged " + conn.connectionId + "\n" + textDebug.text;
    }

    //Error Handling    
    public override void OnClientError(NetworkConnection conn, int errorCode) {
        Debug.LogWarning("OnClientError " + conn.connectionId + " errorCode:" + errorCode);
        textDebug.text = "OnClientError " + conn.connectionId + " errorCode:" + errorCode + "\n" + textDebug.text;
    }
}