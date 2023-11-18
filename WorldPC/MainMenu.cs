using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;

namespace WorldPC
{
    public partial class MainMenu : Form
    {
        DataBase db = new DataBase();

        public string nameComponent;
        public string nameComponentChange;
        public string position;
        public string FIO;
        public int idPerson;
        public int idClient;
        public int idQuery;
        public int idRepair;

        public MainMenu()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }
        private void MainMenu_Load(object sender, EventArgs e)
        {
            // Вывод авторизованного пользователя

            label13.Text = "Пользователь: " + FIO;

            // Выгрузка данных в выплывающие списки и таблицы

            LoadDataPerson();
            LoadDataClient();
            LoadQueryCheck();

            ComboLoadType(comboBoxType);
            ComboLoadType(comboBoxComponentAdder);
            LoadRepair(viewNewPepair);
            LoadRepair(viewCollectorRepair);
            ComboLoadPosition();
            ComboLoadMaleFemale();
            ComboLoadStatus();
            ComboLoadTypeTech();
            ComboLoadGetterComponent();
            LoadDataCopmonent();

            // Ограничение страниц (функций) на роли доступа

            if (position == "Директор")
            {
                Client.Parent = null;
                QueryAddChange.Parent = null;
                Component.Parent = null;
                RepairCollector.Parent = null;

                label37.Visible = false;
                label38.Visible = false;
                label39.Visible = false;

                textBoxNameRepair.Visible = false;
                comboBoxRepair.Visible = false;
                textBox1.Visible = false;

                viewCollectorRepair.Height = 289;
            }
            else if (position == "Сборщик")
            {
                Client.Parent = null;
                Person.Parent = null;
                QueryAddChange.Parent = null;
                Component.Parent = null;
                RepairCollector.Parent = null;
            }
            else if (position == "Менеджер-кассир")
            {
                Person.Parent = null;
            }

            // Ограничение на длину поля

            textBoxPerson1.MaxLength = 50;
            textBoxPerson2.MaxLength = 50;
            textBoxPerson3.MaxLength = 50;
            textBoxPerson4.MaxLength = 20;
            textBoxPerson5.MaxLength = 20;

            textBoxClient1.MaxLength = 50;
            textBoxClient2.MaxLength = 50;
            textBoxClient3.MaxLength = 50;
            textBoxClient4.MaxLength = 100;
            textBoxClient5.MaxLength = 4;
            textBoxClient6.MaxLength = 6;
            textBoxClient7.MaxLength = 100;
        }

        public void ComboLoadMaleFemale()
        {
            string fm = "SELECT DISTINCT Наименование FROM Пол;";

            using (SqlCommand cmd = new SqlCommand(fm, db.getConnection()))
            {
                db.OpenConnection();
                cmd.CommandType = CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                MaleFemale.DisplayMember = "Наименование";
                MaleFemale.ValueMember = "ID_Пол";
                MaleFemale.DataSource = table;
                db.CloseConnection();
            }
        }

        public void ComboLoadStatus()
        {
            string status = "SELECT DISTINCT Наименование FROM Статус;";

            using (SqlCommand cmd = new SqlCommand(status, db.getConnection()))
            {
                db.OpenConnection();
                cmd.CommandType = CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                Status.DisplayMember = "Наименование";
                Status.ValueMember = "ID_Статус";
                Status.DataSource = table;
                db.CloseConnection();
            }
        }

        public void ComboLoadPosition()
        {
            string position = "SELECT DISTINCT Наименование FROM Должность;";

            using (SqlCommand cmd = new SqlCommand(position, db.getConnection()))
            {
                db.OpenConnection();
                cmd.CommandType = CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                comboBoxPerson1.DisplayMember = "Наименование";
                comboBoxPerson1.ValueMember = "ID_Должность";
                comboBoxPerson1.DataSource = table;
                db.CloseConnection();
            }
        }

        public void ComboLoadType(ComboBox comboBox)
        {
            string type = "SELECT DISTINCT Наименование FROM Тип;";

            using (SqlCommand cmd = new SqlCommand(type, db.getConnection()))
            {
                db.OpenConnection();
                cmd.CommandType = CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                comboBox.DisplayMember = "Наименование";
                comboBox.ValueMember = "ID_Тип";
                comboBox.DataSource = table;
                db.CloseConnection();
            }
        }

        public void ComboLoadGetterComponent()
        {
            string type = "SELECT DISTINCT Наименование FROM Поставщик;";

            using (SqlCommand cmd = new SqlCommand(type, db.getConnection()))
            {
                db.OpenConnection();
                cmd.CommandType = CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                comboBoxGetter.DisplayMember = "Наименование";
                comboBoxGetter.ValueMember = "ID_Поставщик";
                comboBoxGetter.DataSource = table;
                db.CloseConnection();
            }
        }

        public void ComboLoadTypeTech()
        {
            string type = "SELECT DISTINCT Наименование FROM Техника;";

            using (SqlCommand cmd = new SqlCommand(type, db.getConnection()))
            {
                db.OpenConnection();
                cmd.CommandType = CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                comboBoxRepair.DisplayMember = "Наименование";
                comboBoxRepair.ValueMember = "ID_Техника";
                comboBoxRepair.DataSource = table;
                db.CloseConnection();
            }
        }

        public void ComboLoadComponent()
        {
            string component = $"SELECT DISTINCT Наименование FROM Комплектующие WHERE ID_Тип = (SELECT T.ID_Тип FROM Тип AS T WHERE T.Наименование = '{comboBoxType.Text}');";

            using (SqlCommand cmd = new SqlCommand(component, db.getConnection()))
            {
                db.OpenConnection();
                cmd.CommandType = CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                comboBoxComponent.DisplayMember = "Наименование";
                comboBoxComponent.ValueMember = "ID_Комплектующего";
                comboBoxComponent.DataSource = table;
                db.CloseConnection();
            }
        }

        public void LoadDataClient()
        {
            string query = $"SELECT M.ID_Клиент AS ID, Фамилия, Имя, Отчество, MF.Наименование AS Пол, " +
                           $"[Дата рождения], Адрес, S.Наименование AS Статус, " +
                           $"P.[Серия паспорта] AS [Серия паспорта], P.[Номер паспорта] AS [Номер паспорта], P.[Дата выдачи], P.[Дата окончания], " +
                           $"P.Орган FROM ОсновныеДанныеКлиент AS M " +
                           $"INNER JOIN ПаспортныеДанныеКлиент AS P ON M.ID_Клиент = P.ID_Клиент " +
                           $"JOIN Пол AS MF ON M.ID_Пол = MF.ID_Пол " +
                           $"JOIN Статус AS S ON M.ID_Статус = S.ID_Статус";

            using (SqlCommand cmd = new SqlCommand(query, db.getConnection()))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);

                viewClient.DataSource = table;
            }
        }

        public void LoadDataCopmonent()
        {
            string query = $"SELECT M.Наименование, S.Наименование AS Тип, G.Наименование AS Поставщик, Стоимость " +
                           $"FROM Комплектующие AS M " +
                           $"JOIN Тип AS S ON M.ID_Тип = S.ID_Тип " +
                           $"JOIN Поставщик AS G ON M.ID_Поставщик = G.ID_Поставщик";

            using (SqlCommand cmd = new SqlCommand(query, db.getConnection()))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);

                viewComponent.DataSource = table;
            }

            viewComponent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public void LoadDataPerson()
        {
            string query = $"SELECT ID_Сотрудник AS ID, Фамилия, Имя, Отчество, " +
                           $"Логин, Пароль, [Дата рождения], P.Наименование AS Должность, Фотография " +
                           $"FROM Сотрудник AS M " +
                           $"JOIN Должность AS P ON M.ID_Должность = P.ID_Должность";

            using (SqlCommand cmd = new SqlCommand(query, db.getConnection()))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);

                viewPerson.DataSource = table;
            }
        }

        public void LoadQueryCheck()
        {
            string query = $"SELECT ID_Заказ AS ID, (M.Фамилия + ' ' + M.Имя + ' ' + M.Отчество) AS [ФИО клиента], " +
                           $"(P.[Серия паспорта] + ' ' + P.[Номер паспорта]) AS Паспорт, " +
                           $"(SELECT SUM(Стоимость) FROM ЗаказКомплектующие AS QC JOIN Комплектующие AS C ON " +
                           $"QC.ID_Комплектующего = C.ID_Комплектующего WHERE ID_Заказ = Q.ID_Заказ) AS [Стоимость комплектующих], " +
                           $"Срок " +
                           $"FROM Заказ AS Q " +
                           $"JOIN ОсновныеДанныеКлиент AS M ON Q.ID_Клиент = M.ID_Клиент " +
                           $"JOIN ПаспортныеДанныеКлиент AS P ON Q.ID_Клиент = P.ID_Клиент";

            using (SqlCommand cmd = new SqlCommand(query, db.getConnection()))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);

                viewQuery.DataSource = table;
            }
        }

        public void LoadAboutQuery(int idQuery)
        {
            string query = $"SELECT C.Наименование AS Комплектующие, C.Стоимость " +
                           $"FROM ЗаказКомплектующие AS QC " +
                           $"JOIN Комплектующие AS C ON QC.ID_Комплектующего = C.ID_Комплектующего " +
                           $"WHERE ID_Заказ = {idQuery}";

            using (SqlCommand cmd = new SqlCommand(query, db.getConnection()))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);

                viewAboutQuery.DataSource = table;
            }

            viewAboutQuery.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public void LoadAddChangeQuery(int idQuery)
        {
            string query = $"SELECT C.Наименование AS Комплектующие, C.Стоимость " +
                           $"FROM ЗаказКомплектующие AS QC " +
                           $"JOIN Комплектующие AS C ON QC.ID_Комплектующего = C.ID_Комплектующего " +
                           $"WHERE ID_Заказ = {idQuery}";

            using (SqlCommand cmd = new SqlCommand(query, db.getConnection()))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);

                addchangeQuery.DataSource = table;
            }

            addchangeQuery.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public void LoadRepair(DataGridView dataGridView)
        {
            string query = $"SELECT M.ID_Ремонт AS ID, M.[Дата поступления], " +
                           $"(K.Фамилия + ' ' + K.Имя + ' ' + K.Отчество) AS [ФИО клиента], " +
                           $"M.Описание, V.Наименование, T.Наименование AS [Тип техники], V.Характеристика " +
                           $"FROM Наряд AS M " +
                           $"INNER JOIN Учет AS V ON M.ID_Ремонт = V.ID_Ремонт " +
                           $"LEFT JOIN ОсновныеДанныеКлиент AS K ON M.ID_Клиент = K.ID_Клиент " +
                           $"LEFT JOIN Техника AS T ON V.ID_Техника = T.ID_Техника";

            using (SqlCommand cmd = new SqlCommand(query, db.getConnection()))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridView.DataSource = table;
            }

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void exitApp_Click(object sender, EventArgs e)
        {
            Auth auth = new Auth();
            auth.Show();
            this.Hide();
        }

        private void addPerson_Click(object sender, EventArgs e)
        {
            string queryLogin = $"DECLARE @Count int SELECT @Count FROM Сотрудник " +
                                $"WHERE Логин = '{textBoxPerson4.Text}'";

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            using (SqlCommand command = new SqlCommand(queryLogin, db.getConnection()))
            {
                db.OpenConnection();
                adapter.SelectCommand = command;
                adapter.Fill(table);
                db.CloseConnection();
            }

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Логин уже занят!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                byte[] imageBytes;
                if (pictureBox1.Image != null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        pictureBox1.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imageBytes = stream.ToArray();
                    }
                }
                else
                {
                    imageBytes = null;
                }

                string query = $"INSERT INTO Сотрудник " +
                               $"(ID_Сотрудник, Фамилия, Имя, Отчество, Логин, " +
                               $" Пароль, [Дата рождения], ID_Должность, Фотография) " +
                               $"VALUES " +
                               $"((SELECT MAX(ID_Сотрудник) + 1 FROM Сотрудник), '{textBoxPerson1.Text}', '{textBoxPerson2.Text}', " +
                               $"'{textBoxPerson3.Text}', '{textBoxPerson4.Text}', '{textBoxPerson5.Text}', '{datePerson.Value}', " +
                               $"(SELECT ID_Должность FROM Должность WHERE Наименование = '{comboBoxPerson1.Text}'), " +
                               $"'{imageBytes}')";

                using (SqlCommand command = new SqlCommand(query, db.getConnection()))
                {
                    db.OpenConnection();
                    adapter.SelectCommand = command;
                    adapter.Fill(table);
                    db.CloseConnection();
                }

                LoadDataPerson();

                MessageBox.Show("Сотрудник успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            idPerson = 0;
        }

        private void updatePerson_Click(object sender, EventArgs e)
        {
            // Обновить сотрудника

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            if (textBoxPerson1.MaxLength == 0 || textBoxPerson2.MaxLength == 0 || textBoxPerson3.MaxLength == 0 ||
                textBoxPerson4.MaxLength == 0 || textBoxPerson5.MaxLength == 0)
            {
                MessageBox.Show("Заполните поля данными!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                byte[] imageBytes;
                if (pictureBox1.Image != null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        pictureBox1.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imageBytes = stream.ToArray();
                    }
                }
                else
                {
                    imageBytes = null;
                }

                string query = $"UPDATE Сотрудник SET Фамилия = '{textBoxPerson1.Text}', Имя = '{textBoxPerson2.Text}', Отчество = '{textBoxPerson3.Text}', " +
                               $"Логин = '{textBoxPerson4.Text}', Пароль = '{textBoxPerson5.Text}', [Дата рождения] = '{datePerson.Value.ToString("yyyy-MM-dd")}', " +
                               $"ID_Должность = (SELECT ID_Должность FROM Должность WHERE Наименование = '{comboBoxPerson1.Text}'), Фотография = '{imageBytes}' " +
                               $"WHERE ID_Сотрудник = {idPerson}";
                    
                using (SqlCommand command = new SqlCommand(query, db.getConnection()))
                {
                    db.OpenConnection();
                    adapter.SelectCommand = command;
                    adapter.Fill(table);
                    db.CloseConnection();
                }

                LoadDataPerson();

                MessageBox.Show("Данные сотрудника изменены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            idPerson = 0;
        }

        private void AddPhoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
                }
            }
        }


        private void addClient_Click(object sender, EventArgs e)
        {
            // Добавить клиента

            string queryLogin = $"DECLARE @Count int SELECT @Count FROM ПаспортныеДанныеКлиент " +
                                $"WHERE ([Серия паспорта] + [Номер паспорта]) = '{textBoxClient5.Text + textBoxClient6.Text}'";

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            using (SqlCommand command = new SqlCommand(queryLogin, db.getConnection()))
            {
                db.OpenConnection();
                adapter.SelectCommand = command;
                adapter.Fill(table);
                db.CloseConnection();
            }

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Данные паспорта совпадают с имеющимеся!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (textBoxClient5.TextLength != 4 && textBoxClient6.TextLength != 6)
                {
                    MessageBox.Show("Ошибка при добавление!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (textBoxClient1.MaxLength == 0 || textBoxClient2.MaxLength == 0 || textBoxClient3.MaxLength == 0 || textBoxClient4.MaxLength == 0 ||
                    textBoxClient5.MaxLength == 0 || textBoxClient6.MaxLength == 0 || textBoxClient7.MaxLength == 0)
                    {
                        MessageBox.Show("Заполните поля данными!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string query = $"INSERT INTO ОсновныеДанныеКлиент (ID_Клиент, Фамилия, Имя, Отчество, ID_Пол, [Дата рождения], Адрес, ID_Статус) " +
                                       $"VALUES ((SELECT MAX(ID_Клиент) + 1 FROM ОсновныеДанныеКлиент), '{textBoxClient1.Text}', '{textBoxClient2.Text}', '{textBoxClient3.Text}', " +
                                       $"(SELECT ID_Пол FROM Пол WHERE Наименование = '{MaleFemale.Text}'), '{dateClient.Value.ToString("yyyy-MM-dd")}', '{textBoxClient4.Text}', " +
                                       $"(SELECT ID_Статус FROM Статус WHERE Наименование = '{Status.Text}')) " +
                                       $"INSERT INTO ПаспортныеДанныеКлиент (ID_Клиент, [Серия паспорта], [Номер паспорта], [Дата выдачи], [Дата окончания], Орган) " +
                                       $"VALUES ((SELECT MAX(ID_Клиент) + 1 FROM ПаспортныеДанныеКлиент), '{textBoxClient5.Text}', '{textBoxClient6.Text}', " +
                                       $"'{datepPassport1.Value.ToString("yyyy-MM-dd")}', '{datepPassport2.Value.ToString("yyyy-MM-dd")}', '{textBoxClient7.Text}')";

                        using (SqlCommand command = new SqlCommand(query, db.getConnection()))
                        {
                            db.OpenConnection();
                            adapter.SelectCommand = command;
                            adapter.Fill(table);
                            db.CloseConnection();
                        }

                        LoadDataClient();

                        MessageBox.Show("Клиент успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

            idClient = 0;
        }

        private void updateClient_Click(object sender, EventArgs e)
        {
            // Обновить клиента

            string queryLogin = $"DECLARE @Count int SELECT @Count FROM ПаспортныеДанныеКлиент " +
                                $"WHERE ([Серия паспорта] + [Номер паспорта]) = '{textBoxClient5.Text + textBoxClient6.Text}'";

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            using (SqlCommand command = new SqlCommand(queryLogin, db.getConnection()))
            {
                db.OpenConnection();
                adapter.SelectCommand = command;
                adapter.Fill(table);
                db.CloseConnection();
            }

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Данные паспорта совпадают с имеющимеся!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (textBoxClient5.TextLength != 4 && textBoxClient6.TextLength != 6)
                {
                    MessageBox.Show("Ошибка при добавление!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (textBoxClient1.MaxLength == 0 || textBoxClient2.MaxLength == 0 || textBoxClient3.MaxLength == 0 || textBoxClient4.MaxLength == 0 ||
                    textBoxClient5.MaxLength == 0 || textBoxClient6.MaxLength == 0 || textBoxClient7.MaxLength == 0)
                    {
                        MessageBox.Show("Поля должны заполнить!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string query = $"UPDATE ОсновныеДанныеКлиент SET Фамилия = '{textBoxClient1.Text}', Имя = '{textBoxClient2.Text}', Отчество = '{textBoxClient3.Text}', " +
                                       $"ID_Пол = '{MaleFemale.Text}', [Дата рождения] = '{dateClient.Value.ToString("yyyy-MM-dd")}', Адрес = '{textBoxClient4.Text}', ID_Статус = '{Status.Text}' " +
                                       $"WHERE ID_Клиент = {idClient} " +
                                       $"UPDATE ПаспортныеДанныеКлиент SET [Серия паспорта] = '{textBoxClient5.Text}', [Номер паспорта] = '{textBoxClient6.Text}' [Дата выдачи] = '{datepPassport1.Value.ToShortDateString()}', " +
                                       $"[Дата окончания] = '{datepPassport2.Value.ToString("yyyy-MM-dd")}', Орган = '{textBoxClient7.Text}' " +
                                       $"WHERE ID_Клиент = {idClient}; ";

                        using (SqlCommand command = new SqlCommand(query, db.getConnection()))
                        {
                            db.OpenConnection();
                            adapter.SelectCommand = command;
                            adapter.Fill(table);
                            db.CloseConnection();
                        }

                        LoadDataClient();

                        MessageBox.Show("Данные клиента изменены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

            idClient = 0;
        }

        private void cancelChangeQuery_Click(object sender, EventArgs e)
        {
            idQuery = 0;

            LoadAddChangeQuery(idQuery);
        }

        private void addComponent_Click(object sender, EventArgs e)
        {
            string query = $"INSERT INTO ЗаказКомплектующие " +
                           $"(ID_Заказ, ID_Комплектующего) " +
                           $"VALUES " +
                           $"({idQuery}, (SELECT ID_Комплектующего FROM Комплектующие WHERE Наименование = '{comboBoxComponent.Text}'))";

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            using (SqlCommand command = new SqlCommand(query, db.getConnection()))
            {
                db.OpenConnection();
                adapter.SelectCommand = command;
                adapter.Fill(table);
                db.CloseConnection();
            }

            LoadAddChangeQuery(idQuery);
            LoadQueryCheck();
        }

        private void deleteComponent_Click(object sender, EventArgs e)
        {
            string query = $"DELETE FROM ЗаказКомплектующие WHERE ID_Комплектующего = " +
                           $"(SELECT ID_Комплектующего FROM Комплектующие WHERE Наименование = '{nameComponent}') " +
                           $"AND ID_Заказ = {idQuery}";

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            using (SqlCommand command = new SqlCommand(query, db.getConnection()))
            {
                db.OpenConnection();
                adapter.SelectCommand = command;
                adapter.Fill(table);
                db.CloseConnection();
            }

            LoadAddChangeQuery(idQuery);
            LoadQueryCheck();
        }

        private void addQuery_Click(object sender, EventArgs e)
        {
            string query = $"INSERT INTO Заказ " +
                           $"(ID_Заказ, ID_Клиент, Срок) " +
                           $"VALUES " +
                           $"((SELECT MAX(ID_Заказ) + 1 FROM Заказ), {idClient}, '{dateQuery.Value.ToString("yyyy-MM-dd")}')";

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            using (SqlCommand command = new SqlCommand(query, db.getConnection()))
            {
                db.OpenConnection();
                adapter.SelectCommand = command;
                adapter.Fill(table);
                db.CloseConnection();
            }

            MessageBox.Show("Заказ успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadQueryCheck();
        }

        private void changeQuary_Click(object sender, EventArgs e)
        {
            string query = $"UPDATE Заказ SET ID_Клиент = {idClient}, Срок = '{dateQuery.Value.ToString("yyyy-MM-dd")}' WHERE ID_Заказ = {idQuery}";

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            using (SqlCommand command = new SqlCommand(query, db.getConnection()))
            {
                db.OpenConnection();
                adapter.SelectCommand = command;
                adapter.Fill(table);
                db.CloseConnection();
            }

            MessageBox.Show("Заказ успешно изменен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadQueryCheck();
        }

        private void add_Click(object sender, EventArgs e)
        {
            AddClientQuery AC = new AddClientQuery();
            AC.ShowDialog();
            idClient = AC.id;
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboLoadComponent();
        }

        private void cnangeComponent_Click(object sender, EventArgs e)
        {
            string query = $"UPDATE Комплектующие SET Наименование = '{textBoxComponent.Text}', ID_Тип = (SELECT ID_Тип FROM Тип WHERE Наименование = '{comboBoxComponentAdder.Text}'), " +
                           $"ID_Поставщик = (SELECT ID_Поставщик FROM Поставщик WHERE Наименование = '{comboBoxGetter.Text}'), Стоимость = '{Convert.ToDouble(costComponent.Text)}' " +
                           $"WHERE ID_Комплектующего = (SELECT ID_Комплектующего FROM Комплектующие WHERE Наименование = '{nameComponentChange}')";

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            using (SqlCommand command = new SqlCommand(query, db.getConnection()))
            {
                db.OpenConnection();
                adapter.SelectCommand = command;
                adapter.Fill(table);
                db.CloseConnection();
            }

            MessageBox.Show("Данные комплектующего изменены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadDataCopmonent();
        }

        private void addComponentButton_Click(object sender, EventArgs e)
        {
            string query = $"INSERT INTO Комплектующие " +
                           $"(ID_Комплектующего, Наименование, ID_Тип, ID_Поставщик, Стоимость) " +
                           $"VALUES " +
                           $"((SELECT MAX(ID_Комплектующего) + 1 FROM Комплектующие), '{textBoxComponent.Text}', " +
                           $"(SELECT ID_Тип FROM Тип WHERE Наименование = '{comboBoxComponentAdder.Text}'), " +
                           $"(SELECT ID_Поставщик FROM Поставщик WHERE Наименование = '{comboBoxGetter.Text}'), " +
                           $"{Convert.ToDouble(costComponent.Text)})";

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            using (SqlCommand command = new SqlCommand(query, db.getConnection()))
            {
                db.OpenConnection();
                adapter.SelectCommand = command;
                adapter.Fill(table);
                db.CloseConnection();
            }

            MessageBox.Show("Комплектующие добавлено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadDataCopmonent();
        }

        private void addNewRepair_Click(object sender, EventArgs e)
        {
            string query = $"INSERT INTO Наряд " +
                           $"(ID_Ремонт, ID_Клиент, Описание, [Дата поступления]) " +
                           $"VALUES " +
                           $"((SELECT MAX(ID_Ремонт) + 1 FROM Наряд), {idClient}, '{textBoxRepair.Text}', " +
                           $"'{dateTimePickerNewRepair.Value.ToString("yyyy-MM-dd")}') " +
                           $"INSERT INTO Учет " +
                           $"(ID_Ремонт) " +
                           $"VALUES " +
                           $"((SELECT MAX(ID_Ремонт) FROM Наряд))";

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            using (SqlCommand command = new SqlCommand(query, db.getConnection()))
            {
                db.OpenConnection();
                adapter.SelectCommand = command;
                adapter.Fill(table);
                db.CloseConnection();
            }

            MessageBox.Show("Наряд добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadRepair(viewNewPepair);

            idClient = 0;
            idRepair = 0;
        }

        private void changeNewRepair_Click(object sender, EventArgs e)
        {
            string query = $"UPDATE Наряд SET ID_Клиент = {idClient}, Описание = '{textBoxRepair.Text}', " +
                           $"[Дата поступления] = '{dateTimePickerNewRepair.Value.ToString("yyyy-MM-dd")}' " +
                           $"WHERE ID_Ремонт = {idRepair}";

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            using (SqlCommand command = new SqlCommand(query, db.getConnection()))
            {
                db.OpenConnection();
                adapter.SelectCommand = command;
                adapter.Fill(table);
                db.CloseConnection();
            }

            MessageBox.Show("Наряд изменен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadRepair(viewNewPepair);
            LoadRepair(viewCollectorRepair);

            idClient = 0;
            idRepair = 0;
        }

        private void changeRepair_Click(object sender, EventArgs e)
        {
            string query = $"UPDATE Учет SET Наименование = '{textBoxNameRepair.Text}', " +
                           $"ID_Техника = (SELECT ID_Техника FROM Техника WHERE Наименование = '{comboBoxRepair.Text}'), " +
                           $"Характеристика = '{textBox1.Text}' " +
                           $"WHERE ID_Ремонт = {idRepair}";

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            using (SqlCommand command = new SqlCommand(query, db.getConnection()))
            {
                db.OpenConnection();
                adapter.SelectCommand = command;
                adapter.Fill(table);
                db.CloseConnection();
            }

            MessageBox.Show("Отчет изменен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadRepair(viewNewPepair);
            LoadRepair(viewCollectorRepair);

            idClient = 0;
            idRepair = 0;
        }

        private void viewNewPepair_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = viewNewPepair.Rows[e.RowIndex];

            idRepair = Convert.ToInt32(selectedRow.Cells[0].Value);

            textBoxRepair.Text = selectedRow.Cells[3].Value.ToString();
            dateTimePickerNewRepair.Value = Convert.ToDateTime(selectedRow.Cells[1].Value);
        }

        private void addClientRepair_Click(object sender, EventArgs e)
        { 
            AddClientQuery AC = new AddClientQuery();
            AC.ShowDialog();
            idClient = AC.id;
        }

        private void viewCollectorRepair_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = viewCollectorRepair.Rows[e.RowIndex];

            idRepair = Convert.ToInt32(selectedRow.Cells[0].Value);

            textBoxNameRepair.Text = selectedRow.Cells[4].Value.ToString();
            textBox1.Text = selectedRow.Cells[6].Value.ToString();
        }

        private void viewPerson_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = viewPerson.Rows[e.RowIndex];

            idPerson = Convert.ToInt32(selectedRow.Cells[0].Value);

            textBoxPerson1.Text = selectedRow.Cells[1].Value.ToString();
            textBoxPerson2.Text = selectedRow.Cells[2].Value.ToString();
            textBoxPerson3.Text = selectedRow.Cells[3].Value.ToString();
            textBoxPerson4.Text = selectedRow.Cells[4].Value.ToString();
            textBoxPerson5.Text = selectedRow.Cells[5].Value.ToString();

            datePerson.Value = Convert.ToDateTime(selectedRow.Cells[6].Value);

            byte[] imageBytes = selectedRow.Cells[8].Value as byte[];
            if (imageBytes != null)
            {
                using (MemoryStream stream = new MemoryStream(imageBytes))
                {
                    pictureBox1.Image = Image.FromStream(stream);
                }
            }
            else
            {
                pictureBox1.Image = null;
            }
        }

        private void viewClient_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = viewClient.Rows[e.RowIndex];

            idClient = Convert.ToInt32(selectedRow.Cells[0].Value);

            textBoxClient1.Text = selectedRow.Cells[1].Value.ToString();
            textBoxClient2.Text = selectedRow.Cells[2].Value.ToString();
            textBoxClient3.Text = selectedRow.Cells[3].Value.ToString();
            textBoxClient4.Text = selectedRow.Cells[6].Value.ToString();
            textBoxClient5.Text = selectedRow.Cells[8].Value.ToString();
            textBoxClient6.Text = selectedRow.Cells[9].Value.ToString();
            textBoxClient7.Text = selectedRow.Cells[12].Value.ToString();

            dateClient.Value = Convert.ToDateTime(selectedRow.Cells[5].Value);
            datepPassport1.Value = Convert.ToDateTime(selectedRow.Cells[10].Value);
            datepPassport2.Value = Convert.ToDateTime(selectedRow.Cells[11].Value);
        }

        private void viewQuery_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = viewQuery.Rows[e.RowIndex];

            idQuery = Convert.ToInt32(selectedRow.Cells[0].Value);

            LoadAboutQuery(idQuery);
            LoadAddChangeQuery(idQuery);
        }

        private void addchangeQuery_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = addchangeQuery.Rows[e.RowIndex];

            nameComponent = Convert.ToString(selectedRow.Cells[0].Value);
        }

        private void viewComponent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = viewComponent.Rows[e.RowIndex];

            textBoxComponent.Text = selectedRow.Cells[0].Value.ToString();
            costComponent.Text = selectedRow.Cells[3].Value.ToString();

            nameComponentChange = Convert.ToString(selectedRow.Cells[0].Value);
        }
    }
}
