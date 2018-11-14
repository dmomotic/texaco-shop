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
    public partial class Administrador : Form
    {
        public string id_usuario;

        public Administrador()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Usuarios usuarios = new Usuarios();
            usuarios.id_usuario = id_usuario;
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
            ventas.id_usuario = id_usuario;
            ventas.Show();
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            VentanaReportes reportes = new VentanaReportes();
            reportes.Show();
        }

        private void btnRegistroVentas_Click(object sender, EventArgs e)
        {
            RegistroVentas registroVentas = new RegistroVentas();
            registroVentas.Show();
        }

        private void btnCompras_Click(object sender, EventArgs e)
        {
            Compras compras = new Compras();
            compras.id_usuario = id_usuario;
            compras.Show();
        }

        private void Administrador_Load(object sender, EventArgs e)
        {

        }
    }
}
