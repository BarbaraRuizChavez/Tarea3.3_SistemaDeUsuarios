using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}


