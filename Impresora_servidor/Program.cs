using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Impresora_servidor
{
    //TODO detectar sistema operativo
    //TODO detectar impresora, enviar estado impresora a cliente
    //TODO con: puertos, enviar archivo stream - borrar archivos enviados

    class Program
    {
        string carpeta = Path.GetTempPath();
        string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string archivo;
        List<String> archivos; //lista archivos guardados para borrar
        static bool conectar = false, apagar = false;
        Socket s = null;

        public void detectarImpresora()
        {
            //managementScope
            ManagementScope scope = new ManagementScope(@"\root\cimv2");
            scope.Connect();

            //seleccionar impresoras
            ManagementObjectSearcher buscar = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

            string nombreImpresora = "";
            if (buscar.Get().Count == 0)
            {
                Console.WriteLine("No hay impresora conectada");
            }
            else
            {
                foreach (ManagementObject impresora in buscar.Get())
                {
                    nombreImpresora = impresora["Name"].ToString().ToLower();
                    Console.WriteLine(nombreImpresora);
                    if (impresora["WorkOffline"].ToString().ToLower().Equals("true"))
                    {
                        Console.WriteLine("Impresora offline." + nombreImpresora);
                    }
                    else
                    {
                        Console.WriteLine("Impresora online." + nombreImpresora);
                    }
                }
            }
        }


        public bool imprime(string archivo)
        {
            try
            {
                Process p = new Process();
                // p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.FileName = archivo;
                p.StartInfo.Verb = "print";
                p.Start();
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                if (p.HasExited == false)
                {
                    p.WaitForExit(10000);
                }
                p.EnableRaisingEvents = true;
                //p.CloseMainWindow();
                p.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool imprimePDF(string impresora, string papel, string archivo, int copias)
        {
            try
            {
                // Propiedades impresora
                var printerSettings = new PrinterSettings
                {
                    PrinterName = impresora,
                    Copies = (short)copias,
                };

                // Propiedades página, tamaño página
                var pageSettings = new PageSettings(printerSettings)
                {
                    Margins = new Margins(0, 0, 0, 0),
                };
                foreach (PaperSize paperSize in printerSettings.PaperSizes)
                {
                    if (paperSize.PaperName == papel)
                    {
                        pageSettings.PaperSize = paperSize;
                        break;
                    }
                }

                // Imprimir PDF
                using (var document = PdfDocument.Load(archivo))
                {
                    using (var printDocument = document.CreatePrintDocument())
                    {
                        printDocument.PrinterSettings = printerSettings;
                        printDocument.DefaultPageSettings = pageSettings;
                        printDocument.PrintController = new StandardPrintController();
                        printDocument.Print();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }


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

                    //imprimePDF(printerG, "PaperKind.A4", archivo, 1);

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
                Console.WriteLine("Cliente " + ieCliente.Address + "enviando documento: " + mensaje);
                Console.WriteLine(carpeta + mensaje);

                using (var output = File.Create(carpeta + mensaje)) //TODO stream, UnauthorizedAccessException
                {
                    //1KB
                    var buffer = new byte[1024];
                    int bytesRead;
                    while ((bytesRead = ns.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        output.Write(buffer, 0, bytesRead);
                    }
                }
                Console.Write(carpeta);

            }
            catch (IOException e)
            {
                Console.WriteLine("Se ha producido un error con el archivo. Error: " + e.Message);

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
