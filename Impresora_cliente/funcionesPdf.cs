using System;
using System.IO;
using Microsoft.Office.Interop.Word;
using iTextSharp.text.pdf;

namespace Impresora_cliente
{
    //TODO arreglar wordPDF

    class funcionesPdf
    {
        /// <summary>
        /// Método get y sed de un Document Word
        /// </summary>
        private Microsoft.Office.Interop.Word.Document wordDocument { get; set; }

        /// <summary>
        /// Método que dado una ruta de un archivo, nombre y ruta de la carpeta caché convierte un archivo a pdf.
        /// Necesita Office para convertir los archivos.
        /// </summary>
        /// <param name="archivoPath"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="path"></param>
        /// <returns></returns>        
        public string wordPdf(string archivoPath, string nombreArchivo, string path)
        {
            {
                Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
                string ruta = path + "\\" + nombreArchivo + ".pdf";
                wordDocument = appWord.Documents.Open(archivoPath);
                wordDocument.ExportAsFixedFormat(ruta , WdExportFormat.wdExportFormatPDF);
                wordDocument.Close();
                appWord.Quit();
                return ruta;
            }
        }

        /// <summary>
        /// Método que dado un archivo entradaPdf y una carpeta, recorta el archivo dependiendo de la expresión usada y
        /// lo guarda de nuevo con el nombre modificado.
        /// Devuelve un string con la ruta del archivo cortado.
        /// </summary>
        /// <param name="entradaPdf"></param>
        /// <param name="salidaPdf"></param>
        /// <param name="carpeta"></param>
        /// <param name="paginaSelec"></param>
        /// <returns></returns>
        public string cortarPDF(string entradaPdf, string salidaPdf, string carpeta, string paginaSelec)
        {
            string inputPdf = entradaPdf;
            string outputPath = carpeta; //Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string outputPdf = Path.GetFileNameWithoutExtension(entradaPdf) + "_cortado" + Path.GetExtension(entradaPdf);
            //string pageSelection = "1-3,!2";
            using (PdfReader reader = new PdfReader(inputPdf))
            {
                reader.SelectPages(paginaSelec);

                using (PdfStamper stamper = new PdfStamper(reader, File.Create(outputPath + "\\" + outputPdf)))
                {
                    stamper.Close();
                }
            }
            return outputPath + "\\" + outputPdf;
        }

        /// <summary>
        /// Método que muestra el número de páginas de un documento PDF.
        /// Devuelve un int con el número de páginas.
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        public int rangoPdf(string archivo)
        {
            var pdfDocument = PdfiumViewer.PdfDocument.Load(archivo);
            return pdfDocument.PageCount;
        }

        //NO SE USA -> El servidor ya ejecuta n veces un PDF en las funciones de impresión.
        /// <summary>
        /// Método que repite el documento n veces y lo añade al original. 
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="destino"></param>
        /// <param name="repetir"></param>
        public void repetirPdf(string origen, string destino, int repetir)
        {
            for (int i = 0; i < repetir; i++)
            {
                var sourceDocumentStream = new FileStream(origen, FileMode.Open);
                var destinationDocumentStream = new FileStream(destino, FileMode.Create);
                var pdfConcat = new PdfConcatenate(destinationDocumentStream);

                var pdfReader = new PdfReader(sourceDocumentStream);
                pdfConcat.AddPages(pdfReader);

                pdfReader.Close();
                pdfConcat.Close();
            }
        }

        /// <summary>
        /// Método que comprueba si la ruta de un archivo existe. 
        /// Devuelve true si existe.
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        public bool compruebaArchivo(string archivo)
        {
            if (File.Exists(Path.GetFullPath(archivo)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
