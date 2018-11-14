namespace Impresora_cliente
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnImprimir = new System.Windows.Forms.Button();
            this.lbArchivo = new System.Windows.Forms.Label();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.txtDocumento = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.gbImpresora = new System.Windows.Forms.GroupBox();
            this.gbPaginas = new System.Windows.Forms.GroupBox();
            this.txtFin = new System.Windows.Forms.TextBox();
            this.txtInicio = new System.Windows.Forms.TextBox();
            this.lblFin = new System.Windows.Forms.Label();
            this.lblInicio = new System.Windows.Forms.Label();
            this.rbRango = new System.Windows.Forms.RadioButton();
            this.txtSeleccion = new System.Windows.Forms.TextBox();
            this.rbSeleccion = new System.Windows.Forms.RadioButton();
            this.rbTodo = new System.Windows.Forms.RadioButton();
            this.gbCopias = new System.Windows.Forms.GroupBox();
            this.checkIntercalado = new System.Windows.Forms.CheckBox();
            this.lblIntercalado = new System.Windows.Forms.Label();
            this.lblCopias = new System.Windows.Forms.Label();
            this.cbCopias = new System.Windows.Forms.ComboBox();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.herramientasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opcionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ayudaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acercadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPdf = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gbImpresora.SuspendLayout();
            this.gbPaginas.SuspendLayout();
            this.gbCopias.SuspendLayout();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnImprimir
            // 
            this.btnImprimir.Location = new System.Drawing.Point(361, 372);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(75, 23);
            this.btnImprimir.TabIndex = 0;
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.UseVisualStyleBackColor = true;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // lbArchivo
            // 
            this.lbArchivo.AutoSize = true;
            this.lbArchivo.Location = new System.Drawing.Point(12, 304);
            this.lbArchivo.Name = "lbArchivo";
            this.lbArchivo.Size = new System.Drawing.Size(137, 13);
            this.lbArchivo.TabIndex = 1;
            this.lbArchivo.Text = "Selecciona un documento: ";
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(12, 372);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(75, 23);
            this.btnBuscar.TabIndex = 2;
            this.btnBuscar.Text = "Seleccionar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnbuscar_Click);
            // 
            // txtDocumento
            // 
            this.txtDocumento.Location = new System.Drawing.Point(15, 332);
            this.txtDocumento.Name = "txtDocumento";
            this.txtDocumento.Size = new System.Drawing.Size(251, 20);
            this.txtDocumento.TabIndex = 3;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(6, 29);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(50, 13);
            this.lblNombre.TabIndex = 4;
            this.lblNombre.Text = "Nombre: ";
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(6, 66);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(46, 13);
            this.lblEstado.TabIndex = 7;
            this.lblEstado.Text = "Estado: ";
            // 
            // gbImpresora
            // 
            this.gbImpresora.Controls.Add(this.lblNombre);
            this.gbImpresora.Controls.Add(this.lblEstado);
            this.gbImpresora.Location = new System.Drawing.Point(12, 39);
            this.gbImpresora.Name = "gbImpresora";
            this.gbImpresora.Size = new System.Drawing.Size(430, 100);
            this.gbImpresora.TabIndex = 8;
            this.gbImpresora.TabStop = false;
            this.gbImpresora.Text = "Impresora";
            // 
            // gbPaginas
            // 
            this.gbPaginas.Controls.Add(this.txtFin);
            this.gbPaginas.Controls.Add(this.txtInicio);
            this.gbPaginas.Controls.Add(this.lblFin);
            this.gbPaginas.Controls.Add(this.lblInicio);
            this.gbPaginas.Controls.Add(this.rbRango);
            this.gbPaginas.Controls.Add(this.txtSeleccion);
            this.gbPaginas.Controls.Add(this.rbSeleccion);
            this.gbPaginas.Controls.Add(this.rbTodo);
            this.gbPaginas.Location = new System.Drawing.Point(12, 154);
            this.gbPaginas.Name = "gbPaginas";
            this.gbPaginas.Size = new System.Drawing.Size(254, 126);
            this.gbPaginas.TabIndex = 9;
            this.gbPaginas.TabStop = false;
            this.gbPaginas.Text = "Páginas";
            // 
            // txtFin
            // 
            this.txtFin.Location = new System.Drawing.Point(212, 54);
            this.txtFin.Name = "txtFin";
            this.txtFin.Size = new System.Drawing.Size(34, 20);
            this.txtFin.TabIndex = 7;
            // 
            // txtInicio
            // 
            this.txtInicio.Location = new System.Drawing.Point(120, 54);
            this.txtInicio.Name = "txtInicio";
            this.txtInicio.Size = new System.Drawing.Size(34, 20);
            this.txtInicio.TabIndex = 6;
            // 
            // lblFin
            // 
            this.lblFin.AutoSize = true;
            this.lblFin.Location = new System.Drawing.Point(167, 57);
            this.lblFin.Name = "lblFin";
            this.lblFin.Size = new System.Drawing.Size(39, 13);
            this.lblFin.TabIndex = 5;
            this.lblFin.Text = "hasta: ";
            // 
            // lblInicio
            // 
            this.lblInicio.AutoSize = true;
            this.lblInicio.Location = new System.Drawing.Point(72, 57);
            this.lblInicio.Name = "lblInicio";
            this.lblInicio.Size = new System.Drawing.Size(42, 13);
            this.lblInicio.TabIndex = 4;
            this.lblInicio.Text = "desde: ";
            // 
            // rbRango
            // 
            this.rbRango.AutoSize = true;
            this.rbRango.Location = new System.Drawing.Point(9, 55);
            this.rbRango.Name = "rbRango";
            this.rbRango.Size = new System.Drawing.Size(57, 17);
            this.rbRango.TabIndex = 3;
            this.rbRango.TabStop = true;
            this.rbRango.Text = "Rango";
            this.rbRango.UseVisualStyleBackColor = true;
            // 
            // txtSeleccion
            // 
            this.txtSeleccion.Location = new System.Drawing.Point(90, 92);
            this.txtSeleccion.Name = "txtSeleccion";
            this.txtSeleccion.Size = new System.Drawing.Size(156, 20);
            this.txtSeleccion.TabIndex = 2;
            this.toolTip1.SetToolTip(this.txtSeleccion, "Expresión 1-3, !2 imprime hojas 1 y 3");
            // 
            // rbSeleccion
            // 
            this.rbSeleccion.AutoSize = true;
            this.rbSeleccion.Location = new System.Drawing.Point(9, 92);
            this.rbSeleccion.Name = "rbSeleccion";
            this.rbSeleccion.Size = new System.Drawing.Size(75, 17);
            this.rbSeleccion.TabIndex = 1;
            this.rbSeleccion.TabStop = true;
            this.rbSeleccion.Text = "Selección:";
            this.rbSeleccion.UseVisualStyleBackColor = true;
            // 
            // rbTodo
            // 
            this.rbTodo.AutoSize = true;
            this.rbTodo.Location = new System.Drawing.Point(9, 19);
            this.rbTodo.Name = "rbTodo";
            this.rbTodo.Size = new System.Drawing.Size(50, 17);
            this.rbTodo.TabIndex = 0;
            this.rbTodo.TabStop = true;
            this.rbTodo.Text = "Todo";
            this.rbTodo.UseVisualStyleBackColor = true;
            // 
            // gbCopias
            // 
            this.gbCopias.Controls.Add(this.checkIntercalado);
            this.gbCopias.Controls.Add(this.lblIntercalado);
            this.gbCopias.Controls.Add(this.lblCopias);
            this.gbCopias.Controls.Add(this.cbCopias);
            this.gbCopias.Location = new System.Drawing.Point(284, 154);
            this.gbCopias.Name = "gbCopias";
            this.gbCopias.Size = new System.Drawing.Size(158, 126);
            this.gbCopias.TabIndex = 0;
            this.gbCopias.TabStop = false;
            // 
            // checkIntercalado
            // 
            this.checkIntercalado.AutoSize = true;
            this.checkIntercalado.Location = new System.Drawing.Point(20, 82);
            this.checkIntercalado.Name = "checkIntercalado";
            this.checkIntercalado.Size = new System.Drawing.Size(15, 14);
            this.checkIntercalado.TabIndex = 3;
            this.checkIntercalado.UseVisualStyleBackColor = true;
            // 
            // lblIntercalado
            // 
            this.lblIntercalado.AutoSize = true;
            this.lblIntercalado.Location = new System.Drawing.Point(40, 82);
            this.lblIntercalado.Name = "lblIntercalado";
            this.lblIntercalado.Size = new System.Drawing.Size(51, 13);
            this.lblIntercalado.TabIndex = 2;
            this.lblIntercalado.Text = "Intercalar";
            // 
            // lblCopias
            // 
            this.lblCopias.AutoSize = true;
            this.lblCopias.Location = new System.Drawing.Point(17, 31);
            this.lblCopias.Name = "lblCopias";
            this.lblCopias.Size = new System.Drawing.Size(74, 13);
            this.lblCopias.TabIndex = 1;
            this.lblCopias.Text = "Nº de copias: ";
            // 
            // cbCopias
            // 
            this.cbCopias.FormattingEnabled = true;
            this.cbCopias.Location = new System.Drawing.Point(97, 28);
            this.cbCopias.Name = "cbCopias";
            this.cbCopias.Size = new System.Drawing.Size(48, 21);
            this.cbCopias.TabIndex = 0;
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.herramientasToolStripMenuItem,
            this.ayudaToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(463, 24);
            this.menu.TabIndex = 11;
            this.menu.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator,
            this.salirToolStripMenuItem});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.archivoToolStripMenuItem.Text = "&Archivo";
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(149, 6);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.salirToolStripMenuItem.Text = "&Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // herramientasToolStripMenuItem
            // 
            this.herramientasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.opcionesToolStripMenuItem});
            this.herramientasToolStripMenuItem.Name = "herramientasToolStripMenuItem";
            this.herramientasToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.herramientasToolStripMenuItem.Text = "&Herramientas";
            // 
            // opcionesToolStripMenuItem
            // 
            this.opcionesToolStripMenuItem.Name = "opcionesToolStripMenuItem";
            this.opcionesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.opcionesToolStripMenuItem.Text = "&Opciones";
            this.opcionesToolStripMenuItem.Click += new System.EventHandler(this.btnPing_Click);
            // 
            // ayudaToolStripMenuItem
            // 
            this.ayudaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.acercadeToolStripMenuItem});
            this.ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            this.ayudaToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.ayudaToolStripMenuItem.Text = "Ay&uda";
            // 
            // acercadeToolStripMenuItem
            // 
            this.acercadeToolStripMenuItem.Name = "acercadeToolStripMenuItem";
            this.acercadeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.acercadeToolStripMenuItem.Text = "&Acerca de...";
            this.acercadeToolStripMenuItem.Click += new System.EventHandler(this.acercadeToolStripMenuItem_Click);
            // 
            // btnPdf
            // 
            this.btnPdf.Location = new System.Drawing.Point(191, 372);
            this.btnPdf.Name = "btnPdf";
            this.btnPdf.Size = new System.Drawing.Size(75, 23);
            this.btnPdf.TabIndex = 13;
            this.btnPdf.Text = "Previsualizar";
            this.btnPdf.UseVisualStyleBackColor = true;
            this.btnPdf.Click += new System.EventHandler(this.btnPdf_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 420);
            this.Controls.Add(this.btnPdf);
            this.Controls.Add(this.menu);
            this.Controls.Add(this.gbCopias);
            this.Controls.Add(this.gbPaginas);
            this.Controls.Add(this.gbImpresora);
            this.Controls.Add(this.txtDocumento);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.lbArchivo);
            this.Controls.Add(this.btnImprimir);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Cliente IMP";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gbImpresora.ResumeLayout(false);
            this.gbImpresora.PerformLayout();
            this.gbPaginas.ResumeLayout(false);
            this.gbPaginas.PerformLayout();
            this.gbCopias.ResumeLayout(false);
            this.gbCopias.PerformLayout();
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Label lbArchivo;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.TextBox txtDocumento;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.GroupBox gbImpresora;
        private System.Windows.Forms.GroupBox gbPaginas;
        private System.Windows.Forms.GroupBox gbCopias;
        private System.Windows.Forms.Label lblCopias;
        private System.Windows.Forms.ComboBox cbCopias;
        private System.Windows.Forms.TextBox txtSeleccion;
        private System.Windows.Forms.RadioButton rbSeleccion;
        private System.Windows.Forms.RadioButton rbTodo;
        private System.Windows.Forms.CheckBox checkIntercalado;
        private System.Windows.Forms.Label lblIntercalado;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem herramientasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem opcionesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ayudaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem acercadeToolStripMenuItem;
        private System.Windows.Forms.TextBox txtFin;
        private System.Windows.Forms.TextBox txtInicio;
        private System.Windows.Forms.Label lblFin;
        private System.Windows.Forms.Label lblInicio;
        private System.Windows.Forms.RadioButton rbRango;
        private System.Windows.Forms.Button btnPdf;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

