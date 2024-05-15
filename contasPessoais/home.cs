using iTextSharp.text.pdf;
using Npgsql;
using Npgsql.Internal;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace contasPessoais
{
    public partial class home : Form
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=862945;Database=pessoais";
        private string pastaExportacao = @"D:\documentosFiscais";
        public home()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 obj = new Form1();
            obj.ShowDialog();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Cria a pasta de exportação se não existir
                if (!Directory.Exists(pastaExportacao))
                {
                    Directory.CreateDirectory(pastaExportacao);
                }

                // Conecta-se ao banco de dados e executa uma consulta para obter os dados
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    string query = "SELECT * FROM pessoais"; // Substitua SuaTabela pelo nome da sua tabela
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Monta a tabela em formato HTML
                        StringBuilder htmlBuilder = new StringBuilder();
                        htmlBuilder.AppendLine("<html>");
                        htmlBuilder.AppendLine("<head><title>Dados da Tabela</title></head>");
                        htmlBuilder.AppendLine("<body>");
                        htmlBuilder.AppendLine("<h1>Nota</h1>");
                        htmlBuilder.AppendLine("<table border='1'>");

                        // Adiciona cabeçalhos da tabela
                        htmlBuilder.AppendLine("<tr>");
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            htmlBuilder.AppendLine($"<th>{column.ColumnName}</th>");
                        }
                        htmlBuilder.AppendLine("</tr>");

                        // Adiciona linhas de dados da tabela
                        foreach (DataRow row in dataTable.Rows)
                        {
                            htmlBuilder.AppendLine("<tr>");
                            foreach (var item in row.ItemArray)
                            {
                                htmlBuilder.AppendLine($"<td>{item}</td>");
                            }
                            htmlBuilder.AppendLine("</tr>");
                        }

                        htmlBuilder.AppendLine("</table>");
                        htmlBuilder.AppendLine("</body>");
                        htmlBuilder.AppendLine("</html>");

                        // Salva a tabela em um arquivo na pasta de exportação
                        string caminhoArquivo = Path.Combine(pastaExportacao, "dados_tabela.html");
                        File.WriteAllText(caminhoArquivo, htmlBuilder.ToString());

                        MessageBox.Show("Exportação concluída com sucesso!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao exportar os dados: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

  

