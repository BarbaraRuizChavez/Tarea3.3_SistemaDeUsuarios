using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace SistemaDeUsuarios
{
    public static class ConexionBD
    {
        private static string servidor = "localhost";
        private static string bd = "sistemaUsuarios";
        private static string usuario = "root";
        private static string password = "b2r4c6h8";
        private static string cadenaConexion = $"Server={servidor};Database={bd};Uid={usuario};Pwd={password};";

        public static MySqlConnection ObtenerConexion()
        {
            var conexion = new MySqlConnection(cadenaConexion);
            conexion.Open();
            return conexion;
        }
    }
}

