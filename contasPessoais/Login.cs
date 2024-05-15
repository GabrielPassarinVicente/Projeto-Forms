using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace contasPessoais
{
    public partial class Login : Form
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=862945;Database=Login";

        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string usuario = textBox1.Text;
            string senha = textBox2.Text;

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Por favor, insira nome de usuário e senha.");
                return;
            }

            bool isAuthenticated = AuthenticateUser(usuario, senha);

            if (isAuthenticated)
            {
                MessageBox.Show("Login bem-sucedido!");
                home obj = new home();
                obj.Show();
                // Aqui você pode abrir a próxima janela ou executar a lógica apropriada após o login
            }
            else
            {
                MessageBox.Show("Nome de usuário ou senha inválidos.");
            }
        }

        private bool AuthenticateUser(string usuario, string senha)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = "SELECT COUNT(*) FROM Login WHERE usuario = @usuario AND senha = @senha";
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@usuario", usuario);
                        command.Parameters.AddWithValue("@senha", senha);

                        int count = Convert.ToInt32(command.ExecuteScalar());

                        // Se o número de linhas retornadas for maior que 0, o usuário está autenticado
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao autenticar: " + ex.Message);
                    return false;
                }
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }
    }
}
