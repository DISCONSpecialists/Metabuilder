namespace MetaBuilder.UIControls.Dialogs.RelationshipManager
{
    partial class TreeContainer
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
            this.tree = new System.Windows.Forms.TreeView();
            this.cxMenuObjectTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cxMenuObjectTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // tree
            // 
            this.tree.BackColor = System.Drawing.SystemColors.Window;
            this.tree.CheckBoxes = true;
            this.tree.ContextMenuStrip = this.cxMenuObjectTree;
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Name = "tree";
            this.tree.Size = new System.Drawing.Size(292, 273);
            this.tree.TabIndex = 2;
            this.tree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterCheck);
            // 
            // cxMenuObjectTree
            // 
            this.cxMenuObjectTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addItemsToolStripMenuItem});
            this.cxMenuObjectTree.Name = "contextMenuStrip1";
            this.cxMenuObjectTree.Size = new System.Drawing.Size(147, 26);
            // 
            // addItemsToolStripMenuItem
            // 
            this.addItemsToolStripMenuItem.Name = "addItemsToolStripMenuItem";
            this.addItemsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.addItemsToolStripMenuItem.Text = "Add Items...";
            this.addItemsToolStripMenuItem.Click += new System.EventHandler(this.addItemsToolStripMenuItem_Click);
            // 
            // TreeContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.tree);
            this.Name = "TreeContainer";
            this.TabText = "TreeContainer";
            this.Text = "TreeContainer";
            this.cxMenuObjectTree.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.ContextMenuStrip cxMenuObjectTree;
        private System.Windows.Forms.ToolStripMenuItem addItemsToolStripMenuItem;
    }
}