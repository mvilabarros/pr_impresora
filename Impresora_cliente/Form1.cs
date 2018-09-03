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
        //TODO pingForm

        string archivo, nombreArchivo;
        static string ip = "127.0.0.1";
        static int puerto = 31416;
        static int hojas = 100; //hojas documento
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
                    sw.WriteLine(nombreArchivo);
                    sw.Flush();
                    servidor.SendFile(archivo);
                    lbArchivo.Text += "Enviando archivo...";
                }
                servidor.Close();
            }
            catch (SocketException ex)
            {
                lbArchivo.Text = String.Format("Error de conexión: {0}" + Environment.NewLine + "Código de error: {1}({2})", ex.Message, (SocketError)ex.ErrorCode, ex.ErrorCode);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conexion(ip, puerto);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //label1.Text = "Conectado a: ";
            for (int i = 1; i <= hojas; i++)
            {
                string[] num = { i.ToString() };
                cbCopias.Items.AddRange(num);
                cbCopias.SelectedIndex = 0;
            }
        }

        private void btnPing_Click(object sender, EventArgs e)
        {
            //lblNombre
            //lblEstado
            /*
             * if ping correcto
             *      lblNombre.Text ++
             *      lblEstado.Text ++
             *  else
             *      lbl no hay datos disponibles
             */
        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt (*.txt)|*.txt|Todos (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            //getExtension
                            nombreArchivo = Path.GetFileName(openFileDialog1.FileName);
                            archivo = Path.GetFullPath(openFileDialog1.FileName);
                            lbArchivo.Text = archivo;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Se ha producido un error con el archivo. Error: " + ex.Message);
                }
            }

        }
    }
}
