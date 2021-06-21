
//var net = require('net');

//var server = net.createServer(функция(сокет)) {
//	socket.write('Echo server\r\n');
//socket.pipe(розетка);
//});

//server.listen(4040, '127.0.0.1');

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
				if (error) throw error; // если возникла ошибка
				msg = data;
			});
		if (msg == null || "") {
			fs.writeFile("GUID.txt", data, function (error) {
				if (error) throw error; // если возникла ошибка
			});
		}
	});

	client.on('close', function () {
		console.log("Соединение закрыто");
	});
});

