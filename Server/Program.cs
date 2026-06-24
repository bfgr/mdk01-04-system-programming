using System;
using System.IO.Pipes;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("SERVER STARTED");

        using var pipe = new NamedPipeServerStream("chat_pipe", PipeDirection.InOut);
        pipe.WaitForConnection();

        Console.WriteLine("CLIENT CONNECTED");

        byte[] buffer = new byte[1024];

        while (true)
        {
            int bytes = pipe.Read(buffer, 0, buffer.Length);
            string msg = Encoding.UTF8.GetString(buffer, 0, bytes);

            Console.WriteLine("Client: " + msg);

            string response =
                msg == "time" ? DateTime.Now.ToString() :
                msg.StartsWith("echo ") ? msg.Substring(5) :
                msg == "exit" ? "bye" :
                "unknown";

            byte[] data = Encoding.UTF8.GetBytes(response);
            pipe.Write(data, 0, data.Length);

            if (msg == "exit")
                break;
        }
    }
}


// test commit
