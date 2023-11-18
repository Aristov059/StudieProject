using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WorldPC
{
    public partial class Auth : Form
    {
        DataBase db = new DataBase();

        public string position;
        public string FIO;
        public int count;

        public Auth()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;

            textBox1.MaxLength = 20;
            textBox2.MaxLength = 20;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string query = $"SELECT Фамилия, Имя, Отчество, Логин, Наименование FROM Сотрудник " +
                           $"JOIN Должность ON Сотрудник.ID_Должность = Должность.ID_Должность " +
                           $"WHERE Логин = '{login}' AND Пароль = '{password}';";

            using (SqlCommand command = new SqlCommand(query, db.getConnection()))
            {
                db.OpenConnection();
                adapter.SelectCommand = command;
                count = adapter.Fill(table);
                db.CloseConnection();
            }

            if (count != 1)
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else 
            {
                for (int i = 0; i <= 2; i++)
                {
                    using (SqlCommand cmd = new SqlCommand(query, db.getConnection()))
                    {
                        db.OpenConnection();
                        SqlDataReader reader;
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            FIO += reader[i].ToString() + " ";
                        }
                        db.CloseConnection();
                    }
                }

                using (SqlCommand cmd = new SqlCommand(query, db.getConnection()))
                {
                    db.OpenConnection();
                    SqlDataReader reader;
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        position = reader[4].ToString();
                    }
                    db.CloseConnection();
                }

                MessageBox.Show($"Добро пожаловать, {FIO}!", "Вход в систему", MessageBoxButtons.OK, MessageBoxIcon.None);

                MainMenu main = new MainMenu();
                main.FIO = this.FIO;
                main.position = this.position;
                main.Show();
                this.Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Показать пароль

            textBox2.UseSystemPasswordChar = false;
            button2.Visible = false;
            button3.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Скрыть пароль

            textBox2.UseSystemPasswordChar = true;
            button2.Visible = true;
            button3.Visible = false;
        }
    }
}
