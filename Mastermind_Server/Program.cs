using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind_Server
{
    internal class Program
    {
        private static TcpClient encryptor;
        private static TcpClient decryptor;
        private static TcpListener server;

        private static bool isCancelled = false;

        public static void Main(string[] args)
        {
            try
            {
                server = new TcpListener(IPAddress.Any, 80); // Создаем TCP сервер на порту 9999
                server.Start();
                Console.WriteLine("Сервер запущен...");

                encryptor = server.AcceptTcpClient(); // Принимаем клиента
                Console.WriteLine("Первый игрок подключен...");
                decryptor = server.AcceptTcpClient(); // Принимаем клиента
                Console.WriteLine("Второй игрок подключен...");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            while (true) // цикл обработки действий клиентов
            {
                if (isCancelled) break;
                Task task1 = Task.Run(() => HandleClient(encryptor, decryptor));
                Task task2 = Task.Run(() => HandleClient(decryptor, encryptor));
                Task.WaitAll(task1, task2);
                task1.Dispose();
                task2.Dispose();
            }
        }

        static async Task HandleClient(TcpClient sender, TcpClient receiver)
        {
            try
            {
                NetworkStream senderStream = sender.GetStream(); // получение потока отправителя
                NetworkStream receiverStream = receiver.GetStream(); // получение потока получателя
                byte[] buffer = new byte[1024];

                while (true)
                {
                    // Читаем сообщение от отправителя
                    int bytesRead = await senderStream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // Если клиент отключился, выходим из цикла
                    Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                    // Пересылаем сообщение получателю
                    await receiverStream.WriteAsync(buffer, 0, bytesRead);
                }
            }
            catch (Exception ex)
            {
                isCancelled = true;
            }
        }
    }
}