using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using iTextSharp.text.pdf;
using PdfiumViewer;

namespace Impresora_cliente
{
    public partial class Form1 : Form
    {
        //TODO convertir archivo a PDF -> aplicar opciones -> nuevo PDF -> enviar al servidor
        //comprobar impresora + comprobar archivo ->comprobar páginas -> cortar PDF -> mandar pdf nuevo a server + copias, intercalar
        //TODO verificar archivos


        static string ip = "127.0.0.1";
        static int puerto = 31416;

        static string dataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        static string documentosImpresora = Path.Combine(dataDir, "documentosImpresora");

        private string archivo, nombreArchivo, nombreImpresora, estadoImpresora;

        private string archivoCortado;

        static int hojas = 100;

        private object oMissing = System.Reflection.Missing.Value;

        private IPEndPoint ie;
        private Socket servidor;
        private NetworkStream ns;
        private StreamReader sr;
        private StreamWriter sw;

        private bool arch = false; //si archivo seleccionado = true
        private bool con = false; //si impresora conectada = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Directory.CreateDirectory(documentosImpresora);

            //funcionesPdf pdf = new funcionesPdf();

            btnPdf.Enabled = false;
            btnImprimir.Enabled = false;
            lblIntercalado.Enabled = false;

            conexion(ip, puerto, "ping");
            rbTodo.Checked = true;

            for (int i = 1; i <= hojas; i++)
            {
                string[] num = { i.ToString() };
                cbCopias.Items.AddRange(num);
                cbCopias.SelectedIndex = 0;
            }

            if (!con)
            {
                lblEstado.Text = "No conectado";
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e) //btnImprimir
        {
            funcionesPdf pdf = new funcionesPdf();
            //btnPing -> conexion("127.0.0.1", 31416, "ping");

            if (rbRango.Checked)
            {
                string txt = txtInicio.Text + "-" + txtFin.Text;
                if (txtInicio.Text != null && txtFin.Text != null)
                {
                    pdf.cortarPDF(archivo, archivoCortado, txt);
                    nombreArchivo = Path.GetFileName(archivoCortado);
                    archivo = Path.GetFullPath(archivoCortado);
                }
            }

            if (rbSeleccion.Checked)
            {
                if (txtSeleccion.Text != null)
                {
                    pdf.cortarPDF(archivo, archivoCortado, txtSeleccion.Text);
                    nombreArchivo = Path.GetFileName(archivoCortado);
                    archivo = Path.GetFullPath(archivoCortado);
                }

            }
           
            conexion(ip, puerto, "imprimir");
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
            PingForm ping = new PingForm();
            DialogResult dia;
            dia = ping.ShowDialog();
            switch (dia)
            {
                case DialogResult.OK:
                    conexion(ping.txtIP.Text, Convert.ToInt32(ping.txtPuerto.Text), "ping");
                    //conexion("127.0.0.1", 31416, "ping");
                    break;
                case DialogResult.Cancel:
                    con = false;
                    break;
            }

        }
        private void conexion(string ip, int puerto, string opcion)
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
                    if (opcion == "imprimir")
                    {
                        sw.WriteLine(nombreArchivo);
                        sw.Flush();
                        servidor.SendFile(archivo);
                        lbArchivo.Text += "Enviando archivo...";
                    }
                    else
                    {
                        sw.WriteLine("ping");
                        sw.Flush();
                        nombreImpresora = sr.ReadLine();
                        estadoImpresora = sr.ReadLine();
                        lblNombre.Text += nombreImpresora;
                        lblEstado.Text += estadoImpresora;
                        con = true;
                    }
                }
                servidor.Close();
            }
            catch (SocketException ex)
            {
                lbArchivo.Text = String.Format("Error de conexión: {0}" + Environment.NewLine + "Código de error: {1}({2})", ex.Message, (SocketError)ex.ErrorCode, ex.ErrorCode);
                con = false;
            }
            catch (IOException e)
            {
                lbArchivo.Text = String.Format(e.Message);
                con = false;
            }
            finally
            {
                if (sw != null) sw.Close();
                if (sr != null) sr.Close();
                if (ns != null) ns.Close();
            }
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            Stream str = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt (*.txt)|*.txt|Todos (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((str = openFileDialog1.OpenFile()) != null)
                    {
                        using (str)
                        {
                            //getExtension
                            nombreArchivo = Path.GetFileName(openFileDialog1.FileName);
                            archivo = Path.GetFullPath(openFileDialog1.FileName);
                            txtDocumento.Text = archivo;

                            arch = true;
                            btnPdf.Enabled = true;
                            btnImprimir.Enabled = true;
                            lblIntercalado.Enabled = true;
                            funcionesPdf pdf = new funcionesPdf();
                            txtInicio.Text = "0";
                            txtFin.Text = pdf.rangoPdf(archivo).ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Se ha producido un error con el archivo. Error: " + ex.Message);
                    arch = false;
                }
            }
        }

        private void btnPdf_Click(object sender, EventArgs e)
        {
            //cambiar texto form a nombre archivo 
            if (arch)
            {
                CargarPdf cargar = new CargarPdf();
                cargar.ruta = archivo;
                cargar.ShowDialog();
            }
            //else no hay archivo seleccionado
        }


        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(documentosImpresora))
            {
                Directory.Delete(documentosImpresora, true);
            }
            this.Close();
        }
    }
}