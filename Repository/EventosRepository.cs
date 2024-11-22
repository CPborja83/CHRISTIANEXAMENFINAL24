using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CHRISTIANEXAMENFINAL.Models;
using CHRISTIANEXAMENFINAL.Data;
using CHRISTIANEXAMENFINAL.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;


namespace CHRISTIANEXAMENFINAL.Repository
{
    public class EventosRepository
    {
        private readonly string connectionString = "Server=svr-sql-ctezo.southcentralus.cloudapp.azure.com;Database=db_banco;User Id=usr_DesaWeb;Password=GuasTa360#;TrustServerCertificate=true";

        // Obtener todos los tipos de evento
        public async Task<List<TipoEvento>> ListarTiposEvento()
        {
            List<TipoEvento> tipos = new List<TipoEvento>();
            string query = "SELECT TipoEventoID, Nombre, Descripcion FROM TiposEvento";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                tipos.Add(new TipoEvento
                                {
                                    TipoEventoID = (int)reader["TipoEventoID"],
                                    Nombre = reader["Nombre"].ToString(),
                                    Descripcion = reader["Descripcion"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return tipos;
        }

        // Obtener todos los eventos corporativos
        public async Task<List<EventoCorporativo>> ListarEventos()
        {
            List<EventoCorporativo> eventos = new List<EventoCorporativo>();
            string query = @"
            SELECT E.EventoID, E.Nombre, E.Fecha, E.Ubicacion, E.Descripcion, 
                   T.Nombre AS TipoEventoNombre, T.Descripcion AS TipoEventoDescripcion
            FROM EventosCorporativos E
            JOIN TiposEvento T ON E.TipoEventoID = T.TipoEventoID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                eventos.Add(new EventoCorporativo
                                {
                                    EventoID = (int)reader["EventoID"],
                                    Nombre = reader["Nombre"].ToString(),
                                    Fecha = (DateTime)reader["Fecha"],
                                    Ubicacion = reader["Ubicacion"].ToString(),
                                    Descripcion = reader["Descripcion"].ToString(),
                                    TipoEvento = new TipoEvento
                                    {
                                        Nombre = reader["TipoEventoNombre"].ToString(),
                                        Descripcion = reader["TipoEventoDescripcion"].ToString()
                                    }
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return eventos;
        }

        public async Task<List<AsistenteEvento>> ListarAsistentes()
        {
            List<AsistenteEvento> asistentes = new List<AsistenteEvento>();
            string query = @"
    SELECT AA.AsistenteID, AA.Nombre, AA.Correo, AA.Telefono, AA.Rol, 
           E.EventoID, E.Nombre AS EventoNombre, E.Fecha, E.Ubicacion, E.Descripcion,
           T.TipoEventoID, T.Nombre AS TipoEventoNombre, T.Descripcion AS TipoEventoDescripcion
    FROM AsistentesEvento AA
    JOIN EventosCorporativos E ON AA.EventoID = E.EventoID
    JOIN TiposEvento T ON E.TipoEventoID = T.TipoEventoID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    Console.WriteLine("Conexión abierta");
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            Console.WriteLine("Leyendo resultados...");
                            while (await reader.ReadAsync())
                            {
                                Console.WriteLine("Asistente encontrado: " + reader["Nombre"]);
                                asistentes.Add(new AsistenteEvento
                                {
                                    AsistenteID = (int)reader["AsistenteID"],
                                    Nombre = reader["Nombre"].ToString(),
                                    Correo = reader["Correo"].ToString(),
                                    Telefono = reader["Telefono"].ToString(),
                                    Rol = reader["Rol"].ToString(),
                                    EventoID = (int)reader["EventoID"],
                                    Evento = new EventoCorporativo
                                    {
                                        EventoID = (int)reader["EventoID"],
                                        Nombre = reader["EventoNombre"].ToString(),
                                        Fecha = (DateTime)reader["Fecha"],
                                        Ubicacion = reader["Ubicacion"].ToString(),
                                        Descripcion = reader["Descripcion"].ToString(),
                                        TipoEvento = new TipoEvento
                                        {
                                            TipoEventoID = (int)reader["TipoEventoID"],
                                            Nombre = reader["TipoEventoNombre"].ToString(),
                                            Descripcion = reader["TipoEventoDescripcion"].ToString()
                                        }
                                    }
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener asistentes: " + ex.Message);
            }

            return asistentes;
        }




        public async Task<string> EditarTipos(TipoEvento tipos)
        {
            string query = @"
            UPDATE TiposEvento 
            SET nombre = @Nombre, descripcion = @Descripcion
            WHERE TipoEventoID = @TipoEventoID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TipoEventoID", tipos.TipoEventoID);
                        cmd.Parameters.AddWithValue("@Nombre", tipos.Nombre);
                        cmd.Parameters.AddWithValue("@Descripcion", tipos.Descripcion);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Insertar nuevo evento
        public async Task<string> GuardarEvento(EventoCorporativo evento)
        {
            string query = @"
            INSERT INTO EventosCorporativos (Nombre, Fecha, Ubicacion, TipoEventoID, Descripcion) 
            VALUES (@Nombre, @Fecha, @Ubicacion, @TipoEventoID, @Descripcion)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", evento.Nombre);
                        cmd.Parameters.AddWithValue("@Fecha", evento.Fecha);
                        cmd.Parameters.AddWithValue("@Ubicacion", evento.Ubicacion);
                        cmd.Parameters.AddWithValue("@TipoEventoID", evento.TipoEventoID);
                        cmd.Parameters.AddWithValue("@Descripcion", evento.Descripcion);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> EditarEvento(EventoCorporativo evento)
        {
            string query = @"
    UPDATE EventosCorporativos 
    SET Nombre = @Nombre, Fecha = @Fecha, Ubicacion = @Ubicacion, 
    TipoEventoID = @TipoEventoID, Descripcion = @Descripcion
    WHERE EventoID = @EventoID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EventoID", evento.EventoID);
                        cmd.Parameters.AddWithValue("@Nombre", evento.Nombre);
                        cmd.Parameters.AddWithValue("@Fecha", evento.Fecha);
                        cmd.Parameters.AddWithValue("@Ubicacion", evento.Ubicacion);
                        cmd.Parameters.AddWithValue("@TipoEventoID", evento.TipoEventoID); // Aquí está el TipoEventoID
                        cmd.Parameters.AddWithValue("@Descripcion", evento.Descripcion);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        // Eliminar evento
        public async Task<string> EliminarEvento(int id)
        {
            string query = "DELETE FROM EventosCorporativos WHERE EventoID = @EventoID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EventoID", id);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Obtener asistentes para un evento
        public async Task<List<AsistenteEvento>> ListarAsistentes(int eventoID)
        {
            List<AsistenteEvento> asistentes = new List<AsistenteEvento>();
            string query = @"
            SELECT A.AsistenteID, A.Nombre, A.Correo, A.Telefono, A.Rol 
            FROM AsistentesEvento A
            WHERE A.EventoID = @EventoID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EventoID", eventoID);
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                asistentes.Add(new AsistenteEvento
                                {
                                    AsistenteID = (int)reader["AsistenteID"],
                                    Nombre = reader["Nombre"].ToString(),
                                    Correo = reader["Correo"].ToString(),
                                    Telefono = reader["Telefono"].ToString(),
                                    Rol = reader["Rol"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return asistentes;
        }
    }
}