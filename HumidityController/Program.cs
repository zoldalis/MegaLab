using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {

        // Функция запрашивает строку, конвертирует ее в тип Т и валидирует выходные данные
        private static T Promt<T>(string promt, // Сообщение
            Predicate<T> validation = null, // Функция валидации, принимает аргумент типа Т и возвращает true/false (необязательний аргумент)
            string errorMessage = null // Сообщение в случае ошибки (необязятельный аргумент)
        )
        {
            T result;
            while (true)
            {
                Console.Write(promt);
                try
                {
                    IConvertible res = Console.ReadLine(); // Считываем строку как тип IConvertible
                    result = (T)res.ToType(typeof(T), null); // Попытка сконвертировать строку в тип Т 
                }
                catch (FormatException e)
                {
                    // Если произошла ошибка
                    Console.WriteLine(errorMessage ?? e.Message); // выводим сообщение об ошибке
                    continue; // и продолжаем цикл
                }


                if (validation == null || validation(result)) // Если функция валидации не задана или возвращает истину
                {
                    break; // Выходим из цикла
                }
                else
                {
                    Console.WriteLine(errorMessage ?? "Введенная строка невалидна!");
                }
            }
            return result;
        }

        static async Task Main(string[] args)
        {
            const int port = 4040;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");  // Адрес сервера (адрес локального компьютера - 127.0.0.1) 
            TcpClient client = new TcpClient();

            try
            {
                client.Connect(address, portNumber); // Подключение к серверу
            }
            catch (Exception) // Если произошла ошибка 
            {
                Console.WriteLine("Не удалось подключиться к серверу"); // Выводим сообщение
                return; // Завераем выполнение программы
            }


            var stream = client.GetStream(); // Открываем сетевой поток
            client.ReceiveBufferSize = 2048 * 2048; // Устанавливаем размер буфера

            var buffer = new byte[client.ReceiveBufferSize]; // Создание буффера
            var lengthBuffer = new byte[8]; // дополниетльный буффер


            try
            {
                while (client.Connected)
                {

                    await stream.ReadAsync(lengthBuffer, 0, lengthBuffer.Length); // Считываем длину файла в массив байт        
                    var size = BitConverter.ToInt64(lengthBuffer, 0); // конвертируем массив байт в число типа long



                    if (
                        Promt<string>($"Принять файл весом {(size / 2048.0 / 2048.0).ToString("F")}MB?(да/нет)\n").ToLower() == "да"
                    // Если пользователь подтверждает загрузку
                    )
                    {
                        stream.WriteByte(1); // Подтверждаем загрузку файла
                        await stream.ReadAsync(lengthBuffer, 0, lengthBuffer.Length); //Считываем длину файла
                        var c = await stream.ReadAsync(buffer, 0, (int)BitConverter.ToInt64(lengthBuffer, 0)); // Считываем название файла
                        var name = Encoding.Unicode.GetString(buffer, 0, c); // Конвертирем массив байт в строку
                        var fs = File.OpenWrite(name); // Открываем/создаем файл
                        long readed = 0; // К-во считаных байт
                        while (readed < size) // Пока к-во считаных байт меньше размера файла
                        {
                            var count = await stream.ReadAsync(buffer, 0, buffer.Length); // Читаем кусок из сетевого потока
                            readed += count;
                            await fs.WriteAsync(buffer, 0, count); // Записываем считаное в файл
                        }
                        fs.Close(); // Закрываем файл
                        Console.WriteLine($"Файл {name} сохранен");
                    }
                    else
                    {
                        stream.WriteByte(0); // Не подтверждаем загрузку
                    }
                }
            }
            catch (SocketException) // Если сервер отключился
            {
                Console.WriteLine("Сервер прервал соединие"); // Вывод сообщения
            }

        }
    }
}
