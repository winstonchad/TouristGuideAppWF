namespace TouristGuideAppWF
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
            SearchBtn = new Button();
            CityNameInput = new TextBox();
            label2 = new Label();
            ResultTextBox = new RichTextBox();
            button1 = new Button();
            menuStrip1 = new MenuStrip();
            historyToolStripMenuItem = new ToolStripMenuItem();
            viewHistoryToolStripMenuItem = new ToolStripMenuItem();
            editHistoryToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // SearchBtn
            // 
            SearchBtn.Location = new Point(242, 215);
            SearchBtn.Margin = new Padding(3, 4, 3, 4);
            SearchBtn.Name = "SearchBtn";
            SearchBtn.Size = new Size(186, 67);
            SearchBtn.TabIndex = 1;
            SearchBtn.Text = "Search";
            SearchBtn.UseVisualStyleBackColor = true;
            SearchBtn.Click += SearchBtn_Click;
            // 
            // CityNameInput
            // 
            CityNameInput.Location = new Point(162, 134);
            CityNameInput.Margin = new Padding(3, 4, 3, 4);
            CityNameInput.Name = "CityNameInput";
            CityNameInput.Size = new Size(348, 27);
            CityNameInput.TabIndex = 2;
            CityNameInput.TextChanged += textBox1_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(225, 63);
            label2.Name = "label2";
            label2.Size = new Size(216, 20);
            label2.TabIndex = 3;
            label2.Text = "Name the city you want to visit:";
            // 
            // ResultTextBox
            // 
            ResultTextBox.Location = new Point(14, 307);
            ResultTextBox.Margin = new Padding(3, 4, 3, 4);
            ResultTextBox.Name = "ResultTextBox";
            ResultTextBox.Size = new Size(660, 433);
            ResultTextBox.TabIndex = 4;
            ResultTextBox.Text = "";
            ResultTextBox.TextChanged += ResultTextBox_TextChanged;
            // 
            // button1
            // 
            button1.Location = new Point(447, 215);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(186, 67);
            button1.TabIndex = 5;
            button1.Text = "Open Map in Browser";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { historyToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(682, 28);
            menuStrip1.TabIndex = 6;
            menuStrip1.Text = "menuStrip1";
            // 
            // historyToolStripMenuItem
            // 
            historyToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { viewHistoryToolStripMenuItem, editHistoryToolStripMenuItem });
            historyToolStripMenuItem.Name = "historyToolStripMenuItem";
            historyToolStripMenuItem.Size = new Size(70, 24);
            historyToolStripMenuItem.Text = "History";
            // 
            // viewHistoryToolStripMenuItem
            // 
            viewHistoryToolStripMenuItem.Name = "viewHistoryMenuClick";
            viewHistoryToolStripMenuItem.Size = new Size(224, 26);
            viewHistoryToolStripMenuItem.Text = "View History";
            viewHistoryToolStripMenuItem.Click += viewHistoryMenuClick;
            // 
            // editHistoryToolStripMenuItem
            // 
            editHistoryToolStripMenuItem.Name = "editHistoryToolStripMenuItem";
            editHistoryToolStripMenuItem.Size = new Size(224, 26);
            editHistoryToolStripMenuItem.Text = "Edit History";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(682, 753);
            Controls.Add(button1);
            Controls.Add(ResultTextBox);
            Controls.Add(label2);
            Controls.Add(CityNameInput);
            Controls.Add(SearchBtn);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Form1";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button SearchBtn;
        private TextBox CityNameInput;
        private Label label2;
        private RichTextBox ResultTextBox;
        private Button button1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem historyToolStripMenuItem;
        private ToolStripMenuItem viewHistoryToolStripMenuItem;
        private ToolStripMenuItem editHistoryToolStripMenuItem;
    }
}
