using System;
using System.IO;
using Microsoft.Office.Interop.Word;
using iTextSharp.text.pdf;

namespace Impresora_cliente
{


    class funcionesPdf
    {
        private Microsoft.Office.Interop.Word.Document wordDocument { get; set; }

        private void wordPdf(string entradaArchivo, string salidaArchivo)
        {
            //necesario Office para convertir archivos!            
            {
                string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string file = "C:\\Users\\Mario\\Downloads\\borrar.docx";

                Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
                wordDocument = appWord.Documents.Open(@file);
                wordDocument.ExportAsFixedFormat(escritorio + "/" + "mop.pdf", WdExportFormat.wdExportFormatPDF);
                wordDocument.Close();
                appWord.Quit();
            }
            //TODO verificar archivo existe, si existe no hay error ni se crea de nuevo.
        }

        private void cortarPDF(string entradaPdf, string salidaPdf)
        {
            string inputPdf = @"C:\Users\Mario\Downloads\asd.pdf";
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

    }
}
