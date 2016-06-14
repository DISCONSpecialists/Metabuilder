namespace MetaBuilder.UIControls.GraphingUI.SubgraphBinding
{
    partial class PromptKeepAssociations
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
            this.tableModel1 = new XPTable.Models.TableModel();
            this.table1 = new XPTable.Models.Table();
            this.columnModel1 = new XPTable.Models.ColumnModel();
            this.textColumn1 = new XPTable.Models.TextColumn();
            this.textColumn2 = new XPTable.Models.TextColumn();
            this.checkBoxColumn1 = new XPTable.Models.CheckBoxColumn();
            this.table2 = new XPTable.Models.Table();
            this.textColumn3 = new XPTable.Models.TextColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new MetaControls.MetaButton();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // table1
            // 
            this.table1.Location = new System.Drawing.Point(12, 12);
            this.table1.Name = "table1";
            this.table1.Size = new System.Drawing.Size(523, 211);
            this.table1.TabIndex = 0;
            this.table1.Text = "table1";
            // 
            // columnModel1
            // 
            this.columnModel1.Columns.AddRange(new XPTable.Models.Column[] {
            this.textColumn1,
            this.textColumn2,
            this.textColumn3,
            this.checkBoxColumn1});
            // 
            // textColumn1
            // 
            this.textColumn1.Text = "Container";
            this.textColumn1.Width = 200;
            // 
            // textColumn2
            // 
            this.textColumn2.Text = "Child Object";
            this.textColumn2.Width = 200;
            // 
            // checkBoxColumn1
            // 
            this.checkBoxColumn1.Text = "Keep This Association?";
            this.checkBoxColumn1.Width = 130;
            // 
            // table2
            // 
            this.table2.ColumnModel = this.columnModel1;
            this.table2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table2.Location = new System.Drawing.Point(0, 0);
            this.table2.Name = "table2";
            this.table2.Size = new System.Drawing.Size(655, 309);
            this.table2.TabIndex = 0;
            this.table2.TableModel = this.tableModel1;
            this.table2.Text = "table2";
            // 
            // textColumn3
            // 
            this.textColumn3.Text = "Type";
            this.textColumn3.Width = 120;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 309);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(655, 44);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.table2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(655, 309);
            this.panel2.TabIndex = 2;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(577, 9);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // PromptKeepAssociations
            // 
            this.ClientSize = new System.Drawing.Size(655, 353);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "PromptKeepAssociations";
            this.Text = "Do you want to keep these associations?";
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private XPTable.Models.TableModel tableModel1;
        private XPTable.Models.Table table1;
        private XPTable.Models.ColumnModel columnModel1;
        private XPTable.Models.TextColumn textColumn1;
        private XPTable.Models.TextColumn textColumn2;
        private XPTable.Models.CheckBoxColumn checkBoxColumn1;
        private XPTable.Models.Table table2;
        private XPTable.Models.TextColumn textColumn3;
        private System.Windows.Forms.Panel panel1;
        private MetaControls.MetaButton btnOK;
        private System.Windows.Forms.Panel panel2;
    }
}