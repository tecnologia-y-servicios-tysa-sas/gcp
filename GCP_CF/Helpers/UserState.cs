using GCP_CF.Models;
using System.Collections.Generic;

namespace GCP_CF.Helpers
{
    public class UserState
    {
        public string Name = string.Empty;
        public string Email = string.Empty;
        public string UserId = string.Empty;
        public bool IsSuperUser = false;
        public string[] RoleList = new string[]{};

        public override string ToString()
        {
            return string.Join("|", new string[] { this.UserId, this.Name, this.IsSuperUser.ToString(), string.Join(",", this.RoleList) });
        }

        public bool FromString(string itemString)
        {
            if (string.IsNullOrEmpty(itemString)) return false;

            string[] strings = itemString.Split('|');
            if (strings.Length < 3) return false;

            UserId = strings[0];
            Name = strings[1];
            IsSuperUser = strings[2] == "True";
            RoleList = strings[3].Split(',');

            return true;
        }

        public void FromUser(Usuarios usuario)
        {
            UserId = usuario.Usuario;
            Name = usuario.NombreCompleto;
            Email = usuario.CorreoElectronico;
            IsSuperUser = RolHelper.UsuarioTieneRol(usuario.IdRoles, 0);
        }

        public bool IsEmpty()
        {
            if (string.IsNullOrEmpty(this.UserId) || string.IsNullOrEmpty(this.Name)) return true;

            return false;
        }
    }
}