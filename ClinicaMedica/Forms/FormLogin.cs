using System;
using System.Drawing;
using System.Windows.Forms;
using ClinicaMedica.Data;
using ClinicaMedica.Models;

namespace ClinicaMedica.Forms
{
    // =================================================================
    // TELA DE LOGIN (RF02).
    // Toda a interface é criada por código (sem usar o designer),
    // assim o arquivo é um .cs único e fácil de versionar.
    // =================================================================
    public class FormLogin : Form
    {
        private Label lblTitulo = new Label();
        private Label lblLogin  = new Label();
        private Label lblSenha  = new Label();
        private TextBox txtLogin = new TextBox();
        private TextBox txtSenha = new TextBox();
        private Button btnEntrar = new Button();

        private UsuarioDAO usuarioDAO = new UsuarioDAO();

        public FormLogin()
        {
            ConstruirTela();
        }

        // Monta os componentes visuais da tela.
        private void ConstruirTela()
        {
            this.Text = "Login - Clínica Médica";
            this.Size = new Size(380, 270);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            lblTitulo.Text = "Clínica Médica";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(100, 25);

            lblLogin.Text = "Usuário:";
            lblLogin.AutoSize = true;
            lblLogin.Location = new Point(40, 85);

            txtLogin.Location = new Point(130, 82);
            txtLogin.Width = 180;

            lblSenha.Text = "Senha:";
            lblSenha.AutoSize = true;
            lblSenha.Location = new Point(40, 125);

            txtSenha.Location = new Point(130, 122);
            txtSenha.Width = 180;
            txtSenha.PasswordChar = '*';

            btnEntrar.Text = "Entrar";
            btnEntrar.Location = new Point(130, 165);
            btnEntrar.Width = 180;
            btnEntrar.Click += BtnEntrar_Click;

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblLogin);
            this.Controls.Add(txtLogin);
            this.Controls.Add(lblSenha);
            this.Controls.Add(txtSenha);
            this.Controls.Add(btnEntrar);
        }

        // Evento do botão Entrar: valida o usuário e abre o menu.
        private void BtnEntrar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = usuarioDAO.BuscarPorLogin(txtLogin.Text.Trim());

                // Compara a senha. (Observação: em um sistema real a senha
                // deveria estar criptografada - RNF03.)
                if (usuario == null || usuario.Senha != txtSenha.Text)
                {
                    MessageBox.Show("Usuário ou senha inválidos.");
                    return;
                }

                // Abre o menu principal e esconde a tela de login.
                FormMenu menu = new FormMenu(usuario);
                menu.FormClosed += (s, args) => this.Close(); // ao fechar o menu, fecha o app
                menu.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao acessar o banco de dados:\n" + ex.Message);
            }
        }
    }
}
