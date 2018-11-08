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

namespace Texaco
{
    public partial class Productos : Form
    {
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5433; User Id=postgres;Password=avengers;Database = Texaco");
        NpgsqlDataAdapter adp;
        DataSet ds;

        public Productos()
        {
            InitializeComponent();
        }

        private void Productos_Load(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                adp = new NpgsqlDataAdapter("Select * from producto", conn);
                ds = new DataSet();
                adp.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Producto producto = new Producto("nuevo");
            producto.Show();
            this.Close();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Producto producto = new Producto("editar");
            producto.id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            producto.codigo_barra = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            producto.nombre = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            producto.existencia = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            producto.precio_venta = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            producto.Show();
            this.Close();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Producto producto = new Producto("eliminar");
            producto.id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            producto.codigo_barra = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            producto.nombre = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            producto.existencia = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            producto.precio_venta = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            producto.Show();
            this.Close();
        }
    }
}
