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
    public partial class FormularioPrincipal : Form
    {
        public string id_usuario;
        public bool administrador;
        public Form login;

        public FormularioPrincipal()
        {
            InitializeComponent();
        }

        #region funcionalidades del formulario
        
        //METODO PARA ARRASTRAR EL FORMULARIO---------------------------------------------------------------------
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panelBarraTitulo_MouseMove(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        #endregion

        //Abrir formulario en el panel
        private void AbrirFormEnPanel<Forms>() where Forms : Form, new()
        {
            Form formulario;
            formulario = panelContenedor.Controls.OfType<Forms>().FirstOrDefault();

            //si el formulario/instancia no existe, creamos nueva instancia y mostramos
            if (formulario == null)
            {
                formulario = new Forms();
                formulario.TopLevel = false;
                formulario.FormBorderStyle = FormBorderStyle.None;
                formulario.Dock = DockStyle.Fill;
                formulario.Location = new Point((panelContenedor.Width + panelMenu.Width - formulario.Width) / 2, (panelContenedor.Height + panelBarraTitulo.Height - formulario.Height) / 2);
                formulario.Anchor = AnchorStyles.None;
                panelContenedor.Controls.Add(formulario);
                panelContenedor.Tag = formulario;
                formulario.Show();

                formulario.BringToFront();
                formulario.FormClosed += new FormClosedEventHandler(CloseForms);               
            }
            else
            {
                //si la Formulario/instancia existe, lo traemos a frente
                formulario.BringToFront();

                //Si la instancia esta minimizada mostramos
                if (formulario.WindowState == FormWindowState.Minimized)
                {
                    formulario.WindowState = FormWindowState.Normal;
                }

            }
        }

        private void CloseForms(object sender, FormClosedEventArgs e)
        {
            if (Application.OpenForms["Ventas"] == null)
                btnVenta.BackColor = Color.FromArgb(4, 41, 68);
            if (Application.OpenForms["Compras"] == null)
                btnCompras.BackColor = Color.FromArgb(4, 41, 68);
            if (Application.OpenForms["Usuarios"] == null)
                btnEmpleados.BackColor = Color.FromArgb(4, 41, 68);
            if (Application.OpenForms["RegistroVentas"] == null)
                btnRegistroVentas.BackColor = Color.FromArgb(4, 41, 68);
            if (Application.OpenForms["VentanaReportes"] == null)
                btnReportes.BackColor = Color.FromArgb(4, 41, 68);
            if (Application.OpenForms["Productos"] == null)
                btnProductos.BackColor = Color.FromArgb(4, 41, 68);
        }

        private void btnVenta_Click(object sender, EventArgs e)
        {
            CerrarTodosFormularios();
            AbrirFormVentas();
            btnVenta.BackColor = Color.FromArgb(12, 61, 92);
        }

        private void AbrirFormVentas()
        {
            Ventas formulario;
            formulario = panelContenedor.Controls.OfType<Ventas>().FirstOrDefault();
            //si el formulario/instancia no existe, creamos nueva instancia y mostramos
            if (formulario == null)
            {
                formulario = new Ventas();
                formulario.TopLevel = false;
                formulario.FormBorderStyle = FormBorderStyle.None;
                formulario.Dock = DockStyle.Fill;
                formulario.Location = new Point((panelContenedor.Width + panelMenu.Width - formulario.Width) / 2, (panelContenedor.Height + panelBarraTitulo.Height - formulario.Height) / 2);
                formulario.Anchor = AnchorStyles.None;
                formulario.id_usuario = id_usuario;
                panelContenedor.Controls.Add(formulario);
                panelContenedor.Tag = formulario;
                formulario.Show();
                formulario.BringToFront();
                formulario.FormClosed += new FormClosedEventHandler(CloseForms);
            }
            else
            {
                //si la Formulario/instancia existe, lo traemos a frente
                formulario.BringToFront();

                //Si la instancia esta minimizada mostramos
                if (formulario.WindowState == FormWindowState.Minimized)
                {
                    formulario.WindowState = FormWindowState.Normal;
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CerrarTodosFormularios();
            AbrirFormCompras();
            btnCompras.BackColor = Color.FromArgb(12, 61, 92);
        }

        private void AbrirFormCompras()
        {
            Compras formulario;
            formulario = panelContenedor.Controls.OfType<Compras>().FirstOrDefault();
            //si el formulario/instancia no existe, creamos nueva instancia y mostramos
            if (formulario == null)
            {
                formulario = new Compras();
                formulario.TopLevel = false;
                formulario.FormBorderStyle = FormBorderStyle.None;
                formulario.Dock = DockStyle.Fill;
                formulario.Location = new Point((panelContenedor.Width + panelMenu.Width - formulario.Width) / 2, (panelContenedor.Height + panelBarraTitulo.Height - formulario.Height) / 2);
                formulario.Anchor = AnchorStyles.None;
                formulario.id_usuario = id_usuario;
                panelContenedor.Controls.Add(formulario);
                panelContenedor.Tag = formulario;
                formulario.Show();
                formulario.BringToFront();
                formulario.FormClosed += new FormClosedEventHandler(CloseForms);
            }
            else
            {
                //si la Formulario/instancia existe, lo traemos a frente
                formulario.BringToFront();

                //Si la instancia esta minimizada mostramos
                if (formulario.WindowState == FormWindowState.Minimized)
                {
                    formulario.WindowState = FormWindowState.Normal;
                }

            }
        }

        private void CerrarTodosFormularios()
        {
            foreach(Form form in panelContenedor.Controls.OfType<Form>().ToArray())
            {
                form.Close();
                form.Dispose();
            }
        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            CerrarTodosFormularios();
            AbrirFormEmpleados();
            btnEmpleados.BackColor = Color.FromArgb(12, 61, 92);
        }

        private void AbrirFormEmpleados()
        {
            Usuarios formulario;
            formulario = panelContenedor.Controls.OfType<Usuarios>().FirstOrDefault();
            //si el formulario/instancia no existe, creamos nueva instancia y mostramos
            if (formulario == null)
            {
                formulario = new Usuarios();
                formulario.TopLevel = false;
                formulario.FormBorderStyle = FormBorderStyle.None;
                formulario.Dock = DockStyle.Fill;
                formulario.Location = new Point((panelContenedor.Width + panelMenu.Width - formulario.Width) / 2, (panelContenedor.Height + panelBarraTitulo.Height - formulario.Height) / 2);
                formulario.Anchor = AnchorStyles.None;
                formulario.id_usuario = id_usuario;
                panelContenedor.Controls.Add(formulario);
                panelContenedor.Tag = formulario;
                formulario.panelContenedor = panelContenedor; /*envio conteneodor*/
                formulario.Show();
                formulario.BringToFront();
                formulario.FormClosed += new FormClosedEventHandler(CloseForms);
            }
            else
            {
                //si la Formulario/instancia existe, lo traemos a frente
                formulario.BringToFront();

                //Si la instancia esta minimizada mostramos
                if (formulario.WindowState == FormWindowState.Minimized)
                {
                    formulario.WindowState = FormWindowState.Normal;
                }

            }
        }

        private void btnRegistroVentas_Click(object sender, EventArgs e)
        {
            CerrarTodosFormularios();
            AbrirFormRegistroVentas();
            btnRegistroVentas.BackColor = Color.FromArgb(12, 61, 92);
        }

        private void AbrirFormRegistroVentas()
        {
            RegistroVentas formulario;
            formulario = panelContenedor.Controls.OfType<RegistroVentas>().FirstOrDefault();
            //si el formulario/instancia no existe, creamos nueva instancia y mostramos
            if (formulario == null)
            {
                formulario = new RegistroVentas();
                formulario.TopLevel = false;
                formulario.FormBorderStyle = FormBorderStyle.None;
                formulario.Dock = DockStyle.Fill;
                formulario.Location = new Point((panelContenedor.Width + panelMenu.Width - formulario.Width) / 2, (panelContenedor.Height + panelBarraTitulo.Height - formulario.Height) / 2);
                formulario.Anchor = AnchorStyles.None;
                panelContenedor.Controls.Add(formulario);
                panelContenedor.Tag = formulario;
                formulario.Show();
                formulario.BringToFront();
                formulario.FormClosed += new FormClosedEventHandler(CloseForms);
            }
            else
            {
                //si la Formulario/instancia existe, lo traemos a frente
                formulario.BringToFront();

                //Si la instancia esta minimizada mostramos
                if (formulario.WindowState == FormWindowState.Minimized)
                {
                    formulario.WindowState = FormWindowState.Normal;
                }

            }
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            CerrarTodosFormularios();
            AbrirFormReportes();
            btnReportes.BackColor = Color.FromArgb(12, 61, 92);
        }

        private void AbrirFormReportes()
        {
            VentanaReportes formulario;
            formulario = panelContenedor.Controls.OfType<VentanaReportes>().FirstOrDefault();
            //si el formulario/instancia no existe, creamos nueva instancia y mostramos
            if (formulario == null)
            {
                formulario = new VentanaReportes();
                formulario.TopLevel = false;
                formulario.FormBorderStyle = FormBorderStyle.None;
                formulario.Dock = DockStyle.Fill;
                formulario.Location = new Point((panelContenedor.Width + panelMenu.Width - formulario.Width) / 2, (panelContenedor.Height + panelBarraTitulo.Height - formulario.Height) / 2);
                formulario.Anchor = AnchorStyles.None;
                panelContenedor.Controls.Add(formulario);
                panelContenedor.Tag = formulario;
                formulario.Show();
                formulario.BringToFront();
                formulario.FormClosed += new FormClosedEventHandler(CloseForms);
            }
            else
            {
                //si la Formulario/instancia existe, lo traemos a frente
                formulario.BringToFront();

                //Si la instancia esta minimizada mostramos
                if (formulario.WindowState == FormWindowState.Minimized)
                {
                    formulario.WindowState = FormWindowState.Normal;
                }

            }
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            CerrarTodosFormularios();
            AbrirFormProductos();
            btnProductos.BackColor = Color.FromArgb(12, 61, 92);
        }

        private void AbrirFormProductos()
        {
            Productos formulario;
            formulario = panelContenedor.Controls.OfType<Productos>().FirstOrDefault();
            //si el formulario/instancia no existe, creamos nueva instancia y mostramos
            if (formulario == null)
            {
                formulario = new Productos();
                formulario.TopLevel = false;
                formulario.FormBorderStyle = FormBorderStyle.None;
                formulario.Dock = DockStyle.Fill;
                formulario.Location = new Point((panelContenedor.Width + panelMenu.Width - formulario.Width) / 2, (panelContenedor.Height + panelBarraTitulo.Height - formulario.Height) / 2);
                formulario.Anchor = AnchorStyles.None;
                panelContenedor.Controls.Add(formulario);
                panelContenedor.Tag = formulario;
                formulario.panelContenedor = panelContenedor; /*envio conteneodor*/
                formulario.Show();
                formulario.BringToFront();
                formulario.FormClosed += new FormClosedEventHandler(CloseForms);
            }
            else
            {
                //si la Formulario/instancia existe, lo traemos a frente
                formulario.BringToFront();

                //Si la instancia esta minimizada mostramos
                if (formulario.WindowState == FormWindowState.Minimized)
                {
                    formulario.WindowState = FormWindowState.Normal;
                }

            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("Realmente desea salir?", "Alerta", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (boton == DialogResult.OK)
            {
                login.Show();
                this.Dispose();
            } 
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("Realmente desea salir?", "Alerta", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (boton == DialogResult.OK)
            {
                Application.Exit();
            } 
        }

        private void FormularioPrincipal_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblHora.Text = DateTime.Now.ToLongTimeString();
            lblFecha.Text = DateTime.Now.ToLongDateString();
            if (!administrador)
            {
                btnEmpleados.Visible = false;
                btnCompras.Visible = false;
                btnReportes.Visible = false;
                btnProductos.Visible = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToLongTimeString();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Info form = new Info();
            form.ShowDialog();
        }
    }
}
