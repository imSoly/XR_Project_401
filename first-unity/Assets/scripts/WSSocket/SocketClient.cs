using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp; // C#���� �������� �����ϴ� ���̺귯��
using Newtonsoft.Json; // JSON�� ����ϱ� ���� ���̺귯��
using System.Text;

public class MyData
{
    public string clientID; // �������� �����ؼ� Ŭ���̾�Ʈ�� ���� �� ��
    public string message;
    public int requestType; // ��û ��ȣ JSON���� ����
}
public class SocketClient : MonoBehaviour
{
    private WebSocket webSocket;
    private bool isConnected = false;
    // ���� �õ� Ƚ��
    private int connectionAttempt = 0; 
    // �ִ� ���� �õ� Ƚ��
    private const int maxConnectionAttempts = 3; 

    MyData sendData = new MyData { message = "�޼��� ����" };

    // Start is called before the first frame update
    void Start()
    {
        ConnectWebSocket();
    }

    void ConnectWebSocket()
    {
        // localhost 127.0.0.1 port:8000, ws => websocket
        webSocket = new WebSocket("ws://localhost:8000");

        // ������ ���� �̺�Ʈ�� �߻����Ѽ� �Լ��� �����Ų��.
        webSocket.OnOpen += OnWebSocketOpen;
        webSocket.OnMessage += OnWebSocketMessage;
        webSocket.OnClose += OnWebSocketClose;

        webSocket.ConnectAsync();
    }

    // �������� ���µǰ� ����Ǿ��� ��
    void OnWebSocketOpen(object sender, System.EventArgs e)
    {
        Debug.Log("WebSocket Connected.");
        isConnected = true;
        connectionAttempt = 0;
    }

    // �������� ����� �� Message�� ���� ��
    void OnWebSocketMessage(object sender, MessageEventArgs e)
    {
        // MessageEventArgs�� ���� RowData�� JSON���� ���ڵ�
        string jsonData = Encoding.Default.GetString(e.RawData);
        Debug.Log("Received JSON Data: " + jsonData);

        // JSON �����͸� ��ü�� ������ȭ
        MyData receivedData = JsonConvert.DeserializeObject<MyData>(jsonData);

        // receivedData ���� ������� ���� ��
        if (receivedData != null && !string.IsNullOrEmpty(receivedData.clientID))
        {
            // �������� �޾ƿ� ID ���� MyData�� �ִ´�.
            sendData.clientID = receivedData.clientID;
        }
    }

    // ������ ������ ������ ��
    void OnWebSocketClose(object sender, CloseEventArgs e)
    {
        Debug.Log("WebSocket Connection Closed.");
        // ���� ���� flag
        isConnected = false;

        // �� 3���� �õ�
        if(connectionAttempt < maxConnectionAttempts)
        {
            connectionAttempt++;
            Debug.Log("Attempting to reconnect. Attemp: " + connectionAttempt);
            // Ŀ�ؼ� �õ�
            ConnectWebSocket();
        }
        else
        {
            Debug.Log("Failed to connect");
        }
        
    }

    // ���α׷� ���� �ÿ� ȣ��Ǵ� �Լ�
    void OnApplicationQuit()
    {
        DisConnectedSockedt();
    }

    // ����� socket�� Release ���ش�.
    void DisConnectedSockedt()
    {
        if(webSocket != null && isConnected)
        {
            webSocket.Close();
            isConnected = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(webSocket == null || !isConnected)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            sendData.requestType = 0;
            // MyData�� JSONȭ
            string jsonData = JsonConvert.SerializeObject(sendData);

            webSocket.Send(jsonData);
        }
    }
}
