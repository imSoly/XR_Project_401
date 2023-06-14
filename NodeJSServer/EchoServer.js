const webSocket = require('ws');

// 서버 8000번 포트 오픈
const wss = new webSocket.Server({ port: 8000 }, () => {
    console.log('서버 시작');
    console.log(getKey(7));
});

const userList = [];

wss.on('connection', function connection(ws) {
    ws.clientID = getKey(8);

    // message 이벤트
    ws.on('message', (data) => {
        // 받은 데이터를 json 파싱
        const jsonData = JSON.parse(data);
        console.log('받은 데이터:', jsonData);

        // 접속한 클라이언트들에게 send
        wss.clients.forEach((client) => {
            client.send(data);
        });
    });

    // 새로 연결된 클라이언트를 유저 리스트에 추가
    userList.push(ws.clientID);

    // 클라이언트에게 임시 유저 이름 전송
    ws.send(JSON.stringify({ clientID: ws.clientID }));

    console.log('클라이언트 연결 - ID', ws.clientID);
});

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