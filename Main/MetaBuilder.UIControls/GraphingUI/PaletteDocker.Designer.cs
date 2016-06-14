using System.Drawing;

namespace MetaBuilder.UIControls.GraphingUI
{
    partial class PaletteDocker
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaletteDocker));
            MetaBuilder.Docking.DockPanelSkin dockPanelSkin5 = new MetaBuilder.Docking.DockPanelSkin();
            MetaBuilder.Docking.AutoHideStripSkin autoHideStripSkin5 = new MetaBuilder.Docking.AutoHideStripSkin();
            MetaBuilder.Docking.DockPanelGradient dockPanelGradient13 = new MetaBuilder.Docking.DockPanelGradient();
            MetaBuilder.Docking.TabGradient tabGradient29 = new MetaBuilder.Docking.TabGradient();
            MetaBuilder.Docking.DockPaneStripSkin dockPaneStripSkin5 = new MetaBuilder.Docking.DockPaneStripSkin();
            MetaBuilder.Docking.DockPaneStripGradient dockPaneStripGradient5 = new MetaBuilder.Docking.DockPaneStripGradient();
            MetaBuilder.Docking.TabGradient tabGradient30 = new MetaBuilder.Docking.TabGradient();
            MetaBuilder.Docking.DockPanelGradient dockPanelGradient14 = new MetaBuilder.Docking.DockPanelGradient();
            MetaBuilder.Docking.TabGradient tabGradient31 = new MetaBuilder.Docking.TabGradient();
            MetaBuilder.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient5 = new MetaBuilder.Docking.DockPaneStripToolWindowGradient();
            MetaBuilder.Docking.TabGradient tabGradient32 = new MetaBuilder.Docking.TabGradient();
            MetaBuilder.Docking.TabGradient tabGradient33 = new MetaBuilder.Docking.TabGradient();
            MetaBuilder.Docking.DockPanelGradient dockPanelGradient15 = new MetaBuilder.Docking.DockPanelGradient();
            MetaBuilder.Docking.TabGradient tabGradient34 = new MetaBuilder.Docking.TabGradient();
            MetaBuilder.Docking.TabGradient tabGradient35 = new MetaBuilder.Docking.TabGradient();
            this.contextMenuTabPage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cxMenuItemClose = new System.Windows.Forms.ToolStripMenuItem();
            this.cxMenuItemCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.cxMenuItemCloseAllButThis = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.stripButtonNew = new System.Windows.Forms.ToolStripButton();
            this.stripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.stripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.dockPanel1 = new MetaBuilder.Docking.DockPanel();
            this.panelDescription = new System.Windows.Forms.Panel();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelClass = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonDescriptions = new System.Windows.Forms.ToolStripButton();
            this.contextMenuTabPage.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.panelDescription.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuTabPage
            // 
            this.contextMenuTabPage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cxMenuItemClose,
            this.cxMenuItemCloseAll,
            this.cxMenuItemCloseAllButThis});
            this.contextMenuTabPage.Name = "contextMenuTabPage";
            this.contextMenuTabPage.Size = new System.Drawing.Size(167, 70);
            // 
            // cxMenuItemClose
            // 
            this.cxMenuItemClose.Name = "cxMenuItemClose";
            this.cxMenuItemClose.Size = new System.Drawing.Size(166, 22);
            this.cxMenuItemClose.Text = "&Close";
            this.cxMenuItemClose.Click += new System.EventHandler(this.cxMenuItemClose_Click);
            // 
            // cxMenuItemCloseAll
            // 
            this.cxMenuItemCloseAll.Name = "cxMenuItemCloseAll";
            this.cxMenuItemCloseAll.Size = new System.Drawing.Size(166, 22);
            this.cxMenuItemCloseAll.Text = "Close &All";
            // 
            // cxMenuItemCloseAllButThis
            // 
            this.cxMenuItemCloseAllButThis.Name = "cxMenuItemCloseAllButThis";
            this.cxMenuItemCloseAllButThis.Size = new System.Drawing.Size(166, 22);
            this.cxMenuItemCloseAllButThis.Text = "Close All &But This";
            // 
            // toolStripMain
            // 
            this.toolStripMain.AllowMerge = false;
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stripButtonNew,
            this.stripButtonOpen,
            this.stripButtonSave,
            this.toolStripSeparator1,
            this.toolStripButtonDescriptions});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(232, 25);
            this.toolStripMain.TabIndex = 7;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // stripButtonNew
            // 
            this.stripButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripButtonNew.Image = ((System.Drawing.Image)(resources.GetObject("stripButtonNew.Image")));
            this.stripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripButtonNew.MergeIndex = 1;
            this.stripButtonNew.Name = "stripButtonNew";
            this.stripButtonNew.Size = new System.Drawing.Size(23, 22);
            this.stripButtonNew.Text = "&New";
            this.stripButtonNew.Click += new System.EventHandler(this.stripButtonNew_Click);
            // 
            // stripButtonOpen
            // 
            this.stripButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripButtonOpen.Image = ((System.Drawing.Image)(resources.GetObject("stripButtonOpen.Image")));
            this.stripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripButtonOpen.MergeIndex = 2;
            this.stripButtonOpen.Name = "stripButtonOpen";
            this.stripButtonOpen.Size = new System.Drawing.Size(23, 22);
            this.stripButtonOpen.Text = "&Open";
            this.stripButtonOpen.Click += new System.EventHandler(this.stripButtonOpen_Click);
            // 
            // stripButtonSave
            // 
            this.stripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("stripButtonSave.Image")));
            this.stripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripButtonSave.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.stripButtonSave.MergeIndex = 3;
            this.stripButtonSave.Name = "stripButtonSave";
            this.stripButtonSave.Size = new System.Drawing.Size(23, 22);
            this.stripButtonSave.Text = "&Save";
            this.stripButtonSave.Click += new System.EventHandler(this.stripButtonSave_Click);
            // 
            // dockPanel1
            // 
            this.dockPanel1.ActiveAutoHideContent = null;
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.DockBackColor = System.Drawing.SystemColors.AppWorkspace;
            this.dockPanel1.DocumentStyle = MetaBuilder.Docking.DocumentStyle.DockingWindow;
            this.dockPanel1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.dockPanel1.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(232, 314);
            dockPanelGradient13.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient13.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin5.DockStripGradient = dockPanelGradient13;
            tabGradient29.EndColor = System.Drawing.SystemColors.Control;
            tabGradient29.StartColor = System.Drawing.SystemColors.Control;
            tabGradient29.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin5.TabGradient = tabGradient29;
            dockPanelSkin5.AutoHideStripSkin = autoHideStripSkin5;
            tabGradient30.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient30.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient30.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient5.ActiveTabGradient = tabGradient30;
            dockPanelGradient14.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient14.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient5.DockStripGradient = dockPanelGradient14;
            tabGradient31.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient31.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient31.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient5.InactiveTabGradient = tabGradient31;
            dockPaneStripSkin5.DocumentGradient = dockPaneStripGradient5;
            tabGradient32.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient32.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient32.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient32.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient5.ActiveCaptionGradient = tabGradient32;
            tabGradient33.EndColor = System.Drawing.SystemColors.Control;
            tabGradient33.StartColor = System.Drawing.SystemColors.Control;
            tabGradient33.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient5.ActiveTabGradient = tabGradient33;
            dockPanelGradient15.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient15.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient5.DockStripGradient = dockPanelGradient15;
            tabGradient34.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient34.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient34.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient34.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient5.InactiveCaptionGradient = tabGradient34;
            tabGradient35.EndColor = System.Drawing.Color.Transparent;
            tabGradient35.StartColor = System.Drawing.Color.Transparent;
            tabGradient35.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient5.InactiveTabGradient = tabGradient35;
            dockPaneStripSkin5.ToolWindowGradient = dockPaneStripToolWindowGradient5;
            dockPanelSkin5.DockPaneStripSkin = dockPaneStripSkin5;
            this.dockPanel1.Skin = dockPanelSkin5;
            this.dockPanel1.TabIndex = 8;
            // 
            // panelDescription
            // 
            this.panelDescription.Controls.Add(this.labelDescription);
            this.panelDescription.Controls.Add(this.labelClass);
            this.panelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDescription.Location = new System.Drawing.Point(0, 0);
            this.panelDescription.Name = "panelDescription";
            this.panelDescription.Size = new System.Drawing.Size(232, 89);
            this.panelDescription.TabIndex = 9;
            // 
            // labelDescription
            // 
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDescription.Location = new System.Drawing.Point(0, 13);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(232, 76);
            this.labelDescription.TabIndex = 0;
            this.labelDescription.Text = "Description";
            // 
            // labelClass
            // 
            this.labelClass.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelClass.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelClass.Location = new System.Drawing.Point(0, 0);
            this.labelClass.Name = "labelClass";
            this.labelClass.Size = new System.Drawing.Size(232, 13);
            this.labelClass.TabIndex = 0;
            this.labelClass.Text = "Class";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dockPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelDescription);
            this.splitContainer1.Size = new System.Drawing.Size(232, 407);
            this.splitContainer1.SplitterDistance = 314;
            this.splitContainer1.TabIndex = 10;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonDescriptions
            // 
            this.toolStripButtonDescriptions.Checked = true;
            this.toolStripButtonDescriptions.CheckOnClick = true;
            this.toolStripButtonDescriptions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonDescriptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDescriptions.Image = global::MetaBuilder.UIControls.Properties.Resources.DescriptionIcon;
            this.toolStripButtonDescriptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDescriptions.Name = "toolStripButtonDescriptions";
            this.toolStripButtonDescriptions.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDescriptions.Text = "Descriptions";
            this.toolStripButtonDescriptions.ToolTipText = "Display Descriptions for the Selected Shape";
            // 
            // PaletteDocker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(232, 432);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStripMain);
            this.DockAreas = ((MetaBuilder.Docking.DockAreas)((((MetaBuilder.Docking.DockAreas.Float | MetaBuilder.Docking.DockAreas.DockLeft)
                        | MetaBuilder.Docking.DockAreas.DockRight)
                        | MetaBuilder.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "PaletteDocker";
            this.TabText = "Stencils";
            this.Text = "Stencils";
            this.contextMenuTabPage.ResumeLayout(false);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.panelDescription.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuTabPage;
        private System.Windows.Forms.ToolStripMenuItem cxMenuItemClose;
        private System.Windows.Forms.ToolStripMenuItem cxMenuItemCloseAllButThis;
        private System.Windows.Forms.ToolStripMenuItem cxMenuItemCloseAll;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripButton stripButtonSave;
        internal System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton stripButtonNew;
        private System.Windows.Forms.ToolStripButton stripButtonOpen;
        internal Docking.DockPanel dockPanel1;
        private System.Windows.Forms.Panel panelDescription;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelClass;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonDescriptions;

    }
}