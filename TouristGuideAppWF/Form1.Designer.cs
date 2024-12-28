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
            SuspendLayout();
            // 
            // SearchBtn
            // 
            SearchBtn.Location = new Point(14, 117);
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
            CityNameInput.Location = new Point(236, 64);
            CityNameInput.Margin = new Padding(3, 4, 3, 4);
            CityNameInput.Name = "CityNameInput";
            CityNameInput.Size = new Size(348, 27);
            CityNameInput.TabIndex = 2;
            CityNameInput.TextChanged += textBox1_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 71);
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
            ResultTextBox.Size = new Size(886, 276);
            ResultTextBox.TabIndex = 4;
            ResultTextBox.Text = "";
            ResultTextBox.TextChanged += ResultTextBox_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 600);
            Controls.Add(ResultTextBox);
            Controls.Add(label2);
            Controls.Add(CityNameInput);
            Controls.Add(SearchBtn);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button SearchBtn;
        private TextBox CityNameInput;
        private Label label2;
        private RichTextBox ResultTextBox;
    }
}
