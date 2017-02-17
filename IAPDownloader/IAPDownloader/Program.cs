using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace IAPDownloader
{
    class Program
    {
        static SerialPort com = null;

        static void Main(string[] args)
        {            
            try
            {
                com = new SerialPort(args[0]);
                com.BaudRate = 115200;
                com.Encoding = Encoding.GetEncoding("ASCII");
                com.DataReceived += com_DataReceived;
                com.Open();
                com.DiscardInBuffer();
                com.DiscardOutBuffer();
                char ch = (char)9;
                com.Write(ch.ToString());
                System.Threading.Thread.Sleep(500);
                ch = 'c';
                com.Write(ch.ToString());
                System.Threading.Thread.Sleep(1000);
                ch = '4';
                com.Write(ch.ToString());
                System.Threading.Thread.Sleep(1000);
                ch = '1';
                com.Write(ch.ToString());
                System.Threading.Thread.Sleep(1000);
                com.ReadTimeout = 5000;
                com.DataReceived -= com_DataReceived;
                Ymodem ymodem = new Ymodem(com);
                if (ymodem.YmodemUploadFile(args[1]))
                {
                    com.DataReceived += com_DataReceived;
                    System.Threading.Thread.Sleep(2000);
                    ch = '3';
                    com.Write(ch.ToString());
                    System.Threading.Thread.Sleep(1000);
                }                                                                               
                else {
                    com.DataReceived += com_DataReceived;
                    System.Threading.Thread.Sleep(2000);
                    ch = 'a';
                    com.Write(ch.ToString());
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch
            {
                Console.WriteLine("Failed.");
            }
            finally {
                if (com != null) {
                    com.Close();
                }
            }
        }

        static void com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Console.Write(com.ReadExisting());
        }
    }
}
