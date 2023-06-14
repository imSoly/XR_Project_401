﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;               //C# 에서 웹 소캣을 지원하는 라이브러리
using System.Text;
using Newtonsoft.Json;              //JSON 을 사용하기위한 라이브러리 
using UnityEngine.UI;               //UI를 통해서 패킷 변경

public class MyData
{
    public string clientID;                     //서버에서 제작 해서 클라이언트에 접속시 줌
    public string message;
    public int requestType;                     // 요청 번호 json로 보냄
}

public class InfoData                           //서버에서 제작한 패킷 선언
{
    public string type;
    public InfoParams myParams;
}

public class InfoParams                          //서버에서 제작한 패킷 선언 (내부)
{
    public string room;
    public int loopTimeCount;
}

public class SocketClient : MonoBehaviour
{
    private WebSocket webSocket;
    private bool isConnected = false;
    private int connectionAttempt = 0;              // 연결 시도 횟수 
    private const int maxConnectionAttempts = 3;    // 최대 연결 시도 횟수

    MyData sendData = new MyData { message = "메세지 전송" };

    public Button sendButton;                           //JSON 전송 버튼
    public Button ReconnectButton;                      //연결이 끊겼을때 다시 연결 하는 버튼
    public Text typeText;                               //메세지 종류 데이터 받아와서 패킷에 보내기 위해 선언
    public Text messageText;
    public Text uiLoopCountText;                        //루프 카운트를 확인하기 위한 UI

    public int loopCount;

    // Start is called before the first frame update
    void Start()
    {
        ConnectWebSocekt();

        sendButton.onClick.AddListener(() =>                        //SEND 버튼을 눌렀을 때 
        {
            //JSON 데이터 생성
            sendData.requestType = int.Parse(typeText.text);        //0 ,10, 100, 200, 300 
            sendData.message = messageText.text;
            string jsonData = JsonConvert.SerializeObject(sendData);

            webSocket.Send(jsonData);       //wectSocket으로 JSON 데이터 전송 
        });

        ReconnectButton.onClick.AddListener(() =>
        {
            ConnectWebSocekt();
        });
    }

    void ConnectWebSocekt()
    {
        webSocket = new WebSocket("ws://localhost:8000");           //localhost 127.0.0.1 port : 8000 , ws => websocket
        webSocket.OnOpen += OnWebSocketOpen;                        //웹 소캣이 연결 되었을 때 이벤트를 발생시켜서 함수를 실행 시킨다. 
        webSocket.OnMessage += OnWebSocketMessage;                  //웹 소캣 메세지가 왔을 때 이벤트를 발생시켜 Message 함수를 실행 시킨다.
        webSocket.OnClose += OnWebSocketClose;                      //웹 소캣 연결이 끊어졌을때 이벤트를 발생시켜 Close 함수를 실행 시킨다. 

        webSocket.ConnectAsync();
    }

    void OnWebSocketOpen(object sender, System.EventArgs e)         //웹 소캣이 오픈되고 연결 되었을 때 
    {
        Debug.Log("WebSocket connected");
        isConnected = true;
        connectionAttempt = 0;
    }

    void OnWebSocketMessage(object sender, MessageEventArgs e)      //웹 소캣이 연결된후 Message가 왔을 때 
    {
        string jsonData = Encoding.Default.GetString(e.RawData);    //MessageEventArgs에 들어온 RawData를 Json으로 인코딩 한다. 
        Debug.Log("Received JSON data : " + jsonData);

        MyData receivedData = JsonConvert.DeserializeObject<MyData>(jsonData);          //JSON 데이터를 객체로 역직렬화

        InfoData infoData = JsonConvert.DeserializeObject<InfoData>(jsonData);

        if (infoData != null)
        {
            string room = infoData.myParams.room;
            loopCount = infoData.myParams.loopTimeCount;
        }

        if (receivedData != null && !string.IsNullOrEmpty(receivedData.clientID))        //receivedData 값이 비어 있지 않을 때
        {
            sendData.clientID = receivedData.clientID;                                  //서버에서 받아온 ID 값을 MyData에 넣는다. 
        }

    }

    void OnWebSocketClose(object sender, CloseEventArgs e)              //웹 소캣 연결이 끊겼을 
    {
        Debug.Log("WebSocket connection closed");
        isConnected = false;                                            //연결 끈김 flag 

        if (connectionAttempt < maxConnectionAttempts)                   //총 3번의 시도 
        {
            connectionAttempt++;
            Debug.Log("Attempting to reconnect. Attempt : " + connectionAttempt);
            ConnectWebSocekt();                                                         //Connect 시도를 한다.
        }
        else
        {
            Debug.Log("Failed to connect ");
        }
    }

    void OnApplicationQuit()                        //프로그램 종료시에 호출 되는 함수 
    {
        DisconnectWebSocket();
    }

    void DisconnectWebSocket()                      //연결된 socket를 Relese 해준다. 
    {
        if (webSocket != null && isConnected)
        {
            webSocket.Close();
            isConnected = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (webSocket == null || !isConnected)
        {
            return;
        }

        uiLoopCountText.text = "LoopCount : " + loopCount.ToString();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            sendData.requestType = 0;
            string jsonData = JsonConvert.SerializeObject(sendData);                //Mydata 를 Json 으로 만들어줌 

            webSocket.Send(jsonData);

        }
    }
}