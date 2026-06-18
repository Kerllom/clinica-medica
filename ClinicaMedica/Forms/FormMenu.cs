using System;
using System.Drawing;
using System.Windows.Forms;
using ClinicaMedica.Models;

namespace ClinicaMedica.Forms
{
    // =================================================================
    // MENU PRINCIPAL.
    // Aparece após o login e dá acesso às demais telas do sistema.
    // =================================================================
    public class FormMenu : Form
    {
        private Label lblTitulo = new Label();
        private Button btnCadastrarPaciente = new Button();
        private Button btnAgendar = new Button();
        private Button btnConsultas = new Button();
        private Button btnSair = new Button();

        // Guarda o usuário que fez login (usado em outras telas).
        private Usuario usuarioLogado;

        public FormMenu(Usuario usuario)
        {
            usuarioLogado = usuario;
            ConstruirTela();
        }

        private void ConstruirTela()
        {
            this.Text = "Menu Principal - Clínica Médica";
            this.Size = new Size(420, 340);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            lblTitulo.Text = "Bem-vindo(a), " + usuarioLogado.Login;
            lblTitulo.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(30, 25);

            btnCadastrarPaciente.Text = "Cadastrar Paciente";
            btnCadastrarPaciente.Location = new Point(90, 80);
            btnCadastrarPaciente.Size = new Size(220, 40);
            btnCadastrarPaciente.Click += (s, e) =>
            {
                FormCadastroPaciente tela = new FormCadastroPaciente(usuarioLogado);
                tela.ShowDialog();
            };

            btnAgendar.Text = "Agendar Consulta";
            btnAgendar.Location = new Point(90, 130);
            btnAgendar.Size = new Size(220, 40);
            btnAgendar.Click += (s, e) =>
            {
                FormAgendamento tela = new FormAgendamento();
                tela.ShowDialog();
            };

            btnConsultas.Text = "Consultas (ver / cancelar)";
            btnConsultas.Location = new Point(90, 180);
            btnConsultas.Size = new Size(220, 40);
            btnConsultas.Click += (s, e) =>
            {
                FormMinhasConsultas tela = new FormMinhasConsultas();
                tela.ShowDialog();
            };

            btnSair.Text = "Sair";
            btnSair.Location = new Point(90, 240);
            btnSair.Size = new Size(220, 40);
            btnSair.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitulo);
            this.Controls.Add(btnCadastrarPaciente);
            this.Controls.Add(btnAgendar);
            this.Controls.Add(btnConsultas);
            this.Controls.Add(btnSair);
        }
    }
}
