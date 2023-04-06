using System.Net;
using System.Net.Sockets;
using System.Text;

TcpListener tcpListener = new(IPAddress.Any,1337);

tcpListener.Start();
Random random = new Random();
var client = await tcpListener.AcceptTcpClientAsync();
Console.WriteLine(client.Client.LocalEndPoint);
var ClientStream = client.GetStream();

while (true)
{
    var buffer = new byte[4];

    var result = await ClientStream.ReadAsync(buffer,0, buffer.Length);

    Console.WriteLine();

    buffer = BitConverter.GetBytes(random.Next(0,3));

    await ClientStream.WriteAsync(buffer,0,result);
}