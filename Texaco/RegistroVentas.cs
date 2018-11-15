using CrystalDecisions.Shared;
using Npgsql;
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
    public partial class RegistroVentas : Form
    {
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5433; User Id=postgres;Password=avengers;Database = Texaco");
        NpgsqlDataAdapter adp;
        DataSet ds;

        public RegistroVentas()
        {
            InitializeComponent();
        }

        private void RegistroVentas_Load(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = "SELECT V.id, V.comprobante, V.fecha, sum(DV.precio * DV.cantidad) total";
                query += "\n FROM venta V, detalle_venta DV";
                query += "\n WHERE V.id = DV.id_venta";
                query += "\n GROUP BY V.id, V.comprobante, V.fecha";
                adp = new NpgsqlDataAdapter(query, conn);
                ds = new DataSet();
                adp.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Descending);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnComprobante_Click(object sender, EventArgs e)
        {
            //Para generar el comprobante valido que existan filas en el datagrid
            if(dataGridView1.Rows.Count > 0)
            {
                //Parametro del procedimiento almacenado
                string nombreParametro = "p_comprobante";
                string comprobante = dataGridView1.CurrentRow.Cells[1].Value.ToString();

                ReporteForm form = new ReporteForm();
                ReporteVenta oRep = new ReporteVenta();
                ParameterField pf = new ParameterField();
                ParameterFields pfs = new ParameterFields();
                ParameterDiscreteValue pdv = new ParameterDiscreteValue();
                pf.Name = nombreParametro;
                pdv.Value = comprobante;
                pf.CurrentValues.Add(pdv);
                pfs.Add(pf);
                form.crystalReportViewer1.ParameterFieldInfo = pfs;
                oRep.SetParameterValue(nombreParametro, comprobante, oRep.Subreports[0].Name.ToString());
                form.crystalReportViewer1.ReportSource = oRep;
                form.Show();
            }
            else
            {
                MessageBox.Show("No existen ventas para generar el comprobante");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
