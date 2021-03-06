﻿using AiVacina.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System;
using System.Linq;

namespace AiVacina.DAL
{
    public static class DataBase
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["azureAiVacina"].ToString();
        //private static string connectionString = ConfigurationManager.ConnectionStrings["sqlAiVacina"].ConnectionString;
        private static List<String> perfisPaciente = new List<string>() {
            "Paciente", "Administrador"
        };

        public static bool CadastrarPaciente(Paciente paciente)
        {
            int cadastrado = 0;
            string insertEndereco = "INSERT INTO enderecos (rua,bairro,cidade) "
                                + "VALUES (@rua, @bairro, @cidade); "
                                + "SELECT CAST(SCOPE_IDENTITY() as int) ";
            string insertPaciente = "INSERT INTO pacientes (cartaoCidadao,nome,dataNascimento,senha,idEndereco,perfil,email) "
                                + "VALUES (@cartao, @nome, @nascimento,@senha, @idEndereco,'Paciente',@email)";

            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    paciente.endereco.id = conn.Query<int>(insertEndereco, new
                    {
                        rua = paciente.endereco.rua,
                        bairro = paciente.endereco.bairro,
                        cidade = paciente.endereco.cidade
                    }).Single();

                    cadastrado = conn.Execute(insertPaciente, new
                    {
                        cartao = paciente.numCartaoCidadao,
                        nome = paciente.nome,
                        nascimento = paciente.data,
                        senha = paciente.senha,
                        idEndereco = paciente.endereco.id,
                        email = String.IsNullOrEmpty(paciente.email)? null: paciente.email
                    });
                }

                return (cadastrado > 0);
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public static bool CadastrarVacina(Vacina vacina)
        {
            bool cadastrado = false;
            string insertVacina = "INSERT INTO Vacinas (codVacina,loteVacina,nomeVacina,quantidade,dataValidade,grupoalvo,postoCNPJ) "
                                + "VALUES (@cod, @lote, @nome, @quant, @data, @grupo,@cnpj)";

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
                        data = DateTime.Parse(vacina.dataValidade),
                        //data = vacina.dataValidade,
                        grupo = vacina.grupoAlvo,
                        cnpj = vacina.postoCNPJ
                    });
                }
                cadastrado = (result > 0);
                return cadastrado;
            }
            catch (SqlException ex)
            {
                throw new Exception("Código e/ou lote da vacina ja cadastrados.");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao cadastrar a vacina no banco.");
            }
        }

        public static IEnumerable<Vacina> ListaVacinas()
        {
            string listaVacinas = "SELECT codVacina, loteVacina, nomeVacina, quantidade, dataValidade,grupoalvo,postoCNPJ "
                                + "FROM vacinas "
                                + "WHERE quantidade > 0 "
                                +"ORDER BY dataValidade";

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
        
        public static IEnumerable<Vacina> ListaVacinasPorLote(string lote)
        {
            string listaVacinas = "SELECT codVacina, loteVacina, nomeVacina, quantidade, dataValidade,grupoalvo,postoCNPJ "
                                + "FROM vacinas "
                                + "WHERE quantidade > 0 AND loteVacina = @lote "
                                + "ORDER BY dataValidade";

            IEnumerable<Vacina> vacinas;
            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    vacinas = conn.Query<Vacina>(listaVacinas, new { lote = lote });
                }

                return vacinas;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static IEnumerable<Vacina> ListaVacinas(string cnpj)
        {
            string listaVacinas = "SELECT codVacina, loteVacina, nomeVacina, quantidade, dataValidade,grupoalvo, postoCNPJ "
                                + "FROM vacinas WHERE postoCNPJ = @cnpj and quantidade > 0 "
                                + "ORDER BY dataValidade";
            IEnumerable<Vacina> vacinas;
            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    vacinas = conn.Query<Vacina>(listaVacinas, new { cnpj = cnpj});
                }

                return vacinas;
            }
            catch (SqlException ex)
            {
                throw new Exception("Não foi possível cadastrar esta vacina, tente novamente mais tarde.");
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

        public static IEnumerable<Posto> ListaPostosPorVacina(string lote)
        {
            string listaPostos = "SELECT * FROM postos LEFT JOIN enderecos "
                                + "ON postos.idEndereco = enderecos.id "
                                + "WHERE cnpj = (select postoCNPJ from vacinas where codVacina = @lote)";
            IEnumerable<Posto> postos;
            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    postos = conn.Query<Posto, Endereco, Posto>(listaPostos,
                        (posto, endereco) => { posto.endereco = endereco; return posto; },
                        new { lote = lote});
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
            string listaPostos =  "UPDATE postos SET admPosto = @adm, "
                                + "cpfAdmPosto = @cpf "
                                + "WHERE cnpj = @cnpj "
                                + "INSERT INTO pacientes(nome, senha,idEndereco, perfil,cpfAdm, cartaoCidadao ) "
                                + "VALUES(@adm, @senha, 1, 'Administrador', @cpf, @cartaoCidadao) ";
                                

            
            int atualizado = 0;
            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    atualizado = conn.Execute(listaPostos, new
                    {
                        adm = posto.admPosto,
                        senha = posto.senha,
                        cpf = posto.cpfAdmPosto,
                        cnpj = posto.cnpj,
                        cartaoCidadao = posto.cpfAdmPosto
                    });
                }
                if(atualizado <= 0 )
                    throw new Exception("Posto não atualizado. Houve um erro interno, favor contate o administrador.");
            }
            catch (SqlException ex)
            {
                throw new Exception("Houve um erro ao realizar o cadastro do administrador. Contate o suporte.");
            }
        }
        
        public static bool SalvaAgendamento(AgendamentoVacina agendamento)
        {
            bool agendado = false;
            //string insertAgendamento = "INSERT INTO AgendamentoVacinas(idPosto, idVacina, cartaocidadao, dataAgendamento) "
            //                  + "VALUES(@posto, @vacina, @cidadao, @data)";


            string insertAgendamento = "IF ((SELECT COUNT(*) FROM AgendamentoVacinas WHERE dataAgendamento = @data) < 2 "
                                       + "AND (SELECT COUNT(*) FROM AgendamentoVacinas WHERE dataAgendamento = @data "
                                       + "AND cartaocidadao = @cidadao AND idPosto = @posto) < 1) "
                                       + "BEGIN "
                                            + "INSERT INTO AgendamentoVacinas(idPosto, idVacina, cartaocidadao, dataAgendamento) "
                                            + "VALUES(@posto, @vacina, @cidadao, @data) "
                                       + "END ELSE BEGIN "
                                            + "RAISERROR('As vagas para este horario estão cheias ou você ja tem um agendamento neste horário.', 16, 16) "
                                       + "END"; 
                                      

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
                throw new Exception("Não foi possível realizar o agendamento. Por favor, tente mais tarde.");
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível realizar o agendamento. Por favor, tente mais tarde.");
            }
            return agendado;
        }

        public static bool SalvaVacinaAplicada(string vacina, DateTime dataVacinacao, DateTime dataReforco, string cartao, string cnpj)
        {
            string insertVacina = "INSERT INTO VacinasAplicadas(vacina, dataVacinação, dataReforco, idCarteira,posto) "
                                + "VALUES(@vacina, @dataAplicada, @dataReforceo, "
                                + "(SELECT id from CarteiraVacinacao WHERE cartaoCidadao = @cartao), "
                                + " (SELECT nomeEstabelecimento from postos WHERE cnpj = @cnpj)) ";
            bool agendado = false;

            try
            {
                int resultado = 0;
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    resultado = conn.Execute(insertVacina, new
                    {
                        vacina = vacina,
                        dataAplicada = dataVacinacao,
                        dataReforceo = dataReforco,
                        cartao = cartao,
                        cnpj = cnpj
                    });
                }

                agendado = (resultado > 0);

            }
            catch (SqlException ex)
            {
                throw new Exception("Não foi possível adicionar a vacina "+vacina +" a carteira de Vacinacao. Por favor, tente mais tarde.");
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro, por favor contate o adminstrador e tente mais tarde.");
            }
            return agendado;
        }

        public static bool SalvaVacinaAplicada(string vacina, DateTime dataVacinacao, string cartao, string posto)
        {
            string insertVacina = "INSERT INTO VacinasAplicadas(vacina, dataVacinação, idCarteira, posto) "
                                + "VALUES(@vacina, @dataAplicada, "
                                + "(SELECT id from CarteiraVacinacao WHERE cartaoCidadao = @cartao),@posto) ";
            bool agendado = false;

            try
            {
                int resultado = 0;
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    resultado = conn.Execute(insertVacina, new
                    {
                        vacina = vacina,
                        dataAplicada = dataVacinacao,
                        cartao = cartao,
                        posto = posto
                    });
                }

                agendado = (resultado > 0);

            }
            catch (SqlException ex)
            {
                throw new Exception("Não foi possível adicionar a vacina " + vacina + " a carteira de Vacinacao. Por favor, tente mais tarde.");
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro, por favor contate o adminstrador e tente mais tarde.");
            }
            return agendado;
        }

        public static bool CadastraCarteiraVacinacao(CarteiraVacinacao carteira)
        {
            bool agendado = false;

            string insertCarteira = "INSERT INTO CarteiraVacinacao(cartaoCidadao,nomeCompleto,dataNascimento, dataCadastro) "
                              + "VALUES(@cartao,@nome, (SELECT dataNascimento FROM Pacientes WHERE cartaoCidadao = @cartao), GETDATE())";
            try
            {
                int resultado = 0;
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    resultado = conn.Execute(insertCarteira, new
                    {
                        cartao = carteira.numCartaoCidadao,
                        nome = carteira.nome
                    });
                }

                agendado = (resultado > 0);

            }
            catch (SqlException ex)
            {
                throw new Exception("Houve um erro no cadastro de sua carteira. Por favor tente mais tarde.");
            }
            return agendado;
        }

        public static bool CarteiraVacinacaoCadastrada(string cartao)
        {
            string selectCarteira = "SELECT carteira.id,carteira.nomeCompleto as nome,carteira.dataNascimento,carteira.cartaoCidadao as numCartaoCidadao "
                                    + "FROM carteiravacinacao carteira WHERE carteira.cartaoCidadao = @cartao";

            try
            {
                CarteiraVacinacao carteira = null;
                using (IDbConnection conn = new SqlConnection(connectionString))
                {

                    carteira = conn.Query<CarteiraVacinacao>(selectCarteira,
                         new { cartao = cartao }).First();
                }

                return (carteira !=null);

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static CarteiraVacinacao GetCarteiraVacinacao(string cartao)
        {
            string selectCarteira = "SELECT carteira.id,carteira.nomeCompleto as nome,carteira.dataNascimento,carteira.cartaoCidadao as numCartaoCidadao, carteira.dataCadastro as dataCadastro "
                                    + "FROM carteiravacinacao carteira WHERE carteira.cartaoCidadao = @cartao";
            string selectVacinas = "SELECT vacinas.idVacinaAplicada,vacinas.vacina,vacinas.dataVacinação,vacinas.dataReforco, vacinas.posto "
                                    + "FROM  VacinasAplicadas vacinas WHERE vacinas.idCarteira = @idCarteira ";

            try
            {
                CarteiraVacinacao carteira = null;
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    //carteira = conn.Query<CarteiraVacinacao, Posto, CarteiraVacinacao>(selectCarteira,
                    //     (carteiravacinacao, posto) => { carteiravacinacao.Posto = posto; return carteiravacinacao; },
                    //    new { cartao = cartao },
                    //    splitOn: "idEstabelecimento").First();

                   carteira = conn.Query<CarteiraVacinacao>(selectCarteira,
                        new { cartao = cartao }).First();

                    carteira.minhasVacinas = conn.Query<VacinaAplicada > (selectVacinas,
                        new { idCarteira = carteira.id }).ToList();
                }

                return carteira;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AdicionarHorariosBloqueados(HorariosBloqueados horarios)
        {
            bool bloqueado = false;

            string insertCarteira = "INSERT INTO HorariosCancelados(dia, horarios) "
                              + "VALUES(@diaBloquear,@horarioBloquear)";
            try
            {
                int resultado = 0;
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    resultado = conn.Execute(insertCarteira, new
                    {
                        diaBloquear = horarios.diaBloqueado,
                        horarioBloquear = horarios.horariosBloqueados
                    });
                }

                bloqueado = (resultado > 0);

            }
            catch (SqlException ex)
            {
                return false;
            }
            return bloqueado;
        }

        public static string GetHorariosBloqueados(string data)
        {
            string selectHorarios = "SELECT horarios from HorariosCancelados "
                                    + "WHERE dia = @diaConsulta";
            try
            {
                String resultado = String.Empty;
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    resultado = conn.QueryFirst<String>(selectHorarios, new
                    {
                        diaConsulta = data
                    });
                }

                return resultado;

            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        public static IEnumerable<AgendaVacina> AgendamentosVacina(string cartaoCidadao)
        {
            IEnumerable<AgendaVacina> agendamentos;

            string listaAgendamentos = "SELECT agendamento.id, p.nomeEstabelecimento, e.rua, e.bairro, v.nomeVacina, agendamento.dataAgendamento "
                                    + "FROM agendamentovacinas agendamento "
                                    + "JOIN postos p on p.idEstabelecimento = agendamento.idPosto "
                                    + "JOIN vacinas v on v.codVacina = agendamento.idVacina "
                                    + "JOIN enderecos e on e.id = p.idEndereco "
                                    + "WHERE agendamento.cartaocidadao = @cartao AND agendamento.dataAgendamento > GETDATE() "
                                    + "ORDER BY agendamento.dataAgendamento";
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

        public static AgendaVacina GetProximoAgendamento(string cartaoCidadao)
        {
            AgendaVacina agendamentos;

            string listaAgendamentos = "SELECT agendamento.id, p.nomeEstabelecimento, e.rua, e.bairro, v.nomeVacina, agendamento.dataAgendamento "
                                    + "FROM agendamentovacinas agendamento "
                                    + "JOIN postos p on p.idEstabelecimento = agendamento.idPosto "
                                    + "JOIN vacinas v on v.codVacina = agendamento.idVacina "
                                    + "JOIN enderecos e on e.id = p.idEndereco "
                                    + "WHERE agendamento.cartaocidadao = @cartao "
                                    + "AND agendamento.dataAgendamento >= GETDATE()"
                                    + "ORDER BY agendamento.dataAgendamento;";
            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    agendamentos = conn.QueryFirst<AgendaVacina>(listaAgendamentos, new
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

        public static IEnumerable<AgendaVacina> AgendamentosPosto(string cnpj)
        {
            IEnumerable<AgendaVacina> agendamentos;

            string listaAgendamentos = "SELECT agendamento.id, p.nomeEstabelecimento, e.rua, e.bairro, v.nomeVacina, agendamento.dataAgendamento "
                                    + "FROM agendamentovacinas agendamento "
                                    + "JOIN postos p on p.idEstabelecimento = agendamento.idPosto "
                                    + "JOIN vacinas v on v.codVacina = agendamento.idVacina "
                                    + "JOIN enderecos e on e.id = p.idEndereco "
                                    + "WHERE p.cnpj = @cnpj AND agendamento.dataAgendamento > GETDATE() "
                                    + "ORDER BY agendamento.dataAgendamento ";
            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    agendamentos = conn.Query<AgendaVacina>(listaAgendamentos, new
                    {
                        cnpj = cnpj,
                    });
                }

                return agendamentos;
            }
            catch (SqlException ex)
            {
                return null;
            }
        }

        public static IEnumerable<AgendaVacina> AgendamentosPosto(string cnpj, DateTime data)
        {
            IEnumerable<AgendaVacina> agendamentos;

            string listaAgendamentos = "SELECT agendamento.id, p.nomeEstabelecimento, e.rua, e.bairro, v.nomeVacina, agendamento.dataAgendamento "
                                    + "FROM agendamentovacinas agendamento "
                                    + "JOIN postos p on p.idEstabelecimento = agendamento.idPosto "
                                    + "JOIN vacinas v on v.codVacina = agendamento.idVacina "
                                    + "JOIN enderecos e on e.id = p.idEndereco "
                                    + "WHERE p.cnpj = @cnpj and CAST(agendamento.dataAgendamento as date) = @data "
                                    + "ORDER BY agendamento.dataAgendamento";
            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    agendamentos = conn.Query<AgendaVacina>(listaAgendamentos, new
                    {
                        cnpj = cnpj,
                        data = data
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

        public static bool DeletaVacina(string loteVacina, string postoCNPJ)
        {

            string deletVacinas = "DELETE FROM Vacinas  "
                              + "WHERE loteVacina = @lote AND postoCNPJ = @cnpj";
            bool deletado = false;
            try
            {
                int resultado = 0;
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    resultado = conn.Execute(deletVacinas, new
                    {
                        lote = loteVacina,
                        cnpj =  postoCNPJ
                    });
                }

                deletado = (resultado > 0);

            }
            catch (SqlException ex)
            {
                throw new Exception("A vacina não pôde ser deletada, tente novamente mais tarde.");
            }

            return deletado;
        }

        public static PacienteLogin GetLoginPaciente(string cartaoCidadao)
        {

            string selectPaciente = "SELECT nome, cartaoCidadao as numCartaoCidadao, senha, perfil, email from pacientes "
                                    + "WHERE cartaoCidadao = @cartao";
            try
            {
                PacienteLogin paciente;
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    paciente = conn.QueryFirst<PacienteLogin>(selectPaciente, new
                    {
                        cartao = cartaoCidadao
                    });
                }
                if (paciente == null)
                {
                    throw new Exception("Usuário inexistente, por favor cadastre-se.");
                }
                else if (perfisPaciente.Contains(paciente.perfil))
                {
                    return paciente;
                }
                else
                {
                    throw new Exception("Você não tem autorização para acessar essa página. Contate o administrador do posto.");
                }
                

            }
            catch (Exception ex)
            {
                if(ex.Message.Equals("A sequência não contém elementos"))
                    throw new Exception("Paciente não encontrado. Por favor, cadastre-se.");
                throw new Exception(ex.Message);
            }
        }

        public static PacienteLogin GetLoginPacienteCPF(string cpf)
        {

            string selectPaciente = "SELECT pacientes.nome, pacientes.cpfAdm as numCartaoCidadao, pacientes.senha, pacientes.perfil, postos.cnpj "
                                    + "FROM pacientes JOIN postos on postos.cpfAdmPosto = @cpf "
                                    + "WHERE cpfAdmPosto = @cpf";
            try
            {
                PacienteLogin paciente;
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    paciente = conn.QueryFirst<PacienteLogin>(selectPaciente, new
                    {
                        cpf = cpf
                    });
                }
                if (paciente == null)
                {
                    throw new Exception("Usuário inexistente, por favor cadastre-se.");
                }
                else if (perfisPaciente.Contains(paciente.perfil))
                {
                    return paciente;
                }
                else
                {
                    throw new Exception("Você não tem autorização para acessar essa página. Contate o administrador do posto.");
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static String GetRolesPaciente(string cartaoCidadao)
        {
            string selectPaciente = "SELECT perfil from pacientes "
                                    + "WHERE cartaoCidadao = @cartao";
            try
            {
                String perfil;
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    perfil = conn.QueryFirst<String>(selectPaciente, new
                    {
                        cartao = cartaoCidadao
                    });
                }

                return perfil;

            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        public static Paciente GetDadosPaciente(string cartao)
        {
            string selectPaciente = "SELECT p.cartaoCidadao as numCartaoCidadao, p.nome, p.dataNascimento,  "
                                  + "e.id, e.rua, e.bairro, e.cidade "
                                  + "FROM pacientes p join Enderecos e on p.idEndereco = e.id "
                                  + "WHERE cartaoCidadao = @cartao";

            try
            {
                Paciente paciente = null;
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    paciente = conn.Query<Paciente,Endereco,Paciente>(selectPaciente, 
                        (paci, end) => { paci.endereco = end; return paci; }, new
                    {
                        cartao = cartao
                    }, splitOn: "id").First();
                }

                return paciente;

            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao acessar os dados do paciente. Por favor tente mais tarde.");
            }
        }
    }
}