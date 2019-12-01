using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Notifications.Models;
using MySql.Data.MySqlClient;

namespace Notifications.Data
{
    public class Context 
    {
        public string ConnectionString { get; set; }

        public Context(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<Notificacion> GetAllNotifications()
        {
            List<Notificacion> list = new List<Notificacion>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("uspNotifications", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Notificacion()
                        {
                            Mensaje = reader["Mensaje"].ToString(),
                            Correo = reader["Correo"].ToString()
                        });
                    }
                }
            }
            return list;
        }
    }
}
