using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WorldPC
{
    public partial class AddClientQuery : Form
    {
        DataBase db = new DataBase();

        public int id;

        public AddClientQuery()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        private void AddClientQuery_Load(object sender, EventArgs e)
        {
            LoadDataClient();
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

                viewClientQuery.DataSource = table;
            }
        }

        private void viewClientQuery_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = viewClientQuery.Rows[e.RowIndex];

            id = Convert.ToInt32(selectedRow.Cells[0].Value);

            this.Hide();
        }
    }
}
