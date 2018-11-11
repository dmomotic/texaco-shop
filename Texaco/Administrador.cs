using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Texaco.Reportes;

namespace Texaco
{
    public partial class Administrador : Form
    {
        public Administrador()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Usuarios usuarios = new Usuarios();
            usuarios.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Productos productos = new Productos();
            productos.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Ventas ventas = new Ventas();
            ventas.Show();
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            VentanaReporteVenta form = new VentanaReporteVenta();
            ReporteVenta oRep = new ReporteVenta();
            ParameterField pf = new ParameterField();
            ParameterFields pfs = new ParameterFields();
            ParameterDiscreteValue pdv = new ParameterDiscreteValue();
            pf.Name = "p_comprobante";
            pdv.Value = "C3FADE1C";
            pf.CurrentValues.Add(pdv);
            pfs.Add(pf);
            form.crystalReportViewer1.ParameterFieldInfo = pfs;
            oRep.SetParameterValue("p_comprobante", "C3FADE1C", oRep.Subreports[0].Name.ToString());
            form.crystalReportViewer1.ReportSource = oRep ;
            form.Show();
        }
    }
}
