const webSocket = require('ws');
const CREATE = require("./Create.js");

// 서버 8000번 포트 오픈
const wss = new webSocket.Server({ port: 8000 }, () => {
    console.log('서버 시작');
    console.log(getKey(7));
});

const userList = [];

// 방 접속 최대 인원 수
const maxClients = 5;
// 룸 배열
let rooms = {};
// 유저
let joinUserTemp = 1;

wss.on('connection', function connection(ws) {
    ws.clientID = getKey(8);

    // 방 생성 객체 선언
    const create = new CREATE();

    // message 이벤트
    ws.on('message', (data) => {
        // 받은 데이터를 json 파싱
        const jsonData = JSON.parse(data);

        let requestType = jsonData.requestType;
        let params = jsonData.message;

        console.log('받은 데이터:', jsonData, requestType, params);

        if (requestType === 10) {
            ws.send(JSON.stringify({ userList }));
        }

        if(requestType === 100) {
            create.createRoom(params, rooms, ws);
        }

        if(requestType === 200) {
            joinRoom(params, ws);
        }

        if(requestType === 300) {
            leaveRoom(params);
        }

        if (requestType === 0) {
            // 접속한 클라이언트들에게 send
            wss.clients.forEach((client) => {
                client.send(data);
            });
        }
    });

    ws.on('close', () => {
        // 배열에서 해당 클라이언트 제거
        const index = userList.indexOf(ws.clientID);
        if (index !== -1) {
            console.log(`클라이언트 해제: ${ws.clientID}`);
            userList.splice(index, 1);
        }
    });

    // 새로 연결된 클라이언트를 유저 리스트에 추가
    userList.push(ws.clientID);

    // 클라이언트에게 임시 유저 이름 전송
    ws.send(JSON.stringify({ clientID: ws.clientID }));

    console.log('클라이언트 연결 - ID', ws.clientID);
});

function generalInfomation (ws) {
    let obj;

    if(ws["room"] !== undefined) {
        // ws배열에 방이 있을 경우 진입
        obj = {
            type: "info",
            myParams: {
                room: ws["room"],
                "no-client": rooms[ws["room"]].length,
            }
        }
    } else {
        // 방이 없을 경우
        obj = {
            type: "info",
            myParams: {
                room: "No Room",
            }
        }
    }

    // 클라이언트에 전달해준다.
    ws.send(JSON.stringify(obj));
}

function joinRoom (params, ws) {
    const room = params;

    // 룸이 없으면 존재하지 않는다는 메세지
    if(!Object.keys(rooms).includes(room)) {
        console.warn(`Room ${room} does not exist!`);
        return;
    }

    // 최대 인원보다 많이 들어갈 수 없음
    if (rooms[room].length >= maxClients) {
        console.warn(`Room ${room} is full!`);
        return;
    }

    // ws 소켓을 룸에 넣는다.
    rooms[room].push(ws);
    ws["room"] = room;

    generalInfomation(ws);

    let userList = '';

    for (let i = 0; i < rooms[room].length; i++) {
        userList += `User: ${rooms[room][i].user}` + "\n";
    }
    joinUserTemp += 1;

    obj = {
        type: "info",
        myParams: {
            room: ws["room"],
            userList,
        }
    }

    for (let i = 0; i < rooms[room].length; i++) {
        rooms[room][i].send(JSON.stringify(obj));
    }
}

// 룸을 나갈 경우
function leaveRoom (params) {
    const room = ws.room;

    // 룸이 존재할 때
    if (rooms[room].length > 0) {
        rooms[room] = rooms[room].filter(so => so !== ws);
        ws["room"] = undefined;

        // 룸이 0명이 되었을 때
        if (rooms[room].length === 0) {
            close(room);
        }
    }

    // 룸 제거 함수
    function close (room) {
        if (rooms.length > 0) {
            rooms = rooms.filter(key => key !== room);
        }
    }
}

wss.on('listening', () => {
    console.log('Listening...');
})

function getKey(length) {
    let result = '';
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';

    for(let i = 0; i < length; i++) {
        result += characters.charAt(Math.floor(Math.random() * characters.length))
    }

    return result;
}

getKey(7);