using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {


        static void Main(string[] args)
        {
            System.Threading.Thread.Sleep(5000);
            try
            {
                
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer
                // connected to the same address as specified by the server, port
                // combination.
                int port = 4040;
                

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                string path = @"GUID.txt";
                string path2 = @"settings.txt";
                string settings = "";
                string GUID = "";
                int H,HS, HE, inter;
                /*try
                {//ЗАПИСЬ В ФАЙЛ
                    // Create the file, or overwrite if the file exists.
                    using (FileStream fs = File.Create(path))
                    {
                        byte[] info = new UTF8Encoding(true).GetBytes("be1d78b9-46ac-4bb7-b93f-6b90a30a612b");
                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);
                    }

                    
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }*/
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                       // Console.WriteLine(line);
                        GUID = line;
                    }
                }
                Console.WriteLine("GUID = " + GUID);


                

                using (StreamReader sr = new StreamReader(path2, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        // Console.WriteLine(line);
                        settings = line;
                    }
                }
                Console.WriteLine("SETTINGS = " + settings);
                if (settings == "")
                {
                    try
                    {//ЗАПИСЬ В ФАЙЛ
                     // Create the file, or overwrite if the file exists.
                        using (FileStream fs = File.Create(path2))
                        {
                            byte[] info = new UTF8Encoding(true).GetBytes("40|70|500");
                            // Add some information to the file.
                            fs.Write(info, 0, info.Length);
                        }
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                string[] words = settings.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                H = Convert.ToInt32(words[0]);
                HS = Convert.ToInt32(words[1]);
                HE = Convert.ToInt32(words[2]);
                inter = Convert.ToInt32(words[3]);
                Console.WriteLine(" H = " + H + " HS = " + HS + " HE = " + HE + " inter = " + inter);
                bool flag = false;
                string message = "";
                
                for (int i = 0; i < 20; i++)
                {
                    TcpClient client = new TcpClient("127.0.0.1", port);
                    NetworkStream stream = client.GetStream();
                    System.Threading.Thread.Sleep(inter);
                    
                    if (H <= HS && flag == false)
                    {
                        Console.WriteLine("Начало полива. Влажность = " + H);
                        message = GUID + "|send_data|" + H;
                        H += 3;
                        flag = true;
                    }
                    else if (flag == true && H<HE)
                    {
                        Console.WriteLine("Идет полив. Влажность = " + H);
                        message = GUID + "|send_data|" + H;
                        H += 3;
                    }
                    else if (flag == true && H >= HE)
                    {
                        Console.WriteLine("Конец полива. Влажность = " + H);
                        message = GUID + "|send_data|" + H;
                        H += 3;
                        flag = false;
                    }
                    else
                    {
                        Console.WriteLine("Влажность в норме. Влажность = " + H);
                        message = GUID + "|send_data|" + H;
                        H -= 5;
                    }

                    
                    byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                    

                    // Send the message to the connected TcpServer.
                    stream.Write(data, 0, data.Length);

                    Console.WriteLine("Sent: {0}", message);

                    // Receive the TcpServer.response.

                    // Buffer to store the response bytes.
                    //Byte[] data2 = new Byte[256];

                    // String to store the response ASCII representation.
                    //String responseData = String.Empty;

                    // Read the first batch of the TcpServer response bytes.
                    //Int32 bytes = stream.Read(data2, 0, data2.Length);
                    //responseData = System.Text.Encoding.ASCII.GetString(data2, 0, bytes);
                    //Console.WriteLine("Received: {0}", responseData);

                    // Close everything.
                    stream.Close();
                    client.Close();
                }
                
                   

                
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
          Console.WriteLine("\n Press Enter to continue...");
            Console.Read();



        }
    }
}
