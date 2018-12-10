using System.Security.Cryptography;
using System.Text;

namespace StoreReactNET.WebAPI.Services
{
    public class SHA256Service
    {
        public static string GetHashedString(string Input)
        {
            if(Input != null)
            {
                var sha256Hash = SHA256.Create();

                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(Input));
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
            else
            {
                return null;
            }
            
           
        }
    }
}
