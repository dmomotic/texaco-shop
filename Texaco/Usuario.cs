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
    public partial class Usuario : Form
    {
        //Conexion postgres
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5433; User Id=postgres;Password=avengers;Database = Texaco");
        string operacion;

        public string id;
        public string nombre;
        public string dpi;
        public string usuario;
        public string contraseña;
        public string tipo;
        public Panel panelContenedor;

        string query = "";

        public Usuario()
        {
            InitializeComponent();
        }

        public Usuario(string op)
        {
            InitializeComponent();
            this.operacion = op;

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
           
            nombre = txtNombre.Text.Trim();
            dpi = txtDpi.Text.Trim();
            usuario = txtUsuario.Text.Trim();
            contraseña = txtContraseña.Text.Trim();
            tipo = cbxTipo.SelectedIndex == 0 ? "true" : "false";

            if (erroresEnCampos())
            {
                return;
            }

            if (operacion.Equals("nuevo"))
            {
                guardar();
            }
            else if (operacion.Equals("editar"))
            {
                editar();
            }
            else if (operacion.Equals("eliminar"))
            {
                eliminar();
            }
            ventanaUsuarios();
            ActualizarDatos();
        }

        private void ActualizarDatos()
        {
            Usuarios formulario;
            formulario = panelContenedor.Controls.OfType<Usuarios>().FirstOrDefault();
            //si el formulario/instancia no existe, creamos nueva instancia y mostramos
            if (formulario != null)
            {
                formulario.CargarDatos();
            }
        }


        private void limpiarCampos()
        {
            txtNombre.Clear();
            txtDpi.Clear();
            txtUsuario.Clear();
            txtContraseña.Clear();
            cbxTipo.SelectedIndex = -1;
            query = "";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ventanaUsuarios();
        }

        private void guardar()
        {
            try
            {
                conn.Open();
                query += "INSERT INTO usuario(nombre,dpi,usuario,contraseña,administrador) values(";
                query += "'" + nombre + "'";
                query += ",'" + dpi + "'";
                query += ",'" + usuario + "'";
                query += ",'" + contraseña + "'";
                query += "," + tipo + ")";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Empleado registrado");
                    limpiarCampos();
                }
                else
                {
                    MessageBox.Show("Error al ingresar empleado");
                }

                conn.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void editar()
        {
            try
            {
                conn.Open();
                query += "UPDATE usuario SET ";
                query += "nombre = '" + nombre + "'";
                query += ", dpi='" + dpi + "'";
                query += ", usuario='" + usuario + "'";
                query += ", contraseña='" + contraseña + "'";
                query += ", administrador=" + tipo + " ";
                query += "WHERE id=" + id;
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Informacion actualizada");
                    limpiarCampos();
                }
                else
                {
                    MessageBox.Show("Error al actualizar la informacion");
                }

                conn.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void eliminar()
        {
            try
            {
                conn.Open();
                query += "UPDATE usuario SET borrado = true ";
                query += "WHERE id = " + id;
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Empleado eliminado");
                    limpiarCampos();
                }
                else
                {
                    MessageBox.Show("Error al eliminar empleado");
                }

                conn.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void ventanaUsuarios()
        {
            this.Close();
            this.Dispose();
        }

        private bool erroresEnCampos()
        {
            if (nombre.Equals(""))
            {
                MessageBox.Show("Por favor llene el campo nombre");
                return true;
            }
            if (usuario.Equals(""))
            {
                MessageBox.Show("Por favor llene el campo usuario");
                return true;
            }
            if (contraseña.Equals(""))
            {
                MessageBox.Show("Por favor llene el campo contraseña");
                return true;
            }
            if(cbxTipo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione el tipo de usuario");
                return true;
            }
            return false;
        }

        private void Usuario_Load(object sender, EventArgs e)
        {
            //Si es una edicion o eliminacion
            if (operacion.Equals("editar") || operacion.Equals("eliminar"))
            {
                txtNombre.Text = nombre;
                txtDpi.Text = dpi;
                txtUsuario.Text = usuario;
                txtContraseña.Text = contraseña;
                cbxTipo.SelectedItem = tipo;
            }

            //Si es una eliminacion no permito editar los campos
            if (operacion.Equals("eliminar"))
            {
                txtNombre.Enabled = false;
                txtDpi.Enabled = false;
                txtUsuario.Enabled = false;
                txtContraseña.Enabled = false;
                cbxTipo.Enabled = false;
            }
        }

        private void txtDpi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }
    }
}
