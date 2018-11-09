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
    public partial class Login : Form
    {

        //Conexion postgres
        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5433; User Id=postgres;Password=avengers;Database = Texaco");
        public Login()
        {
            InitializeComponent();
            txtUsuario.Text = "charlie";
            txtContraseña.Text = "charlie";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string usuario;
            string contraseña;

            usuario = txtUsuario.Text.Trim();
            contraseña = txtContraseña.Text.Trim();

            try
            {
                conn.Open();
                //valido si es un administrador
                NpgsqlCommand cmd = new NpgsqlCommand("Select * from usuario where usuario='" + usuario +"'"+"and contraseña='" +contraseña + "' and administrador=true", conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    
                    MessageBox.Show("Bienvenido " + usuario);
                    Administrador administrador = new Administrador();
                    administrador.Show();
                    this.Hide();
                }
                else
                {
                    //Valido que no existan las credenciales
                    MessageBox.Show("Credenciales incorrectas");
                    txtUsuario.Clear();
                    txtContraseña.Clear();
                }
                
                conn.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
    }
}
