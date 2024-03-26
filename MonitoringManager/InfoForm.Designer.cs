namespace MonitoringTelegramBot
{
    partial class InfoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            label2 = new Label();
            monitoring_infoTextBox = new TextBox();
            label3 = new Label();
            typeComboBox = new ComboBox();
            topicTextBox = new TextBox();
            button3 = new Button();
            filesLable = new Label();
            label4 = new Label();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(548, 525);
            button1.Margin = new Padding(4, 3, 4, 3);
            button1.Name = "button1";
            button1.Size = new Size(88, 27);
            button1.TabIndex = 0;
            button1.Text = "Отмена";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(411, 525);
            button2.Margin = new Padding(4, 3, 4, 3);
            button2.Name = "button2";
            button2.Size = new Size(88, 27);
            button2.TabIndex = 1;
            button2.Text = "Ок";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(83, 47);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 2;
            label1.Text = "Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(83, 114);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(28, 15);
            label2.TabIndex = 3;
            label2.Text = "Info";
            // 
            // monitoring_infoTextBox
            // 
            monitoring_infoTextBox.Location = new Point(148, 111);
            monitoring_infoTextBox.Margin = new Padding(4, 3, 4, 3);
            monitoring_infoTextBox.Multiline = true;
            monitoring_infoTextBox.Name = "monitoring_infoTextBox";
            monitoring_infoTextBox.Size = new Size(460, 215);
            monitoring_infoTextBox.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(83, 79);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(35, 15);
            label3.TabIndex = 5;
            label3.Text = "Topic";
            // 
            // typeComboBox
            // 
            typeComboBox.FormattingEnabled = true;
            typeComboBox.Location = new Point(148, 44);
            typeComboBox.Margin = new Padding(4, 3, 4, 3);
            typeComboBox.Name = "typeComboBox";
            typeComboBox.Size = new Size(460, 23);
            typeComboBox.TabIndex = 6;
            // 
            // topicTextBox
            // 
            topicTextBox.Location = new Point(148, 76);
            topicTextBox.Margin = new Padding(4, 3, 4, 3);
            topicTextBox.Name = "topicTextBox";
            topicTextBox.Size = new Size(460, 23);
            topicTextBox.TabIndex = 7;
            // 
            // button3
            // 
            button3.Location = new Point(148, 347);
            button3.Margin = new Padding(4, 3, 4, 3);
            button3.Name = "button3";
            button3.Size = new Size(272, 27);
            button3.TabIndex = 8;
            button3.Text = "Загрузить файл";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // filesLable
            // 
            filesLable.AutoSize = true;
            filesLable.Location = new Point(314, 353);
            filesLable.Margin = new Padding(4, 0, 4, 0);
            filesLable.Name = "filesLable";
            filesLable.Size = new Size(0, 15);
            filesLable.TabIndex = 9;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(83, 353);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(25, 15);
            label4.TabIndex = 10;
            label4.Text = "File";
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(106, 399);
            pictureBox1.Margin = new Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(279, 152);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 11;
            pictureBox1.TabStop = false;
            // 
            // InfoForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(720, 600);
            Controls.Add(pictureBox1);
            Controls.Add(label4);
            Controls.Add(filesLable);
            Controls.Add(button3);
            Controls.Add(topicTextBox);
            Controls.Add(typeComboBox);
            Controls.Add(label3);
            Controls.Add(monitoring_infoTextBox);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "InfoForm";
            Text = "monitoring_infoForm";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Label label1;
        private Label label2;
        private TextBox monitoring_infoTextBox;
        private Label label3;
        private ComboBox typeComboBox;
        private TextBox topicTextBox;
        private Button button3;
        private Label filesLable;
        private Label label4;
        private PictureBox pictureBox1;
    }
}