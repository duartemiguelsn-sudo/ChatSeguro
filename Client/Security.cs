using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Client
{
    public static class Security
    {
        // Tirámos o "readonly" para poder ser substituído pela chave do servidor
        private static byte[] Key = Encoding.UTF8.GetBytes("12345678901234567890123456789012");

        // IV continua fixo
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("1234567890123456");

        /// <summary>
        /// Cifra uma string com AES usando a chave da sessão atual.
        /// Devolve o resultado em Base64.
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
        /// Decifra uma string em Base64 com AES usando a chave da sessão atual.
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
        /// A password nunca é guardada em texto simples.
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
        /// Substitui a chave AES hardcoded pela chave recebida do servidor via RSA.
        /// A partir deste momento, Encrypt e Decrypt usam esta chave dinâmica.
        /// </summary>
        public static void DefinirChaveAES(byte[] chaveAES)
        {
            Key = chaveAES;
        }

        // Nome único dos ficheiros de chaves para este processo
        private static string _idChaves = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();

        /// <summary>
        /// Gera um par de chaves RSA e guarda em ficheiro com nome único por processo.
        /// Evita que dois clientes no mesmo PC sobrescrevam as chaves um do outro.
        /// </summary>
        public static void GerarChavesRSA()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            File.WriteAllText($"chave_publica_{_idChaves}.xml", rsa.ToXmlString(false));
            File.WriteAllText($"chaves_completas_{_idChaves}.xml", rsa.ToXmlString(true));
        }

        public static string ObterChavePublica()
        {
            return File.ReadAllText($"chave_publica_{_idChaves}.xml");
        }

        public static byte[] DecifrarChaveAES(string chaveAESCifrada)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(File.ReadAllText($"chaves_completas_{_idChaves}.xml"));

            byte[] chaveAESBytes = Convert.FromBase64String(chaveAESCifrada);
            return rsa.Decrypt(chaveAESBytes, true);
        }

        public static string AssinarMensagem(string mensagem)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(File.ReadAllText($"chaves_completas_{_idChaves}.xml"));

            byte[] msgBytes = Encoding.UTF8.GetBytes(mensagem);
            byte[] assinatura = rsa.SignData(msgBytes, new SHA256CryptoServiceProvider());

            return Convert.ToBase64String(assinatura);
        }
    }
}
