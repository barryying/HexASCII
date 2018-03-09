namespace HexASCII
{
    partial class Form1
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
            this.txt_value = new System.Windows.Forms.TextBox();
            this.txt_ebcdicstring = new System.Windows.Forms.TextBox();
            this.ValueToTxt = new System.Windows.Forms.Button();
            this.comb_isComp3 = new System.Windows.Forms.ComboBox();
            this.comb_isUnsign = new System.Windows.Forms.ComboBox();
            this.comb_isInt = new System.Windows.Forms.ComboBox();
            this.txt_integerPartnum = new System.Windows.Forms.TextBox();
            this.txt_decimalsPartnum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt_value
            // 
            this.txt_value.Location = new System.Drawing.Point(175, 23);
            this.txt_value.Name = "txt_value";
            this.txt_value.Size = new System.Drawing.Size(212, 25);
            this.txt_value.TabIndex = 0;
            // 
            // txt_ebcdicstring
            // 
            this.txt_ebcdicstring.Location = new System.Drawing.Point(481, 132);
            this.txt_ebcdicstring.Multiline = true;
            this.txt_ebcdicstring.Name = "txt_ebcdicstring";
            this.txt_ebcdicstring.Size = new System.Drawing.Size(323, 81);
            this.txt_ebcdicstring.TabIndex = 1;
            // 
            // ValueToTxt
            // 
            this.ValueToTxt.Location = new System.Drawing.Point(537, 48);
            this.ValueToTxt.Name = "ValueToTxt";
            this.ValueToTxt.Size = new System.Drawing.Size(168, 23);
            this.ValueToTxt.TabIndex = 2;
            this.ValueToTxt.Text = "ValueToTxt";
            this.ValueToTxt.UseVisualStyleBackColor = true;
            this.ValueToTxt.Click += new System.EventHandler(this.ValueToTxt_Click);
            // 
            // comb_isComp3
            // 
            this.comb_isComp3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comb_isComp3.FormattingEnabled = true;
            this.comb_isComp3.Location = new System.Drawing.Point(175, 68);
            this.comb_isComp3.Name = "comb_isComp3";
            this.comb_isComp3.Size = new System.Drawing.Size(121, 23);
            this.comb_isComp3.TabIndex = 3;
            // 
            // comb_isUnsign
            // 
            this.comb_isUnsign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comb_isUnsign.FormattingEnabled = true;
            this.comb_isUnsign.Location = new System.Drawing.Point(175, 114);
            this.comb_isUnsign.Name = "comb_isUnsign";
            this.comb_isUnsign.Size = new System.Drawing.Size(121, 23);
            this.comb_isUnsign.TabIndex = 4;
            // 
            // comb_isInt
            // 
            this.comb_isInt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comb_isInt.FormattingEnabled = true;
            this.comb_isInt.Location = new System.Drawing.Point(175, 161);
            this.comb_isInt.Name = "comb_isInt";
            this.comb_isInt.Size = new System.Drawing.Size(121, 23);
            this.comb_isInt.TabIndex = 5;
            this.comb_isInt.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // txt_integerPartnum
            // 
            this.txt_integerPartnum.Location = new System.Drawing.Point(175, 204);
            this.txt_integerPartnum.Name = "txt_integerPartnum";
            this.txt_integerPartnum.Size = new System.Drawing.Size(212, 25);
            this.txt_integerPartnum.TabIndex = 6;
            // 
            // txt_decimalsPartnum
            // 
            this.txt_decimalsPartnum.Location = new System.Drawing.Point(175, 254);
            this.txt_decimalsPartnum.Name = "txt_decimalsPartnum";
            this.txt_decimalsPartnum.Size = new System.Drawing.Size(212, 25);
            this.txt_decimalsPartnum.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "value";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "isComp3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "isUnsign";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 169);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 15);
            this.label4.TabIndex = 11;
            this.label4.Text = "isInt";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(38, 207);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 15);
            this.label5.TabIndex = 12;
            this.label5.Text = "integerPartnum";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(38, 257);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(127, 15);
            this.label6.TabIndex = 13;
            this.label6.Text = "decimalsPartnum";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 382);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_decimalsPartnum);
            this.Controls.Add(this.txt_integerPartnum);
            this.Controls.Add(this.comb_isInt);
            this.Controls.Add(this.comb_isUnsign);
            this.Controls.Add(this.comb_isComp3);
            this.Controls.Add(this.ValueToTxt);
            this.Controls.Add(this.txt_ebcdicstring);
            this.Controls.Add(this.txt_value);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_value;
        private System.Windows.Forms.TextBox txt_ebcdicstring;
        private System.Windows.Forms.Button ValueToTxt;
        private System.Windows.Forms.ComboBox comb_isComp3;
        private System.Windows.Forms.ComboBox comb_isUnsign;
        private System.Windows.Forms.ComboBox comb_isInt;
        private System.Windows.Forms.TextBox txt_integerPartnum;
        private System.Windows.Forms.TextBox txt_decimalsPartnum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}

