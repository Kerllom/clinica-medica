using System;
using System.Drawing;
using System.Windows.Forms;
using ClinicaMedica.Models;
using ClinicaMedica.Data;
using ClinicaMedica.Services;

namespace ClinicaMedica.Forms
{
    // =================================================================
    // TELA DE CONSULTAS (RF06 cancelar / RF08 listar).
    // Mostra as consultas de um paciente e permite cancelar.
    // =================================================================
    public class FormMinhasConsultas : Form
    {
        private Label lblPaciente = new Label();
        private ComboBox cmbPaciente = new ComboBox();
        private DataGridView dgvConsultas = new DataGridView();
        private Button btnCancelar = new Button();
        private Button btnAtualizar = new Button();

        private PacienteDAO pacienteDAO = new PacienteDAO();
        private ConsultaService consultaService = new ConsultaService();

        public FormMinhasConsultas()
        {
            ConstruirTela();
            CarregarPacientes();
        }

        private void ConstruirTela()
        {
            this.Text = "Consultas do Paciente";
            this.Size = new Size(640, 420);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblPaciente.Text = "Paciente:";
            lblPaciente.AutoSize = true;
            lblPaciente.Location = new Point(20, 20);

            cmbPaciente.Location = new Point(90, 17);
            cmbPaciente.Width = 300;
            cmbPaciente.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPaciente.SelectedIndexChanged += (s, e) => CarregarConsultas();

            dgvConsultas.Location = new Point(20, 60);
            dgvConsultas.Size = new Size(590, 270);
            dgvConsultas.ReadOnly = true;
            dgvConsultas.AllowUserToAddRows = false;
            dgvConsultas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvConsultas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            btnAtualizar.Text = "Atualizar";
            btnAtualizar.Location = new Point(20, 345);
            btnAtualizar.Width = 120;
            btnAtualizar.Click += (s, e) => CarregarConsultas();

            btnCancelar.Text = "Cancelar Consulta";
            btnCancelar.Location = new Point(160, 345);
            btnCancelar.Width = 160;
            btnCancelar.Click += BtnCancelar_Click;

            this.Controls.Add(lblPaciente);
            this.Controls.Add(cmbPaciente);
            this.Controls.Add(dgvConsultas);
            this.Controls.Add(btnAtualizar);
            this.Controls.Add(btnCancelar);
        }

        private void CarregarPacientes()
        {
            try
            {
                cmbPaciente.DisplayMember = "Nome";
                cmbPaciente.ValueMember = "Id";
                cmbPaciente.DataSource = pacienteDAO.ListarTodos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar pacientes: " + ex.Message);
            }
        }

        private void CarregarConsultas()
        {
            try
            {
                if (cmbPaciente.SelectedValue == null) return;
                int idPaciente = (int)cmbPaciente.SelectedValue;
                dgvConsultas.DataSource = consultaService.ListarConsultasDoPaciente(idPaciente);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar consultas: " + ex.Message);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvConsultas.CurrentRow != null &&
                    dgvConsultas.CurrentRow.DataBoundItem is Consulta consulta)
                {
                    consultaService.CancelarConsulta(consulta.Id);
                    MessageBox.Show("Consulta cancelada.");
                    CarregarConsultas();
                }
                else
                {
                    MessageBox.Show("Selecione uma consulta na lista.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
