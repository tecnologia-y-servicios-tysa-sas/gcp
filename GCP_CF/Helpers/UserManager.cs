using GCP_CF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCP_CF.Helpers
{
    public class UserManager
    {
        public Usuarios EsValido(GCPContext db, string correoElectronico, string password)
        {
            string decodedPassword = Base64Encode(password);
            return db.Usuarios.Where(u => u.CorreoElectronico == correoElectronico && u.Password == decodedPassword && u.EsActivo).FirstOrDefault();
        }

        public string Base64Encode(string sData) // Encode
        {
            if (string.IsNullOrEmpty(sData)) return sData;

            try
            {
                byte[] encData_byte = new byte[sData.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(sData);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en base64Encode" + ex.Message);
            }
        }

        public string Base64Decode(string sData) // Decode
        {
            if (string.IsNullOrEmpty(sData)) return sData;

            try
            {
                var encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecodeByte = Convert.FromBase64String(sData);
                int charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
                char[] decodedChar = new char[charCount];
                utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);
                string result = new String(decodedChar);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en base64Decode" + ex.Message);
            }
        }
    }
}