using Notifications.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Notifications.Models
{
    public class Notificacion
    {
        private Context context;

        public string Mensaje { get; set; }
        public string Correo { get; set; }
    }
}
