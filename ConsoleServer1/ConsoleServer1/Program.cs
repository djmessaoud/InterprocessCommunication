using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.IO;
namespace ConsoleServer1
{
    class Program
    {
        private static double doTheMath(string Data) //Function to take string from pipe and do the math :D
        {
            int n1 = 0, n2 = 0;
            char op = 'Q';
            string tempUse = "";
            for (int i = 0; i < Data.Length; i++) // take first number into n1
            {
                if ((int)Data[i] >= 48 && (int)Data[i] <= 57) tempUse += Data[i];
                else // take operand
                {
                    Console.WriteLine("Number 1 : " + tempUse);
                    op = Data[i];
                    n1 = int.Parse(tempUse);
                    tempUse = "";
                }
            }
            Console.WriteLine("Operad : " + op);
            Console.WriteLine("Number 2 : " + tempUse);
            n2 = int.Parse(tempUse); //n2 will stay alone so we take it
            switch (op) // do the math
            {
                case '+': { return n1 + n2; break; }
                case '-': { return n1 - n2; break; }
                case '*': { return n1 * n2; break; }
                case '/': { return n1 / n2; break; }
            }
            return 0;
        }
    
    static void Main(string[] args)
        {
            NamedPipeServerStream pipe = new NamedPipeServerStream("Intern", PipeDirection.InOut); //NamedPipeLine

            pipe.WaitForConnection(); 
            Console.WriteLine("Client Connected ! "); //Declaring 
            byte[] bufferr = new byte[1024]; 
            string MessageFromClient = ""; // MessageFromClient, we will use it to filter from byte array only real chars.
            int k = 0;
            while (pipe.IsConnected)
            {
                MessageFromClient = "";
                pipe.Read(bufferr, 0, bufferr.Length); //Read from client
                while (bufferr[k]!='\0') //Filtring real chars from the rest initialized chars in byte array [1024]
                {       
                    MessageFromClient += (char)bufferr[k];
                    k++;
                }   
                double result = 0;
                if (MessageFromClient != "") //If the message is not empty, do the math and send the result back to client.
                {
                    result = doTheMath(MessageFromClient);
                    Console.WriteLine("Result =  " + result);
                    byte[] resultBuffer = Encoding.UTF8.GetBytes(result.ToString());
                    pipe.Write(resultBuffer, 0, resultBuffer.Length);
                }
                MessageFromClient = "";
                k = 0;
            }
            Console.Read();
        }

    }

}
