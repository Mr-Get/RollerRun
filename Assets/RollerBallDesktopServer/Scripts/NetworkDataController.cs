using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;
using UnityEngine.UI;
using System;

/** 
    Autor: Robert Strohmaier <br />
    Letzte Änderung: 30.03.2017    

    Daten der Client Anwendungen (Smartphones) entgegennehmen und verarbeiten. <br />
    Die Daten werden in Objekten der Klasse SensorDataStorage gespeichtert, die am selben GameObjekt dieses Scripts, automatisch als Components hinzugefügt werden. 
    
    Funktion
    --------
    Netzwerk Server - Stellt Sensor Daten der verbundenen Smartphones zur Verfügung
    
*/

public class NetworkDataController : MonoBehaviour {

    [Header("Debug Feedback")]
    public bool isNetworkDebugEnabled = false;
    public GameObject debugCanvasGameObject; 
    public Text textReceiveMeanBasis;
    public Text textReceiveCnt;
    public Text textReceiveRateMean;
    public Text textDebugNetPerf;

    [Header("Sensor Werte")]
    /// Datenspeicher der Sensor Werte
    public SensorDataStorage[] sensorDataStorage;

    private NetworkManagerServerController networkController;
    private static NetworkDataController instance = null; //Referenz auf dieses Objekt

    //Debugging Netzwerk Performance
    private float timeDataReceivedNow = 0f;
    private float timeDataReceivedLast = 0f;
    private int netPerfCnt = 0;
    private float delayToLastReceiveSum = 0f;
    private float receiveMeanBasis = 2f;
    private float receiveRateMean = 0f;
    private int receivedValuesCnt = 0;




    //Debug der Netzwerk Performance
    private void DebugNetworkPerformanceReceivedDataNow(string cat, string val) {
        if (isNetworkDebugEnabled) {
            timeDataReceivedNow = Time.unscaledTime;
            float delayToLastReceive = timeDataReceivedNow - timeDataReceivedLast;
            textDebugNetPerf.text += "\ncnt:" + netPerfCnt + "Cat:" + cat + ":" + " val:" + val + timeDataReceivedNow + " DelayToLast:" + delayToLastReceive;
            timeDataReceivedLast = timeDataReceivedNow;

            netPerfCnt++;

            delayToLastReceiveSum += delayToLastReceive;
            receivedValuesCnt++;
            if (delayToLastReceiveSum > receiveMeanBasis) {
                receiveRateMean = 1000.0f / (receivedValuesCnt / delayToLastReceiveSum);

                textReceiveMeanBasis.text = "tBasis:" + receiveMeanBasis.ToString();
                textReceiveCnt.text = "cnt:" + receivedValuesCnt.ToString();
                textReceiveRateMean.text = "MeanRate:" + receiveRateMean.ToString() + "ms";

                delayToLastReceiveSum = 0;
                receivedValuesCnt = 0;
            }


            if (netPerfCnt % 60 == 0) {
                textDebugNetPerf.text = "";
                //netPerfCnt = 0;
            }
        }
    }




    
    void Awake() { //Standard Funktion von Unity; wird vor Start ausgeführt
        this.InitController();
    }
    ///NetworkDataController initislisieren. Es wird darauf geachtet, dass dieses Objekt nur einmal vorkommt. DontDestroyOnLoad stellt sicher, dass das GameObject auch nach Szenenwechsel zur Verfügung steht.
    public void InitController() {
        if (instance == null) {
            instance = this;
            //Erstellen der Datenspeicher Objekte und hinzufügen als Components
            this.gameObject.AddComponent<SensorDataStorage>();
            this.gameObject.AddComponent<SensorDataStorage>();
            //Verwaltung der REferenzen auf die beiden Objekte
            sensorDataStorage = new SensorDataStorage[2];
            sensorDataStorage = this.gameObject.GetComponents<SensorDataStorage>();
            //Dieses Objekt darf bei einem Szenenwechsel nicht zerstört werden. 
            DontDestroyOnLoad(instance);
        }
        else if (this != instance) {
            //Sollte InstanciateController() ein weiteres Mal aufgerufen werden, das eben erstellte neue Objekt löschen
            //Dies verhindert mehrfache Erstellung des Objekts
            Destroy(this.gameObject);
        }
    }


    private void Start() {
        
        //Debug Canvas GO ein / ausblenden        
        debugCanvasGameObject.SetActive(isNetworkDebugEnabled);
        
        //Network Manager - Zugrfiff auf Methoden um Daten verschiedener Sensoren anzufordern
        networkController = GameObject.Find("NetworkController").GetComponent<NetworkManagerServerController>();


        if (NetworkServer.active) {
            Debug.Log("Server active: " + NetworkServer.active);

            //Handler der Daten vom Client registrieren
            NetworkServer.RegisterHandler(networkController.msgIdButtonTouch01, ReceiveButtonTouch01);
            NetworkServer.RegisterHandler(networkController.msgIdButtonBluetooth01, ReceiveButtonBluetooth01);
            NetworkServer.RegisterHandler(networkController.msgIdGyroGravity, ReceiveSensorGyroGravity);
            NetworkServer.RegisterHandler(networkController.msgIdGyroAcceleration, ReceiveSensorGyroAcceleration);
            NetworkServer.RegisterHandler(networkController.msgIdGyroRotation, ReceiveSensorGyroRotation);
            NetworkServer.RegisterHandler(networkController.msgIdMicrophone, ReceiveSensorMicrophone);
            NetworkServer.RegisterHandler(networkController.msgIdAccelerator, ReceiveSensorAccelerator);
            NetworkServer.RegisterHandler(networkController.msgIdCompass, ReceiveSensorCompass);

        }
        Debug.Log("#NetworkDataController || Network.player.ipAddress " + Network.player.ipAddress);
    }

    ///Getter für den einfachen Zugriff auf die Daten Objekte jedes Players
    public SensorDataStorage GetSensordataStorageForPlayer(int player=1) {
        return sensorDataStorage[player - 1];
    }


    //Daten der Smartphones empfangen
    //-------------------------------------------------------------------------------
    //Button Touch Screen 01
    private void ReceiveButtonTouch01(NetworkMessage message) {        
        //reading message
        message.reader.SeekZero();
        string text = message.ReadMessage<StringMessage>().value;
        string[] textArray = text.Split(':');
        int playerID = int.Parse(textArray[0]);
        int btnID = int.Parse(textArray[1]);
        bool isPressed = Convert.ToBoolean(int.Parse(textArray[2]));
        int cnt = int.Parse(textArray[3]);
        sensorDataStorage[playerID - 1].isPressedButtonTouchscreen[btnID] = isPressed;
        sensorDataStorage[playerID - 1].buttonCountTouchscreen[btnID] = cnt;

        if (isNetworkDebugEnabled) DebugNetworkPerformanceReceivedDataNow("BtnTouch", isPressed.ToString());
    }
    private void ReceiveButtonBluetooth01(NetworkMessage message) {        
        //reading message
        message.reader.SeekZero();
        string text = message.ReadMessage<StringMessage>().value;
        string[] textArray = text.Split(':');
        int playerID = int.Parse(textArray[0]);
        int btnID = int.Parse(textArray[1]);
        bool isPressed = Convert.ToBoolean(int.Parse(textArray[2]));
        int cnt = int.Parse(textArray[3]);
        sensorDataStorage[playerID - 1].isPressedButtonBluetooth[btnID] = isPressed;
        sensorDataStorage[playerID - 1].buttonCountBluetooth[btnID] = cnt;

        if (isNetworkDebugEnabled) DebugNetworkPerformanceReceivedDataNow("BtnBT", isPressed.ToString());
    }
    //Mikrofon
    private void ReceiveSensorMicrophone(NetworkMessage message) {
        //reading message
        message.reader.SeekZero();
        string text = message.ReadMessage<StringMessage>().value;
        string[] textArray = text.Split(':');
        int playerID = int.Parse( textArray[0]);
        float microphoneLevel = float.Parse(textArray[1]);
        sensorDataStorage[playerID - 1].microphoneLevel = microphoneLevel;
        //Debug.Log("Microphone | Player: "+playerID+" microphoneLevel: " + microphoneLevel);      

        if (isNetworkDebugEnabled) DebugNetworkPerformanceReceivedDataNow("Mic", microphoneLevel.ToString());
    }
    //GyroAcceleration
    private void ReceiveSensorGyroAcceleration(NetworkMessage message) {
        //reading message
        message.reader.SeekZero();
        string text = message.ReadMessage<StringMessage>().value;       
        string[] textArray = text.Split(':');
        int playerID = int.Parse(textArray[0]);
        //Daten in das richtige Objekt des Players speichern
        sensorDataStorage[playerID - 1].gyroAcceleration.x = float.Parse(textArray[1]);
        sensorDataStorage[playerID - 1].gyroAcceleration.y = float.Parse(textArray[2]);
        sensorDataStorage[playerID - 1].gyroAcceleration.z = float.Parse(textArray[3]);

        if (isNetworkDebugEnabled) DebugNetworkPerformanceReceivedDataNow("GyroA", textArray[1]);
    }
    //GyroRotation
    private void ReceiveSensorGyroRotation(NetworkMessage message) {
        //reading message
        message.reader.SeekZero();
        string text = message.ReadMessage<StringMessage>().value;
        string[] textArray = text.Split(':');
        int playerID = int.Parse(textArray[0]);
        //Daten in das richtige Objekt des Players speichern
        sensorDataStorage[playerID - 1].gyroRotation.x = float.Parse(textArray[1]);
        sensorDataStorage[playerID - 1].gyroRotation.y = float.Parse(textArray[2]);
        sensorDataStorage[playerID - 1].gyroRotation.z = float.Parse(textArray[3]);

        if (isNetworkDebugEnabled) DebugNetworkPerformanceReceivedDataNow("GyroR", textArray[1]);
    }
    //GyroGravity
    private void ReceiveSensorGyroGravity(NetworkMessage message) {
        //reading message
        message.reader.SeekZero();
        string text = message.ReadMessage<StringMessage>().value;
        //Debug.Log("Received: "+text);
        string[] textArray = text.Split(':');
        int playerID = int.Parse( textArray[0]);
        //Daten in das richtige Objekt des Players speichern
        sensorDataStorage[playerID - 1].gyroGravity.x = float.Parse(textArray[1]);
        sensorDataStorage[playerID - 1].gyroGravity.y = float.Parse(textArray[2]);
        sensorDataStorage[playerID - 1].gyroGravity.z = float.Parse(textArray[3]);
        //Debug.Log("Player: " + playerID + " sensorGyroGravX: " + tmpStorageForPlayerDebug.gyroGravity.x + " sensorGyroGravY: " + tmpStorageForPlayerDebug.gyroGravity.y + " sensorGyroGravZ: " + tmpStorageForPlayerDebug.gyroGravity.z);

        if (isNetworkDebugEnabled) DebugNetworkPerformanceReceivedDataNow("GyroG", textArray[1]);
    }
    //Accelerometer
    private void ReceiveSensorAccelerator(NetworkMessage message) {
        //reading message
        message.reader.SeekZero();
        string text = message.ReadMessage<StringMessage>().value;
        //Debug.Log("Received: "+text);
        string[] textArray = text.Split(':');
        int playerID = int.Parse(textArray[0]);
        //Daten in das richtige Objekt des Players speichern
        sensorDataStorage[playerID - 1].accelerator.x = float.Parse(textArray[1]);
        sensorDataStorage[playerID - 1].accelerator.y = float.Parse(textArray[2]);
        sensorDataStorage[playerID - 1].accelerator.z = float.Parse(textArray[3]);
        //Debug.Log("Player: " + playerID + " sensorGyroGravX: " + tmpStorageForPlayerDebug.gyroGravity.x + " sensorGyroGravY: " + tmpStorageForPlayerDebug.gyroGravity.y + " sensorGyroGravZ: " + tmpStorageForPlayerDebug.gyroGravity.z);

        if (isNetworkDebugEnabled) DebugNetworkPerformanceReceivedDataNow("Accelerator", textArray[1]);
    }
    //Kompass
    private void ReceiveSensorCompass(NetworkMessage message) {
        //reading message
        message.reader.SeekZero();
        string text = message.ReadMessage<StringMessage>().value;
        //Debug.Log("Received: "+text);
        string[] textArray = text.Split(':');
        int playerID = int.Parse(textArray[0]);
        //Daten in das richtige Objekt des Players speichern
        sensorDataStorage[playerID - 1].compassMagneticHeading = float.Parse(textArray[1]);
        sensorDataStorage[playerID - 1].compassTrueHeading = float.Parse(textArray[2]);
        sensorDataStorage[playerID - 1].compassAccuracy= float.Parse(textArray[3]);
        sensorDataStorage[playerID - 1].compassLastDataGot = double.Parse(textArray[4]);

        if (isNetworkDebugEnabled) DebugNetworkPerformanceReceivedDataNow("Compass", textArray[1]);
    }


    ///Daten an die Smartphones (Clients) senden
    //-------------------------------------------------------------------------------
    //Vibra Motor an den Smartphones auslösen    
    public void SendMessageVibraToPlayer(int playerID = 1) {
        IntegerMessage msg = new IntegerMessage(); //Nachricht an alle Clients erstellen
        msg.value = playerID;
        //Von Server an den Client senden
        NetworkServer.SendToAll(networkController.msgIdVibraMotor, msg);
    }
    /*
    public void SendMessageVibraToAllClients(string str="") {
        StringMessage myMessage = new StringMessage(); //Nachricht an alle Clients erstellen
        myMessage.value = str;
        //Von Server an den Client senden
        NetworkServer.SendToAll(msgIdVibraMotor, myMessage);
    }
    */

   
}