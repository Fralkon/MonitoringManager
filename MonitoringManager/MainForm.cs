using System.Data;

namespace MonitoringTelegramBot
{
    enum FormType
    {
        User,
        Equipment,
        Info
    }
    public partial class MainForm : Form
    {
        MySQL mySQL = new MySQL();
        FormType formType;
        string filterBranch = "";
        string filterEq = "";
        string filterText = "";
        bool filterMonitor = false;
        bool filterStatus = false;
        public MainForm()
        {
            InitializeComponent();
            ToolStripMenuItem addMenuItem = new ToolStripMenuItem("Добавить");
            ToolStripMenuItem changeMenuItem = new ToolStripMenuItem("Изменить");
            ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem("Удалить");
            ToolStripMenuItem activateMenuItem = new ToolStripMenuItem("Активация");
            ToolStripMenuItem deactivateMenuItem = new ToolStripMenuItem("Деактивация");

            contextMenuStrip1.Items.AddRange(new[] { addMenuItem, changeMenuItem, deleteMenuItem, activateMenuItem, deactivateMenuItem });

            dataGridView1.ContextMenuStrip = contextMenuStrip1;

            deleteMenuItem.Click += deleteMenuItem_Click;
            addMenuItem.Click += addMenuItem_Click;
            changeMenuItem.Click += changeMenuItem_Click;
            activateMenuItem.Click += ActivateMenuItem_Click;
            deactivateMenuItem.Click += DeactivateMenuItem_Click;

            mySQL.WaitConnectToBD();

            ChangeFormType(FormType.User);
            Init();
            statusComboBox.Items.Add("offline");
            statusComboBox.Items.Add("online");

        }
        private void DeactivateMenuItem_Click(object? sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (formType == FormType.User)
                {
                    DialogResult dR = MessageBox.Show("Вы действительно хотите удалить пользователя : " + row.Cells[1].Value.ToString(), "Удаление пользователя", MessageBoxButtons.YesNo);
                    if (dR == DialogResult.Yes)
                    {
                        mySQL.SendSQL("UPDATE monitoring_user SET monitoring = 0 WHERE id = " + row.Cells[0].Value.ToString());
                    }
                }
                else if (formType == FormType.Equipment)
                {
                    DialogResult dR = MessageBox.Show("Вы действительно хотите деактивировать оборудование : " + row.Cells[3].Value.ToString(), "Деактивация оборудования", MessageBoxButtons.YesNo);
                    if (dR == DialogResult.Yes)
                    {
                        mySQL.SendSQL("UPDATE monitoring_equipment SET monitoring = 0 WHERE id = " + row.Cells[0].Value.ToString());
                    }
                }
            }
            ChangeFormType(formType);
        }
        private void ActivateMenuItem_Click(object? sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (formType == FormType.User)
                {
                    DialogResult dR = MessageBox.Show("Вы действительно хотите удалить пользователя : " + row.Cells[1].Value.ToString(), "Удаление пользователя", MessageBoxButtons.YesNo);
                    if (dR == DialogResult.Yes)
                    {
                        mySQL.SendSQL("UPDATE monitoring_user SET monitoring = 1 WHERE id = " + row.Cells[0].Value.ToString());
                    }
                }
                else if (formType == FormType.Equipment)
                {
                    DialogResult dR = MessageBox.Show("Вы действительно хотите активировать оборудование : " + row.Cells[1].Value.ToString(), "Активация оборудования", MessageBoxButtons.YesNo);
                    if (dR == DialogResult.Yes)
                    {
                        mySQL.SendSQL("UPDATE monitoring_equipment SET monitoring = 1 WHERE id = " + row.Cells[0].Value.ToString());
                    }
                }
            }
            ChangeFormType(formType);
        }
        private void Init()
        {
            eqComboBox.Items.Clear();
            DataTable dataTable = mySQL.GetDataTableSQL("SELECT type FROM monitoring_equipment_type ORDER BY type");
            eqComboBox.Items.Add("");
            foreach (DataRow dr in dataTable.Rows)
            {
                eqComboBox.Items.Add(dr[0].ToString());
            }
            branchComboBox.Items.Clear();
            DataTable branchTable = mySQL.GetDataTableSQL("SELECT name FROM monitoring_branch ORDER BY name");
            branchComboBox.Items.Add("");
            foreach (DataRow dr in branchTable.Rows)
            {
                branchComboBox.Items.Add(dr[0].ToString());
            }
        }
        private void оборудованиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeFormType(FormType.Equipment);
        }
        private void пользователиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeFormType(FormType.User);
        }
        private void ChangeFormType(FormType type)
        {
            formType = type;
            if (type == FormType.User)
            {
                label1.Text = "Пользователи";
                branchComboBox.Visible = false;
                eqComboBox.Visible = false;
                statusComboBox.Visible = false;
                monitoringCheckBox.Visible = false;
                string WHERE = "";
                if (filterText != "")
                {
                    WHERE = "WHERE id_chat LIKE '%" + filterText + "%' OR name LIKE '%" + filterText + "%'";
                }
                DataTable tableVisionUser = mySQL.GetDataTableSQL("SELECT * FROM monitoring_user " + WHERE);
                foreach (DataRow dr in tableVisionUser.Rows) {
                    using (DataTable branchsData = mySQL.GetDataTableSQL("SELECT name FROM monitoring_branch WHERE id IN(0" + dr["branchs"].ToString() + ")"))
                    {
                        List<string> branchsName = new List<string>();
                        foreach (DataRow row in branchsData.Rows)
                            branchsName.Add(row["name"].ToString());
                        dr["branchs"] = string.Join(", ", branchsName);
                    }
                }
                dataGridView1.DataSource = tableVisionUser;
            }
            else if (type == FormType.Equipment)
            {
                label1.Text = "Оборудование";
                branchComboBox.Visible = true;
                eqComboBox.Visible = true;
                statusComboBox.Visible = true;
                monitoringCheckBox.Visible = true;
                string WHERE = "";
                if (filterEq != "" || filterBranch != "" || filterText != "" || statusComboBox.Text != "")
                {
                    if (filterEq != "")
                        WHERE += " AND monitoring_equipment_type.type = '" + filterEq + "'";
                    if (filterBranch != "")
                        WHERE += " AND monitoring_branch.name = '" + filterBranch + "'";
                    if (statusComboBox.Text != "")
                        WHERE += " AND status = " + filterStatus;
                    if (filterText != "")
                        WHERE += " AND (ip LIKE '%" + filterText + "%' OR monitoring_equipment.name LIKE '%" + filterText + "%')";
                }
                dataGridView1.DataSource = mySQL.GetDataTableSQL("SELECT * FROM monitoring_equipment, monitoring_branch, monitoring_equipment_type WHERE monitoring_equipment.type_id = monitoring_equipment_type.id AND branch_id = monitoring_branch.id AND monitoring_equipment.monitoring = " + 
                    (filterMonitor == true ? "1" : "0") + WHERE + " ORDER BY monitoring_branch.name");
            }
            else if (type == FormType.Info)
            {
                label1.Text = "Info";
                branchComboBox.Visible = false;
                eqComboBox.Visible = false;
                statusComboBox.Visible = false;
                monitoringCheckBox.Visible = false;
                string WHERE = "";
                if (filterText != "")
                {
                    WHERE = "WHERE type LIKE '%" + filterText + "%' OR topic LIKE '%" + filterText + "%'";
                }
                dataGridView1.DataSource = mySQL.GetDataTableSQL("SELECT * FROM monitoring_info " + WHERE);
            }
        }
        private void addMenuItem_Click(object? sender, EventArgs e)
        {
            if (formType == FormType.User)
            {
                UserForm addUserForm = new UserForm(mySQL);
                addUserForm.ShowDialog();
            }
            else if (formType == FormType.Equipment)
            {
                EqForm addEqForm = new EqForm(mySQL, null);
                addEqForm.ShowDialog();
            }
            else if (formType == FormType.Info)
            {
                InfoForm infoForm = new InfoForm(mySQL);
                infoForm.ShowDialog();
            }
            ChangeFormType(formType);
        }
        private void deleteMenuItem_Click(object? sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (formType == FormType.User)
                {
                    DialogResult dR = MessageBox.Show("Вы действительно хотите удалить пользователя : " + row.Cells[1].Value.ToString(), "Удаление пользователя", MessageBoxButtons.YesNo);
                    if (dR == DialogResult.Yes)
                    {
                        mySQL.SendSQL("DELETE FROM users WHERE id = " + row.Cells[0].Value.ToString() + "; ");
                    }
                }
                else if (formType == FormType.Equipment)
                {
                    DialogResult dR = MessageBox.Show("Вы действительно хотите удалить оборудование : " + row.Cells[1].Value.ToString(), "Удаление оборудования", MessageBoxButtons.YesNo);
                    if (dR == DialogResult.Yes)
                    {
                        mySQL.SendSQL("DELETE FROM equipments WHERE id = " + row.Cells[0].Value.ToString() + "; ");
                    }
                }
                else if (formType == FormType.Info)
                {
                    DialogResult dR = MessageBox.Show("Вы действительно хотите удалить info : " + row.Cells[1].Value.ToString(), "Удаление info", MessageBoxButtons.YesNo);
                    if (dR == DialogResult.Yes)
                    {
                        mySQL.SendSQL("DELETE FROM monitoring_info WHERE id = " + row.Cells[0].Value.ToString() + "; ");
                    }
                }
            }
            ChangeFormType(formType);
        }
        private void changeMenuItem_Click(object? sender, EventArgs e)
        {
            if (formType == FormType.User)
            {
                UserForm addUserForm = new UserForm(mySQL, dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                addUserForm.ShowDialog();
            }
            else if (formType == FormType.Equipment)
            {
                EqForm addEqForm = new EqForm(mySQL, dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                addEqForm.ShowDialog();
            }
            else if (formType == FormType.Info)
            {
                InfoForm infoForm = new InfoForm(mySQL, dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                infoForm.ShowDialog();
            }
            ChangeFormType(formType);
        }
        private void оПрогрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Создатель : Бояркин Илья Сергеевич.\nВсе права защищены.");
        }
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void eqChange(object sender, EventArgs e)
        {
            filterEq = eqComboBox.Text;
            ChangeFormType(formType);
        }
        private void branchChange(object sender, EventArgs e)
        {
            filterBranch = branchComboBox.Text;
            ChangeFormType(formType);
        }
        private void searchText_TextChanged(object sender, EventArgs e)
        {
            filterText = searchText.Text;
            ChangeFormType(formType);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void monitoring_infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeFormType(FormType.Info);
        }
        private void statusComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (statusComboBox.Text == "offline")
                filterStatus = false;
            else if (statusComboBox.Text == "online")
                filterStatus = true;
            ChangeFormType(formType);
        }
        private void monitoringCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterMonitor = monitoringCheckBox.Checked;
            ChangeFormType(formType);
        }
        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Init();
            filterBranch = "";
            filterEq = "";
            ChangeFormType(formType);
        }
        private void общееСообщениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //GeneralMess generalMess = new GeneralMess(teleBot);
            //generalMess.ShowDialog();
        }
    }
}