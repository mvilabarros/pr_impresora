using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Impresora_cliente
{
    public partial class Form1 : Form
    {
        static string ip = "127.0.0.1";
        static int puerto = 31416;
        IPEndPoint ie;
        Socket servidor;
        NetworkStream ns;
        StreamReader sr;
        StreamWriter sw;

        public Form1()
        {
            InitializeComponent();
        }

        private void conexion(string ip, int puerto)
        {
            try
            {
                ie = new IPEndPoint(IPAddress.Parse(ip), puerto);
                servidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                servidor.Connect(ie);
                ns = new NetworkStream(servidor);
                sr = new StreamReader(ns);
                sw = new StreamWriter(ns);

                if (servidor.Available == 0)
                {
                    label1.Text += "Enviado";
                }
                servidor.Close();
            }
            catch (SocketException ex)
            {
                label1.Text = String.Format("Error de conexión: {0}" + Environment.NewLine + "Código de error: {1}({2})", ex.Message, (SocketError)ex.ErrorCode, ex.ErrorCode);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conexion(ip, puerto);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //label1.Text = "Conectado a: ";
        }
    }
}
