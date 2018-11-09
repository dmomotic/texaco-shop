using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Texaco
{
    public partial class Producto : Form
    {
        //Conexion postgres
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5433; User Id=postgres;Password=avengers;Database = Texaco");
        string operacion;

        public string id;
        public string codigo_barra;
        public string nombre;
        public string precio_venta;
        public string existencia;

        string query = "";

        public Producto()
        {
            InitializeComponent();
        }

        public Producto(string op)
        {
            InitializeComponent();
            this.operacion = op;

        }

        private void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) &&  (e.KeyChar != (char)Keys.Back) && !(e.KeyChar == ('.')))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtContraseña_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            codigo_barra = txtCodigoBarra.Text.Trim();
            nombre = txtNombre.Text.Trim();
            precio_venta = txtPrecioVenta.Text.Trim();
            existencia = txtExistencias.Text.Trim();

            if (erroresEnCampos())
            {
                return;
            }

            if (operacion.Equals("nuevo"))
            {
                if (validarCodigoBarraRepetido())
                {
                    return;
                }
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

            ventanaProductos();
        }

        //Retorna true si ya existe un producto con el codigo de barra ingresado
        private bool validarCodigoBarraRepetido()
        {
            if(codigo_barra.Length > 0)
            {
                try
                {
                    conn.Open();
                    string qr = "SELECT * FROM producto WHERE codigo_barra = '" + codigo_barra + "'";
                    NpgsqlCommand cmd = new NpgsqlCommand(qr,conn);
                    NpgsqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        MessageBox.Show("Ya existe un producto con el mismo codigo de barra","Advertencia",MessageBoxButtons.OK);
                        conn.Close();
                        return true;
                    }
                    else
                    {
                        conn.Close();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al validar codigo de barra repetido " + ex.Message);
                }
            }
            return false;
        }

        private void guardar()
        {
            try
            {
                conn.Open();
                query += "INSERT INTO producto(codigo_barra,nombre,precio_venta,existencia) values(";
                query += "'" + codigo_barra + "'";
                query += ",'" + nombre + "'";
                query += "," + precio_venta;
                query += "," + existencia + ")";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Producto registrado");
                    limpiarCampos();
                }
                else
                {
                    MessageBox.Show("Error al registrar producto");
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
                query += "UPDATE producto SET ";
                query += "codigo_barra = '" + codigo_barra + "'";
                query += ", nombre='" + nombre + "'";
                query += ", existencia=" + existencia;
                query += ", precio_venta=" + precio_venta + " ";
                query += "WHERE id=" + id;
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Producto actualizado correctamente");
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
                query += "DELETE FROM producto ";
                query += "WHERE id = " + id;
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Producto eliminado");
                    limpiarCampos();
                }
                else
                {
                    MessageBox.Show("Error al eliminar producto");
                }

                conn.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void limpiarCampos()
        {
            txtNombre.Clear();
            txtCodigoBarra.Clear();
            txtPrecioVenta.Clear();
            txtExistencias.Clear();
            query = "";
        }

        private bool erroresEnCampos()
        {
            if (nombre.Equals(""))
            {
                MessageBox.Show("Por favor llene el campo nombre");
                return true;
            }
            if (precio_venta.Equals(""))
            {
                MessageBox.Show("Por favor llene el campo precio de venta");
                return true;
            }
            if (existencia.Equals(""))
            {
                MessageBox.Show("Por favor llene el campo existencia");
                return true;
            }
            /*Valido el formato del precio de venta*/
            double numero;
            if (!Double.TryParse(precio_venta, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo, out numero))
            {
                MessageBox.Show("Por favor ingrese un precio de venta valido");
                return true;
            }
            else
            {
                precio_venta = numero.ToString().Replace(',','.');
            }
            return false;
        }

        private void ventanaProductos()
        {
            Productos productos= new Productos();
            productos.Show();
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ventanaProductos();
        }

        private void Producto_Load(object sender, EventArgs e)
        {
            //Si es una edicion o eliminacion
            if (operacion.Equals("editar") || operacion.Equals("eliminar"))
            {
                txtCodigoBarra.Text = codigo_barra;
                txtNombre.Text = nombre;
                txtExistencias.Text = existencia;
                txtPrecioVenta.Text = precio_venta;
            }

            //Si es una eliminacion no permito editar los campos
            if (operacion.Equals("eliminar"))
            {
                txtCodigoBarra.Enabled = false;
                txtNombre.Enabled = false;
                txtExistencias.Enabled = false;
                txtPrecioVenta.Enabled = false;
            }
        }

        private void txtCodigoBarra_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si es un enter paso al siguiente campo de texto
            if(e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }
    }
}
