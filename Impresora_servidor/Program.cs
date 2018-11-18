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
        static bool conectar = false, apagar = false;
        Socket s = null;
        static int sistemaValido = 0;
        static System.Threading.Timer timer;
        static string estadoActualImpresora;

        /// <summary>
        /// Método que busca en el sistema que se ejecuta, una impresora instalada,
        /// muestra si está encendida y guarda el valor en estadoImpresora.
        /// </summary>
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

        /// <summary>
        /// Método que usa el comando "print" de Windows para imprimir un archivo.
        /// Duplex usa varios valores: horizontal (dos lados horizontal), simplex (un lado), vertical (dos lados vertical), default.
        /// Copias es el número de veces que se va a imprimir el archivo.
        /// </summary>
        /// <param name="impresora"></param>
        /// <param name="archivo"></param>
        /// <param name="copias"></param>
        /// <param name="duplex"></param>
        /// <returns></returns>
        public bool imprime(string impresora, string archivo, short copias, Duplex duplex)
        {
            try
            {
                PrinterSettings opciones = new PrinterSettings();
                opciones.PrinterName = nombreImpresora;
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

        /// <summary>
        /// Método que usa la librería PdfiumViewer para imprimir PDF sin Adobe.
        /// Duplex usa varios valores: horizontal (dos lados horizontal), simplex (un lado), vertical (dos lados vertical), default.
        /// Copias es el número de veces que se va a imprimir el archivo.
        /// </summary>
        /// <param name="impresora"></param>
        /// <param name="archivo"></param>
        /// <param name="copias"></param>
        /// <param name="duplex"></param>
        /// <returns></returns>
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

                // Imprimir PDF
                using (var document = PdfDocument.Load(archivo))
                {
                    using (var printDocument = document.CreatePrintDocument())
                    {
                        printDocument.PrinterSettings = printerSettings;
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

        //Funciona, pero dependiendo de la impresora sólo muestra que está imprimiendo aunque la impresora no tenga papel,
        //no tenga tinta, etc. porque ya lo muestra con los drivers propios en vez de mostrarlos en la cola de impresión.
        /// <summary>
        /// Método que accede a la cola de impresión de Windows y recoge un trabajo que envía a Trabajos()
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Método que dado un trabajo devuelve un estado de la impresora actual.
        /// </summary>
        /// <param name="trabajo"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Método que crea un socket para recibir clientes y llama a hiloCliente cuando uno se conecta.
        /// </summary>
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

        /// <summary>
        /// Método que usando un socket, recibe parámetros "ping" e "imprimir"
        /// ping recoge el valor del nombre de la impresora y el estado de la impresora que detectarImpresora recoge previamente.
        /// imprimir guarda un archivo en la carpeta caché y varios parámetros que se usan en los método de impresión.
        /// A mayores muestra el estado de la impresión cada 5 segundos.
        /// </summary>
        /// <param name="socket"></param>
        public void hiloCliente(object socket)
        {
            Socket cliente = (Socket)socket;
            IPEndPoint ieCliente = (IPEndPoint)cliente.RemoteEndPoint;
            Console.WriteLine("Conectado con el cliente {0} en el puerto {1}", ieCliente.Address, ieCliente.Port);
            NetworkStream ns = new NetworkStream(cliente);
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            string mensaje, numCopias, valorDuplex, archivo;
            Duplex aux = Duplex.Default;

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
                    if (!comprobarArchivo(mensaje)) //No permite imprimir archivos con mismo nombre.
                    {
                        Console.WriteLine("Server: " + docImpresora + "//" + mensaje);
                        using (var output = File.Create(docImpresora + mensaje))
                        {
                            //1KB
                            var buffer = new byte[1024];
                            int bytesRead;
                            while ((bytesRead = ns.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                output.Write(buffer, 0, bytesRead);
                            }
                            archivo = docImpresora + "//" + mensaje;
                        }
                        //Impresión de archivo.
                        if (sistemaValido == 1)
                        {
                            numCopias = sr.ReadLine();
                            valorDuplex = sr.ReadLine();
                            Console.WriteLine(valorDuplex);
                            if (Convert.ToBoolean(valorDuplex))
                            {
                                aux = Duplex.Horizontal;
                            }
                            else
                            {
                                aux = Duplex.Simplex;
                            }

                            //1 = lib pdf
                            imprimePDF(nombreImpresora, archivo, Convert.ToInt32(numCopias), aux);

                        }
                        else
                        {
                            numCopias = sr.ReadLine();
                            valorDuplex = sr.ReadLine();
                            if (Convert.ToBoolean(valorDuplex))
                            {
                                aux = Duplex.Horizontal;
                            }
                            else
                            {
                                aux = Duplex.Simplex;
                            }

                            //0 = lib c#
                            imprime(nombreImpresora, archivo, Convert.ToInt16(numCopias), aux);
                        }

                        var startTimeSpan = TimeSpan.Zero;
                        var periodTimeSpan = TimeSpan.FromSeconds(5);
                        var timer = new System.Threading.Timer((e) =>
                        {
                            impCheck();
                        }, null, startTimeSpan, periodTimeSpan);
                    }
                    else
                    {
                        Console.Write("Archivo ya existe. Por favor, cambia el nombre del archivo.");
                    }
                }
            }
            catch (IOException e)
            {
                //Console.WriteLine("Server: se ha producido un error con el archivo. Error: " + e.Message);
            }
            catch (ObjectDisposedException) { }
            catch (UnauthorizedAccessException) { }
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


        /// <summary>
        /// Método que comprueba la versión actual del sistema operativo que ejecuta el servidor.
        /// Devuelve 1 si es un sistema válido para el uso de otros métodos.
        /// 1 librería pdf, 0 librería compatible.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Método que crea la carpeta caché donde guardar los archivos recibidos.
        /// Borra la carpeta si hay una existente y la crea de nuevo vacía.
        /// </summary>
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

        /// <summary>
        /// Método que comprueba si un archivo existe.
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Método que inicializa la carpeta caché, recoge el valor del sistema para usar un método de impresión u otro,
        /// detecta el estado de la impresora y el servidor.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Program imp = new Program();
            imp.carpetaDoc();
            sistemaValido = imp.infOS();
            imp.iniciaServidorImpresora();
           
            Console.ReadLine();
        }
    }
}
