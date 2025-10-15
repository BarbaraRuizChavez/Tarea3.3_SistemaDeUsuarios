using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeUsuarios
{
    public class UsuarioRepository
    {
        public bool RegistrarUsuario(Usuario usuario)
        {
            try
            {
                using (var conexion = ConexionBD.ObtenerConexion())
                {
                    conexion.Open();

                    string query = @"INSERT INTO usuarios 
                                (nombre, apellidos, usuario, password, correo, telefono, fecha_nac)
                                VALUES (@nombre, @apellidos, @usuario, @password, @correo, @telefono, @fecha_nac)";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
                        cmd.Parameters.AddWithValue("@apellidos", usuario.Apellidos);
                        cmd.Parameters.AddWithValue("@usuario", usuario.NombreUsuario);
                        cmd.Parameters.AddWithValue("@password", usuario.Password);
                        cmd.Parameters.AddWithValue("@correo", string.IsNullOrWhiteSpace(usuario.Correo) ? "" : usuario.Correo);
                        cmd.Parameters.AddWithValue("@telefono", string.IsNullOrWhiteSpace(usuario.Telefono) ? "" : usuario.Telefono);
                        cmd.Parameters.AddWithValue("@fecha_nac", usuario.FechaNacimiento);

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    conexion.Close();
                    conexion.Dispose();
                }

                return true;
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                    throw new Exception("El nombre de usuario ya existe. Elige otro.");
                else
                    throw new Exception("Error de base de datos: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el usuario: " + ex.Message);
            }
        }

        public bool ValidarUsuario(string usuario, string password)
        {
            try
            {
                using (var conexion = ConexionBD.ObtenerConexion())
                {
                    if (conexion.State != System.Data.ConnectionState.Open)
                        conexion.Open();

                    string query = "SELECT COUNT(*) FROM usuarios WHERE usuario=@usuario AND password=@password";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@password", ConvertirSHA1(password));

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error de MySQL al validar usuario: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar usuario: " + ex.Message);
            }
        }

        private string ConvertirSHA1(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }

        public List<Usuario> ObtenerUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                using (var conexion = ConexionBD.ObtenerConexion())
                {
                    conexion.Open();

                    string query = "SELECT nombre, apellidos, usuario, correo FROM usuarios";
                    MySqlCommand cmd = new MySqlCommand(query, conexion);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Usuario u = new Usuario
                        {
                            Nombre = dr["nombre"].ToString(),
                            Apellidos = dr["apellidos"].ToString(),
                            NombreUsuario = dr["usuario"].ToString(),
                            Correo = dr["correo"].ToString()
                        };
                        lista.Add(u);
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener usuarios: " + ex.Message);
            }

            return lista;
        }
    }
}


