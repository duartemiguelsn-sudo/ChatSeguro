using System;
using System.Windows.Forms;

namespace Client
{
    /// <summary>
    /// Ponto de entrada da aplicação cliente.
    /// Gere o fluxo de autenticação antes de abrir o formulário de chat.
    /// </summary>
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Abre o formulário de login — aqui é também estabelecida a ligação TCP
            LoginForm login = new LoginForm();

            if (login.ShowDialog() == DialogResult.OK)
            {
                // Login bem-sucedido: reutiliza a ligação TCP já aberta pelo LoginForm
                // e passa o nome do utilizador autenticado para o formulário de chat
                Application.Run(new Client(login.GetClient(), login.GetStream(), login.NomeUsuario));
            }
            else
            {
                // Utilizador fechou o login sem autenticar
                Application.Exit();
            }
        }
    }
}