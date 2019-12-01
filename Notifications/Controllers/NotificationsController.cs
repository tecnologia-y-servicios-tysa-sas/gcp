using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Notifications.Data;
using Notifications.Models;


namespace Notifications.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        Context context;
        public NotificationsController(Context context)
        {
            this.context = context;
        }
        [HttpGet]
        public IEnumerable<Notificacion> Get()
        {

            Context context = HttpContext.RequestServices.GetService(typeof(Context)) as Context;
            return (context.GetAllNotifications());
        }
    }
}