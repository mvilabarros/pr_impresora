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
                switch (Path.GetExtension(entradaArchivo))
                {
                    //Word
                    case ".doc":
                    case ".docx":
                        Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
                        wordDocument = appWord.Documents.Open(@file);
                        wordDocument.ExportAsFixedFormat(escritorio + "/" + "mop.pdf", WdExportFormat.wdExportFormatPDF);
                        wordDocument.Close();
                        appWord.Quit();
                        break;
                    //Excel
                    case ".xls":
                    case ".xlsx":
                        break;
                    //PowerPoint
                    case ".ppt":
                    case ".pptx":
                        break;
                    //Access
                    case ".accdr":
                    case ".accdt":
                        break;
                }
              
            }
            //TODO verificar archivo existe, si existe no hay error ni se crea de nuevo.
        }

        public string cortarPDF(string entradaPdf, string salidaPdf, string carpeta, string paginaSelec)
        {
            string inputPdf = entradaPdf;
            string outputPath = carpeta; //Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string outputPdf = Path.GetFileNameWithoutExtension(entradaPdf)+ "_cortado" + Path.GetExtension(entradaPdf);
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

        public int rangoPdf(string archivo)
        {
            var pdfDocument = PdfiumViewer.PdfDocument.Load(archivo);
            return pdfDocument.PageCount; 
        }


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
