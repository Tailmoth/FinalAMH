namespace AeroMaterialHandlingDatabaseApplication
{
    partial class ListItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbPictureListItem = new System.Windows.Forms.PictureBox();
            this.lblTitleListItem = new System.Windows.Forms.Label();
            this.tbShortDescListItem = new System.Windows.Forms.TextBox();
            this.lblTagsListItem = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbPictureListItem)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbPictureListItem
            // 
            this.pbPictureListItem.Image = global::AeroMaterialHandlingDatabaseApplication.Properties.Resources.Logo_Complete_Short_Hook_Transparent_300dpi_405_;
            this.pbPictureListItem.Location = new System.Drawing.Point(4, 4);
            this.pbPictureListItem.Margin = new System.Windows.Forms.Padding(4);
            this.pbPictureListItem.Name = "pbPictureListItem";
            this.pbPictureListItem.Size = new System.Drawing.Size(200, 185);
            this.pbPictureListItem.TabIndex = 0;
            this.pbPictureListItem.TabStop = false;
            this.pbPictureListItem.DoubleClick += new System.EventHandler(this.pbPictureListItem_DoubleClick);
            // 
            // lblTitleListItem
            // 
            this.lblTitleListItem.AutoSize = true;
            this.lblTitleListItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleListItem.Location = new System.Drawing.Point(232, 13);
            this.lblTitleListItem.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitleListItem.Name = "lblTitleListItem";
            this.lblTitleListItem.Size = new System.Drawing.Size(66, 29);
            this.lblTitleListItem.TabIndex = 1;
            this.lblTitleListItem.Text = "Title";
            // 
            // tbShortDescListItem
            // 
            this.tbShortDescListItem.BackColor = System.Drawing.SystemColors.GrayText;
            this.tbShortDescListItem.Location = new System.Drawing.Point(228, 91);
            this.tbShortDescListItem.Margin = new System.Windows.Forms.Padding(4);
            this.tbShortDescListItem.Multiline = true;
            this.tbShortDescListItem.Name = "tbShortDescListItem";
            this.tbShortDescListItem.ReadOnly = true;
            this.tbShortDescListItem.Size = new System.Drawing.Size(467, 58);
            this.tbShortDescListItem.TabIndex = 2;
            // 
            // lblTagsListItem
            // 
            this.lblTagsListItem.AutoSize = true;
            this.lblTagsListItem.BackColor = System.Drawing.SystemColors.ControlDark;
            this.lblTagsListItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTagsListItem.Location = new System.Drawing.Point(232, 52);
            this.lblTagsListItem.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTagsListItem.Name = "lblTagsListItem";
            this.lblTagsListItem.Size = new System.Drawing.Size(57, 25);
            this.lblTagsListItem.TabIndex = 3;
            this.lblTagsListItem.Text = "Tags";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 158);
            this.panel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.panel2.Location = new System.Drawing.Point(31, 23);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(134, 112);
            this.panel2.TabIndex = 0;
            // 
            // ListItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblTagsListItem);
            this.Controls.Add(this.tbShortDescListItem);
            this.Controls.Add(this.lblTitleListItem);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ListItem";
            this.Size = new System.Drawing.Size(709, 165);
            this.Load += new System.EventHandler(this.ListItem_Load);
            this.Click += new System.EventHandler(this.ListItem_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pbPictureListItem)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPictureListItem;
        private System.Windows.Forms.Label lblTitleListItem;
        private System.Windows.Forms.TextBox tbShortDescListItem;
        private System.Windows.Forms.Label lblTagsListItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}
