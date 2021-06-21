
//var net = require('net');

//var server = net.createServer(�������(�����)) {
//	socket.write('Echo server\r\n');
//socket.pipe(�������);
//});

//server.listen(4040, '127.0.0.1');

/*
� ����������� � �������� tcp �� ��������� ������ � ������� netcat, *nix 
������� ��� ������ � ������ ����� ������� ���������� tcp/udp. � ������ 
����������� ��� ��� ������� ����.
$ netcat 127.0.0.1 1337
�� ������ �������:
> ���-������
*/

/* ��� ����������� ���� ������ ������� tcp, ����������� � node.js. (������ � 
������ ���� �� 
http://www.hacksparrow.com/tcp-socket-programming-in-node-js.html.) */


var net = require('net');

const fs = require("fs");
let msg = null;

function sleep(ms) {
	return new Promise(resolve => setTimeout(resolve, ms));
}
sleep(5000).then(() => {
	var client = new net.Socket();
	client.setEncoding('utf8');

	client.connect(4040, '127.0.0.1', function () {
		console.log('Connected');
		client.write('Good evening!');
	});

	client.on('data', function (data) {
		console.log('Received:' + data);
		fs.readFile("GUID.txt", "utf8",
			function (error, data) {
				if (error) throw error; // ���� �������� ������
				msg = data;
			});
		if (msg == null || "") {
			fs.writeFile("GUID.txt", data, function (error) {
				if (error) throw error; // ���� �������� ������
			});
		}
	});

	client.on('close', function () {
		console.log("���������� �������");
	});
});

