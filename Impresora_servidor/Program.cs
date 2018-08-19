using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Impresora_servidor
{
    class Program
    {
        string carpeta = Path.GetTempPath();
        static bool conectar = false, apagar = false;
        Socket s = null;

        public void iniciaServidorImpresora()
        {
            try
            {
                IPEndPoint ie = new IPEndPoint(IPAddress.Any, 31416);
                s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s.Bind(ie);
                s.Listen(5);
                Console.WriteLine("Usando puerto: " + 31416);
                Console.WriteLine("A la espera");
                conectar = true;

                while (conectar)
                {
                    Socket cliente = s.Accept();
                    Thread hilo = new Thread(hiloCliente);
                    hilo.IsBackground = true;
                    hilo.Start(cliente);
                }
            }
            catch (SocketException)
            {
                conectar = false;
                if (!apagar)
                    Console.WriteLine("Puerto " + 31416 + " ocupado. Cerrando servidor.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
            }
        }

        public void hiloCliente(object socket)
        {
            //TODO comprobar servidor tiene impresora 
            Socket cliente = (Socket)socket;
            IPEndPoint ieCliente = (IPEndPoint)cliente.RemoteEndPoint;
            Console.WriteLine("Conectado con el cliente {0} en el puerto {1}", ieCliente.Address, ieCliente.Port);
            NetworkStream ns = new NetworkStream(cliente);
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            string mensaje;
            try
            {
                mensaje = sr.ReadLine();
                Console.WriteLine(mensaje);
              
            }
            catch (IOException)
            {

            }
            catch (ObjectDisposedException)
            {

            }
            catch (SocketException)
            {
                cliente.Close();
                s.Close();
            }
            finally
            {
                sw.Close();
                sr.Close();
                ns.Close();

                cliente.Close();
            }
        }


        static void Main(string[] args)
        {
            Program imp = new Program();
            imp.iniciaServidorImpresora();
            Console.ReadLine();

        }
    }
}
