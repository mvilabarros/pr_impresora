using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Impresora_cliente
{
    public partial class CargarPdf : Form
    {
        int paginas = 0;

        public CargarPdf()
        {
            InitializeComponent();
        }
    
        /// <summary>
        /// Método que carga en el formulario los botones y llama a MostrarPdf().
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CargarPdf_Load(object sender, EventArgs e)
        {
            btnMas.Text = char.ConvertFromUtf32(0x2192);
            btnMenos.Text = char.ConvertFromUtf32(0x2190);
            this.MinimumSize = new Size(420, 525);
            txtPaginas.Text += (paginas +1).ToString();
            MostrarPdf(ruta);
        }

        /// <summary>
        /// Método que dada una ruta muestra un archivo PDF en el formulario.
        /// </summary>
        /// <param name="valor"></param>
        private void MostrarPdf(string valor)
        {
            // Crear PDF            
            var pdfDocument = PdfiumViewer.PdfDocument.Load(valor);
            paginas = pdfDocument.PageCount;
            // Cargar PDF
            pdfRenderer1.Load(pdfDocument);
        }

        /// <summary>
        /// Método get y set de string ruta
        /// </summary>
        public string ruta { get;  set; }

        /// <summary>
        /// Método que retrocede una página en un documento PDF cargado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMenos_Click(object sender, EventArgs e)
        {
            if (pdfRenderer1.Page > 0)
            {
                pdfRenderer1.Page -= 1;
            }
            txtPaginas.Text = (pdfRenderer1.Page + 1).ToString();
        }

        /// <summary>
        /// Método que avanza una página en un documento PDF cargado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMas_Click(object sender, EventArgs e)
        {
            if (pdfRenderer1.Page <= paginas)
            {
                pdfRenderer1.Page += 1;
            }
            txtPaginas.Text = (pdfRenderer1.Page + 1).ToString();
        }
    }
}
