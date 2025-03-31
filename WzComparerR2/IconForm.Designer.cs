namespace WzComparerR2
{
    partial class IconForm
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
            listBox1 = new System.Windows.Forms.ListBox();
            panel1 = new System.Windows.Forms.Panel();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 16;
            listBox1.Items.AddRange(new object[] { "Cash", "Consume", "Weapon", "Cap", "Coat", "Longcoat", "Pants", "Shoe", "Glove", "Ring", "Cape", "Accessory", "Shield", "TamingMob", "Hair", "Face", "Map", "Mob", "Skill", "Npc", "Pet", "Install", "Android", "Mechanic", "PetEquip", "Bit", "MonsterBattle", "Totem", "Morph", "Familiar", "Damage Skin", "Etc", "Reacter", "Package", "", "", "", "", "", "", "", "", "", "" });
            listBox1.Location = new System.Drawing.Point(9, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new System.Drawing.Size(131, 532);
            listBox1.TabIndex = 0;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel1.Location = new System.Drawing.Point(152, 13);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(808, 529);
            panel1.TabIndex = 1;
            // 
            // IconForm
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(969, 550);
            Controls.Add(panel1);
            Controls.Add(listBox1);
            Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            Name = "IconForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Icon";
            TopMost = true;
            FormClosing += IconForm_FormClosing;
            Load += IconForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel1;
    }
}