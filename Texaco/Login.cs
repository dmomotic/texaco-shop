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
                NpgsqlCommand cmd = new NpgsqlCommand("Select id, administrador from usuario where usuario='" + usuario + "'" + "and contraseña='" + contraseña + "'", conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    bool administrador = (bool)dr["administrador"];
                    string id_usuario = dr["id"].ToString();

                    //Si es administrador
                    if (administrador)
                    {
                        MessageBox.Show("Bienvenido " + usuario);
                        Administrador admin = new Administrador();
                        admin.id_usuario = id_usuario;
                        admin.Show();
                        this.Hide();
                    }
                    //
                    else
                    {
                        MessageBox.Show("No soy administrador");
                    }
                }
                else
                {
                    MessageBox.Show("Credenciales invalidas");
                    txtContraseña.Clear();
                    txtUsuario.Clear();
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
