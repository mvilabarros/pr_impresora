namespace Impresora_cliente
{
    partial class CargarPdf
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pdfRenderer1 = new PdfiumViewer.PdfRenderer();
            this.btnMas = new System.Windows.Forms.Button();
            this.btnMenos = new System.Windows.Forms.Button();
            this.txtPaginas = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // pdfRenderer1
            // 
            this.pdfRenderer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pdfRenderer1.Location = new System.Drawing.Point(16, 12);
            this.pdfRenderer1.Name = "pdfRenderer1";
            this.pdfRenderer1.Page = 0;
            this.pdfRenderer1.Rotation = PdfiumViewer.PdfRotation.Rotate0;
            this.pdfRenderer1.Size = new System.Drawing.Size(375, 424);
            this.pdfRenderer1.TabIndex = 0;
            this.pdfRenderer1.Text = "pdfRenderer1";
            this.pdfRenderer1.ZoomMode = PdfiumViewer.PdfViewerZoomMode.FitHeight;
            // 
            // btnMas
            // 
            this.btnMas.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnMas.Location = new System.Drawing.Point(252, 451);
            this.btnMas.Name = "btnMas";
            this.btnMas.Size = new System.Drawing.Size(42, 23);
            this.btnMas.TabIndex = 1;
            this.btnMas.UseVisualStyleBackColor = true;
            this.btnMas.Click += new System.EventHandler(this.btnMas_Click);
            // 
            // btnMenos
            // 
            this.btnMenos.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnMenos.Location = new System.Drawing.Point(104, 454);
            this.btnMenos.Name = "btnMenos";
            this.btnMenos.Size = new System.Drawing.Size(42, 23);
            this.btnMenos.TabIndex = 2;
            this.btnMenos.UseVisualStyleBackColor = true;
            this.btnMenos.Click += new System.EventHandler(this.btnMenos_Click);
            // 
            // txtPaginas
            // 
            this.txtPaginas.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtPaginas.Location = new System.Drawing.Point(180, 454);
            this.txtPaginas.Name = "txtPaginas";
            this.txtPaginas.ReadOnly = true;
            this.txtPaginas.Size = new System.Drawing.Size(36, 20);
            this.txtPaginas.TabIndex = 3;
            // 
            // CargarPdf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 487);
            this.Controls.Add(this.txtPaginas);
            this.Controls.Add(this.btnMenos);
            this.Controls.Add(this.btnMas);
            this.Controls.Add(this.pdfRenderer1);
            this.Name = "CargarPdf";
            this.Text = "CargarPdf";
            this.Load += new System.EventHandler(this.CargarPdf_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PdfiumViewer.PdfRenderer pdfRenderer1;
        private System.Windows.Forms.Button btnMas;
        private System.Windows.Forms.Button btnMenos;
        private System.Windows.Forms.TextBox txtPaginas;
    }
}