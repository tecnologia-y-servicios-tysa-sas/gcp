using GCP_CF.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace GCP_CF.Helpers
{
    public static class RolHelper
    {
        public const char SEPARADOR_ROLES = '|';
        public const char SEPARADOR_VALORES = ',';

        public const string USER_ID = "IdUsuario";
        public const string SUPERUSUARIO = "SuperUsuario";
        public const string LECTURA = "PuedeLeer";
        public const string ESCRITURA = "PuedeEscribir";
        public const string TODOS_LOS_CONTRATOS = "TodosLosContratos";
        public const string LISTADO_CONTRATOS = "ListaContratos";

        public const string TODOS = SUPERUSUARIO + "," + LECTURA + "," + ESCRITURA;
        public const string PUEDE_LEER = SUPERUSUARIO + "," + LECTURA;
        public const string PUEDE_ESCRIBIR = SUPERUSUARIO + "," + ESCRITURA;

        private static readonly GCPContext db = new GCPContext();

        public static List<KeyValuePair<int, string>> ObtenerListadoRoles()
        {
            List<KeyValuePair<int, string>> listadoRoles = new List<KeyValuePair<int, string>>();
            string[] paresRoles = ConfigurationManager.AppSettings["roles"].Split(new char[] { SEPARADOR_ROLES }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string parRol in paresRoles) {
                string[] valorRol = parRol.Split(new char[] { SEPARADOR_VALORES }, StringSplitOptions.RemoveEmptyEntries);
                KeyValuePair<int, string> rol = new KeyValuePair<int, string>(int.Parse(valorRol[0]), valorRol[1]);
                listadoRoles.Add(rol);
            }

            return listadoRoles;
        }

        public static bool EsSuperUsuario(string rolUsuario)
        {
            string rolSuperUsuario = ConfigurationManager.AppSettings["rolSuperUsuario"];
            if (!string.IsNullOrEmpty(rolSuperUsuario)) return rolSuperUsuario == rolUsuario;

            return false;
        }

        //public static KeyValuePair<int, string> ObtenerRolUsuario(string rolesUsuario)
        public static string ObtenerRolUsuario(int? rolId)
        {
            //KeyValuePair<int, string> rolUsuario = new KeyValuePair<int, string>();

            //if (!string.IsNullOrEmpty(rolesUsuario))
            //{
            //    int rolActualUsuario = int.Parse(rolesUsuario);
            //    List<KeyValuePair<int, string>> listadoRoles = ObtenerListadoRoles();

            //    foreach (KeyValuePair<int, string> parValor in listadoRoles)
            //    {
            //        if (parValor.Key == rolActualUsuario)
            //            rolUsuario = parValor;
            //    }
            //}

            string rolUsuario = db.Rols.Where(x => x.RolId == rolId).Select(x=> x.Descripción).FirstOrDefault();
            return rolUsuario;
        }

        public static int ObtenerIdRol(string tipoRol)
        {
            List<KeyValuePair<int, string>> listadoRoles = ObtenerListadoRoles();
            return listadoRoles.Select(rol => rol.Value == tipoRol ? rol.Key : -1).FirstOrDefault();
        }

        public static string ObtenerValorRol(int idRol)
        {
            List<KeyValuePair<int, string>> listadoRoles = ObtenerListadoRoles();
            return listadoRoles.Select(rol => rol.Key == idRol ? rol.Value : string.Empty).FirstOrDefault();
        }

        private static List<int> ObtenerIdRolesPorUsuario(string rolesUsuario)
        {
            return Array.ConvertAll<string, int>(rolesUsuario.Split(new char[] { SEPARADOR_VALORES }, StringSplitOptions.RemoveEmptyEntries), 
                                                 new Converter<string, int>(Convert.ToInt32)).ToList();
        }

        public static bool UsuarioTieneRol(string rolesUsuario, int idRol)
        {
            List<int> idRolesPorUsuario = ObtenerIdRolesPorUsuario(rolesUsuario);
            return idRolesPorUsuario.Contains(idRol);
        }

    }
}