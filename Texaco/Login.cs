using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
        }

        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            if (txtUsuario.Text.Equals("USUARIO"))
            {
                txtUsuario.Clear();
                txtUsuario.ForeColor = Color.LightGray;
            }
        }

        private void txtUsuario_Leave(object sender, EventArgs e)
        {
            if (txtUsuario.Text.Equals(""))
            {
                txtUsuario.Text = "USUARIO";
                txtUsuario.ForeColor = Color.DimGray;
            }
        }

        private void txtContraseña_Enter(object sender, EventArgs e)
        {
            if (txtContraseña.Text.Equals("CONTRASEÑA"))
            {
                txtContraseña.Clear();
                txtContraseña.ForeColor = Color.LightGray;
                txtContraseña.UseSystemPasswordChar = true;
            }
        }

        private void txtContraseña_Leave(object sender, EventArgs e)
        {
            if (txtContraseña.Text.Equals(""))
            {
                txtContraseña.Text = "CONTRASEÑA";
                txtContraseña.ForeColor = Color.DimGray;
                txtContraseña.UseSystemPasswordChar = false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuario;
            string contraseña;

            usuario = txtUsuario.Text.Trim();
            contraseña = txtContraseña.Text.Trim();

            try
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("Select id, administrador from usuario where usuario='" + usuario + "'" + "and contraseña='" + contraseña + "' and borrado = false", conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    MessageBox.Show("Bienvenido " + usuario);
                    FormularioPrincipal formulario = new FormularioPrincipal();
                    formulario.id_usuario = dr["id"].ToString(); ;
                    //Indico si el usuario es administrador o no
                    formulario.administrador = (bool)dr["administrador"];
                    formulario.login = this;
                    formulario.lblUsuario.Text = usuario.ToUpper();
                    formulario.Show();
                    txtUsuario.Clear();
                    txtContraseña.Clear();
                    txtUsuario.Focus();
                    txtContraseña.Focus();
                    btnIngresar.Focus();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Credenciales invalidas");
                }
                conn.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }


        //METODO PARA ARRASTRAR EL FORMULARIO---------------------------------------------------------------------
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void Login2_MouseMove(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Info form = new Info();
            form.ShowDialog();
        }
    }
}
