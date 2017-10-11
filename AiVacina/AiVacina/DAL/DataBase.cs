using AiVacina.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System;

namespace AiVacina.DAL
{
    public static class DataBase
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["azureAiVacina"].ToString();
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
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    paciente.endereco.id = conn.Execute(insertEndereco, new
                    {
                        rua = paciente.endereco.rua,
                        bairro = paciente.endereco.bairro,
                        cidade = paciente.endereco.cidade
                    });

                    conn.Execute(insertPaciente, new
                    {
                        cartao = paciente.numCartaoCidadao,
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

        public static bool CadastrarVacina(Vacina vacina)
        {
            bool cadastrado = false;
            string insertVacina = "INSERT INTO Vacinas (codVacina,loteVacina,nomeVacina,quantidade,dataValidade,grupoalvo) "
                                + "VALUES (@cod, @lote, @nome, @quant, @data, @grupo)";

            try
            {
                var result = 0;
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    result = conn.Execute(insertVacina, new
                    {
                        cod = vacina.codVacina,
                        lote = vacina.loteVacina,
                        nome = vacina.nomeVacina,
                        quant = vacina.quantidade,
                        data = vacina.dataValidade,
                        grupo = vacina.grupoAlvo
                    });
                }
                cadastrado = (result > 0);
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
                using (IDbConnection conn = new SqlConnection(connectionString))
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
                using (IDbConnection conn = new SqlConnection(connectionString))
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

        public static void AtualizarAdmPosto(Posto posto)
        {
            string listaPostos = "UPDATE postos SET admPosto = @adm, "
                                + "cpfAdmPosto = @cpf "
                                + "WHERE idEstabelecimento = @id";
            int atualizado = 0;
            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    atualizado = conn.Execute(listaPostos, new
                    {
                        adm = posto.admPosto,
                        cpf = posto.cpfAdmPosto,
                        id = posto.idEstabelecimento
                    });
                }
                if(atualizado <= 0 )
                    throw new Exception("Posto não atualizado. Houve um erro interno, favor contate o administrador.");
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// TODO:
        /// Arrumar o datetime, não reconhece por ser EN
        public static bool SalvaAgendamento(AgendamentoVacina agendamento)
        {
            bool agendado = false;

            string insertAgendamento = "INSERT INTO AgendamentoVacinas(idPosto, idVacina, cartaocidadao, dataAgendamento) "
                              + "VALUES(@posto, @vacina, @cidadao, @data)";
            try
            {
                int resultado = 0;
                using (IDbConnection conn = new SqlConnection(connectionString))
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

        public static bool CadastraCarteiraVacinacao(CarteiraVacinacao carteira)
        {
            bool agendado = false;

            string insertCarteira = "INSERT INTO CarteiraVacinacao(cartaoCidadao, idPosto) "
                              + "VALUES(@cartao,@posto)";
            try
            {
                int resultado = 0;
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    resultado = conn.Execute(insertCarteira, new
                    {
                        posto = carteira.Posto.idEstabelecimento,
                        cartao = carteira.numCartaoCidadao
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
                using (IDbConnection conn = new SqlConnection(connectionString))
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

        public static bool DeletaAgendamento(int id) {

            string deletAgendamento = "DELETE FROM AgendamentoVacinas  "
                              + "WHERE id = @id";
            bool deletado = false;
            try
            {
                int resultado = 0;
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    resultado = conn.Execute(deletAgendamento, new
                    {
                        id = id
                    });
                }

                deletado = (resultado > 0);

            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return deletado;
        }


    }
}