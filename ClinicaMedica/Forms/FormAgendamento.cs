using System;
using System.Drawing;
using System.Windows.Forms;
using ClinicaMedica.Models;
using ClinicaMedica.Data;
using ClinicaMedica.Services;

namespace ClinicaMedica.Forms
{
    // =================================================================
    // TELA DE AGENDAMENTO (RF03) - a tela central do sistema.
    // Fluxo: escolher paciente -> especialidade -> médico -> dia ->
    // horário -> agendar. Usa os DAOs (Igor) + o ConsultaService.
    // =================================================================
    public class FormAgendamento : Form
    {
        private Label lblPaciente = new Label();
        private Label lblEspecialidade = new Label();
        private Label lblMedico = new Label();
        private Label lblData = new Label();
        private Label lblHorario = new Label();

        private ComboBox cmbPaciente = new ComboBox();
        private ComboBox cmbEspecialidade = new ComboBox();
        private ComboBox cmbMedico = new ComboBox();
        private DateTimePicker dtpData = new DateTimePicker();
        private ComboBox cmbHorario = new ComboBox();
        private Button btnAgendar = new Button();

        // Acesso a dados e regras de negócio.
        private PacienteDAO pacienteDAO = new PacienteDAO();
        private EspecialidadeDAO especialidadeDAO = new EspecialidadeDAO();
        private MedicoDAO medicoDAO = new MedicoDAO();
        private ConsultaService consultaService = new ConsultaService();

        public FormAgendamento()
        {
            ConstruirTela();
            CarregarPacientes();
            CarregarEspecialidades();
            CarregarHorarios();
        }

        private void ConstruirTela()
        {
            this.Text = "Agendar Consulta";
            this.Size = new Size(460, 360);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            int x1 = 30;
            int x2 = 150;
            int largura = 270;

            lblPaciente.Text = "Paciente:"; lblPaciente.Location = new Point(x1, 30); lblPaciente.AutoSize = true;
            cmbPaciente.Location = new Point(x2, 27); cmbPaciente.Width = largura;
            cmbPaciente.DropDownStyle = ComboBoxStyle.DropDownList;

            lblEspecialidade.Text = "Especialidade:"; lblEspecialidade.Location = new Point(x1, 70); lblEspecialidade.AutoSize = true;
            cmbEspecialidade.Location = new Point(x2, 67); cmbEspecialidade.Width = largura;
            cmbEspecialidade.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEspecialidade.SelectedIndexChanged += (s, e) => CarregarMedicos();

            lblMedico.Text = "Médico:"; lblMedico.Location = new Point(x1, 110); lblMedico.AutoSize = true;
            cmbMedico.Location = new Point(x2, 107); cmbMedico.Width = largura;
            cmbMedico.DropDownStyle = ComboBoxStyle.DropDownList;

            lblData.Text = "Data:"; lblData.Location = new Point(x1, 150); lblData.AutoSize = true;
            dtpData.Location = new Point(x2, 147); dtpData.Width = largura;
            dtpData.Format = DateTimePickerFormat.Short;
            dtpData.MinDate = DateTime.Today; // não deixa escolher dia passado

            lblHorario.Text = "Horário:"; lblHorario.Location = new Point(x1, 190); lblHorario.AutoSize = true;
            cmbHorario.Location = new Point(x2, 187); cmbHorario.Width = largura;
            cmbHorario.DropDownStyle = ComboBoxStyle.DropDownList;

            btnAgendar.Text = "Agendar";
            btnAgendar.Location = new Point(x2, 240);
            btnAgendar.Width = largura;
            btnAgendar.Click += BtnAgendar_Click;

            this.Controls.Add(lblPaciente);      this.Controls.Add(cmbPaciente);
            this.Controls.Add(lblEspecialidade); this.Controls.Add(cmbEspecialidade);
            this.Controls.Add(lblMedico);        this.Controls.Add(cmbMedico);
            this.Controls.Add(lblData);          this.Controls.Add(dtpData);
            this.Controls.Add(lblHorario);       this.Controls.Add(cmbHorario);
            this.Controls.Add(btnAgendar);
        }

        // Preenche a lista de pacientes.
        private void CarregarPacientes()
        {
            try
            {
                cmbPaciente.DisplayMember = "Nome"; // mostra o nome
                cmbPaciente.ValueMember = "Id";     // guarda o id
                cmbPaciente.DataSource = pacienteDAO.ListarTodos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar pacientes: " + ex.Message);
            }
        }

        // Preenche a lista de especialidades.
        private void CarregarEspecialidades()
        {
            try
            {
                cmbEspecialidade.DisplayMember = "Nome";
                cmbEspecialidade.ValueMember = "Id";
                cmbEspecialidade.DataSource = especialidadeDAO.ListarTodos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar especialidades: " + ex.Message);
            }
        }

        // Preenche os médicos conforme a especialidade escolhida.
        private void CarregarMedicos()
        {
            try
            {
                if (cmbEspecialidade.SelectedValue == null) return;

                int idEspecialidade = (int)cmbEspecialidade.SelectedValue;

                cmbMedico.DisplayMember = "Nome";
                cmbMedico.ValueMember = "Id";
                cmbMedico.DataSource = medicoDAO.ListarPorEspecialidade(idEspecialidade);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar médicos: " + ex.Message);
            }
        }

        // Preenche os horários disponíveis (de hora em hora, 08h às 17h).
        private void CarregarHorarios()
        {
            cmbHorario.Items.Clear();
            for (int hora = 8; hora <= 17; hora++)
            {
                cmbHorario.Items.Add(hora.ToString("00") + ":00");
            }
            if (cmbHorario.Items.Count > 0)
                cmbHorario.SelectedIndex = 0;
        }

        // Evento do botão Agendar.
        private void BtnAgendar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbPaciente.SelectedValue == null)
                    throw new Exception("Selecione um paciente.");
                if (cmbMedico.SelectedValue == null)
                    throw new Exception("Selecione um médico.");
                if (cmbHorario.SelectedItem == null)
                    throw new Exception("Selecione um horário.");

                Consulta consulta = new Consulta
                {
                    IdPaciente = (int)cmbPaciente.SelectedValue,
                    IdMedico = (int)cmbMedico.SelectedValue,
                    Data = dtpData.Value.Date,
                    Horario = TimeSpan.Parse(cmbHorario.SelectedItem.ToString())
                };

                // O service aplica as regras (horário livre, não no passado).
                consultaService.AgendarConsulta(consulta);

                MessageBox.Show("Consulta agendada com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
