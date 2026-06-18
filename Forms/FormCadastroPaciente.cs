using System;
using System.Drawing;
using System.Windows.Forms;
using ClinicaMedica.Models;
using ClinicaMedica.Services;

namespace ClinicaMedica.Forms
{
    // =================================================================
    // TELA DE CADASTRO DE PACIENTE (RF01).
    // Usa o PacienteService, que valida CPF, e-mail e nome (RNF05).
    // =================================================================
    public class FormCadastroPaciente : Form
    {
        private Label lblNome = new Label();
        private Label lblCpf = new Label();
        private Label lblTelefone = new Label();
        private Label lblEndereco = new Label();
        private Label lblEmail = new Label();
        private Label lblNascimento = new Label();

        private TextBox txtNome = new TextBox();
        private TextBox txtCpf = new TextBox();
        private TextBox txtTelefone = new TextBox();
        private TextBox txtEndereco = new TextBox();
        private TextBox txtEmail = new TextBox();
        private DateTimePicker dtpNascimento = new DateTimePicker();

        private Button btnSalvar = new Button();

        private Usuario usuarioLogado;
        private PacienteService pacienteService = new PacienteService();

        public FormCadastroPaciente(Usuario usuario)
        {
            usuarioLogado = usuario;
            ConstruirTela();
        }

        private void ConstruirTela()
        {
            this.Text = "Cadastro de Paciente";
            this.Size = new Size(440, 380);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            int x1 = 30;   // coluna dos rótulos
            int x2 = 140;  // coluna dos campos
            int largura = 250;

            lblNome.Text = "Nome:";       lblNome.Location = new Point(x1, 30);  lblNome.AutoSize = true;
            txtNome.Location = new Point(x2, 27);  txtNome.Width = largura;

            lblCpf.Text = "CPF:";         lblCpf.Location = new Point(x1, 70);  lblCpf.AutoSize = true;
            txtCpf.Location = new Point(x2, 67);  txtCpf.Width = largura;

            lblTelefone.Text = "Telefone:"; lblTelefone.Location = new Point(x1, 110); lblTelefone.AutoSize = true;
            txtTelefone.Location = new Point(x2, 107); txtTelefone.Width = largura;

            lblEndereco.Text = "Endereço:"; lblEndereco.Location = new Point(x1, 150); lblEndereco.AutoSize = true;
            txtEndereco.Location = new Point(x2, 147); txtEndereco.Width = largura;

            lblEmail.Text = "E-mail:";    lblEmail.Location = new Point(x1, 190); lblEmail.AutoSize = true;
            txtEmail.Location = new Point(x2, 187); txtEmail.Width = largura;

            lblNascimento.Text = "Nascimento:"; lblNascimento.Location = new Point(x1, 230); lblNascimento.AutoSize = true;
            dtpNascimento.Location = new Point(x2, 227); dtpNascimento.Width = largura;
            dtpNascimento.Format = DateTimePickerFormat.Short;

            btnSalvar.Text = "Salvar";
            btnSalvar.Location = new Point(x2, 280);
            btnSalvar.Width = largura;
            btnSalvar.Click += BtnSalvar_Click;

            this.Controls.Add(lblNome);       this.Controls.Add(txtNome);
            this.Controls.Add(lblCpf);        this.Controls.Add(txtCpf);
            this.Controls.Add(lblTelefone);   this.Controls.Add(txtTelefone);
            this.Controls.Add(lblEndereco);   this.Controls.Add(txtEndereco);
            this.Controls.Add(lblEmail);      this.Controls.Add(txtEmail);
            this.Controls.Add(lblNascimento); this.Controls.Add(dtpNascimento);
            this.Controls.Add(btnSalvar);
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Paciente paciente = new Paciente
                {
                    // Vincula ao usuário logado (quem fez o cadastro).
                    // Em um sistema real, o paciente poderia ter o próprio usuário.
                    IdUsuario = usuarioLogado.Id,
                    Nome = txtNome.Text.Trim(),
                    Cpf = txtCpf.Text.Trim(),
                    Telefone = txtTelefone.Text.Trim(),
                    Endereco = txtEndereco.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    DataNascimento = dtpNascimento.Value.Date
                };

                // O service valida os dados antes de gravar (RNF05).
                pacienteService.CadastrarPaciente(paciente);

                MessageBox.Show("Paciente cadastrado com sucesso!");
                LimparCampos();
            }
            catch (Exception ex)
            {
                // Mostra a mensagem da regra de negócio (ex.: "CPF inválido").
                MessageBox.Show(ex.Message);
            }
        }

        private void LimparCampos()
        {
            txtNome.Clear();
            txtCpf.Clear();
            txtTelefone.Clear();
            txtEndereco.Clear();
            txtEmail.Clear();
            dtpNascimento.Value = DateTime.Now;
        }
    }
}
