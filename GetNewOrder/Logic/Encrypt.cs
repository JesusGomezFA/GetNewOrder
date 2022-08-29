using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetNewOrder.Logic
{
    internal class Encrypt
    {
        public static string Base64_Decode(string str)
        {
            try
            {
                byte[] decbuff = Convert.FromBase64String(str);
                return System.Text.Encoding.UTF8.GetString(decbuff);
            }
            catch
            {
                //si se envia una cadena si codificación base64, mandamos vacio
                return "";
            }
        }
    }
}
