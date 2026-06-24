using System;
using System.IO.Pipes;
using System.Text;

class Program
{
    static void Main()
    {
        using var pipe = new NamedPipeClientStream(".", "chat_pipe", PipeDirection.InOut);
        pipe.Connect();

        Console.WriteLine("CONNECTED");

        byte[] buffer = new byte[1024];

        while (true)
        {
            Console.Write(">>> ");
            string input = Console.ReadLine();

            byte[] data = Encoding.UTF8.GetBytes(input);
            pipe.Write(data, 0, data.Length);

            int bytes = pipe.Read(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytes);

            Console.WriteLine("Server: " + response);

            if (input == "exit")
                break;
        }
    }
}
