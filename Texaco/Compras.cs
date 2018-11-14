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
    public partial class Compras : Form
    {
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5433; User Id=postgres;Password=avengers;Database = Texaco");
        NpgsqlDataAdapter adp;
        DataSet ds;

        DataTable dtCompra;

        string comprobante, fecha, id_compra, id_producto, cantidad, precio_compra, codigo_barra, nombre_producto;
        public string id_usuario;

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && !(e.KeyChar == ('.')))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtPrecioUnitario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && !(e.KeyChar == ('.')))
            {
                e.Handled = true;
                return;
            }
        }

        public Compras()
        {
            InitializeComponent();
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Valido que existan filas en el datagrid2 de venta
            if (dataGridView2.Rows.Count > 0)
            {
                //Elimino la fila de la lista de venta
                dataGridView2.Rows.Remove(dataGridView2.CurrentRow);
            }
        }

        private void dataGridView2_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Utilizado para mostrar menu con opcion de eliminar
            if (e.Button == MouseButtons.Right)
            {
                try
                {
                    dataGridView2.CurrentCell = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dataGridView2.Rows[e.RowIndex].Selected = true;
                    dataGridView2.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dataGridView2_MouseClick(object sender, MouseEventArgs e)
        {
            //Utilizado para mostrar menu con opcion de eliminar
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private void Compras_Load(object sender, EventArgs e)
        {
            //Establecemos parametros iniciales
            dtpFecha.Value = DateTime.Now;
            CargarInventario();
            InicialidarGridCompras();
        }

        private void CargarInventario()
        {
            try
            {
                conn.Open();
                adp = new NpgsqlDataAdapter("Select id, codigo_barra, nombre, existencia, precio_venta from producto where borrado = false", conn);
                ds = new DataSet();
                adp.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
                conn.Close();

                //Coloco solo dos decimales en la columna precio
                foreach (DataGridViewRow item in dataGridView1.Rows)
                {
                    item.Cells[4].Value = (Math.Truncate(100 * decimal.Parse(item.Cells[4].Value.ToString())) / 100).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            //Valido que existan productos en la lista de compras
            if (dataGridView2.Rows.Count > 0)
            {

            }
            else
            {
                MessageBox.Show("Por favor ingrese los productos comprados");
            }
        }

        private void InicialidarGridCompras()
        {
            dtCompra = new DataTable();
            dtCompra.Columns.Add("Id");
            dtCompra.Columns.Add("Codigo Barra");
            dtCompra.Columns.Add("Nombre");
            dtCompra.Columns.Add("Q. Unidad");
            dtCompra.Columns.Add("Cantidad");
            dtCompra.Columns.Add("Total");

            dataGridView2.DataSource = dtCompra;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtProducto_TextChanged(object sender, EventArgs e)
        {
            string busqueda = txtProducto.Text.Trim();
            if (busqueda.Equals(""))
            {
                ds.Tables[0].DefaultView.RowFilter = "";
                return;
            }

            ds.Tables[0].DefaultView.RowFilter = $"nombre LIKE '%{busqueda}%' or codigo_barra LIKE '%{busqueda}%'";
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                try
                {
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    dataGridView1.Focus();
                }
                catch
                {

                }
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (ValidarCamposDetalle())
            {
                return;
            }
            //Capturo datos del detalle
            id_producto = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            codigo_barra = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            nombre_producto = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            cantidad = txtCantidad.Text.Trim().Replace(',', '.');
            precio_compra = txtPrecioUnitario.Text.Trim().Replace(',', '.');
            string total = (Double.Parse(cantidad, NumberStyles.Any, CultureInfo.InvariantCulture) * Double.Parse(precio_compra, NumberStyles.Any, CultureInfo.InvariantCulture)).ToString();
            //Cargo la info al grid de venta
            dtCompra.Rows.Add(new object[] { id_producto, codigo_barra, nombre_producto, precio_compra, cantidad, total });
            LimpiarCamposDetalle();
        }

        private bool ValidarCamposDetalle()
        {
            if (txtCantidad.Text.Trim().Equals(""))
            {
                MessageBox.Show("Por favor ingrese la cantidad de productos comprados");
                return true;
            }
            if (txtPrecioUnitario.Text.Trim().Equals(""))
            {
                MessageBox.Show("Por favor ingrese el presio unitario del producto");
                return true;
            }
            return false;
        }

        private void LimpiarCamposDetalle()
        {
            txtProducto.Clear();
            txtCantidad.Clear();
            txtPrecioUnitario.Clear();
        }

    }
}
