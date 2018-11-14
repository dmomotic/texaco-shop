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
    public partial class Usuarios : Form
    {
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5433; User Id=postgres;Password=avengers;Database = Texaco");
        NpgsqlDataAdapter adp;
        DataSet ds;
        public string id_usuario;

        public Usuarios()
        {
            InitializeComponent();
        }

        private void Usuarios_Load(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                adp = new NpgsqlDataAdapter("Select * from usuario where borrado = false", conn);
                ds = new DataSet();
                adp.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
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
            Usuario usuario = new Usuario("nuevo");
            usuario.Show();
            this.Close();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow != null)
            {
                Usuario usuario = new Usuario("editar");
                usuario.id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                usuario.nombre = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                usuario.dpi = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                usuario.usuario = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                usuario.contraseña = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                usuario.tipo = dataGridView1.CurrentRow.Cells[5].Value.ToString().Equals("True") ? "Administrador" : "Vendedor";
                usuario.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Por favor seleccione un usuario de la lista");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow != null)
            {
                if (id_usuario.Equals(dataGridView1.CurrentRow.Cells[0].Value.ToString()))
                {
                    MessageBox.Show("No puede eliminar su propia cuenta");
                    return;
                }
                Usuario usuario = new Usuario("eliminar");
                usuario.id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                usuario.nombre = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                usuario.dpi = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                usuario.usuario = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                usuario.contraseña = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                usuario.tipo = dataGridView1.CurrentRow.Cells[5].Value.ToString().Equals("True") ? "Administrador" : "Vendedor";
                usuario.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Por favor seleccione un usuario de la lista");
            }
        }
    }
}
