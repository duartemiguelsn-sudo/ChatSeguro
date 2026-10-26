using ProtoIP;
using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Client
{
    /// <summary>
    /// Formulário de autenticação do cliente.
    /// Estabelece a ligação TCP ao servidor e trata o processo de login.
    /// </summary>
    public partial class LoginForm : Form
    {
        private NetworkStream networkStream;
        private TcpClient client;
        private const int PORT = 10000;

        // Propriedades públicas para o Program.cs aceder após login bem-sucedido
        public string NomeUsuario { get; private set; }
        public TcpClient GetClient() { return client; }
        public NetworkStream GetStream() { return networkStream; }

        /// <summary>
        /// Construtor: abre a ligação TCP ao servidor assim que o formulário é criado.
        /// Trata também o handshake RSA — troca a chave AES com o servidor de forma segura.
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();

            try
            {
                // cria um cliente TCP e liga ao servidor local
                client = new TcpClient();
                client.Connect(IPAddress.Loopback, PORT);
                networkStream = client.GetStream();

                // gera o par de chaves RSA e guarda nos dois ficheiros XML
                Security.GerarChavesRSA();

                // envia a chave pública ao servidor
                string chavePublica = Security.ObterChavePublica();
                Packet p = new Packet();
                p._SetType(Packet.Type.AES_KEY);
                p.SetPayload(chavePublica);
                byte[] raw = Packet.Serialize(p);
                networkStream.Write(raw, 0, raw.Length);

                // recebe a chave AES cifrada do servidor
                byte[] buffer = new byte[65536];
                int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                byte[] data = new byte[bytesRead];
                Array.Copy(buffer, data, bytesRead);

                Packet resp = Packet.Deserialize(data);
                string chaveAESCifrada = resp.GetDataAs<string>();

                // decifra a chave AES com a nossa chave privada e guarda para uso futuro
                byte[] chaveAES = Security.DecifrarChaveAES(chaveAESCifrada);
                Security.DefinirChaveAES(chaveAES);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao ligar ao servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento do botão Login.
        /// Envia as credenciais ao servidor e aguarda resposta de autenticação.
        /// </summary>
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string user = textBoxUser.Text.Trim();
            string pass = textBoxPass.Text.Trim();

            Packet p = new Packet();
            p._SetType(Packet.Type.ACK);
            p.SetPayload($"{user};{pass}");
            byte[] raw = Packet.Serialize(p);
            networkStream.Write(raw, 0, raw.Length);

            byte[] buffer = new byte[65536];
            int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
            byte[] data = new byte[bytesRead];
            Array.Copy(buffer, data, bytesRead);

            Packet resp = Packet.Deserialize(data);
            string resposta = resp.GetDataAs<string>();

            if (resposta == "OK")
            {
                this.NomeUsuario = user;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Utilizador ou Password incorretos!");
            }
        }

        /// <summary>
        /// Abre o formulário de registo passando a ligação existente.
        /// </summary>
        private void buttonGoToRegister_Click(object sender, EventArgs e)
        {
            Cliente.RegisterForm register = new Cliente.RegisterForm(this.networkStream);
            register.ShowDialog();
        }
    }
}