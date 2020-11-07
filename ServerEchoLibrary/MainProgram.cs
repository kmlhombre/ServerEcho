using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerEchoLibrary
{
    class MainProgram
    {
        private static void Main(string[] args)
        {
            ServerEchoAPM serverEchoAPM = new ServerEchoAPM(IPAddress.Parse("127.0.0.1"), 2137);
            serverEchoAPM.Start();
        }
    }
}
