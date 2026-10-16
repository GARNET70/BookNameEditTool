namespace BookNameEditTool
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            button2 = new Button();
            label1 = new Label();
            label2 = new Label();
            panel1 = new Panel();
            textBox6 = new TextBox();
            label8 = new Label();
            textBox5 = new TextBox();
            label7 = new Label();
            textBox4 = new TextBox();
            label6 = new Label();
            textBox3 = new TextBox();
            label5 = new Label();
            textBox2 = new TextBox();
            label4 = new Label();
            textBox1 = new TextBox();
            label3 = new Label();
            button3 = new Button();
            textBox7 = new TextBox();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            label9 = new Label();
            comboBox3 = new ComboBox();
            button7 = new Button();
            label10 = new Label();
            label11 = new Label();
            label12 = new Label();
            textBox8 = new TextBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(558, 33);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 1;
            button1.Text = "取得";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(12, 33);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(504, 28);
            comboBox1.TabIndex = 2;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(12, 87);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(504, 28);
            comboBox2.TabIndex = 3;
            // 
            // button2
            // 
            button2.Location = new Point(558, 141);
            button2.Name = "button2";
            button2.Size = new Size(94, 52);
            button2.TabIndex = 4;
            button2.Text = "検索";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(106, 20);
            label1.TabIndex = 5;
            label1.Text = "対象フォルダパス";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 64);
            label2.Name = "label2";
            label2.Size = new Size(83, 20);
            label2.TabIndex = 6;
            label2.Text = "対象フォルダ";
            // 
            // panel1
            // 
            panel1.Controls.Add(label11);
            panel1.Controls.Add(label10);
            panel1.Controls.Add(textBox6);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(textBox5);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(textBox4);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(textBox3);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(label3);
            panel1.Location = new Point(12, 253);
            panel1.Name = "panel1";
            panel1.Size = new Size(653, 375);
            panel1.TabIndex = 7;
            // 
            // textBox6
            // 
            textBox6.Location = new Point(3, 343);
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(637, 27);
            textBox6.TabIndex = 11;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(3, 320);
            label8.Name = "label8";
            label8.Size = new Size(52, 20);
            label8.TabIndex = 10;
            label8.Text = "memo";
            // 
            // textBox5
            // 
            textBox5.Location = new Point(3, 290);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(125, 27);
            textBox5.TabIndex = 9;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(3, 267);
            label7.Name = "label7";
            label7.Size = new Size(39, 20);
            label7.TabIndex = 8;
            label7.Text = "状態";
            // 
            // textBox4
            // 
            textBox4.Location = new Point(3, 237);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(125, 27);
            textBox4.TabIndex = 7;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(3, 217);
            label6.Name = "label6";
            label6.Size = new Size(39, 20);
            label6.TabIndex = 6;
            label6.Text = "巻数";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(3, 187);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(647, 27);
            textBox3.TabIndex = 5;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(3, 164);
            label5.Name = "label5";
            label5.Size = new Size(39, 20);
            label5.TabIndex = 4;
            label5.Text = "作者";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(3, 108);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(647, 53);
            textBox2.TabIndex = 3;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 85);
            label4.Name = "label4";
            label4.Size = new Size(93, 20);
            label4.TabIndex = 2;
            label4.Text = "タイトル(英語)";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(3, 28);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(647, 54);
            textBox1.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(3, 5);
            label3.Name = "label3";
            label3.Size = new Size(108, 20);
            label3.TabIndex = 0;
            label3.Text = "タイトル(日本語)";
            // 
            // button3
            // 
            button3.Location = new Point(12, 634);
            button3.Name = "button3";
            button3.Size = new Size(653, 48);
            button3.TabIndex = 8;
            button3.Text = "フォルダ名作成";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // textBox7
            // 
            textBox7.Location = new Point(12, 688);
            textBox7.Multiline = true;
            textBox7.Name = "textBox7";
            textBox7.Size = new Size(653, 97);
            textBox7.TabIndex = 9;
            // 
            // button4
            // 
            button4.Location = new Point(12, 791);
            button4.Name = "button4";
            button4.Size = new Size(177, 52);
            button4.TabIndex = 10;
            button4.Text = "リネーム";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(244, 791);
            button5.Name = "button5";
            button5.Size = new Size(177, 52);
            button5.TabIndex = 11;
            button5.Text = "DB登録";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new Point(488, 791);
            button6.Name = "button6";
            button6.Size = new Size(177, 52);
            button6.TabIndex = 12;
            button6.Text = "完了";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(12, 196);
            label9.Name = "label9";
            label9.Size = new Size(69, 20);
            label9.TabIndex = 13;
            label9.Text = "更新候補";
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(12, 219);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(504, 28);
            comboBox3.TabIndex = 14;
            // 
            // button7
            // 
            button7.Location = new Point(558, 218);
            button7.Name = "button7";
            button7.Size = new Size(94, 29);
            button7.TabIndex = 15;
            button7.Text = "OK";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(154, 217);
            label10.Name = "label10";
            label10.Size = new Size(69, 20);
            label10.TabIndex = 12;
            label10.Text = "巻数(DB)";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(154, 237);
            label11.Name = "label11";
            label11.Size = new Size(23, 20);
            label11.TabIndex = 13;
            label11.Text = "xx";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(12, 118);
            label12.Name = "label12";
            label12.Size = new Size(84, 20);
            label12.TabIndex = 16;
            label12.Text = "検索文字列";
            // 
            // textBox8
            // 
            textBox8.Location = new Point(12, 141);
            textBox8.Multiline = true;
            textBox8.Name = "textBox8";
            textBox8.Size = new Size(504, 52);
            textBox8.TabIndex = 17;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(677, 848);
            Controls.Add(textBox8);
            Controls.Add(label12);
            Controls.Add(button7);
            Controls.Add(comboBox3);
            Controls.Add(label9);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(textBox7);
            Controls.Add(button3);
            Controls.Add(panel1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(comboBox2);
            Controls.Add(comboBox1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private Button button2;
        private Label label1;
        private Label label2;
        private Panel panel1;
        private Label label4;
        private TextBox textBox1;
        private Label label3;
        private TextBox textBox5;
        private Label label7;
        private TextBox textBox4;
        private Label label6;
        private TextBox textBox3;
        private Label label5;
        private TextBox textBox2;
        private TextBox textBox6;
        private Label label8;
        private Button button3;
        private TextBox textBox7;
        private Button button4;
        private Button button5;
        private Button button6;
        private Label label9;
        private ComboBox comboBox3;
        private Button button7;
        private Label label11;
        private Label label10;
        private Label label12;
        private TextBox textBox8;
    }
}
