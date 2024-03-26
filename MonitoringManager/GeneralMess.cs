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
    public partial class GeneralMess : Form
    {
        public GeneralMess()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0)
            {
               // teleBot.SendGeneralMessage(textBox1.Text);
                this.Close();
            }
            else
                MessageBox.Show("Сообщение не должно быть пустым");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
