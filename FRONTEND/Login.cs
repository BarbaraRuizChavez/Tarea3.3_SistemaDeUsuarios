using SistemaDeUsuarios;
using System;
using System.Windows.Forms;

namespace SistemaDeUsuarios
{
    public partial class Login : Form
    {
        private readonly UsuarioRepository usuarioRepo;

        public Login()
        {
            InitializeComponent();
            this.AcceptButton = btnIngresar;
            usuarioRepo = new UsuarioRepository();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = txtUsuario.Text.Trim();
                string password = txtPassword.Text.Trim();

                if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Debe ingresar usuario y contraseña.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool valido = usuarioRepo.ValidarUsuario(usuario, password);

                if (valido)
                {
                    Menu menu = new Menu();
                    menu.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            Registro reg = new Registro();
            reg.ShowDialog();
        }
    }
}

