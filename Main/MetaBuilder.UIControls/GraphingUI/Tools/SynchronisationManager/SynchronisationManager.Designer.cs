namespace MetaBuilder.UIControls.GraphingUI.Tools.SynchronisationManager
{
    partial class SynchronisationManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SynchronisationManager));
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelWorkspace = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxWorkspace = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabelWorkspacePermission = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButtonRefreshRepository = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSwitchRepository = new System.Windows.Forms.ToolStripButton();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.buttonExecute = new System.Windows.Forms.Button();
            this.dataGridViewRepository = new System.Windows.Forms.DataGridView();
            this.labelProgress = new System.Windows.Forms.Label();
            this.tableRepository = new XPTable.Models.Table();
            this.columnModel1 = new XPTable.Models.ColumnModel();
            this.textColumn1 = new XPTable.Models.TextColumn();
            this.textColumn2 = new XPTable.Models.TextColumn();
            this.textColumn3 = new XPTable.Models.TextColumn();
            this.textColumn4 = new XPTable.Models.TextColumn();
            this.textColumn5 = new XPTable.Models.TextColumn();
            this.textColumn6 = new XPTable.Models.TextColumn();
            this.comboBoxColumn1 = new XPTable.Models.ComboBoxColumn();
            this.tableModel1 = new XPTable.Models.TableModel();
            this.toolStripButtonLoadEmbedded = new System.Windows.Forms.ToolStripButton();
            this.toolStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRepository)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableRepository)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripMain
            // 
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelWorkspace,
            this.toolStripComboBoxWorkspace,
            this.toolStripLabelWorkspacePermission,
            this.toolStripButtonRefreshRepository,
            this.toolStripButtonSwitchRepository,
            this.toolStripButtonLoadEmbedded});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(703, 25);
            this.toolStripMain.TabIndex = 0;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // toolStripLabelWorkspace
            // 
            this.toolStripLabelWorkspace.Name = "toolStripLabelWorkspace";
            this.toolStripLabelWorkspace.Size = new System.Drawing.Size(71, 22);
            this.toolStripLabelWorkspace.Text = "Workspace :";
            // 
            // toolStripComboBoxWorkspace
            // 
            this.toolStripComboBoxWorkspace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxWorkspace.Name = "toolStripComboBoxWorkspace";
            this.toolStripComboBoxWorkspace.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBoxWorkspace.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxWorkspace_SelectedIndexChanged);
            // 
            // toolStripLabelWorkspacePermission
            // 
            this.toolStripLabelWorkspacePermission.Name = "toolStripLabelWorkspacePermission";
            this.toolStripLabelWorkspacePermission.Size = new System.Drawing.Size(98, 22);
            this.toolStripLabelWorkspacePermission.Text = "Permissions Held";
            // 
            // toolStripButtonRefreshRepository
            // 
            this.toolStripButtonRefreshRepository.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonRefreshRepository.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRefreshRepository.Image")));
            this.toolStripButtonRefreshRepository.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefreshRepository.Name = "toolStripButtonRefreshRepository";
            this.toolStripButtonRefreshRepository.Size = new System.Drawing.Size(50, 22);
            this.toolStripButtonRefreshRepository.Text = "Refresh";
            this.toolStripButtonRefreshRepository.Visible = false;
            this.toolStripButtonRefreshRepository.Click += new System.EventHandler(this.toolStripButtonRefreshRepository_Click);
            // 
            // toolStripButtonSwitchRepository
            // 
            this.toolStripButtonSwitchRepository.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSwitchRepository.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSwitchRepository.Image")));
            this.toolStripButtonSwitchRepository.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSwitchRepository.Name = "toolStripButtonSwitchRepository";
            this.toolStripButtonSwitchRepository.Size = new System.Drawing.Size(136, 22);
            this.toolStripButtonSwitchRepository.Text = "Show Diagram[Objects]";
            this.toolStripButtonSwitchRepository.Visible = false;
            this.toolStripButtonSwitchRepository.Click += new System.EventHandler(this.toolStripButtonSwitchRepository_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(12, 421);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(598, 13);
            this.progressBar.TabIndex = 1;
            // 
            // buttonExecute
            // 
            this.buttonExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExecute.Location = new System.Drawing.Point(616, 411);
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.Size = new System.Drawing.Size(75, 23);
            this.buttonExecute.TabIndex = 2;
            this.buttonExecute.Text = "Execute";
            this.buttonExecute.UseVisualStyleBackColor = true;
            this.buttonExecute.Click += new System.EventHandler(this.buttonExecute_Click);
            // 
            // dataGridViewRepository
            // 
            this.dataGridViewRepository.AllowUserToAddRows = false;
            this.dataGridViewRepository.AllowUserToDeleteRows = false;
            this.dataGridViewRepository.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewRepository.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridViewRepository.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRepository.Location = new System.Drawing.Point(455, 277);
            this.dataGridViewRepository.Name = "dataGridViewRepository";
            this.dataGridViewRepository.ReadOnly = true;
            this.dataGridViewRepository.Size = new System.Drawing.Size(157, 111);
            this.dataGridViewRepository.TabIndex = 3;
            // 
            // labelProgress
            // 
            this.labelProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelProgress.BackColor = System.Drawing.Color.Transparent;
            this.labelProgress.Location = new System.Drawing.Point(12, 404);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(598, 13);
            this.labelProgress.TabIndex = 4;
            this.labelProgress.Text = "Current Action and progress";
            this.labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableRepository
            // 
            this.tableRepository.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableRepository.ColumnModel = this.columnModel1;
            this.tableRepository.EditStartAction = XPTable.Editors.EditStartAction.SingleClick;
            this.tableRepository.EnableHeaderContextMenu = false;
            this.tableRepository.FullRowSelect = true;
            this.tableRepository.GridLines = XPTable.Models.GridLines.Rows;
            this.tableRepository.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.tableRepository.Location = new System.Drawing.Point(12, 28);
            this.tableRepository.Name = "tableRepository";
            this.tableRepository.Size = new System.Drawing.Size(679, 371);
            this.tableRepository.TabIndex = 5;
            this.tableRepository.TableModel = this.tableModel1;
            this.tableRepository.Text = "Repository Table";
            this.tableRepository.EditingStopped += new XPTable.Events.CellEditEventHandler(this.tableRepository_EditingStopped);
            this.tableRepository.SelectionChanged += new XPTable.Events.SelectionEventHandler(this.tableRepository_SelectionChanged);
            this.tableRepository.BeginEditing += new XPTable.Events.CellEditEventHandler(this.tableRepository_BeginEditing);
            // 
            // columnModel1
            // 
            this.columnModel1.Columns.AddRange(new XPTable.Models.Column[] {
            this.textColumn1,
            this.textColumn2,
            this.textColumn3,
            this.textColumn4,
            this.textColumn5,
            this.textColumn6,
            this.comboBoxColumn1});
            // 
            // textColumn1
            // 
            this.textColumn1.Text = "ID";
            // 
            // textColumn2
            // 
            this.textColumn2.Text = "Workspace";
            // 
            // textColumn3
            // 
            this.textColumn3.Text = "User";
            // 
            // textColumn4
            // 
            this.textColumn4.Text = "Description";
            // 
            // textColumn5
            // 
            this.textColumn5.Text = "Client State";
            // 
            // textColumn6
            // 
            this.textColumn6.Text = "Server State";
            // 
            // comboBoxColumn1
            // 
            this.comboBoxColumn1.Text = "Action";
            // 
            // toolStripButtonLoadEmbedded
            // 
            this.toolStripButtonLoadEmbedded.CheckOnClick = true;
            this.toolStripButtonLoadEmbedded.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonLoadEmbedded.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLoadEmbedded.Image")));
            this.toolStripButtonLoadEmbedded.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLoadEmbedded.Name = "toolStripButtonLoadEmbedded";
            this.toolStripButtonLoadEmbedded.Size = new System.Drawing.Size(97, 22);
            this.toolStripButtonLoadEmbedded.Text = "Load Embedded";
            // 
            // SynchronisationManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 442);
            this.Controls.Add(this.tableRepository);
            this.Controls.Add(this.labelProgress);
            this.Controls.Add(this.buttonExecute);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.toolStripMain);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "SynchronisationManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Synchronisation Manager";
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRepository)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableRepository)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxWorkspace;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefreshRepository;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button buttonExecute;
        private System.Windows.Forms.DataGridView dataGridViewRepository;
        private System.Windows.Forms.ToolStripLabel toolStripLabelWorkspacePermission;
        private System.Windows.Forms.ToolStripButton toolStripButtonSwitchRepository;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.ToolStripLabel toolStripLabelWorkspace;
        private XPTable.Models.Table tableRepository;
        private XPTable.Models.ColumnModel columnModel1;
        private XPTable.Models.TextColumn textColumn1;
        private XPTable.Models.TextColumn textColumn2;
        private XPTable.Models.TextColumn textColumn3;
        private XPTable.Models.TextColumn textColumn4;
        private XPTable.Models.TextColumn textColumn5;
        private XPTable.Models.TextColumn textColumn6;
        private XPTable.Models.ComboBoxColumn comboBoxColumn1;
        private XPTable.Models.TableModel tableModel1;
        private System.Windows.Forms.ToolStripButton toolStripButtonLoadEmbedded;
    }
}