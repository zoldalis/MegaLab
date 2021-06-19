
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

function sleep(ms) {
	return new Promise(resolve => setTimeout(resolve, ms));
}

var client = new net.Socket();
client.connect(4040, '127.0.0.1', function() {
	console.log('Connected');
	client.write('������, ������! � �������, ������.');
});

client.on('data', function (data) {
	console.log('Received:' + data);
	//sleep(5000);
	client.destroy(); // ����� ������� ����� ������ �������
});

client.on('close', function () {
	console.log("���������� �������");
});
