using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerEchoLibrary
{
    public class ServerEchoAPM : ServerEcho
    {
        public delegate void TransmissionDataDelegate(NetworkStream stream);

        public ServerEchoAPM(IPAddress IP, int port) : base(IP, port)
        {

        }

        protected override void AcceptClient()
        {
            while (true)
            {
                TcpClient tcpClient = TcpListener.AcceptTcpClient();
                Stream = tcpClient.GetStream();
                TransmissionDataDelegate transmissionDelegate = new TransmissionDataDelegate(BeginDataTransmission);

                transmissionDelegate.BeginInvoke(Stream, TransmissionCallback, tcpClient);
            }
        }


        private void TransmissionCallback(IAsyncResult ar)
        {
            TcpClient tcpClient = (TcpClient)ar.AsyncState;
            tcpClient.Close();
        }
        protected override void BeginDataTransmission(NetworkStream stream)
        {
            byte[] buffer = new byte[Buffer_size];
            byte[] responseBuffer = new byte[Buffer_size];

            while (true)
            {
                try
                {
                    stream.Read(buffer, 0, Buffer_size);

                    string received = System.Text.Encoding.ASCII.GetString(buffer);
                    Console.WriteLine(received);
                    //switch case
                    string response = CaesarCipher(received);

                    responseBuffer = System.Text.Encoding.ASCII.GetBytes(response);
                    stream.Write(responseBuffer, 0, responseBuffer.Length);

                    Array.Clear(buffer, 0, buffer.Length);
                    Array.Clear(responseBuffer, 0, response.Length);
                }
                catch (IOException e)
                {
                    break;
                }
            }
        }
        public override void Start()
        {
            Console.WriteLine("Start serwera");
            StartListening();
            //transmission starts within the accept function
            AcceptClient();
        }

        protected static string CaesarCipher(string word)
        {
            var rand = new Random();
            // losowanie wartosci przesuniecia
            int liczba = rand.Next(1, 21);
            StringBuilder stringBuilder = new StringBuilder(word);

            for(int i=0; i<word.Length; i++)
            {
                stringBuilder[i] += (char) liczba;
            }

            return stringBuilder.ToString();
        }

        protected static string DiagonalColumnTranspositionCipherWithKey(string word, string secretKey)
        {
            string cipher = "";
            int rows = (int)Math.Ceiling(word.Length / secretKey.Length * 1.0);
            int secretKeyLength = secretKey.Length;
            char[,] array = new char[rows, secretKeyLength];
            int counter = 0;

            for(int i=0; i<rows; i++)
            {
                for(int j=0; j<secretKeyLength; j++)
                {
                    if (counter < secretKeyLength)
                        array[i, j] = word[counter];
                    else
                        array[i, j] = 'X';
                    counter++;
                    Console.WriteLine(array[i, j]);
                }
                Console.WriteLine("\n");
            }


            return cipher;
        }

        private static int[] getSortedIndex(string input)
        {
            int[] indexes = new int[input.Length];
            char[] characters = input.ToArray();
            Array.Sort(characters);
            string sorted = new string(characters);
            int index = 0;

            for (int i = 0; i < sorted.Length; i++)
            {
                for (int j = 0; j < input.Length; j++)
                {
                    if (sorted[i] == input[j])
                    {
                        index = j;
                        if (!checkIndex(index, indexes))
                            break;
                    }
                }
                indexes[i] = index;
            }

            return indexes;
        }

        private static bool checkIndex(int index, int[] indexes)
        {
            for (int i = 0; i < indexes.Length; i++)
            {
                if (index == indexes[i])
                    return true;
            }
            return false;
        }
    }
}
