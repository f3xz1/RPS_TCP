using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Xml.Serialization;

internal class Program
{
    private static async Task Main(string[] args)
    {
    string[] options = { "Rock", "Paper", "Scissors" };
        TcpClient tcpClient = new TcpClient("localhost",1337);

        var stream = tcpClient.GetStream();

        Console.WriteLine("Connected to : " + tcpClient.Connected);

        int ClientChoice;
        int ServerChoice;

        int ClientScore = 0;
        int ServerScore = 0;


        while (true)
        {

            await Console.Out.WriteLineAsync($"\nТы: {ClientScore}\nСоперник: {ServerScore}");

            Console.WriteLine("\n 1.Камень\n 2.Бумага\n 3.Ножницы");
            try
            {
                ClientChoice = int.Parse(Console.ReadLine())-1;

                var bufferClient = BitConverter.GetBytes(ClientChoice);

                await stream.WriteAsync(bufferClient, 0, bufferClient.Length);

                var bufferServer = new byte[4];
                await stream.ReadAsync(bufferServer,0,bufferServer.Length);


                ServerChoice = BitConverter.ToInt32(bufferServer);

                Console.Clear();

                await Console.Out.WriteLineAsync($"\nСоперник выбрал {options[ServerChoice]}");

                await Console.Out.WriteLineAsync($"\nВы выбрали выбрал {options[ClientChoice]}");


                Console.WriteLine();
                if (options[ServerChoice] == options[ClientChoice])
                {
                    await Console.Out.WriteLineAsync("Ничья");
                }
                else if ((ClientChoice == 0 && ServerChoice == 2) ||
                         (ClientChoice == 1 && ServerChoice == 0) ||
                         (ClientChoice == 2 && ServerChoice == 1))
                {
                    await Console.Out.WriteLineAsync("Победа!");
                    ClientScore++;
                }
                else
                {
                    await Console.Out.WriteLineAsync("Поражение");
                    ServerScore++;
                }
                if(ServerScore == 3 || ClientScore == 3)
                {
                    await Console.Out.WriteLineAsync($"\nТы: {ClientScore}\n Соперник: {ServerScore}");
                    await Console.Out.WriteLineAsync("Game Over");
                    break;
                }



            }
            catch (Exception e)
            {
                Console.WriteLine("Введите номер!");
                Console.WriteLine(e.Message);
            }
        }
        stream.Close();
    }
}
