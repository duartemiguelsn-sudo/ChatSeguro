using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    public static class Security
    {
        // Chave AES do Security global — não usada para cifrar mensagens (isso é feito por ClientHandler)
        private static byte[] Key = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("1234567890123456");

        /// <summary>
        /// Cifra uma string com AES. Usada internamente pelo servidor se necessário.
        /// </summary>
        public static string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        /// <summary>
        /// Decifra uma string em Base64 com AES. Usada internamente pelo servidor se necessário.
        /// </summary>
        public static string Decrypt(string cipherText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gera um Salt aleatório de 16 bytes para uso no hash de passwords.
        /// </summary>
        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// Cria o Hash SHA256 combinando a password com o salt.
        /// Usado na validação do login.
        /// </summary>
        public static string GenerateHash(string password, string salt)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                string saltedPassword = password + salt;
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Gera uma chave AES aleatória de 32 bytes para uma sessão.
        /// Cada cliente recebe uma chave diferente.
        /// </summary>
        public static byte[] GerarChaveAES()
        {
            byte[] chaveAES = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(chaveAES);
            }
            return chaveAES;
        }

        /// <summary>
        /// Cifra a chave AES com a chave pública RSA do cliente.
        /// Só o cliente consegue abrir com a sua chave privada.
        /// </summary>
        public static string CifrarChaveAES(byte[] chaveAES, string chavePublicaCliente)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(chavePublicaCliente);

            byte[] chaveAESCifrada = rsa.Encrypt(chaveAES, true);
            return Convert.ToBase64String(chaveAESCifrada);
        }

        /// <summary>
        /// Verifica se a assinatura digital da mensagem é válida.
        /// Usa a chave pública do cliente guardada no handshake.
        /// Garante que a mensagem foi mesmo enviada pelo cliente correto e não foi alterada.
        /// </summary>
        public static bool VerificarAssinatura(string mensagem, string assinaturaBase64, string chavePublicaCliente)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(chavePublicaCliente);

            byte[] msgBytes = Encoding.UTF8.GetBytes(mensagem);
            byte[] assinatura = Convert.FromBase64String(assinaturaBase64);

            return rsa.VerifyData(msgBytes, new SHA256CryptoServiceProvider(), assinatura);
        }
    }
}