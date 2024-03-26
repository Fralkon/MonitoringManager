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
    public partial class InfoForm : Form
    {
        public static string GetShortPathFile(string path)
        {
            if (path.Length > 15)
                return path.Remove(0, 15);
            else return "";
        }
        public static string GetFullPathFile(string path)
        {
            return "C:\\ZabbixMTBot\\" + path;
        }
        MySQL mySQL;
        OpenFileDialog ofd = new OpenFileDialog();
        string id = null;
        public InfoForm(MySQL mySQL)
        {
            InitializeComponent();
            this.mySQL = mySQL;
            Init();
        }
        public InfoForm(MySQL mySQL, string id)
        {
            InitializeComponent();
            this.mySQL = mySQL;
            this.id = id;
            Init();
            DataTable typeTable = mySQL.GetDataTableSQL("SELECT * FROM monitoring_info WHERE id = " + id);
            typeComboBox.Text = typeTable.Rows[0]["type"].ToString();
            topicTextBox.Text = typeTable.Rows[0]["topic"].ToString();
            monitoring_infoTextBox.Text = typeTable.Rows[0]["info"].ToString();
            if(typeTable.Rows[0]["file"].ToString() != "")
                ofd.FileName = GetFullPathFile(typeTable.Rows[0]["file"].ToString());
            filesLable.Text = ofd.SafeFileName;
            if (ofd.FileName != "")
            {
                string typeFile = Path.GetExtension(ofd.FileName);
                if (typeFile == ".jpg" || typeFile == ".png" || typeFile == ".jpeg")
                    pictureBox1.Load(ofd.FileName);
            }
        }
        private void Init()
        {
            ofd.Filter = "Image Files(*.JPG;*.PNG;*.JPEG)|*.JPG;*.PNG;*.JPEG|Documents(*.EXE;*.PDF;*.APK)|*.EXE;*.PDF;*.APK";
            ofd.Multiselect = false;
            DataTable typeTable = mySQL.GetDataTableSQL("SELECT DISTINCT type FROM monitoring_info");
            foreach (DataRow row in typeTable.Rows)
                typeComboBox.Items.Add(row[0].ToString());
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filesLable.Text = ofd.SafeFileName;
                string typeFile = Path.GetExtension(ofd.FileName);
                if (typeFile == ".jpg" || typeFile == ".png" || typeFile == ".jpeg")
                    pictureBox1.Load(ofd.FileName);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string path = "";
            foreach (var ch in ofd.FileName)
            {
                if (ch == '\\')
                    path += '\\';
                path += ch;
            }
            if (id == null)
                if (typeComboBox.Text.Length != 0)
                {
                    if (topicTextBox.Text.Length != 0)
                    {
                        if (monitoring_infoTextBox.Text.Length != 0)
                        {
                            mySQL.SendSQL("INSERT monitoring_info (type, topic, info, file) VALUES('" +
                                typeComboBox.Text + "','" +
                                topicTextBox.Text + "','" +
                                monitoring_infoTextBox.Text + "','" +
                                GetShortPathFile(path) + "');");
                            this.Close();
                        }
                        else
                            MessageBox.Show("Укажите название info");
                    }
                    else
                        MessageBox.Show("Укажите название Topic");
                }
                else
                    MessageBox.Show("Укажите название Name");
            else
            {
                mySQL.SendSQL("UPDATE monitoring_info SET type = '" +
                    typeComboBox.Text + "', topic = '" +
                    topicTextBox.Text + "', info = '" +
                    monitoring_infoTextBox.Text + "', file = '" +
                    GetShortPathFile(path) + "' WHERE id = " + id);
                this.Close();
            }
        }
    }
}
