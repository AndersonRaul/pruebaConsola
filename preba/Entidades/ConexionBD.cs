using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace preba.Entidades
{
    public class ConexionDB
    {
        //PARAMETROS DE CONEXION
        //private static string Host = "198.74.55.66";
        //private static string User = "postgres";
        //private static string DBname = "jake_bd";
        //private static string Password = "jake060222$.";
        //private static string Port = "5432";

        private NpgsqlConnection connection;
        private string connectionString;


        public ConexionDB()
        {
            connectionString = GetConnectionString();
        }

        public NpgsqlConnection Connection { get => connection; }

        public string GetConnectionString()
        {
            //return "Host=localhost;Username=postgres;Database=es_dg_01;Port=5432;Password=passs;SSL Mode=Prefer";
            return "Host=198.74.55.66;Username=postgres;Database=sis_jake_bd;Port=5432;Password=jake060222$.;SSL Mode=Prefer";
        }

        private void SetConnectionString(string bdName)
        {
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder(GetConnectionString());
            builder.Database = bdName;
            connectionString = builder.ConnectionString;
        }

        public void Conectar(string dbName = "")
        {
            connection = new NpgsqlConnection(connectionString);
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
            }
            catch (Exception ex)
            {
                connection = null;
                throw ex;
            }
        }

        public void Desconectar()
        {
            if (connection != null)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }

        }

        public bool CheckConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                return true;
            else
                return false;
        }

        public string UpdateConnectionString(string value, string cadena = "")
        {
            NpgsqlConnectionStringBuilder builder;
            if (cadena == "")
            {
                builder = new NpgsqlConnectionStringBuilder(GetConnectionString());
            }
            else
            {
                builder = new NpgsqlConnectionStringBuilder(cadena);
            }

            builder.Host = value;
            return builder.ConnectionString;
        }

        public string GetHost()
        {
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder(GetConnectionString());
            return builder.Host;
        }

        public string GetPass()
        {
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder(GetConnectionString());
            return builder.Password;
        }
    }
    }
