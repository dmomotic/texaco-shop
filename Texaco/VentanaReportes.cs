using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Texaco.Reportes;

namespace Texaco
{
    public partial class VentanaReportes : Form
    {
        public VentanaReportes()
        {
            InitializeComponent();
        }

        private void VentanaReportes_Load(object sender, EventArgs e)
        {
            //Establezco opciones iniciales
            rbProducto.Select();
            rbDiario.Select();
            dtpFechaInicial.Value = DateTime.Now;
            dtpFechaFinal.Value = DateTime.Now;
            lblFechaFinal.Visible = false;
            dtpFechaFinal.Visible = false;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReporte_Click(object sender, EventArgs e)
        {
            string fechaInicial="";
            string fechaFinal="";

            //POR PRODUCTO 
            switch (rbProducto.Checked)
            {
                case true:
                    //DIARIO
                    if (rbDiario.Checked)
                    {
                        fechaInicial = dtpFechaInicial.Value.ToShortDateString();
                        fechaFinal = fechaInicial;
                    }
                    //RANGO DE FECHAS
                    else if (rbRangoFechas.Checked)
                    {
                        fechaInicial = dtpFechaInicial.Value.ToShortDateString();
                        fechaFinal = dtpFechaFinal.Value.ToShortDateString();
                        if(dtpFechaInicial.Value > dtpFechaFinal.Value)
                        {
                            MessageBox.Show("La fecha inicial debe ser menor a la fecha final");
                            return;
                        }
                    }
                    generarReportePorProducto(fechaInicial, fechaFinal);
                    break;
            }
        }

        private void generarReportePorProducto(string fechaInicial, string fechaFinal)
        {
            string nombreParametro = "p_fecha_inicial";
            string nombreParametro2 = "p_fecha_final";
            ReporteForm form = new ReporteForm();
            ReporteVentasDiarias oRep = new ReporteVentasDiarias();
            /*PARAMETRO 1*/
            ParameterField pf = new ParameterField();
            ParameterFields pfs = new ParameterFields();
            ParameterDiscreteValue pdv = new ParameterDiscreteValue();
            pf.Name = nombreParametro;
            pdv.Value = fechaInicial;
            pf.CurrentValues.Add(pdv);
            pfs.Add(pf);
            /*PARAMETRO 2*/
            ParameterField pf2 = new ParameterField();
            ParameterDiscreteValue pdv2 = new ParameterDiscreteValue();
            pf2.Name = nombreParametro2;
            pdv2.Value = fechaFinal;
            pf2.CurrentValues.Add(pdv2);
            pfs.Add(pf2);
            form.crystalReportViewer1.ParameterFieldInfo = pfs;
            form.crystalReportViewer1.ReportSource = oRep;
            form.Show();
        }

        private void rbDiario_CheckedChanged(object sender, EventArgs e)
        {
            //Si se selecciona los reportes por dia
            if (rbDiario.Checked)
            {
                lblFechaFinal.Visible = false;
                dtpFechaFinal.Visible = false;
            }
        }

        private void rbMensual_CheckedChanged(object sender, EventArgs e)
        {
            //Si selecciona los reportes por mes
            if (rbMensual.Checked)
            {
                lblFechaFinal.Visible = false;
                dtpFechaFinal.Visible = false;
            }        
        }

        private void rbRangoFechas_CheckedChanged(object sender, EventArgs e)
        {
            //Si selecciona los reportes por mes
            if (rbRangoFechas.Checked)
            {
                lblFechaFinal.Visible = true;
                dtpFechaFinal.Visible = true;
            }
        }
    }
}
