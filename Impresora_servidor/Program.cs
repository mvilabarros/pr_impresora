﻿using PdfiumViewer;
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

namespace Impresora_servidor
{
    //TODO detectar sistema operativo -- rev
    //TODO detectar impresora, enviar estado impresora a cliente
    //TODO con: enviar archivo stream - borrar archivos enviados -> lista archivos
    //TODO imprimir con/sin color, intercalar, repetir X páginas
    
    class Program
    {
        string carpeta = Path.GetTempPath();
        string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string archivo;
        int puerto = 31416;
        IPEndPoint ie;
        bool conectado = false;
        StreamReader streamToPrint;
        private Font printFont;
        string nombreImpresora = "asd";
        string estadoImpresora = "";


        List<String> archivos; //guardar documentos en temp, borrar al cerrar servidor 
        static bool conectar = false, apagar = false;
        Socket s = null;

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
                foreach (ManagementObject impresora in buscar.Get())
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

   


        public bool imprime(string archivo)//añadir impresora, copias, duplex
        {
         
            try
            {
                PrinterSettings opciones = new PrinterSettings();
                opciones.PrinterName = nombreImpresora; //
                opciones.Duplex = Duplex.Default;
                opciones.Copies = 1;
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

        public bool imprimePDF(string impresora, string papel, string archivo, int copias, Duplex duplex)
        {
            try
            {

                /*
                Default	   -1	

                Horizontal	3	dos lados horinzontal

                Simplex	    1	un lado

                Vertical	2	dos lados vertical
                 */

                // Propiedades impresora
                var printerSettings = new PrinterSettings
                {
                    PrinterName = impresora,
                    Copies = (short)copias,
                    Duplex = duplex
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
                //comprobar SO 
                //imprimePDF(printerG, "PaperKind.A4", archivo, 1);

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
                if (mensaje == "ping")
                {
                    Console.WriteLine("Enviando datos al cliente " + ieCliente.Address + " " + nombreImpresora);
                    sw.WriteLine(nombreImpresora);
                    sw.WriteLine(estadoImpresora);
                    sw.Flush();
                }
                else
                {
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
               // cliente.Close();
               // s.Close();
            }
            finally
            {
                sw.Close();
                sr.Close();
                ns.Close();
                cliente.Close();
            }
        }

        //impresora sólo devuelve estado cuando imprime
        public void estado()
        {

            // Online               0

            // Lid Open        4194432

            // Out of paper      144

            // Out of paper/Lid open 4194448

            // Printing             1024

            // Initializing          32768

            // Manual Feed in Progress 160

            // Offline                 4096
            /*
             * foreach estado in impresora
             * switch estado
             * case 0
             *  console -> online
             * case 144
             *  console -> papel
             * 
             */ 
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




        static void Main(string[] args)
        {
            Program imp = new Program();
            imp.iniciaServidorImpresora();
            Console.ReadLine();
        }
    }
}
