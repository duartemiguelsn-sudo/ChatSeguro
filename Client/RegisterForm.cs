using ProtoIP;
using System;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Cliente
{
    /// <summary>
    /// Formulário de registo de novo utilizador.
    /// Reutiliza a ligação TCP do LoginForm para comunicar com o servidor.
    /// </summary>
    public partial class RegisterForm : Form
    {
        private NetworkStream networkStream;

        /// <summary>
        /// Construtor: recebe o stream TCP já aberto pelo LoginForm.
        /// Não é necessário abrir uma nova ligação.
        /// </summary>
        public RegisterForm(NetworkStream stream)
        {
            InitializeComponent();
            this.networkStream = stream;
        }

        /// <summary>
        /// Valida os requisitos mínimos de segurança da password.
        /// Requisitos: mínimo 8 caracteres, pelo menos 1 número e 1 letra maiúscula.
        /// </summary>
        private bool ValidarPassword(string pass)
        {
            if (pass.Length < 8) return false;

            bool temNumero = false;
            bool temMaiuscula = false;

            foreach (char c in pass)
            {
                if (char.IsDigit(c)) temNumero = true;
                if (char.IsUpper(c)) temMaiuscula = true;
            }

            return temNumero && temMaiuscula;
        }

        /// <summary>
        /// Evento do botão de registo.
        /// Gera salt e hash da password e envia os dados ao servidor para persistência.
        /// </summary>
        private void buttonSubmitRegister_Click(object sender, EventArgs e)
        {
            string user = textBoxNewUser.Text.Trim();
            string pass = textBoxNewPass.Text.Trim();

            // Validação de campos vazios
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Preencha todos os campos!");
                return;
            }

            // Validação dos requisitos de segurança da password
            if (!ValidarPassword(pass))
            {
                MessageBox.Show("A password deve ter:\n- Pelo menos 8 caracteres\n- Uma letra MAIÚSCULA\n- Um número", "Password Fraca");
                return;
            }

            // Gera um salt aleatório e calcula o hash para armazenamento seguro
            // Nunca é guardada a password em texto simples
            string salt = Client.Security.GenerateSalt();
            string hash = Client.Security.GenerateHash(pass, salt);

            // Formato enviado ao servidor: "username;salt;hash"
            string registroDados = $"{user};{salt};{hash}";

            Packet p = new Packet();
            p._SetType(Packet.Type.HANDSHAKE_REQ); // Tipo de registo no protocolo ProtoIP
            p.SetPayload(registroDados);
            byte[] raw = Packet.Serialize(p);
            networkStream.Write(raw, 0, raw.Length);

            // Aguarda resposta do servidor
            byte[] buffer = new byte[65536];
            int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
            byte[] data = new byte[bytesRead];
            Array.Copy(buffer, data, bytesRead);

            Packet resp = Packet.Deserialize(data);
            string resposta = resp.GetDataAs<string>();

            if (resposta == "USER_EXISTS")
            {
                MessageBox.Show("Erro: Nome de utilizador já está a ser usado!");
            }
            else
            {
                MessageBox.Show("Registo efetuado com sucesso!");
                this.Close();
            }
        }
    }
}