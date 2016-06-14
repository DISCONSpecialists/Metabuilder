namespace MetaBuilder.UIControls.Dialogs
{
    partial class RemoveOrMarkForDelete
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
            this.table1 = new XPTable.Models.Table();
            this.columnModel1 = new XPTable.Models.ColumnModel();
            this.colItem = new XPTable.Models.TextColumn();
            this.colType = new XPTable.Models.TextColumn();
            this.colElsewhere = new XPTable.Models.CheckBoxColumn();
            this.comboChoice = new XPTable.Models.ComboBoxColumn();
            this.tableModel1 = new XPTable.Models.TableModel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.gradientCaption1 = new Ascend.Windows.Forms.GradientCaption();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnSetAsDefault = new System.Windows.Forms.Button();
            this.comboDefault = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // table1
            // 
            this.table1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.table1.ColumnModel = this.columnModel1;
            this.table1.ColumnResizing = false;
            this.table1.EditStartAction = XPTable.Editors.EditStartAction.SingleClick;
            this.table1.Location = new System.Drawing.Point(0, 30);
            this.table1.Name = "table1";
            this.table1.Size = new System.Drawing.Size(508, 254);
            this.table1.TabIndex = 0;
            this.table1.TableModel = this.tableModel1;
            this.table1.Text = "table1";
            this.table1.BeforePaintCell += new XPTable.Events.PaintCellEventHandler(this.table1_BeforePaintCell);
            this.table1.CellClick += new XPTable.Events.CellMouseEventHandler(this.table1_CellClick);
            this.table1.BeginEditing += new XPTable.Events.CellEditEventHandler(this.table1_BeginEditing);
            this.table1.CellMouseDown += new XPTable.Events.CellMouseEventHandler(this.table1_CellMouseDown);
            // 
            // columnModel1
            // 
            this.columnModel1.Columns.AddRange(new XPTable.Models.Column[] {
            this.colItem,
            this.colType,
            this.colElsewhere,
            this.comboChoice});
            // 
            // colItem
            // 
            this.colItem.Editable = false;
            this.colItem.Text = "Item";
            this.colItem.Width = 250;
            // 
            // colType
            // 
            this.colType.Editable = false;
            this.colType.Text = "Type";
            // 
            // colElsewhere
            // 
            this.colElsewhere.Text = "Exists Elsewhere";
            this.colElsewhere.Width = 100;
            // 
            // comboChoice
            // 
            this.comboChoice.Editable = false;
            this.comboChoice.Text = "Choice";
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.SystemColors.Control;
            this.pnlTop.Controls.Add(this.gradientCaption1);
            this.pnlTop.Controls.Add(this.table1);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(512, 316);
            this.pnlTop.TabIndex = 1;
            // 
            // gradientCaption1
            // 
            this.gradientCaption1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientCaption1.ForeColor = System.Drawing.Color.Black;
            this.gradientCaption1.GradientHighColor = System.Drawing.Color.White;
            this.gradientCaption1.GradientLowColor = System.Drawing.Color.Orange;
            this.gradientCaption1.Location = new System.Drawing.Point(0, 0);
            this.gradientCaption1.Name = "gradientCaption1";
            this.gradientCaption1.Size = new System.Drawing.Size(512, 31);
            this.gradientCaption1.TabIndex = 1;
            this.gradientCaption1.Text = "Action to be taken:";
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnSetAsDefault);
            this.pnlBottom.Controls.Add(this.comboDefault);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 285);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(512, 31);
            this.pnlBottom.TabIndex = 2;
            // 
            // btnSetAsDefault
            // 
            this.btnSetAsDefault.Location = new System.Drawing.Point(131, 5);
            this.btnSetAsDefault.Name = "btnSetAsDefault";
            this.btnSetAsDefault.Size = new System.Drawing.Size(131, 23);
            this.btnSetAsDefault.TabIndex = 3;
            this.btnSetAsDefault.Text = "Set As Default Action";
            this.btnSetAsDefault.UseVisualStyleBackColor = true;
            this.btnSetAsDefault.Click += new System.EventHandler(this.btnSetAsDefault_Click);
            // 
            // comboDefault
            // 
            this.comboDefault.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDefault.FormattingEnabled = true;
            this.comboDefault.Items.AddRange(new object[] {
            "Remove",
            "Delete",
            "Cancel"});
            this.comboDefault.Location = new System.Drawing.Point(4, 5);
            this.comboDefault.Name = "comboDefault";
            this.comboDefault.Size = new System.Drawing.Size(121, 21);
            this.comboDefault.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(433, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(352, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // RemoveOrMarkForDelete
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(106)))), ((int)(((byte)(179)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(512, 316);
            this.ControlBox = false;
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RemoveOrMarkForDelete";
            this.Text = "Remove Or Mark For Delete";
            this.Load += new System.EventHandler(this.RemoveOrMarkForDelete_Load);
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private XPTable.Models.Table table1;
        private XPTable.Models.TableModel tableModel1;
        private XPTable.Models.ColumnModel columnModel1;
        private XPTable.Models.TextColumn colItem;
        private XPTable.Models.TextColumn colType;
        private XPTable.Models.ComboBoxColumn comboChoice;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBottom;
        private Ascend.Windows.Forms.GradientCaption gradientCaption1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private XPTable.Models.CheckBoxColumn colElsewhere;
        private System.Windows.Forms.Button btnSetAsDefault;
        private System.Windows.Forms.ComboBox comboDefault;
    }
}