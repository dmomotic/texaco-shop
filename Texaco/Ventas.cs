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
        public string id_usuario;

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

            //Calculamos nuevamente el total
            calcularTotalVenta();
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

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Valido que existan filas en el datagrid2 de venta
            if(dataGridView2.Rows.Count > 0)
            {
                //Elimino la fila de la lista de venta
                dataGridView2.Rows.Remove(dataGridView2.CurrentRow);
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Si da doble clic a la lista de inventario agrego el producto a la lista de venta
            //Capturo la informacion y realizo los calculos correspondientes
            id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            codigoBarra = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            nombre = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            existencia = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            precio = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            cantidad = "1";
            total = (Double.Parse(precio) * Double.Parse(cantidad)).ToString();

            //Cargo la info al grid de venta
            dtVenta.Rows.Add(new object[] { id, codigoBarra, nombre, precio, cantidad, total });

            //Limpio cuadro de busqueda
            txtProducto.Clear();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Verifico que existan productos en la lista de venta
            if(dataGridView2.Rows.Count > 0)
            {
                //Capturo los datos para la insercion en tabla venta
                string fecha = dateTimePicker1.Value.ToShortDateString();
                string comprobante = lblComprobante.Text;

                //Realizo insercion en tabla venta
                try
                {
                    conn.Open();
                    string query = "INSERT INTO venta(comprobante,fecha, id_usuario) values(";
                    query += "'" + comprobante+ "'";
                    query += ",'" + fecha + "'";
                    query += "," + id_usuario + ")";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        //Si ya se registro la venta ahora registro el detalle de la venta

                        //Primero obtengo el id del ultimo registro en la tabla venta
                        NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("SELECT * FROM venta ORDER BY id DESC LIMIT 1;", conn);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        DataRow dataRow = dataTable.Rows[0];
                        string idVenta = dataRow.ItemArray[0].ToString();

                        //Realizo la insercion por cada fila en la lista de venta
                        foreach(DataGridViewRow row in dataGridView2.Rows)
                        {
                            //Capturamos los datos
                            string id_producto = row.Cells[0].Value.ToString();
                            string cantidad_producto = row.Cells[4].Value.ToString();
                            string precio_producto = row.Cells[3].Value.ToString().Replace(',','.');

                            //Realizamos insercion
                            query = "INSERT INTO detalle_venta(id_venta,id_producto,cantidad,precio) VALUES(";
                            query += idVenta + ",";
                            query += id_producto + ",";
                            query += cantidad_producto + ",";
                            query += precio_producto + ")";

                            NpgsqlCommand command = new NpgsqlCommand(query, conn);
                            if(command.ExecuteNonQuery() <= 0)
                            {
                                MessageBox.Show("Error al guardar detalle de compra");
                            }
                        }

                        //Cuando toda la transaccion se completo de forma exitosa muestro menaje y limpio componentes
                        MessageBox.Show("Venta registrada");
                        DataTable dtaux = dataGridView2.DataSource as DataTable;
                        if(dtaux != null)
                        {
                            dtaux.Rows.Clear();
                        }
                        lblComprobante.Text = obtenerComprobante();
                    }
                    else
                    {
                        MessageBox.Show("Error al registrar venta");
                    }
                    conn.Close();

                    //Actualizamos lista de productos disponibles
                    cargarInventario();
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
            }
            else
            {
                MessageBox.Show("Aun no ha agregado productos a la lista de venta");
            }

        }

        private void limpiarCampos()
        {
            txtProducto.Clear();
            dataGridView2.Rows.Clear();
            txtProducto.Focus();
        }

        private void dataGridView2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //Calculo el total cada vez que se agrega una fila nueva a la lista de venta
            calcularTotalVenta();
            //Me posicion en el cuadro para la siguiente busqueda
            txtProducto.Focus();
        }

        private void dataGridView2_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            //Calculo el total cada vez que se elimina una fila de la lista de ventas
            calcularTotalVenta();
            //Me posicion en el cuadro para la siguiente busqueda
            txtProducto.Focus();
        }

        private void dataGridView2_MouseClick(object sender, MouseEventArgs e)
        {
            //Utilizado para mostrar menu con opcion de eliminar
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
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


        private void cargarInventario()
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
                foreach(DataGridViewRow item in dataGridView1.Rows)
                {
                    item.Cells[4].Value = (Math.Truncate(100 * decimal.Parse(item.Cells[4].Value.ToString()))/100).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Ventas_Load(object sender, EventArgs e)
        {
            //Establecemos parametros iniciales
            dateTimePicker1.Value = DateTime.Now;
            lblComprobante.Text = obtenerComprobante();

            //Cargamos el inventario de productos
            cargarInventario();
            
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

        private void calcularTotalVenta()
        {
            if(dataGridView2.Rows.Count <= 0)
            {
                lblTotal.Text = "0.00";
            }
            else
            {
                double total = 0.0;
                foreach(DataGridViewRow item in dataGridView2.Rows)
                {
                    total += Double.Parse(item.Cells[5].Value.ToString());
                }
                lblTotal.Text = total.ToString();
            }
        }
    }
}
