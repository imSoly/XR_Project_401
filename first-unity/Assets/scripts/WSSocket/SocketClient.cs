using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp; // C#에서 웹소켓을 지원하는 라이브러리
using Newtonsoft.Json; // JSON을 사용하기 위한 라이브러리
using System.Text;

public class MyData
{
    public string clientID; // 서버에서 제작해서 클라이언트에 접속 시 줌
    public string message;
    public int requestType; // 요청 번호 JSON으로 보냄
}
public class SocketClient : MonoBehaviour
{
    private WebSocket webSocket;
    private bool isConnected = false;
    // 연결 시도 횟수
    private int connectionAttempt = 0; 
    // 최대 연결 시도 횟수
    private const int maxConnectionAttempts = 3; 

    MyData sendData = new MyData { message = "메세지 전송" };

    // Start is called before the first frame update
    void Start()
    {
        ConnectWebSocket();
    }

    void ConnectWebSocket()
    {
        // localhost 127.0.0.1 port:8000, ws => websocket
        webSocket = new WebSocket("ws://localhost:8000");

        // 웹소켓 관련 이벤트를 발생시켜서 함수를 실행시킨다.
        webSocket.OnOpen += OnWebSocketOpen;
        webSocket.OnMessage += OnWebSocketMessage;
        webSocket.OnClose += OnWebSocketClose;

        webSocket.ConnectAsync();
    }

    // 웹소켓이 오픈되고 연결되었을 때
    void OnWebSocketOpen(object sender, System.EventArgs e)
    {
        Debug.Log("WebSocket Connected.");
        isConnected = true;
        connectionAttempt = 0;
    }

    // 웹소켓이 연결된 후 Message가 왔을 때
    void OnWebSocketMessage(object sender, MessageEventArgs e)
    {
        // MessageEventArgs에 들어온 RowData를 JSON으로 인코딩
        string jsonData = Encoding.Default.GetString(e.RawData);
        Debug.Log("Received JSON Data: " + jsonData);

        // JSON 데이터를 객체로 역직렬화
        MyData receivedData = JsonConvert.DeserializeObject<MyData>(jsonData);

        // receivedData 값이 비어있지 않을 때
        if (receivedData != null && !string.IsNullOrEmpty(receivedData.clientID))
        {
            // 서버에서 받아온 ID 값을 MyData에 넣는다.
            sendData.clientID = receivedData.clientID;
        }
    }

    // 웹소켓 연결이 끊겼을 때
    void OnWebSocketClose(object sender, CloseEventArgs e)
    {
        Debug.Log("WebSocket Connection Closed.");
        // 연결 끊김 flag
        isConnected = false;

        // 총 3번의 시도
        if(connectionAttempt < maxConnectionAttempts)
        {
            connectionAttempt++;
            Debug.Log("Attempting to reconnect. Attemp: " + connectionAttempt);
            // 커넥션 시도
            ConnectWebSocket();
        }
        else
        {
            Debug.Log("Failed to connect");
        }
        
    }

    // 프로그램 종료 시에 호출되는 함수
    void OnApplicationQuit()
    {
        DisConnectedSockedt();
    }

    // 연결된 socket을 Release 해준다.
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
            // MyData를 JSON화
            string jsonData = JsonConvert.SerializeObject(sendData);

            webSocket.Send(jsonData);
        }
    }
}
