using System.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace MonitoringTelegramBot
{
    public partial class EqForm : Form
    {
        MySQL mySQL;
        string id = null;
        List<string> typeData = new List<string>();
        List<string> branchsData = new List<string>();
        public EqForm(MySQL mySql, string id)
        {
            InitializeComponent();
            this.mySQL = mySql;

            using (DataTable dataTable = mySQL.GetDataTableSQL("SELECT type FROM equipment_type ORDER BY type"))
                foreach (DataRow dr in dataTable.Rows)
                    typeData.Add(dr[0].ToString());
            comboBox1.DataSource = typeData;

            using (DataTable branchTable = mySQL.GetDataTableSQL("SELECT name FROM branchs ORDER BY name"))
                foreach (DataRow dr in branchTable.Rows)
                    branchsData.Add(dr[0].ToString());
            comboBox2.DataSource = branchsData;
            if (id != null)
            {
                this.id = id;
                DataTable userTable = mySQL.GetDataTableSQL("SELECT * FROM equipments WHERE id = " + id);
                var userData = userTable.Select();
                comboBox2.Text = userData[0].ItemArray[1].ToString();
                textBox1.Text = userData[0].ItemArray[2].ToString();
                textBox2.Text = userData[0].ItemArray[3].ToString();
                comboBox1.Text = userData[0].ItemArray[4].ToString();
                checkBox1.Checked = userData[0].ItemArray[6].ToString() == "True" ? true : false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0)
                if (CheckIP())
                    if (comboBox2.Text.Length != 0)
                        if (comboBox1.Text.Length != 0)
                        {
                            if (id == null)
                            {
                                mySQL.SendSQL("INSERT equipments (branch, name, ip, type, monitoring, time_off) VALUES('" + comboBox2.Text + "','" + textBox1.Text + "','" + textBox2.Text + "','" + comboBox1.Text + "'," + (checkBox1.Checked ? 1 : 0).ToString() + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');");
                                this.Close();
                            }
                            else
                            {
                                mySQL.SendSQL("UPDATE equipments SET branch = '" + comboBox2.Text +
                                    "', name = '" + textBox1.Text +
                                    "', ip = '" + textBox2.Text +
                                    "', type = '" + comboBox1.Text +
                                    "', monitoring = '" + (checkBox1.Checked ? 1 : 0).ToString() +
                                    "'  WHERE id = " + id);
                                this.Close();
                            }
                        }
                        else
                            MessageBox.Show("Выберите тип оборудования");
                    else
                        MessageBox.Show("Выберите филиал");
                else
                    MessageBox.Show("Введите IP оборудования");
            else
                MessageBox.Show("Введите наименование оборудования");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool CheckIP()
        {
            if (textBox2.Text.Length != 0)
            {
                string[] ipSplit = textBox2.Text.Split('.');
                if (ipSplit.Length == 4)
                    return true;
            }
            return false;
        }
        private void загрузитьСExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.DefaultExt = "*.xls;*.xlsx";
            ofd.Filter = "Microsoft Excel (*.xls*)|*.xls*";
            ofd.Title = "Выберите документ Excel";
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("Вы не выбрали файл для открытия", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string xlFileName = ofd.FileName; //имя нашего Excel файла

            //рабоата с Excel
            Excel.Workbook xlWB;
            Excel.Worksheet xlSht;
            int iLastRow;

            Excel.Application xlApp = new Excel.Application(); //создаём приложение Excel
            xlWB = xlApp.Workbooks.Open(xlFileName); //открываем наш файл           
            xlSht = (Excel.Worksheet)xlWB.ActiveSheet; //или так xlSht = xlWB.ActiveSheet //активный лист
            for (int i = 2; i <= xlSht.Columns.Count; i++)
            {
                mySQL.SendSQL("INSERT equipments (branch, name, ip, type, monitoring, time_off) VALUES('" +
                    xlSht.Cells[i, 1].ToString() + "','" +
                    xlSht.Cells[i, 2].ToString() + "','" +
                    xlSht.Cells[i, 3].ToString() + "','" +
                    xlSht.Cells[i, 4].ToString() + "'," +
                    "1,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');");
            }
            xlApp.Quit();
            this.Close();
        }
    }
}
