using Npgsql;
using System.Windows.Forms;

namespace contasPessoais
{
    public partial class Form1 : Form
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=862945;Database=pessoais";
        private string pastaNotas = @"D:\notasFiscais";

        public Form1()
        {
            InitializeComponent();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string descricao = textBox1.Text;
            string valor = textBox2.Text;
            string parcelas = textBox4.Text;
            DateTime dataCompra = dateTimePicker1.Value;
            string pagamento = textBox3.Text;

            // Construa a consulta SQL parametrizada para o INSERT
            string query = "INSERT INTO pessoais (descricao, valor, parcelas, dataCompra, pagamento) " +
                           "VALUES (@Descricao, @Valor, @Parcelas, @DataCompra, @Pagamento)";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    // Adicione parâmetros à consulta para evitar injeção de SQL
                    command.Parameters.AddWithValue("@Descricao", descricao);
                    command.Parameters.AddWithValue("@Valor", valor);
                    command.Parameters.AddWithValue("@Parcelas", parcelas);
                    command.Parameters.AddWithValue("@DataCompra", dataCompra);
                    command.Parameters.AddWithValue("@Pagamento", pagamento);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Inserção realizada com sucesso!");
                            LimparCampos();
                            connection.Close();

                        }
                        else
                        {
                            MessageBox.Show("Nenhum registro foi inserido.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao inserir no banco de dados: " + ex.Message);
                    }
                }
            }
        }
        private void LimparCampos()
        {
            textBox1.Text = ""; // Limpa o campo de texto da descrição
            textBox2.Text = ""; // Limpa o campo de texto do valor
            textBox3.Text = ""; // Limpa o campo de texto do pagamento
            textBox4.Text = ""; // Limpa o campo de texto das parcelas
            dateTimePicker1.Value = DateTime.Now; // Define a data atual para o DateTimePicker
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Exibe um diálogo para selecionar a imagem da nota fiscal
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Imagens|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog.Title = "Selecione a imagem da nota fiscal";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Obtém o caminho do arquivo da imagem selecionada
                string caminhoImagem = openFileDialog.FileName;

                try
                {
                    // Cria a pasta para armazenar as notas fiscais, se não existir
                    if (!Directory.Exists(pastaNotas))
                    {
                        Directory.CreateDirectory(pastaNotas);
                    }

                    // Gera um nome de arquivo único para a imagem da nota fiscal
                    string nomeArquivo = $"nota_{DateTime.Now:yyyyMMddHHmmssfff}.jpg";

                    // Copia a imagem selecionada para a pasta de notas com o novo nome
                    string caminhoDestino = Path.Combine(pastaNotas, nomeArquivo);
                    File.Copy(caminhoImagem, caminhoDestino);

                    MessageBox.Show("Nota fiscal inserida com sucesso!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao inserir a nota fiscal: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();   
        }
    }
}
