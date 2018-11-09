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
    public partial class Ventas : Form
    {

        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5433; User Id=postgres;Password=avengers;Database = Texaco");
        NpgsqlDataAdapter adp;
        DataSet ds;

        DataTable dtVenta;

        string id, codigoBarra, nombre, precio, cantidad, total, existencia;

        public Ventas()
        {
            InitializeComponent();
        }

        private void txtProducto_KeyUp(object sender, KeyEventArgs e)
        {
            //Si se presiona la tecla (del) elimino todo el contenido
            if (e.KeyCode == Keys.Delete)
            {
                txtProducto.Clear();
            }
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Actualizo los valores al finalizar la edicion de la celda cantidad
            string pre = dataGridView2.CurrentRow.Cells[3].Value.ToString();
            string cant = dataGridView2.CurrentRow.Cells[4].Value.ToString();
            string tot = (Double.Parse(pre) * Double.Parse(cant)).ToString();
            dataGridView2.CurrentRow.Cells[5].Value = tot;
            txtProducto.Focus();
        }

        private void dataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //Utilizado para validar el ingreso unicamente de numero en la columna cantidad
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if (dataGridView2.CurrentCell.ColumnIndex == 4) //Desired Column
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
            }
        }

        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Utilizado para validar ingreso solo de numeros en la columna cantidad
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dateTimePicker1.Value.ToString());
        }

        private void Ventas_Load(object sender, EventArgs e)
        {
            //Establecemos parametros iniciales
            dateTimePicker1.Value = DateTime.Now;
            lblComprobante.Text = obtenerComprobante();

            //Cargamos el inventario de productos
            try
            {
                conn.Open();
                adp = new NpgsqlDataAdapter("Select * from producto", conn);
                ds = new DataSet();
                adp.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            //Inicializamos el grid de ventas
            inicialidarGridVenta();

            //Posicionamos el cursor en la opcion de busqueda
            this.ActiveControl = txtProducto;
        }

        private string obtenerComprobante()
        {
            //Genero comprobante unico al azar
            byte[] buffer = Guid.NewGuid().ToByteArray();
            var FormNumber = BitConverter.ToUInt32(buffer, 0) ^ BitConverter.ToUInt32(buffer, 4) ^ BitConverter.ToUInt32(buffer, 8) ^ BitConverter.ToUInt32(buffer, 12);
            return FormNumber.ToString("X");
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

        private void inicialidarGridVenta()
        {
            dtVenta = new DataTable();
            dtVenta.Columns.Add("Id");
            dtVenta.Columns.Add("Codigo Barra");
            dtVenta.Columns.Add("Nombre");
            dtVenta.Columns.Add("Q. Unidad");
            dtVenta.Columns.Add("Cantidad");
            dtVenta.Columns.Add("Total");

            dataGridView2.DataSource = dtVenta;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //Permito la edicion solo en la columna cantidad
            dataGridView2.ReadOnly = false;
            dataGridView2.Columns[0].ReadOnly = true;
            dataGridView2.Columns[1].ReadOnly = true;
            dataGridView2.Columns[2].ReadOnly = true;
            dataGridView2.Columns[3].ReadOnly = true;
            dataGridView2.Columns[4].ReadOnly = false;
            dataGridView2.Columns[3].ReadOnly = true;
        }

        private void txtProducto_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si se presiona la tecla enter
            if(e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;

                //Verifico que el resultado de la busqueda contenga al menos una respuesta
                if(dataGridView1.CurrentRow != null)
                {
                    //Capturo la informacion y realizo los calculos correspondientes
                    id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    codigoBarra = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    nombre = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    existencia = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    precio = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    cantidad = "1";
                    total = (Double.Parse(precio) * Double.Parse(cantidad)).ToString();

                    //Cargo la info al grid de venta
                    dtVenta.Rows.Add(new object[] { id,codigoBarra,nombre,precio,cantidad,total});

                    //Limpio cuadro de busqueda
                    txtProducto.Clear();
                }
            }
        }
    }
}
