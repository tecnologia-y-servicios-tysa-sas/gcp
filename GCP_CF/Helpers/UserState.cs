using GCP_CF.Models;
using System.Collections.Generic;
using System.Linq;

namespace GCP_CF.Helpers
{
    public class UserState
    {
        public int ID = 0;
        public string Name = string.Empty;
        public string Email = string.Empty;
        public string UserId = string.Empty;
        public bool IsActive = false;
        public bool IsSuperUser = false;
        public bool CanWrite = false;
        public string Role = string.Empty;
        public bool AllContracts = false;
        public string ContractIds = string.Empty;
        public int RolId = 0;

        public override string ToString()
        {
            return string.Join("|", new string[] {
                this.ID.ToString(),
                this.UserId,
                this.Name,
                this.IsActive.ToString(),
                this.IsSuperUser.ToString(),
                this.CanWrite.ToString(),
                this.AllContracts.ToString(),
                this.ContractIds,
                this.Role
            });
        }

        public bool FromString(string itemString)
        {
            if (string.IsNullOrEmpty(itemString)) return false;

            string[] strings = itemString.Split('|');
            if (strings.Length < 7) return false;

            ID = int.Parse(strings[0]);
            //UserId = strings[1];
            Name = strings[1];
            IsActive = strings[2] == "True";
            IsSuperUser = strings[3] == "True";
            CanWrite = strings[4] == "True";
            AllContracts = strings[5] == "True";
            ContractIds = strings[6];
            Role = strings[7];

            return true;
        }
        
        public void FromUser(Usuarios usuario)
        {
            ID = usuario.Usuario_Id;
            //UserId = usuario.Usuario;
            Name = usuario.NombreCompleto;
            Email = usuario.CorreoElectronico;
            IsActive = usuario.EsActivo;
            RolId = usuario.RolId.Value;
            IsSuperUser = usuario.EsSuperUsuario;
            CanWrite = usuario.TipoPermisos == "W";
            AllContracts = usuario.TodosLosContratos;
            ContractIds = usuario.IdContratos;
            Role = usuario.IdRoles;
           
        }

        public List<int> ContractIdListFromUser()
        {
            return ContractIds.Split(',').Select(int.Parse).ToList();
        }

        public bool IsEmpty()
        {
            if (string.IsNullOrEmpty(this.UserId) || string.IsNullOrEmpty(this.Name)) return true;

            return false;
        }
    }
}