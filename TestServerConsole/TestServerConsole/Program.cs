using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestServerConsole
{
    class Program
    {
        static string jsonData = "";
        static QueryToDB q1 = new QueryToDB();

        static void Main(string[] args)
        {
            Console.WriteLine("Сервер запущен... Включен режим прослушивания");
            GetData();
        }

        private static void GetData()
        {
            TcpListener listner = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12000));
            listner.Start();
            while (true)
            {

                TcpClient client = listner.AcceptTcpClient();

                StreamReader sr = new StreamReader(client.GetStream());
                jsonData = sr.ReadLine();
                Console.WriteLine("Получен строка в json: " + jsonData);

                q1 = JsonConvert.DeserializeObject<QueryToDB>(jsonData);
                string answer = q1.RunQuery();

                StreamWriter sw = new StreamWriter(client.GetStream());
                sw.AutoFlush = true;
                sw.WriteLine(answer);
                Console.WriteLine("Запрос клиента выполнен и был передан ответ");

                client.Close();
            }
        }
    }
}
