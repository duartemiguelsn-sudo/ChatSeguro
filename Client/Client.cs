using ProtoIP;
using System;
using System.Drawing;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    /// <summary>
    /// Formulário principal do chat seguro.
    /// Gere o envio e receção de mensagens cifradas via ProtoIP.
    /// </summary>
    public partial class Client : Form
    {
        private NetworkStream networkStream;
        private TcpClient tcpClient;
        private Thread listenThread;
        private string username;

        /// <summary>
        /// Construtor: recebe a ligação já estabelecida pelo LoginForm.
        /// </summary>
        public Client(TcpClient tcpClient, NetworkStream stream, string user)
        {
            InitializeComponent();

            this.tcpClient = tcpClient;
            this.networkStream = stream;
            this.username = user;

            this.Text = "Chat Seguro";
            this.labelUsername.Text = "Logado como: " + this.username;
            this.labelChatTitle.Text = "Chat Geral";

            // Inicia a thread de escuta em background para não bloquear a UI
            listenThread = new Thread(ListenToServer);
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        /// <summary>
        /// Thread de escuta contínua do servidor.
        /// Recebe pacotes ProtoIP e processa conforme o tipo.
        /// </summary>
        private void ListenToServer()
        {
            try
            {
                while (tcpClient.Connected)
                {
                    byte[] buffer = new byte[65536];
                    int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    byte[] data = new byte[bytesRead];
                    Array.Copy(buffer, data, bytesRead);

                    Packet packet = Packet.Deserialize(data);
                    Packet.Type cmdType = (Packet.Type)packet._GetType();
                    string payload = packet.GetDataAs<string>();

                    if (cmdType == Packet.Type.BROADCAST || cmdType == Packet.Type.RELAY)
                    {
                        if (payload.StartsWith("USER_LIST:"))
                            AtualizarListaOnline(payload.Substring("USER_LIST:".Length));
                        else
                            ProcessarMensagemRecebida(payload);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Processa e exibe uma mensagem recebida.
        /// Decifra o conteúdo usando AES e aplica cor diferente para mensagens privadas.
        /// </summary>
        private void ProcessarMensagemRecebida(string pacoteRecebido)
        {
            try
            {
                bool isPrivado = pacoteRecebido.StartsWith("[PRIVADO]");

                // O formato é "NomeUtilizador: <conteudoCifrado>"
                // Usamos IndexOf para encontrar o PRIMEIRO ": " — que separa o nome do conteúdo
                int posCifra = pacoteRecebido.IndexOf(": ");
                string msgFinal;

                if (posCifra != -1)
                {
                    string prefixo = pacoteRecebido.Substring(0, posCifra + 2);
                    string cifra = pacoteRecebido.Substring(posCifra + 2);
                    // Decifra apenas a parte do conteúdo, mantendo o prefixo legível
                    msgFinal = prefixo + Security.Decrypt(cifra);
                }
                else
                {
                    msgFinal = pacoteRecebido;
                }

                this.Invoke((MethodInvoker)delegate {
                    if (isPrivado)
                    {
                        textBoxChat.SelectionColor = Color.FromArgb(100, 60, 180);
                        textBoxChat.SelectionFont = new Font("Segoe UI", 9.5f, FontStyle.Italic);
                    }
                    else
                    {
                        textBoxChat.SelectionColor = Color.FromArgb(40, 40, 40);
                        textBoxChat.SelectionFont = new Font("Segoe UI", 9.5f, FontStyle.Regular);
                    }
                    textBoxChat.AppendText(msgFinal + Environment.NewLine);
                    textBoxChat.ScrollToCaret();
                });
            }
            catch
            {
                this.Invoke((MethodInvoker)delegate {
                    textBoxChat.AppendText(pacoteRecebido + Environment.NewLine);
                });
            }
        }

        /// <summary>
        /// Atualiza a ComboBox de destinatários com a lista de utilizadores online.
        /// </summary>
        private void AtualizarListaOnline(string listaBruta)
        {
            string[] users = listaBruta.Split(';');
            this.Invoke((MethodInvoker)delegate
            {
                object selecionado = comboBoxDestinatarios.SelectedItem;
                comboBoxDestinatarios.Items.Clear();
                comboBoxDestinatarios.Items.Add("Todos");

                foreach (string u in users)
                {
                    if (u != this.username && !string.IsNullOrEmpty(u))
                        comboBoxDestinatarios.Items.Add(u);
                }

                if (selecionado != null && comboBoxDestinatarios.Items.Contains(selecionado))
                    comboBoxDestinatarios.SelectedItem = selecionado;
                else
                    comboBoxDestinatarios.SelectedIndex = 0;

                string dest = comboBoxDestinatarios.SelectedItem?.ToString() ?? "Todos";
                labelChatTitle.Text = dest == "Todos" ? "Chat Geral" : "Chat com " + dest;
            });
        }

        /// <summary>
        /// Evento do botão Enviar.
        /// Cifra a mensagem com AES e assina com RSA antes de enviar.
        /// </summary>
        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxMessage.Text)) return;

            string destino = comboBoxDestinatarios.SelectedItem?.ToString() ?? "Todos";
            string msgOriginal = textBoxMessage.Text;
            textBoxMessage.Clear();
            textBoxMessage.Focus();

            // cifra a mensagem com AES
            string msgCifrada = Security.Encrypt(msgOriginal);

            // assina a mensagem ORIGINAL com a chave privada RSA
            string assinatura = Security.AssinarMensagem(msgOriginal);

            // formato: "destino;mensagemCifrada;assinatura"
            string dataParaEnviar = $"{destino};{msgCifrada};{assinatura}";

            Packet p = new Packet();
            p._SetType(Packet.Type.BYTES);
            p.SetPayload(dataParaEnviar);
            byte[] raw = Packet.Serialize(p);
            networkStream.Write(raw, 0, raw.Length);
        }

        /// <summary>
        /// Envia pacote EOT ao servidor e fecha a ligação TCP de forma limpa.
        /// </summary>
        private void CloseClient()
        {
            try
            {
                if (tcpClient != null && tcpClient.Connected)
                {
                    Packet eot = new Packet();
                    eot._SetType(Packet.Type.EOT);
                    eot.SetPayload("");
                    byte[] raw = Packet.Serialize(eot);
                    networkStream.Write(raw, 0, raw.Length);
                    networkStream.Close();
                    tcpClient.Close();
                }
            }
            catch { }
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e) { CloseClient(); }
        private void buttonQuit_Click(object sender, EventArgs e) { CloseClient(); this.Close(); }
        private void textBoxChat_TextChanged(object sender, EventArgs e) { }
    }
}