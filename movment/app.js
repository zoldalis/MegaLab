var net = require('net'); // ������� ����������
const fs = require("fs"); // ���������� filestream ��� ������ � �������
var client = new net.Socket(); // ������� ����� ��� �������
var time_delay = 1000; // �������� ����� �����������
var sensitivity = 1000; // fake ��������� ��� �������
var move_Flag = false; // ���� �������� (false - �������� ���, true - �������� ����)
var fileContent = fs.readFileSync("settings.txt", "utf8").replace('\n', ''); // ��� ������� ����������� ������������� �������� ����� �� �����
var guid = fileContent;
var message = guid + '|send_data|'; // ��������� ���������
var Chek = false;
setInterval(myFunc, time_delay); // ������ �������� �������� ��������� � ������ ������� ����� ��������� �����������

client.connect(4040, '127.0.0.1', function () {
	console.log('Connected');                        // ������� ����������� � �������
	client.write(guid + '|' + 'get_settings' + '|'); // ��� ����������� ���������� ������ �� ���������
});
function myFunc() {
	if (Chek) {
		FakeSens();
		console.log('Client message: ' + message + move_Flag); // �������� ������� �������� ���������
		client.write(guid + "|send_data|" + move_Flag);
	}
}
function randomIntFromInterval(min, max) { // ������� ������� � ���������� �� ��� �� ���� ������������
	return Math.floor(Math.random() * (max - min + 1) + min)
}
function FakeSens() { // ������� ������� ��� ������ ������� *sensitivity* � ��������� ������ �� 100 �� 2000. � � ����������� �� ���������� ������ ���� ��������
	if (sensitivity > randomIntFromInterval(100, 2000)) {
		move_Flag = true;
	}
	else move_Flag = false;
}
client.on('data', function (data) {
	Chek = true;
	console.log('Received: ' + data + '\n');											// ������� �������� ��������� �� �������
	sensitivity = data;
});
client.on('close', function () {
	console.log('Connection closed');													// ������� �������� �����������
	fs.writeFileSync("settings.txt", guid)     // ��� �������� ����������� ���������� ���������� � ����
});