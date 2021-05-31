using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Client.Data;


namespace TCP_Server
{
    public class ServerService
    {

        private readonly ApplicationDbContext _DBContext;

        const int port = 4040;
        public ServerService(ApplicationDbContext context)
        {
            _DBContext = context;
            Init();
        }

        private void Init()
        {
            TcpListener server = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, port);

                // запуск слушателя
                server.Start();

                while (true)
                {
                    Console.WriteLine("Ожидание подключений... ");

                    // получаем входящее подключение
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Подключен клиент. Выполнение запроса...");
                    // получаем сетевой поток для чтения и записи
                    NetworkStream stream = client.GetStream();
                    byte[] mews = new byte[100];
                    stream.Read(mews);
                    string result = Encoding.UTF8.GetString(mews);
                    MSGHandle(result);
                    Console.WriteLine("Message Is:" + result);
                    // сообщение для отправки клиенту
                    string response = "Hello from server";
                    // преобразуем сообщение в массив байтов
                    byte[] data = Encoding.UTF8.GetBytes(response);

                    // отправка сообщения
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Отправлено сообщение: {0}", response);
                    // закрываем поток
                    stream.Close();
                    // закрываем подключение
                    client.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (server != null)
                    server.Stop();
            }
        }

        private void MSGHandle(string msg)
        {
            InitController(msg);
        }

        private void InitController(string guid)
        {
            Controller cont = new Controller();
            cont.Id = Guid.Parse(guid).ToString();
           _DBContext.Controllers.Add(cont);
        }

    }
}
