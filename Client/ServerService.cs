using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Client.Data;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
//{0-8}{9-1023}
namespace Client
{
    public class ServerService : BackgroundService
    {

        struct msg_to_serv
        {
            byte[] id;
            byte[] msg;
            msg_to_serv(byte[] Id, byte[] Msg)
            {
                id = Id;
                msg = Msg;
            }
        }

        struct msg_to_temp_cont
        {

        }

        struct msg_to_humi_cont
        {

        }

        struct msg_to_light_cont
        {

        }

        struct msg_to_bar_cont
        {

        }

        struct msg_to_move_cont
        {

        }

        private readonly ApplicationDbContext _DBContext;

        const int port = 4040;


        //public IServiceProvider ServiceProvider => throw new NotImplementedException();

        public ServerService(IServiceScopeFactory scopeFactory/*ApplicationDbContext context*/)
        {

            using (var scope = scopeFactory.CreateScope()) // this will use `IServiceScopeFactory` internally
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                _DBContext = context;
            }

        }


        
        //создание экземпляра сервера
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new TcpListener(localAddr, port);
            listener.Start();
            //прослушивание порта
            while (!stoppingToken.IsCancellationRequested)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                Console.WriteLine("a new client connected");
                NetworkStream stream = client.GetStream();

                //обработка принятого запроса
                while (!stoppingToken.IsCancellationRequested)
                {
                    //data - то что пришло с запроса
                    byte[] data = new byte[1024];
                    int read = await stream.ReadAsync(data, 0, 1024, stoppingToken);
                    // если нихуя не пришло, то прекращаем обработку запроса
                    if (data[0] == (byte)0)
                        break;
                    var cmd = Encoding.UTF8.GetString(data, 0, read);

                    //тут нужно реализовать логику обработки данных которые нам передали, распарсить их, понять когда нам передали guid, а когда собщение с данными датчика например



                    Console.WriteLine($"received : {cmd}");



                    //await stream.WriteAsync(data, 0, read, stoppingToken);
                    stream.Write(Encoding.UTF8.GetBytes("Server Answer"),0,13);
                    stream.Flush();

                }
            }

            throw new NotImplementedException();
        }


        public void InitController(string guid)
        {
            Controller cont = new Controller();
            cont.Id = Guid.Parse(guid).ToString();
            _DBContext.Controllers.Add(cont);
        }



        public void MSGHandle(string msg)
        {
            InitController(msg);
        }
    }
}





//МУСОР -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=




//public void Init()
//{
//    TcpListener server = null;
//    try
//    {
//        IPAddress localAddr = IPAddress.Parse("127.0.0.1");
//        server = new TcpListener(localAddr, port);

//        // запуск слушателя
//        server.Start();

//        while (true)
//        {
//            Console.WriteLine("Ожидание подключений... ");

//            // получаем входящее подключение
//            TcpClient client = server.AcceptTcpClient();
//            Console.WriteLine("Подключен клиент. Выполнение запроса...");
//            // получаем сетевой поток для чтения и записи
//            NetworkStream stream = client.GetStream();
//            byte[] mews = new byte[100];
//            stream.Read(mews);
//            string result = Encoding.UTF8.GetString(mews);
//            MSGHandle(result);
//            Console.WriteLine("Message Is:" + result);
//            // сообщение для отправки клиенту
//            string response = "Hello from server";
//            // преобразуем сообщение в массив байтов
//            byte[] data = Encoding.UTF8.GetBytes(response);

//            // отправка сообщения
//            stream.Write(data, 0, data.Length);
//            Console.WriteLine("Отправлено сообщение: {0}", response);
//            // закрываем поток
//            stream.Close();
//            // закрываем подключение
//            client.Close();
//        }
//    }
//    catch (Exception e)
//    {
//        Console.WriteLine(e.Message);
//    }
//    finally
//    {
//        if (server != null)
//            server.Stop();
//    }
//}


