var net = require('net'); // сетевая библиотека
const fs = require("fs"); // библиотека filestream для работы с файлами
var client = new net.Socket(); // создаем соект для клиента
var time_delay = 1000; // задержка между сообщениями
var sensitivity = 1000; // fake настройка для датчика
var move_Flag = false; // флаг движения (false - движения нет, true - движение есть)
var fileContent = fs.readFileSync("settings.txt", "utf8").replace('\n', ''); // при запуске контроллера устанавливаем значение гуида из файла
var guid = fileContent;
var message = guid + '|send_data|'; // заготовка сообщения
var Chek = false;
setInterval(myFunc, time_delay); // задаем интервал отправки сообщения и функию которая будет постоянно выполняться

client.connect(4040, '127.0.0.1', function () {
	console.log('Connected');                        // событие подключения в серверу
	client.write(guid + '|' + 'get_settings' + '|'); // при подключении отправляем запрос на настройки
});
function myFunc() {
	if (Chek) {
		FakeSens();
		console.log('Client message: ' + message + move_Flag); // основная функция отправки сообщения
		client.write(guid + "|send_data|" + move_Flag);
	}
}
function randomIntFromInterval(min, max) { // функция рандома в промежутке от мин до макс включительно
	return Math.floor(Math.random() * (max - min + 1) + min)
}
function FakeSens() { // функция которая при вызове сверяет *sensitivity* с рандомным числом от 100 до 2000. и в зависимости от результата меняет флаг движения
	if (sensitivity > randomIntFromInterval(100, 2000)) {
		move_Flag = true;
	}
	else move_Flag = false;
}
client.on('data', function (data) {
	Chek = true;
	console.log('Received: ' + data + '\n');											// событие принятия сообщения от сервера
	sensitivity = data;
});
client.on('close', function () {
	console.log('Connection closed');													// событие закрытия подключения
	fs.writeFileSync("settings.txt", guid)     // при закрытии подключения записываем переменные в файл
});