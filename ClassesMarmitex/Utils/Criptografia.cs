using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace ClassesMarmitex
{
    public static class CriptografiaMD5
    {
        private static byte[] chave = { };
        private static byte[] iv = { 12, 34, 56, 78, 90, 102, 114, 126 };

        private static string chaveCriptografia = "MaRmItEx2017SuuuuuUUCESSOWW";

        public static string Criptografar(string valor)
        {
            byte[] input;

            try
            {
                input = Encoding.UTF8.GetBytes(valor);
                byte[] hashed = MD5.Create().ComputeHash(input);
                string hash = BitConverter.ToString(hashed).Replace("-", "").ToLower();

                return hash;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Descriptografar(string valor)
        {
            DESCryptoServiceProvider des;
            MemoryStream ms;
            CryptoStream cs; byte[] input;

            try
            {
                des = new DESCryptoServiceProvider();
                ms = new MemoryStream();

                input = new byte[valor.Length];
                input = Convert.FromBase64String(valor.Replace(" ", "+"));

                chave = Encoding.UTF8.GetBytes(chaveCriptografia.Substring(0, 8));

                cs = new CryptoStream(ms, des.CreateDecryptor(chave, iv), CryptoStreamMode.Write);
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
