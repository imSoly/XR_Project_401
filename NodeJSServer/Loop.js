const LOOPS = function() {
    let loop;
    let fps = 1;
    // 룸이 준비되고 시작되었을 때 1초마다 1씩 늘린다.
    let gameLoopTimeCount = 0;

    // 준비 되었다고 만든 함수
    this.LogMsg = function () {
        console.log("GAMELOOPS");
    };

    // 루프 시작
    this.StartLoops = function (params, room, ws, room) {
        // 1초마다 한 번씩 Room 있는 사람들에게 전달
        loop = setInterval(() => {
            gameLoopTimeCount += 1;
            console.log(`Looping: ${gameLoopTimeCount}`);

            // JSON 포맷
            obj = {
                type: "info",
                myParams: {
                    room: ws["room"],
                    loopTimeCount: gameLoopTimeCount
                }
            };

            // 룸 안에 있는 모든 사람들에게 전달
            for (let i = 0; i < room[room].length; i++) {
                // JSON 포맷으로 변환 후 Send
                room[room][i].send(JSON.stringify(obj));
            }
        }, 1000/fps); 
    };
};

module.exports = LOOPS;