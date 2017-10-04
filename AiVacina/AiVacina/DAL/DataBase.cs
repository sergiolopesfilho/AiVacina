using AiVacina.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
namespace AiVacina.DAL
{
    public static class DataBase
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["mySqlAiVacina"].ToString();
        //private static string connectionString = ConfigurationManager.ConnectionStrings["sqlAiVacina"].ConnectionString;
        

        public static bool CadastrarPaciente(Paciente paciente)
        {
            bool cadastrado = false;
            string insertEndereco = "INSERT INTO enderecos (rua,bairro,cidade) "
                                + "VALUES (@rua, @bairro, @cidade)";
            string insertPaciente = "INSERT INTO pacientes (cartaoCidadao,nome,dataNascimento,senha,idEndereco) "
                                + "VALUES (@cartao, @nome, @nascimento,@senha, @idEndereco)";

            try
            {
                using (IDbConnection conn = new MySqlConnection(connectionString))
                {
                    paciente.endereco.id = conn.Execute(insertEndereco, new
                    {
                        rua = paciente.endereco.rua,
                        bairro = paciente.endereco.bairro,
                        cidade = paciente.endereco.cidade
                    });

                    conn.Execute(insertPaciente, new
                    {
                        cartao= paciente.numCartaoCidadao,
                        nome = paciente.nome,
                        nascimento = paciente.dataNascimento,
                        senha = paciente.senha,
                        idEndereco = paciente.endereco.id
                    });
                }

                return cadastrado;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static IEnumerable<Vacina> ListaVacinas()
        {
            string listaVacinas = "SELECT codVacina, loteVacina, nomeVacina, quantidade, dataValidade,grupoalvo "
                                + "FROM vacinas";
            IEnumerable<Vacina> vacinas;
            try
            {
                using (IDbConnection conn = new MySqlConnection(connectionString))
                {
                    vacinas = conn.Query<Vacina>(listaVacinas);
                }

                return vacinas;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static IEnumerable<Posto> ListaPostos()
        {
            string listaPostos = "SELECT * FROM postos LEFT JOIN enderecos "
                                + "ON postos.idEndereco = enderecos.id";
            IEnumerable<Posto> postos;
            try
            {
                using (IDbConnection conn = new MySqlConnection(connectionString))
                {
                    postos = conn.Query<Posto, Endereco, Posto>(listaPostos, 
                        (posto, endereco) => { posto.endereco = endereco; return posto; });
                }

                return postos;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static bool SalvaAgendamento(AgendamentoVacina agendamento)
        {
            bool agendado = false;

            string insertAgendamento = "INSERT INTO AgendamentoVacinas(idPosto, idVacina, cartaocidadao, dataAgendamento) "
                              + "VALUES(@posto, @vacina, @cidadao, @data)";
            try
            {
                int resultado = 0;
                using (IDbConnection conn = new MySqlConnection(connectionString))
                {
                    resultado = conn.Execute(insertAgendamento, new
                    {
                        posto = agendamento.idPosto,
                        vacina = agendamento.idVacina,
                        cidadao = agendamento.cartaocidadao,
                        data = agendamento.dataAgendamento
                    });
                }

                agendado = (resultado > 0);

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return agendado;
        }

        public static IEnumerable<AgendaVacina> AgendamentosVacina(string cartaoCidadao)
        {
            IEnumerable<AgendaVacina> agendamentos;

            string listaAgendamentos = "SELECT agendamento.id, p.nomeEstabelecimento, e.rua, e.bairro, v.nomeVacina, agendamento.dataAgendamento "
                                + "FROM agendamentovacinas agendamento "
                                + "JOIN postos p on p.idEstabelecimento = agendamento.idPosto "
                                + "JOIN vacinas v on v.codVacina = agendamento.idVacina "
                                + "JOIN enderecos e on e.id = p.idEndereco "
                                + "WHERE agendamento.cartaocidadao = @cartao ";
            try
            {
                using (IDbConnection conn = new MySqlConnection(connectionString))
                {
                    agendamentos = conn.Query<AgendaVacina>(listaAgendamentos, new
                    {
                        cartao = cartaoCidadao,
                    });
                }

                return agendamentos;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }



    }
}