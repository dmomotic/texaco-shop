namespace Texaco
{
    partial class VentanaReportes
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbDia = new System.Windows.Forms.RadioButton();
            this.rbProducto = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dtpFechaFinal = new System.Windows.Forms.DateTimePicker();
            this.lblFechaFinal = new System.Windows.Forms.Label();
            this.lblFechaInicial = new System.Windows.Forms.Label();
            this.rbRangoFechas = new System.Windows.Forms.RadioButton();
            this.rbMensual = new System.Windows.Forms.RadioButton();
            this.rbDiario = new System.Windows.Forms.RadioButton();
            this.dtpFechaInicial = new System.Windows.Forms.DateTimePicker();
            this.btnReporte = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbDia);
            this.groupBox1.Controls.Add(this.rbProducto);
            this.groupBox1.Location = new System.Drawing.Point(12, 82);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(167, 103);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Agrupar por:";
            // 
            // rbDia
            // 
            this.rbDia.AutoSize = true;
            this.rbDia.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDia.Location = new System.Drawing.Point(30, 69);
            this.rbDia.Name = "rbDia";
            this.rbDia.Size = new System.Drawing.Size(48, 22);
            this.rbDia.TabIndex = 1;
            this.rbDia.TabStop = true;
            this.rbDia.Text = "Dia";
            this.rbDia.UseVisualStyleBackColor = true;
            // 
            // rbProducto
            // 
            this.rbProducto.AutoSize = true;
            this.rbProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbProducto.Location = new System.Drawing.Point(30, 31);
            this.rbProducto.Name = "rbProducto";
            this.rbProducto.Size = new System.Drawing.Size(87, 22);
            this.rbProducto.TabIndex = 0;
            this.rbProducto.TabStop = true;
            this.rbProducto.Text = "Producto";
            this.rbProducto.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dtpFechaFinal);
            this.groupBox2.Controls.Add(this.lblFechaFinal);
            this.groupBox2.Controls.Add(this.lblFechaInicial);
            this.groupBox2.Controls.Add(this.rbRangoFechas);
            this.groupBox2.Controls.Add(this.rbMensual);
            this.groupBox2.Controls.Add(this.rbDiario);
            this.groupBox2.Controls.Add(this.dtpFechaInicial);
            this.groupBox2.Location = new System.Drawing.Point(190, 82);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(483, 216);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tipo de Reporte";
            // 
            // dtpFechaFinal
            // 
            this.dtpFechaFinal.Location = new System.Drawing.Point(22, 168);
            this.dtpFechaFinal.Name = "dtpFechaFinal";
            this.dtpFechaFinal.Size = new System.Drawing.Size(200, 20);
            this.dtpFechaFinal.TabIndex = 16;
            // 
            // lblFechaFinal
            // 
            this.lblFechaFinal.AutoSize = true;
            this.lblFechaFinal.Location = new System.Drawing.Point(22, 148);
            this.lblFechaFinal.Name = "lblFechaFinal";
            this.lblFechaFinal.Size = new System.Drawing.Size(62, 13);
            this.lblFechaFinal.TabIndex = 15;
            this.lblFechaFinal.Text = "Fecha final:";
            // 
            // lblFechaInicial
            // 
            this.lblFechaInicial.AutoSize = true;
            this.lblFechaInicial.Location = new System.Drawing.Point(19, 78);
            this.lblFechaInicial.Name = "lblFechaInicial";
            this.lblFechaInicial.Size = new System.Drawing.Size(69, 13);
            this.lblFechaInicial.TabIndex = 14;
            this.lblFechaInicial.Text = "Fecha inicial:";
            // 
            // rbRangoFechas
            // 
            this.rbRangoFechas.AutoSize = true;
            this.rbRangoFechas.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbRangoFechas.Location = new System.Drawing.Point(234, 31);
            this.rbRangoFechas.Name = "rbRangoFechas";
            this.rbRangoFechas.Size = new System.Drawing.Size(138, 22);
            this.rbRangoFechas.TabIndex = 13;
            this.rbRangoFechas.TabStop = true;
            this.rbRangoFechas.Text = "Rango de fechas";
            this.rbRangoFechas.UseVisualStyleBackColor = true;
            this.rbRangoFechas.CheckedChanged += new System.EventHandler(this.rbRangoFechas_CheckedChanged);
            // 
            // rbMensual
            // 
            this.rbMensual.AutoSize = true;
            this.rbMensual.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbMensual.Location = new System.Drawing.Point(117, 31);
            this.rbMensual.Name = "rbMensual";
            this.rbMensual.Size = new System.Drawing.Size(82, 22);
            this.rbMensual.TabIndex = 12;
            this.rbMensual.TabStop = true;
            this.rbMensual.Text = "Mensual";
            this.rbMensual.UseVisualStyleBackColor = true;
            this.rbMensual.CheckedChanged += new System.EventHandler(this.rbMensual_CheckedChanged);
            // 
            // rbDiario
            // 
            this.rbDiario.AutoSize = true;
            this.rbDiario.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDiario.Location = new System.Drawing.Point(22, 31);
            this.rbDiario.Name = "rbDiario";
            this.rbDiario.Size = new System.Drawing.Size(65, 22);
            this.rbDiario.TabIndex = 2;
            this.rbDiario.TabStop = true;
            this.rbDiario.Text = "Diario";
            this.rbDiario.UseVisualStyleBackColor = true;
            this.rbDiario.CheckedChanged += new System.EventHandler(this.rbDiario_CheckedChanged);
            // 
            // dtpFechaInicial
            // 
            this.dtpFechaInicial.Location = new System.Drawing.Point(22, 103);
            this.dtpFechaInicial.Name = "dtpFechaInicial";
            this.dtpFechaInicial.Size = new System.Drawing.Size(200, 20);
            this.dtpFechaInicial.TabIndex = 0;
            // 
            // btnReporte
            // 
            this.btnReporte.Location = new System.Drawing.Point(55, 218);
            this.btnReporte.Name = "btnReporte";
            this.btnReporte.Size = new System.Drawing.Size(88, 37);
            this.btnReporte.TabIndex = 10;
            this.btnReporte.Text = "Generar Reporte";
            this.btnReporte.UseVisualStyleBackColor = true;
            this.btnReporte.Click += new System.EventHandler(this.btnReporte_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(55, 268);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(88, 30);
            this.btnSalir.TabIndex = 11;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Montserrat", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(185, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(321, 25);
            this.label1.TabIndex = 28;
            this.label1.Text = "GENERACION DE REPORTES";
            // 
            // VentanaReportes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 334);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnReporte);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "VentanaReportes";
            this.Text = "Reportes";
            this.Load += new System.EventHandler(this.VentanaReportes_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbDia;
        private System.Windows.Forms.RadioButton rbProducto;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dtpFechaInicial;
        private System.Windows.Forms.Button btnReporte;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.DateTimePicker dtpFechaFinal;
        private System.Windows.Forms.Label lblFechaFinal;
        private System.Windows.Forms.Label lblFechaInicial;
        private System.Windows.Forms.RadioButton rbRangoFechas;
        private System.Windows.Forms.RadioButton rbMensual;
        private System.Windows.Forms.RadioButton rbDiario;
        private System.Windows.Forms.Label label1;
    }
}