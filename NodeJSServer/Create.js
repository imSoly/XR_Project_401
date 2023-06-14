const LOOPS = require('./Loop.js');

const CREATE = function () {
    // 방이 만들어졌을 때 콘솔로 알려준다.
    console.log("Create read info ..."); 
};

CREATE.prototype.LogMsg = function () {
    // 방 메세지
    console.log("Create Connect ...");
};

CREATE.prototype.generalInfomation = function (ws, rooms) {
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

CREATE.prototype.createRoom = function (params, rooms, ws) {
    const room = this.getKey(5);
    console.log(`room id: ${room}`);

    rooms[room] = [ws];
    ws["room"] = room;

    this.generalInfomation(ws, rooms);

    // 방이 만들어진 것을 확인 후에 시간 설정
    const loops = new LOOPS();
    // 해당 루프를 실행시킨다.
    loops.StartLoops(params, rooms, ws, room);
};

// 랜덤으로 방 이름을 지정
CREATE.prototype.getKey = function (length) {
    let result = '';
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';

    for (let i = 0; i < length; i++) {
        result += characters.charAt(Math.floor(Math.random() * characters.length))
    }

    return result;
}

module.exports = CREATE;