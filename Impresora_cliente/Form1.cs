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
    //TODO terminar acerca de
    
    public partial class Form1 : Form
    {
        //TODO convertir archivo a PDF -> aplicar opciones -> nuevo PDF -> enviar al servidor

        //comprobar impresora + comprobar archivo ->comprobar páginas -> cortar PDF -> mandar pdf nuevo a server + copias, intercalar
        //TODO verificar archivos + verificar si archivo es pdf o no -> wordpdf
        //TODO si kill -> borrar carpeta si existe y crearla de nuevo

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

        //FALLA -> Llamar al método conexion con el valor ping hará que los archivos enviados estén corruptos desde un principio.
        /// <summary>
        /// Método que inicializa elementos del formulario, así como la creación de una carpeta que servirá de caché.
        /// LLama al método conexión para verificar si el servidor está disponible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            Directory.CreateDirectory(documentosImpresora);
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

        //FALLA -> Este método funciona parcialmente. 
        //Por alguna razón al cargar funcionesPdf y sin modificar las rutas por los check.Checked, el archivo está corrupto.
        //Si se ejecuta antes conexion() funciona la primera vez, pero 
        /// <summary>
        /// Método que llama a las funcionesPdf para poder cortar el archivo por rango o por expresión.
        /// Llama a imprimir y le envía una ip, puerto y string imprimir.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            funcionesPdf pdf = new funcionesPdf();

            if (rbRango.Checked)
            {
                string txt = txtInicio.Text + "-" + txtFin.Text;
                if (txtInicio.Text != null && txtFin.Text != null)
                {
                    archivoCortado = pdf.cortarPDF(archivo, archivoCortado, documentosImpresora, txt);
                    nombreArchivo = Path.GetFileName(archivoCortado);
                    archivo = archivoCortado;
                }
            }

            if (rbSeleccion.Checked)
            {
                if (txtSeleccion.Text != null)
                {
                    archivoCortado = pdf.cortarPDF(archivo, archivoCortado, documentosImpresora, txtSeleccion.Text);
                    nombreArchivo = Path.GetFileName(archivoCortado);
                    archivo = archivoCortado;
                }
            }

            conexion(ip, puerto, "imprimir");
        }

        /// <summary>
        /// Método que muestra un modal para cambiar el puerto y la ip para conectarse al servidor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPing_Click(object sender, EventArgs e)
        {
            PingForm ping = new PingForm();
            DialogResult dia;
            dia = ping.ShowDialog();
            switch (dia)
            {
                case DialogResult.OK:
                    conexion(ping.txtIP.Text, Convert.ToInt32(ping.txtPuerto.Text), "ping");
                    break;
                case DialogResult.Cancel:
                    con = false;
                    break;
            }
        }

        /// <summary>
        /// Método que dado una ip, puerto y opcion se conecta al servidor.
        /// Si opcion ping envía "ping" al servidor y recibe el estado de este.
        /// Si opcion imprimir envía "imprimir", además de varios parámetros de impresión y un archivo.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="puerto"></param>
        /// <param name="opcion"></param>
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
                    //TODO arreglar
                    sw.WriteLine(nombreArchivo);
                    sw.Flush();
                    servidor.SendFile(archivo);
                    lbArchivo.Text += "Enviando archivo...";

                    if (opcion == "imprimir")
                    {
                        sw.WriteLine(nombreArchivo);
                        sw.Flush();
                        servidor.SendFile(archivo);
                        lbArchivo.Text += "Enviando archivo...";

                        sw.WriteLine(cbCopias.Text);
                        sw.Flush();
                        lbArchivo.Text += " Enviando copias: " + cbCopias.Text;

                        sw.WriteLine(checkIntercalado.Checked.ToString());
                        lbArchivo.Text += "Enviando Duplex: " + checkIntercalado.Checked.ToString();
                        sw.Flush();

                        Console.WriteLine("NOMBRE ARCHIVO:" + nombreArchivo + "~~" + archivo);
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
                lbArchivo.Text = String.Format("Error de conexión CLIENTE: {0}" + Environment.NewLine + "Código de error: {1}({2})", ex.Message, (SocketError)ex.ErrorCode, ex.ErrorCode);
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
                if(con)servidor.Shutdown(SocketShutdown.Both);
                servidor.Close();
            }
        }

        /// <summary>
        /// Método que muestra un modal con las librerías usadas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void acercadeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Método que abre un gestor de archivos para seleccionar un archivo pdf o txt.
        /// Guarda la ruta del archivo en una variable.
        /// LLama a funcionesPdf.rango para poder mostrar el rango máximo disponible en el formulario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnbuscar_Click(object sender, EventArgs e)
        {
            Stream str = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "pdf (*.pdf)|*.pdf|txt (*.txt)|*.txt|Todos (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
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
                            txtInicio.Text = "1";
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

        /// <summary>
        /// Método que llama al formulario CargarPdf para poder mostrar el archivo pdf.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Método que cierra el cliente y borra la carpeta de caché.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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