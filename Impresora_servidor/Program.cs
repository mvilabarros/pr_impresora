using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Configuration;
using System.Printing;

namespace Impresora_servidor
{
    //TODO detectar sistema operativo -- rev
    //TODO detectar impresora, enviar estado impresora a cliente
    //TODO con: enviar archivo stream - borrar archivos enviados -> lista archivos
    //TODO imprimir con/sin color, intercalar, repetir X páginas

    class Program
    {
        int puerto = 31416;
        IPEndPoint ie;
        bool conectado = false;
        string nombreImpresora = "";
        string estadoImpresora = "";
        //
        static string dataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        static string docImpresora = Path.Combine(dataDir, "docImpresora");
        //
        static bool conectar = false, apagar = false;
        Socket s = null;
        //
        static int sistemaValido = 0;
        //
        static System.Threading.Timer timer;
        //
        static string estadoActualImpresora;
        //
        public void detectarImpresora()
        {
            //managementScope
            ManagementScope scope = new ManagementScope(@"\root\cimv2");
            scope.Connect();

            //seleccionar impresoras
            ManagementObjectSearcher buscar = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

            nombreImpresora = "";
            if (buscar.Get().Count == 0)
            {
                Console.WriteLine("No hay impresora conectada");
            }
            else
            {
                foreach (ManagementBaseObject impresora in buscar.Get())
                {
                    nombreImpresora = impresora["Name"].ToString().ToLower();
                    Console.WriteLine(nombreImpresora);
                    if (impresora["WorkOffline"].ToString().ToLower().Equals("true"))
                    {
                        Console.WriteLine("Impresora offline: " + nombreImpresora);
                        estadoImpresora = "Offline";
                    }
                    else
                    {
                        Console.WriteLine("Impresora online: " + nombreImpresora);
                        estadoImpresora = "Online";
                    }
                }
            }
        }

        /*
        Default	   -1	

        Horizontal	3	dos lados horinzontal

        Simplex	    1	un lado

        Vertical	2	dos lados vertical
         */

        public bool imprime(string impresora, string archivo, short copias, Duplex duplex)
        {
            try
            {
                PrinterSettings opciones = new PrinterSettings();
                opciones.PrinterName = nombreImpresora; //
                opciones.Duplex = duplex;
                opciones.Copies = copias;
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
            catch (Exception)
            {
                return false;
            }
        }

        public bool imprimePDF(string impresora, string archivo, int copias, Duplex duplex)
        {
            try
            {
                // Propiedades impresora
                var printerSettings = new PrinterSettings
                {
                    PrinterName = impresora,
                    Copies = (short)copias,
                    Duplex = duplex
                };

                // Propiedades página, tamaño página
                /*
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
                */

                // Imprimir PDF
                using (var document = PdfDocument.Load(archivo))
                {
                    using (var printDocument = document.CreatePrintDocument())
                    {
                        printDocument.PrinterSettings = printerSettings;
                        //printDocument.DefaultPageSettings = pageSettings;
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

        public string impCheck()
        {
            estadoActualImpresora = "";
            LocalPrintServer printServer = new LocalPrintServer();
            PrintQueueCollection printQueues = printServer.GetPrintQueues();
            foreach (PrintQueue pq in printQueues)
            {
                pq.Refresh();
                PrintJobInfoCollection pCollection = pq.GetPrintJobInfoCollection();
                foreach (PrintSystemJobInfo trabajo in pCollection)
                {
                    trabajo.Refresh();
                    if (Trabajos(trabajo))
                    {
                        timer.Change(Timeout.Infinite, Timeout.Infinite);
                    }
                }
            }
            return estadoActualImpresora;
        }

        private bool Trabajos(PrintSystemJobInfo trabajo)
        {
            if (((trabajo.JobStatus & PrintJobStatus.Completed) == PrintJobStatus.Completed)
                ||
                ((trabajo.JobStatus & PrintJobStatus.Printed) == PrintJobStatus.Printed))
            {
                Console.WriteLine("Trabajo terminado.");
                estadoActualImpresora = "Trabajo terminado.";

                return true;
            }
            else if (((trabajo.JobStatus & PrintJobStatus.Deleted) == PrintJobStatus.Deleted)
                 ||
                ((trabajo.JobStatus & PrintJobStatus.Deleting) == PrintJobStatus.Deleting))
            {
                Console.WriteLine("Impresión borrada.");
                estadoActualImpresora = "Impresión borrada.";

                return true;
            }
            else if ((trabajo.JobStatus & PrintJobStatus.Error) == PrintJobStatus.Error)
            {
                Console.WriteLine("Error en la impresión.");
                estadoActualImpresora = "Error en la impresión.";

                return true;
            }
            else if ((trabajo.JobStatus & PrintJobStatus.Offline) == PrintJobStatus.Offline)
            {
                Console.WriteLine("Impresora offline");
                estadoActualImpresora = "Impresora offline.";

                return true;
            }
            else if ((trabajo.JobStatus & PrintJobStatus.PaperOut) == PrintJobStatus.PaperOut)
            {
                Console.WriteLine("Falta papel.");
                estadoActualImpresora = "Falta papel.";

                return false;
            }
            else if ((trabajo.JobStatus & PrintJobStatus.Printing) == PrintJobStatus.Printing)
            {
                Console.WriteLine("Imprimiendo documento.");
                estadoActualImpresora = "Imprimiendo documento.";

                return false;
            }

            else if ((trabajo.JobStatus & PrintJobStatus.UserIntervention) == PrintJobStatus.UserIntervention)
            {
                Console.WriteLine("La impresora necesita intervención.");
                estadoActualImpresora = "La impresora necesita intervención.";

                return false;
            }
            else
            {
                Console.WriteLine("~");
                estadoActualImpresora = ".";

                return true;
            }
        }



        //TODO limite puerto 0 inv 1 val
        public void iniciaServidorImpresora()
        {
            while (!conectado)
            {
                try
                {
                    ie = new IPEndPoint(IPAddress.Any, puerto);
                    s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    s.Bind(ie);
                    conectar = true;
                    break;
                }
                catch (SocketException)
                {
                    conectado = false;
                }
                if (!conectado)
                {
                    Console.WriteLine("Puerto " + puerto + " ocupado. Probando siguiente puerto.");
                    puerto++;
                }
            }
            s.Listen(5);
            Console.WriteLine("Usando puerto: " + puerto);
            Console.WriteLine("A la espera");
            detectarImpresora();
            while (conectar)
            {
                Socket cliente = s.Accept();
                Thread hilo = new Thread(hiloCliente);
                hilo.IsBackground = true;
                hilo.Start(cliente);
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
                Console.WriteLine("Server: " + mensaje);
                if (mensaje == "ping")
                {
                    Console.WriteLine("Enviando datos al cliente " + ieCliente.Address + " " + nombreImpresora);
                    sw.WriteLine(nombreImpresora);
                    sw.WriteLine(estadoImpresora);
                    sw.Flush();
                }
                else
                {
                    Console.WriteLine("Server: " + mensaje);
                    Console.WriteLine("Server: cliente " + ieCliente.Address + "enviando documento: " + mensaje);
                    if (!comprobarArchivo(mensaje)) //borrar si archivo existe y permitir añadir nuevo
                    {
                        //TODO 
                        Console.WriteLine("Server: " + docImpresora + "//" + mensaje);
                        using (var output = File.Create(docImpresora + mensaje)) //TODO stream, UnauthorizedAccessException
                        {
                            //1KB
                            var buffer = new byte[1024];
                            int bytesRead;
                            while ((bytesRead = ns.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                output.Write(buffer, 0, bytesRead);
                            }
                        }
                        //




                        if (sistemaValido == 1)
                        {
                            //   numCopias = sr.ReadLine();
                            // valorDuplex = sr.ReadLine();
                            /*
                            if (valorDuplex)
                            {
                                valorDuplex = Duplex.Default;
                            }
                            else
                            {
                                valorDuplex = Duplex.Simplex;
                            }
                            */
                            //Console.WriteLine(mensaje + " " + numCopias + " " + valorDuplex + " ");

                            //1 = lib pdf
                            //imprimePDF(nombreImpresora, "PaperKind.A4", archivo, 1, Duplex.Simplex);

                        }
                        else
                        {
                            /*
                             * numCopias = sr.ReadLine();
                             * valorDuplex = sr.ReadLine();
                            if (valorDuplex)
                            {
                                valorDuplex = Duplex.Default;
                            }
                            else
                            {
                                valorDuplex = Duplex.Simplex;
                            }
                            */
                            //Console.WriteLine(mensaje + " " + numCopias + " " + valorDuplex + " ");
                            //0 = lib c#
                            //imprime(nombreImpresora, archivo, 1, Duplex.Simplex);
                        }

                        var startTimeSpan = TimeSpan.Zero;
                        var periodTimeSpan = TimeSpan.FromSeconds(4);
                        //string var = "";
                        var timer = new System.Threading.Timer((e) =>
                        {
                            //var = impCheck();
                            sw.WriteLine(impCheck());
                        }, null, startTimeSpan, periodTimeSpan);
                    }
                    else
                    {
                        Console.Write("Archivo ya existe!");
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Server: se ha producido un error con el archivo. Error: " + e.Message);
            }
            catch (ObjectDisposedException) { }
            catch (SocketException)
            {
                // cliente.Close();
                // s.Close();
            }
            finally
            {
                sw.Close();
                sr.Close();
                ns.Close();
                cliente.Shutdown(SocketShutdown.Both);
                cliente.Close();
                Console.WriteLine("Conexión cerrada");
            }
        }


        //Return 1 usa lib PDF, 0 C# lib 
        private int infOS()
        {
            OperatingSystem os = Environment.OSVersion;
            Version vs = os.Version;

            int valor = 0;

            if (os.Platform == PlatformID.Win32Windows)
            {
                switch (vs.Minor)
                {
                    case 0:
                        valor = 0;
                        break;
                    case 10:
                        if (vs.Revision.ToString() == "2222A")
                            valor = 0;
                        else
                            valor = 0;
                        break;
                    case 90:
                        valor = 0;
                        break;
                    default:
                        break;
                }
            }
            else if (os.Platform == PlatformID.Win32NT)
            {
                switch (vs.Major)
                {
                    case 3:
                        valor = 0;
                        break;
                    case 4:
                        valor = 0;
                        break;
                    case 5:
                        if (vs.Minor == 0)
                            valor = 0;
                        else
                            valor = 0;
                        break;
                    case 6:
                        if (vs.Minor == 0)
                            valor = 1;
                        else if (vs.Minor == 1)
                            valor = 1; //W7
                        else if (vs.Minor == 2)
                            valor = 1;
                        else
                            valor = 1;
                        break;
                    case 10:
                        valor = 1;
                        break;
                    default:
                        break;
                }
            }
            return valor;
        }

        private void carpetaDoc()
        {
            if (Directory.Exists(docImpresora))
            {
                //Borra datos previos 
                Directory.Delete(docImpresora, true);
                Directory.CreateDirectory(docImpresora);
            }
            else
            {
                Directory.CreateDirectory(docImpresora);
            }
        }

        private bool comprobarArchivo(string archivo)
        {
            if (File.Exists(docImpresora + "//" + archivo))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void Main(string[] args)
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(4);
            Program imp = new Program();
            imp.carpetaDoc();
            sistemaValido = imp.infOS();
            //imp.iniciaServidorImpresora();
            imp.detectarImpresora();
            Console.WriteLine(imp.imprimePDF(imp.nombreImpresora, "C:\\Users\\Mario\\Desktop\\prueba.pdf", 2, Duplex.Horizontal));
            /*
            bool var = false;
            if (!var)
            {
                timer = new System.Threading.Timer((e) =>
                {
                    imp.impCheck();
                }, null, startTimeSpan, periodTimeSpan);
            }
            */
            Console.WriteLine("Sale");
            Console.ReadLine();
        }
    }
}
