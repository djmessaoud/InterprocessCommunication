using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.IO;
using System.Threading;
namespace ConsoleClient1
{
    class Program
    {
      public static  NamedPipeClientStream pipe;
        static void Main(string[] args)
        {
            Random r = new Random();
            pipe = new NamedPipeClientStream(".", "Intern", PipeDirection.InOut);
                pipe.Connect();
                
                if (pipe.IsConnected)
                {
                    Console.WriteLine("Connected to server");
                    string msg = "";
                    Console.Write("Enter Statement in format [A op B] e.g [{0}*{1}] : ",r.Next(0,999).ToString(),r.Next(1,999).ToString());
                    msg = Console.ReadLine();
                    pipe.Write(Encoding.UTF8.GetBytes(msg),0,msg.Length);
                    Thread GetResult = new Thread(ResultShow);
                    GetResult.Start();
                }
                Console.Read();
            
        }
        private static void ResultShow()
        {
            while (pipe.IsConnected)
            {
                byte[] buffer = new byte[1024];
                pipe.Read(buffer, 0, buffer.Length);
                Console.WriteLine(" Result from server = " + Encoding.UTF8.GetString(buffer));
            }
        }
    }
}