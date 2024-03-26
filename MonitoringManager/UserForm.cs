using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonitoringTelegramBot
{
    public partial class UserForm : Form
    {
        MySQL mySQL;
        string id = null;
        List<string> branchsUser = new List<string>();
        List<string> branchsData = new List<string>();

        public UserForm(MySQL mySQL)
        {
            InitializeComponent();
            this.mySQL = mySQL;
            using (DataTable dataTable = mySQL.GetDataTableSQL("SELECT name FROM monitoring_branch"))
                foreach (DataRow dr in dataTable.Rows)
                    branchsData.Add(dr[0].ToString());
            UpDateDate();
        }
        public UserForm(MySQL mySQL, string id)
        {
            InitializeComponent();
            this.mySQL = mySQL;
            this.id = id;
            using (DataTable userTable = mySQL.GetDataTableSQL("SELECT id_chat, branchs, name, monitoring FROM monitoring_user WHERE id = " + id))
            {
                var userData = userTable.Select();
                textBox1.Text = userData[0]["id_chat"].ToString();
                full_nameText.Text = userData[0]["name"].ToString();
                using (DataTable branchsData = mySQL.GetDataTableSQL("SELECT name FROM monitoring_branch WHERE id IN(0" + userData[0]["branchs"].ToString() + ")"))
                {
                    foreach(DataRow row in branchsData.Rows)
                        branchsUser.Add(row["name"].ToString());
                }
                checkBox1.Checked = userData[0]["monitoring"].ToString().Equals("True");
            }
            using (DataTable dataTable = mySQL.GetDataTableSQL("SELECT name FROM monitoring_branch ORDER BY name"))
                foreach (DataRow dr in dataTable.Rows)
                    branchsData.Add(dr[0].ToString());
            branchsData.RemoveAll(branch => branch == "");
            branchsUser.RemoveAll(branch => branch == "");
            foreach (string text in branchsUser)
            {
                foreach (string branch in branchsData)
                {
                    if (branch == text)
                    {
                        branchsData.Remove(text);
                        break;
                    }
                }
            }
            UpDateDate();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0)
            {
                if (listBox2.Items.Count > 0)
                {
                    if (full_nameText.Text.Length != 0)
                    {
                        List<string> branchs = new List<string>();
                        foreach (string text in listBox2.Items)
                            branchs.Add(text);
                        List<string> branchs_id = new List<string>();
                        using (DataTable branchsData = mySQL.GetDataTableSQL("SELECT id FROM monitoring_branch WHERE name IN(" + 
                            string.Join(',', branchs.Select((value) => value = "'" + value + "'")) + ")"))
                        {
                            foreach (DataRow row in branchsData.Rows)
                                branchs_id.Add(row["id"].ToString());
                        }

                        if (id == null)
                            mySQL.SendSQL("INSERT monitoring_user (id_chat, branchs, name, monitoring) VALUES('" +
                                textBox1.Text + "','" +
                                String.Join(",", branchs_id) + "','" +
                                full_nameText.Text + "'," +
                                (checkBox1.Checked ? "1" : "0") + ");");
                        else
                            mySQL.SendSQL("UPDATE monitoring_user SET id_chat = " +
                                textBox1.Text + ", branchs = '" +
                                String.Join(",", branchs_id) + "', name = '" +
                                full_nameText.Text + "', monitoring = " +
                                (checkBox1.Checked ? "1" : "0") + " WHERE id = " + id);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Укажите ФИО");
                    }
                }
                else
                {
                    MessageBox.Show("Укажите филиалы");
                }
            }
            else
            {
                MessageBox.Show("Введите ID Telegram");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                branchsUser.Add(listBox1.SelectedItem.ToString());
                branchsData.Remove(listBox1.SelectedItem.ToString());
                UpDateDate();
            }
            else
            {
                MessageBox.Show("Выберите филиал");
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                branchsUser.Remove(listBox2.SelectedItem.ToString());
                branchsData.Add(listBox2.SelectedItem.ToString());
                UpDateDate();
            }
            else
            {
                MessageBox.Show("Выберите филиал");
            }
        }
        private void UpDateDate()
        {
            branchsData.Sort();
            branchsUser.Sort();

            listBox2.DataSource = null;
            listBox1.DataSource = null;
            listBox2.DataSource = branchsUser;
            listBox1.DataSource = branchsData;
        }
    }
}
