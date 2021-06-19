var net = require('net');

var server = net.createServer(функция(сокет)) {
	socket.write('Echo server\r\n');
socket.pipe(розетка);
});

server.listen(1337, '127.0.0.1');

/*
И соединитесь с клиентом tcp из командной строки с помощью netcat, *nix 
утилита для чтения и записи через сетевые соединения tcp/udp. Я только 
использовал его для отладки себя.
$ netcat 127.0.0.1 1337
Вы должны увидеть:
> Эхо-сервер
*/

/* Или используйте этот пример клиента tcp, написанного в node.js. (Возник с 
пример кода из 
http://www.hacksparrow.com/tcp-socket-programming-in-node-js.html.) */

var net = require('net');

var client = new net.Socket();
client.connect(4040, '127.0.0.1', функция() {
	console.log('Connected');
	клиент.напишите('Привет, сервер! С любовью, Клиент.);
});

client.on('data', function (data) {
	console.log('Received:' + data);
	client.destroy(); // убить клиента после ответа сервера
});

client.on('close', function () {
	console.log("Соединение закрыто");
});
