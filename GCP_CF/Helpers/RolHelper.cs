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

        public static List<KeyValuePair<int, string>> ObtenerRolesUsuario(string rolesUsuario)
        {
            List<KeyValuePair<int, string>> listadoRoles = ObtenerListadoRoles();
            List<int> listadoIdRolesUsuario = ObtenerIdRolesPorUsuario(rolesUsuario);

            List<KeyValuePair<int, string>> listadoRolesUsuario = new List<KeyValuePair<int, string>>();
            foreach (KeyValuePair<int, string> parValor in listadoRoles)
            {
                if (listadoIdRolesUsuario.Contains(parValor.Key))
                {
                    KeyValuePair<int, string> rol = new KeyValuePair<int, string>(parValor.Key, parValor.Value);
                    listadoRolesUsuario.Add(rol);
                }
            }

            return listadoRolesUsuario;
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