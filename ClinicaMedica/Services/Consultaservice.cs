using System;
using System.Collections.Generic;
using ClinicaMedica.Models;
using ClinicaMedica.Data;

namespace ClinicaMedica.Services
{
    
    // SERVICE da Consulta.
    // Camada de REGRAS DE NEGOCIO do agendamento. Fica entre as telas
    // (Forms) e os DAOs: a tela chama o service, o service aplica as
    // regras e, se tudo estiver certo, usa os DAOs para gravar no banco.
    //
    // Quando uma regra e violada, lancamos uma Exception com uma
    // mensagem clara. A tela captura (try/catch) e mostra ao usuario.
    
    public class ConsultaService
    {
        // O service usa os DAOs para acessar o banco.
        private ConsultaDAO consultaDAO = new ConsultaDAO();
        private PagamentoDAO pagamentoDAO = new PagamentoDAO();

        
        // AGENDAR CONSULTA 
        // Aplica as regras antes de gravar.
        
        public void AgendarConsulta(Consulta consulta)
        {
            // Junta data + horario em um unico DateTime para comparar.
            DateTime dataHora = consulta.Data.Date + consulta.Horario;

            // REGRA 1: nao permitir agendamento em data/horario passado.
            if (dataHora < DateTime.Now)
                throw new Exception("Nao e possivel agendar em uma data ou horario que ja passou.");

            // REGRA 2 (RF04/RF05): o horario precisa estar livre para o medico.
            if (!consultaDAO.HorarioDisponivel(consulta.IdMedico, consulta.Data, consulta.Horario))
                throw new Exception("Este horario ja esta ocupado para o medico selecionado. Escolha outro.");

            // Define o estado inicial da consulta.
            consulta.Status = "agendada";
            consulta.IdPagamento = null; // o pagamento e registrado depois

            // Passou nas regras: grava no banco.
            consultaDAO.Inserir(consulta);
        }

        
        // CANCELAR CONSULTA 
        // Nao apaga o registro: apenas muda o status para "cancelada",
        // preservando o historico.
        
        public void CancelarConsulta(int idConsulta)
        {
            Consulta consulta = consultaDAO.BuscarPorId(idConsulta);

            if (consulta == null)
                throw new Exception("Consulta nao encontrada.");

            if (consulta.Status == "realizada")
                throw new Exception("Nao e possivel cancelar uma consulta ja realizada.");

            if (consulta.Status == "cancelada")
                throw new Exception("Esta consulta ja esta cancelada.");

            consulta.Status = "cancelada";
            consultaDAO.Atualizar(consulta);
        }

        
        // REMARCAR CONSULTA
        // Verifica se o novo horario esta livre antes de mover.
        
        public void RemarcarConsulta(int idConsulta, DateTime novaData, TimeSpan novoHorario)
        {
            Consulta consulta = consultaDAO.BuscarPorId(idConsulta);

            if (consulta == null)
                throw new Exception("Consulta nao encontrada.");

            if (consulta.Status != "agendada" && consulta.Status != "confirmada")
                throw new Exception("So e possivel remarcar consultas agendadas ou confirmadas.");

            DateTime dataHora = novaData.Date + novoHorario;
            if (dataHora < DateTime.Now)
                throw new Exception("Nao e possivel remarcar para uma data ou horario que ja passou.");

            // O novo horario precisa estar livre para o mesmo medico.
            if (!consultaDAO.HorarioDisponivel(consulta.IdMedico, novaData, novoHorario))
                throw new Exception("O novo horario ja esta ocupado. Escolha outro.");

            consulta.Data = novaData;
            consulta.Horario = novoHorario;
            consultaDAO.Atualizar(consulta);
        }

        
        // LISTAR CONSULTAS DO PACIENTE
        
        public List<Consulta> ListarConsultasDoPaciente(int idPaciente)
        {
            return consultaDAO.ListarPorPaciente(idPaciente);
        }

        
        // VERIFICAR DISPONIBILIDADE.
        // Atalho para a tela consultar se um horario esta livre antes
        // mesmo de tentar agendar (melhora a experiencia do usuario).
        
        public bool VerificarDisponibilidade(int idMedico, DateTime data, TimeSpan horario)
        {
            return consultaDAO.HorarioDisponivel(idMedico, data, horario);
        }
    }
}