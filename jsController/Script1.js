var net = require('net');

var server = net.createServer(�������(�����)) {
	socket.write('Echo server\r\n');
socket.pipe(�������);
});

server.listen(1337, '127.0.0.1');

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

var client = new net.Socket();
client.connect(4040, '127.0.0.1', �������() {
	console.log('Connected');
	������.��������('������, ������! � �������, ������.);
});

client.on('data', function (data) {
	console.log('Received:' + data);
	client.destroy(); // ����� ������� ����� ������ �������
});

client.on('close', function () {
	console.log("���������� �������");
});
