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


        
    }
}