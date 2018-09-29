using System;
using System.IO;
using Microsoft.Office.Interop.Word;
using iTextSharp.text.pdf;

namespace Impresora_cliente
{
    class funcionesPdf
    {
        private Microsoft.Office.Interop.Word.Document wordDocument { get; set; }

        public void wordPdf(string entradaArchivo, string salidaArchivo)
        {
            //necesario Office para convertir archivos!            
            {
                string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string file = entradaArchivo;

                Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
                wordDocument = appWord.Documents.Open(@file);
                wordDocument.ExportAsFixedFormat(escritorio + "/" + "mop.pdf", WdExportFormat.wdExportFormatPDF);
                wordDocument.Close();
                appWord.Quit();
            }
            //TODO verificar archivo existe, si existe no hay error ni se crea de nuevo.
        }

        public void cortarPDF(string entradaPdf, string salidaPdf, string paginaSelec)
        {
            string inputPdf = entradaPdf;
            string outputPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string outputPdf = "nombre.pdf";
            string pageSelection = "1-3,!2";
            using (PdfReader reader = new PdfReader(inputPdf))
            {
                reader.SelectPages(pageSelection);

                using (PdfStamper stamper = new PdfStamper(reader, File.Create(outputPath + "/" + outputPdf)))
                {
                    stamper.Close();
                }
            }
        }

        public int rangoPdf()
        {


            return 0;
        }

    }
}
