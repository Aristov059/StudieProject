namespace WorldPC
{
    partial class AddClientQuery
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
            this.viewClientQuery = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.viewClientQuery)).BeginInit();
            this.SuspendLayout();
            // 
            // viewClientQuery
            // 
            this.viewClientQuery.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.viewClientQuery.Location = new System.Drawing.Point(12, 12);
            this.viewClientQuery.Name = "viewClientQuery";
            this.viewClientQuery.Size = new System.Drawing.Size(604, 343);
            this.viewClientQuery.TabIndex = 0;
            this.viewClientQuery.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.viewClientQuery_CellClick);
            // 
            // AddClientQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.ClientSize = new System.Drawing.Size(628, 367);
            this.Controls.Add(this.viewClientQuery);
            this.Name = "AddClientQuery";
            this.Text = "Выбор клиента в заказ";
            this.Load += new System.EventHandler(this.AddClientQuery_Load);
            ((System.ComponentModel.ISupportInitialize)(this.viewClientQuery)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView viewClientQuery;
    }
}