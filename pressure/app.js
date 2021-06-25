var net = require('net'); // ������� ����������
const fs = require("fs"); // ���������� filestream ��� ������ � �������
var client = new net.Socket(); // ������� ����� ��� �������
var time_delay = 1000; // �������� ����� �����������
var pressure = 1000; // fake ��������� ��� �������
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
		var perm = FakePressure();
		console.log('Client message: ' + message + perm); // �������� ������� �������� ���������
		client.write(guid + "|send_data|" + perm);
    }
}
function randomIntFromInterval(min, max) { // ������� ������� � ���������� �� ��� �� ���� ������������
	return Math.floor(Math.random() * (max - min + 1) + min)
}
function FakePressure() { // ������� ������� ��� ������ ���������� ��������� ����� ����������� �������� � �������
	return randomIntFromInterval(100000, 103000)
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