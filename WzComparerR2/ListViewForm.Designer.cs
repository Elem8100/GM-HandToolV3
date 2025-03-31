namespace WzComparerR2
{
    partial class ListViewForm
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
            panel1 = new System.Windows.Forms.Panel();
            ListGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)ListGrid).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            panel1.Location = new System.Drawing.Point(8, 8);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(228, 523);
            panel1.TabIndex = 0;
            // 
            // ListGrid
            // 
            ListGrid.AllowUserToAddRows = false;
            ListGrid.AllowUserToDeleteRows = false;
            ListGrid.AllowUserToResizeColumns = false;
            ListGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ListGrid.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            ListGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            ListGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ListGrid.ColumnHeadersVisible = false;
            ListGrid.GridColor = System.Drawing.SystemColors.ScrollBar;
            ListGrid.Location = new System.Drawing.Point(254, 12);
            ListGrid.MultiSelect = false;
            ListGrid.Name = "ListGrid";
            ListGrid.RowHeadersVisible = false;
            ListGrid.RowHeadersWidth = 51;
            ListGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            ListGrid.ShowCellErrors = false;
            ListGrid.ShowCellToolTips = false;
            ListGrid.ShowEditingIcon = false;
            ListGrid.ShowRowErrors = false;
            ListGrid.Size = new System.Drawing.Size(784, 514);
            ListGrid.TabIndex = 1;
            // 
            // ListViewForm
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(1050, 538);
            Controls.Add(ListGrid);
            Controls.Add(panel1);
            Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            MaximumSize = new System.Drawing.Size(1300, 2000);
            Name = "ListViewForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "List View";
            TopMost = true;
            Load += ListViewForm_Load;
            ((System.ComponentModel.ISupportInitialize)ListGrid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView ListGrid;
    }
}