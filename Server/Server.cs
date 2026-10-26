using ProtoIP;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Text;
using System.Linq;

namespace Server
{
    /// <summary>
    /// Classe principal do servidor de chat seguro.
    /// Gere as ligações TCP, reencaminha mensagens e mantém o registo de auditoria.
    /// </summary>
    class Server
    {
        private const int PORT = 10000;

        // Lista partilhada de todos os clientes atualmente ligados
        public static List<ClientHandler> connectedClients = new List<ClientHandler>();

        // Lock para garantir escrita segura no ficheiro de log (multi-thread)
        private static readonly object logLock = new object();
        private const string logFilePath = "server_logs.txt";

        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, PORT);
            listener.Start();
            Console.WriteLine("SERVER ONLINE - À espera de ligações...");
            RegisterLog("SISTEMA", "Servidor Iniciado");

            while (true)
            {
                // Aceita um novo cliente e cria um handler dedicado (multithreading)
                TcpClient client = listener.AcceptTcpClient();
                ClientHandler handler = new ClientHandler(client);
                handler.Handle();
            }
        }

        /// <summary>
        /// Regista eventos importantes num ficheiro de texto para auditoria de segurança.
        /// Utiliza lock para evitar condições de corrida entre threads.
        /// </summary>
        public static void RegisterLog(string evento, string detalhe)
        {
            lock (logLock)
            {
                try
                {
                    string logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{evento}] {detalhe}";
                    File.AppendAllText(logFilePath, logLine + Environment.NewLine, Encoding.UTF8);
                }
                catch { }
            }
        }

        /// <summary>
        /// Envia a lista atualizada de utilizadores online para todos os clientes ligados.
        /// </summary>
        public static void UpdateAllClientLists()
        {
            var names = connectedClients.Select(c => c.Username).Where(u => u != "Anónimo").ToList();
            string listaFinal = "USER_LIST:" + string.Join(";", names);

            foreach (var handler in connectedClients)
            {
                try { SendPacket(handler.Stream, Packet.Type.RELAY, listaFinal); } catch { }
            }
        }

        /// <summary>
        /// Método auxiliar para criar e enviar um pacote ProtoIP pelo stream TCP.
        /// </summary>
        public static void SendPacket(NetworkStream stream, Packet.Type type, string data)
        {
            Packet p = new Packet();
            p._SetType(type);
            p.SetPayload(data);
            byte[] raw = Packet.Serialize(p);
            stream.Write(raw, 0, raw.Length);
        }
    }

    /// <summary>
    /// Gere a comunicação com um cliente individual numa thread dedicada.
    /// Trata login, registo, mensagens e desconexão.
    /// </summary>
    class ClientHandler
    {
        private TcpClient client;
        public NetworkStream Stream { get; private set; }

        // Nome do utilizador após autenticação (começa como "Anónimo")
        public string Username { get; private set; } = "Anónimo";

        // Chave pública RSA deste cliente — guardada no handshake para verificar assinaturas
        public string ChavePublica { get; private set; } = "";

        // Chave AES desta sessão — individual por cliente
        private byte[] chaveAES = null;

        public ClientHandler(TcpClient client)
        {
            this.client = client;
            this.Stream = client.GetStream();
        }

        /// <summary>
        /// Inicia a thread de escuta para este cliente.
        /// </summary>
        public void Handle()
        {
            Thread thread = new Thread(ThreadLoop);
            thread.Start();
        }

        /// <summary>
        /// Loop principal da thread do cliente.
        /// Lê pacotes ProtoIP e processa cada tipo de comando.
        /// </summary>
        private void ThreadLoop()
        {
            Server.connectedClients.Add(this);

            // Antes de qualquer outra coisa, troca a chave AES com o cliente via RSA
            TratarTrocaDeChaves();

            while (true)
            {
                try
                {
                    byte[] buffer = new byte[65536];
                    int bytesRead = Stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    byte[] data = new byte[bytesRead];
                    Array.Copy(buffer, data, bytesRead);

                    Packet packet = Packet.Deserialize(data);
                    Packet.Type cmdType = (Packet.Type)packet._GetType();
                    string payload = packet.GetDataAs<string>();

                    switch (cmdType)
                    {
                        case Packet.Type.BYTES: // MENSAGEM DE CHAT
                            string[] partesMsg = payload.Split(new[] { ';' }, 3);
                            if (partesMsg.Length == 3)
                            {
                                string destino = partesMsg[0];
                                string conteudo = partesMsg[1];
                                Server.RegisterLog("MENSAGEM_CIFRADA", $"De: {this.Username} | Cifrado: {conteudo.Substring(0, Math.Min(20, conteudo.Length))}...");
                                string assinatura = partesMsg[2];

                                // decifra com a chave AES individual deste cliente
                                string msgDecifrada = this.DecifrarMensagem(conteudo);

                                // verifica a assinatura digital — garante autenticidade
                                bool assinaturaValida = Security.VerificarAssinatura(msgDecifrada, assinatura, this.ChavePublica);

                                if (!assinaturaValida)
                                {
                                    Server.RegisterLog("SEGURANÇA", $"Assinatura inválida de {this.Username}!");
                                    break;
                                }

                                Server.RegisterLog("ASSINATURA", $"Mensagem de {this.Username} verificada.");

                                if (destino == "Todos")
                                {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.WriteLine($"[MENSAGEM] De: {this.Username} Para: Todos");
                                    Console.ResetColor();

                                    // recifra com a chave AES individual de cada destinatário
                                    foreach (var handler in Server.connectedClients)
                                    {
                                        try
                                        {
                                            string msgRecifrada = handler.CifrarMensagem(msgDecifrada);
                                            string fullMessage = this.Username + ": " + msgRecifrada;
                                            Server.SendPacket(handler.Stream, Packet.Type.BROADCAST, fullMessage);
                                        }
                                        catch { }
                                    }
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine($"[PRIVADO] De: {this.Username} Para: {destino}");
                                    Console.ResetColor();

                                    // envia só ao destinatário e ao remetente, cifrado com a chave de cada um
                                    string prefixo = "[PRIVADO] " + this.Username + ": ";
                                    foreach (var handler in Server.connectedClients)
                                    {
                                        if (handler.Username == destino || handler.Username == this.Username)
                                        {
                                            try
                                            {
                                                string msgRecifrada = handler.CifrarMensagem(msgDecifrada);
                                                Server.SendPacket(handler.Stream, Packet.Type.RELAY, prefixo + msgRecifrada);
                                            }
                                            catch { }
                                        }
                                    }
                                }
                            }
                            break;

                        case Packet.Type.HANDSHAKE_REQ: // PEDIDO DE REGISTO
                            string dadosRegistro = payload;
                            string novoUser = dadosRegistro.Split(';')[0];

                            if (UserJaExiste(novoUser))
                            {
                                Server.SendPacket(Stream, Packet.Type.ACK, "USER_EXISTS");
                            }
                            else
                            {
                                File.AppendAllText("users.txt", dadosRegistro + Environment.NewLine);
                                Server.SendPacket(Stream, Packet.Type.ACK, "OK");
                                Server.RegisterLog("REGISTO", $"Utilizador criado: {novoUser}");
                            }
                            break;

                        case Packet.Type.ACK: // PEDIDO DE LOGIN
                            string[] partes = payload.Split(';');
                            if (ValidarLogin(partes[0], partes[1]))
                            {
                                this.Username = partes[0];
                                Server.SendPacket(Stream, Packet.Type.ACK, "OK");

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"[LOGIN] '{this.Username}' autenticado.");
                                Console.ResetColor();
                                Server.RegisterLog("AUTH", $"{this.Username} autenticado.");

                                Thread.Sleep(700);
                                Server.UpdateAllClientLists();
                            }
                            else
                            {
                                Server.SendPacket(Stream, Packet.Type.ACK, "ERRO");
                            }
                            break;

                        case Packet.Type.EOT: // CLIENTE DESCONECTOU
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"[SAIR] '{this.Username}' saiu.");
                            Console.ResetColor();
                            Server.RegisterLog("DESCONEXÃO", $"{this.Username} saiu.");
                            Server.connectedClients.Remove(this);
                            Server.UpdateAllClientLists();
                            return;
                    }
                }
                catch { break; }
            }

            Server.connectedClients.Remove(this);
            client.Close();
        }

        /// <summary>
        /// Fase inicial de segurança: recebe a chave pública do cliente,
        /// gera uma chave AES individual, cifra-a com RSA e envia ao cliente.
        /// </summary>
        private void TratarTrocaDeChaves()
        {
            byte[] buffer = new byte[65536];
            int bytesRead = Stream.Read(buffer, 0, buffer.Length);
            byte[] data = new byte[bytesRead];
            Array.Copy(buffer, data, bytesRead);

            Packet packet = Packet.Deserialize(data);
            string chavePublicaCliente = packet.GetDataAs<string>();

            // guarda a chave pública para verificar assinaturas digitais depois
            this.ChavePublica = chavePublicaCliente;
            Server.RegisterLog("RSA", $"Chave pública recebida de {client.Client.RemoteEndPoint}");

            // gera chave AES individual para esta sessão
            this.chaveAES = Security.GerarChaveAES();

            // cifra a chave AES com a chave pública do cliente e envia
            string chaveAESCifrada = Security.CifrarChaveAES(this.chaveAES, chavePublicaCliente);
            Server.SendPacket(Stream, Packet.Type.AES_KEY, chaveAESCifrada);
            Server.RegisterLog("RSA", "Chave AES individual gerada e enviada cifrada com RSA");
        }

        /// <summary>
        /// Cifra uma mensagem com a chave AES individual deste cliente.
        /// Usado para reencaminhar mensagens a este cliente de forma segura.
        /// </summary>
        public string CifrarMensagem(string mensagem)
        {
            using (System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Key = this.chaveAES;
                aes.IV = System.Text.Encoding.UTF8.GetBytes("1234567890123456");
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var ms = new System.IO.MemoryStream())
                {
                    using (var cs = new System.Security.Cryptography.CryptoStream(ms, encryptor, System.Security.Cryptography.CryptoStreamMode.Write))
                    {
                        using (var sw = new System.IO.StreamWriter(cs))
                        {
                            sw.Write(mensagem);
                        }
                        // FlushFinalBlock é chamado automaticamente quando o StreamWriter fecha
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Decifra uma mensagem com a chave AES individual deste cliente.
        /// Usado para decifrar mensagens recebidas deste cliente.
        /// </summary>
        private string DecifrarMensagem(string cifrado)
        {
            using (System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Key = this.chaveAES;
                aes.IV = System.Text.Encoding.UTF8.GetBytes("1234567890123456");
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var ms = new System.IO.MemoryStream(Convert.FromBase64String(cifrado)))
                {
                    using (var cs = new System.Security.Cryptography.CryptoStream(ms, decryptor, System.Security.Cryptography.CryptoStreamMode.Read))
                    {
                        using (var sr = new System.IO.StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Valida as credenciais de login comparando o hash recalculado com o guardado.
        /// Formato do ficheiro: username;salt;hash
        /// </summary>
        private bool ValidarLogin(string user, string pass)
        {
            if (!File.Exists("users.txt")) return false;
            foreach (string linha in File.ReadAllLines("users.txt"))
            {
                string[] dados = linha.Split(';');
                if (dados[0] == user)
                    return Security.GenerateHash(pass, dados[1]) == dados[2];
            }
            return false;
        }

        /// <summary>
        /// Verifica se um username já existe no ficheiro de utilizadores.
        /// Comparação case-insensitive para evitar duplicados.
        /// </summary>
        private bool UserJaExiste(string username)
        {
            if (!File.Exists("users.txt")) return false;
            return File.ReadAllLines("users.txt").Any(l => l.Split(';')[0].ToLower() == username.ToLower());
        }
    }
}