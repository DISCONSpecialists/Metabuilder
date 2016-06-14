namespace MetaBuilder.UIControls.GraphingUI
{
    partial class LogDisplayer
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
            this.textColumn1 = new XPTable.Models.TextColumn();
            this.colorColumn1 = new XPTable.Models.ColorColumn();
            this.textColumn2 = new XPTable.Models.TextColumn();
            this.tableModel1 = new XPTable.Models.TableModel();
            this.table1 = new XPTable.Models.Table();
            this.columnModel1 = new XPTable.Models.ColumnModel();
            this.colRepos = new XPTable.Models.TextColumn();
            this.txtAction = new XPTable.Models.TextColumn();
            this.colFromState = new XPTable.Models.TextColumn();
            this.txtTargetState = new XPTable.Models.TextColumn();
            this.clrResult = new XPTable.Models.ColorColumn();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
            this.SuspendLayout();
            // 
            // textColumn1
            // 
            this.textColumn1.Editable = false;
            this.textColumn1.Text = "Item";
            // 
            // colorColumn1
            // 
            this.colorColumn1.Editable = false;
            this.colorColumn1.Text = "Outcome";
            // 
            // textColumn2
            // 
            this.textColumn2.Editable = false;
            this.textColumn2.Text = "Target State";
            // 
            // table1
            // 
            this.table1.ColumnModel = this.columnModel1;
            this.table1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table1.Location = new System.Drawing.Point(0, 0);
            this.table1.Name = "table1";
            //this.table1.Size = new System.Drawing.Size(675, 500);
            this.table1.TabIndex = 1;
            this.table1.TableModel = this.tableModel1;
            this.table1.Text = "table1";
            // 
            // columnModel1
            // 
            this.columnModel1.Columns.AddRange(new XPTable.Models.Column[] {
            this.colRepos,
            this.txtAction,
            this.colFromState,
            this.txtTargetState,
            this.clrResult});
            // 
            // colRepos
            // 
            this.colRepos.Editable = false;
            this.colRepos.Text = WorkspaceTransfer ? "Object" : "Repository";
            // 
            // txtAction
            // 
            this.txtAction.Editable = false;
            this.txtAction.Text = WorkspaceTransfer ? "Name" : "Item";
            this.txtAction.Width = 340;
            // 
            // colFromState
            // 
            this.colFromState.Editable = false;
            this.colFromState.Text = WorkspaceTransfer ? "Source Workspace" : "From State";
            this.colFromState.Width = 130;
            // 
            // txtTargetState
            // 
            this.txtTargetState.Editable = false;
            this.txtTargetState.Text = WorkspaceTransfer ? "Target Workspace" : "To State";
            this.txtTargetState.Width = 160;
            // 
            // clrResult
            // 
            this.clrResult.Editable = false;
            this.clrResult.Text = "Result";
            this.clrResult.Width = 40;
            // 
            // LogDisplayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 500);
            this.Controls.Add(this.table1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(750, 500);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LogDisplayer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Result Log";
            this.MouseLeave += new System.EventHandler(this.LogDisplayer_MouseLeave);
            this.ResizeEnd += new System.EventHandler(LogDisplayer_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private XPTable.Models.TextColumn textColumn1;
        private XPTable.Models.TextColumn textColumn2;
        private XPTable.Models.ColorColumn colorColumn1;
        private XPTable.Models.TableModel tableModel1;
        private XPTable.Models.Table table1;
        private XPTable.Models.ColumnModel columnModel1;
        private XPTable.Models.TextColumn txtAction;
        private XPTable.Models.TextColumn txtTargetState;
        private XPTable.Models.ColorColumn clrResult;
        private XPTable.Models.TextColumn colFromState;
        private XPTable.Models.TextColumn colRepos;
    }
}