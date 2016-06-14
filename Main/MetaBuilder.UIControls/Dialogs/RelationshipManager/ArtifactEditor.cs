#region Copyright © 2007 - DISCON Specialists
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Modification and/or tampering with the code is also strictly prohibited, and
// punishable by law.
//
// Filename: ArtifactEditor.cs
// Author: Deon Fourie
// Last Modified: 2007-13-26.*
//
#endregion

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Docking;
using MetaBuilder.Meta;
using MetaBuilder.UIControls.Common;
using MetaBuilder.UIControls.GraphingUI;
using MetaBuilder.UIControls.Tree;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.Dialogs.RelationshipManager
{
    /// <summary>
    /// Summary description for ArtifactEditor.
    /// </summary>
    public class ArtifactEditor : DockContent
    {

        #region Fields (15)

        private MetaControls.MetaButton btnSave;
        private int caid;
        private string childMachine;
        private int childObjectID;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components = null;
        private ContextMenu contextTree;
        public bool HasArtifacts;
        private int objectID;
        private string objectMachine;
        private Panel pnlContent;
        private Panel pnlProps;
        private Panel pnlTop;
        private Panel pnlTree;
        private MetaBuilder.MetaControls.MetaPropertyGrid propGrid;
        private TreeView treeArtifacts;

        #endregion Fields

        #region Constructors (1)

        public ArtifactEditor()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties (6)

        public int CAid
        {
            get { return caid; }
            set { caid = value; }
        }

        public string ChildMachine
        {
            get { return childMachine; }
            set { childMachine = value; }
        }

        public int ChildObjectID
        {
            get { return childObjectID; }
            set { childObjectID = value; }
        }

        private TreeNode CurrentNode
        {
            get { return treeArtifacts.SelectedNode; }
            set
            {
                treeArtifacts.SelectedNode = value;
                if (treeArtifacts.SelectedNode is ArtifactNode)
                {
                    propGrid.SelectedObject = (IMetaBase)treeArtifacts.SelectedNode.Tag;
                }
                else if (treeArtifacts.SelectedNode is ArtifactPropertiesNode)
                {
                    ArtifactPropertiesNode apnode = (ArtifactPropertiesNode)value;
                    if (apnode.ibase != null)
                    {
                        propGrid.SelectedObject = apnode.ibase;
                    }
                }
                else
                {
                    propGrid.SelectedObject = null;
                }
            }
        }

        public int ObjectID
        {
            get { return objectID; }
            set { objectID = value; }
        }

        public string ObjectMachine
        {
            get { return objectMachine; }
            set { objectMachine = value; }
        }

        #endregion Properties

        #region Methods (15)

        // Public Methods (5) 

        public void ClearPropertyGrid()
        {
            this.propGrid.SelectedObject = null;
            this.btnSave.Visible = false;
        }

        public void Example()
        {
            TList<Artifact> artifacts = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ArtifactProvider.GetAll();
        }

        public void Refresh(int CAid, int ObjectID, int ChildObjectID, string objectmachine, string childmachine)
        {
            this.CAid = CAid;
            this.ObjectID = ObjectID;
            this.ChildObjectID = ChildObjectID;
            this.ObjectMachine = objectmachine;
            this.ChildMachine = childmachine;
            BindArtifacts();
        }

        public void SaveArtifacts()
        {
            try
            {
                Singletons.GetAssociationHelper().ClearArtifacts(CAid, ObjectID, childObjectID, ObjectMachine, childMachine);
                foreach (TreeNode tnode in treeArtifacts.Nodes)
                {
                    if (tnode is ArtifactClassNode)
                    {
                        ArtifactClassNode acnode = (ArtifactClassNode)tnode;
                        foreach (ArtifactNode anode in acnode.Nodes)
                        {
                            Singletons.GetAssociationHelper().AddArtifact(CAid, ObjectID, childObjectID, anode.mbase.pkid,
                                                                          ObjectMachine, childMachine,
                                                                          anode.mbase.MachineName);
                            anode.Text = anode.mbase.ToString();
                        }
                    }
                }
            }
            catch { }
        }

        public void SaveProperties()
        {
            foreach (TreeNode tnode in treeArtifacts.Nodes)
            {
                if (tnode is ArtifactPropertiesNode)
                {
                    ArtifactPropertiesNode apnode = (ArtifactPropertiesNode)tnode;
                    Singletons.GetAssociationHelper().AddAssociationObject(CAid, ObjectID, childObjectID,
                                                                           ((MetaBase)apnode.ibase).pkid, ObjectMachine,
                                                                           childMachine,
                                                                           ((MetaBase)apnode.ibase).MachineName);
                }
            }
        }

        // Protected Methods (1) 

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        // Private Methods (9) 

        private void ArtifactEditor_Load(object sender, EventArgs e)
        {
        }

        private void BindArtifacts()
        {
            HasArtifacts = false;
            treeArtifacts.Nodes.Clear();
            // Retrieve data about this association & artifact
            DataSet ds = Singletons.GetAssociationHelper().GetArtifactData(CAid, ObjectID, ChildObjectID, ObjectMachine, childMachine);
            DataView dvAssociationObjectClass = ds.Tables[2].DefaultView;
            DataView dvAllowedClasses = ds.Tables[0].DefaultView;
            DataView dvArtifacts = ds.Tables[1].DefaultView;
            // Check if the association has a secondary class associated with it
            if (dvAssociationObjectClass.Count > 0)
            {
                ArtifactPropertiesNode apnode = new ArtifactPropertiesNode("Properties");
                apnode.PropertiesClass = dvAssociationObjectClass[0]["AssociationObjectClass"].ToString();
                if (ds.Tables[3].DefaultView.Count > 0)
                {
                    DataRowView drvAssociationObject = ds.Tables[3].DefaultView[0];
                    IMetaBase imbassociationobject = Loader.GetByID(drvAssociationObject["Class"].ToString(),
                            int.Parse(drvAssociationObject["AssociationObjectID"].ToString()),
                            drvAssociationObject["ArtefactMachine"].ToString());
                    apnode.ibase = imbassociationobject;
                }
                treeArtifacts.Nodes.Add(apnode);
            }
            // Retrieve allowed artifact classes
            foreach (DataRowView drvClass in dvAllowedClasses)
            {
                ArtifactClassNode tnClass = new ArtifactClassNode(drvClass["ArtifactClass"].ToString());
                tnClass.ObjectID = ObjectID;
                tnClass.ChildObjectID = ChildObjectID;
                tnClass.ObjectClass = drvClass["ObjectClass"].ToString();
                tnClass.ChildObjectClass = drvClass["ChildObjectClass"].ToString();
                tnClass.ArtifactClass = drvClass["ArtifactClass"].ToString();

                // Retrieve artifacts for each allowed class
                dvArtifacts.RowFilter = "ArtifactClass='" + drvClass["ArtifactClass"].ToString() + "'";
                foreach (DataRowView drvArtifact in dvArtifacts)
                {
                    IMetaBase imb = Loader.GetByID(drvArtifact["ArtifactClass"].ToString(), int.Parse(drvArtifact["ArtifactObjectID"].ToString()),
                                                             drvArtifact["ArtefactMachine"].ToString());
                    ArtifactNode artnode = new ArtifactNode(imb.ToString());
                    artnode.ibase = imb;
                    tnClass.Nodes.Add(artnode);
                    HasArtifacts = true;
                }
                tnClass.Expand();
                treeArtifacts.Nodes.Add(tnClass);
            }

            if (!HasArtifacts) return;
            treeArtifacts.ExpandAll();
            Activate();
            Show();
            Select();
            Focus();
            treeArtifacts.Show();
            treeArtifacts.Select();
            treeArtifacts.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (propGrid.SelectedObject != null)
            {
                MetaBase imb = (MetaBase)propGrid.SelectedObject;
                imb.Save(Guid.NewGuid());
                //imb.IsInDatabase = true;
                if (CurrentNode != null)
                {
                    if (CurrentNode is ArtifactNode)
                    {
                        CurrentNode.Text = imb.ToString();
                    }
                }
            }
        }

        private void mnuExistingArtifact_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            string Class = menuItem.Text.Replace("Open Existing ", "");
            ObjectFinder ofinder = new ObjectFinder(false);
            ofinder.IncludeStatusCombo = true;
            ofinder.ExcludeStatuses.Add(VCStatusList.Obsolete);
            ofinder.ExcludeStatuses.Add(VCStatusList.MarkedForDelete);
            ofinder.LimitToClass = Class;
            DialogResult result = ofinder.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                foreach (MetaBase mb in ofinder.SelectedObjectsList)
                {
                    // iterate the existing artefacts of this node
                    bool dupefound = false;
                    foreach (TreeNode node in treeArtifacts.SelectedNode.Nodes)
                    {
                        ArtifactNode artNode = node as ArtifactNode;
                        if (artNode != null)
                        {
                            if (artNode.mbase.pkid == mb.pkid && artNode.mbase.MachineName == mb.MachineName)
                            {
                                dupefound = true;
                            }
                        }
                    }
                    if (!dupefound)
                    {
                        ArtifactNode anode = new ArtifactNode(mb.ToString());
                        anode.ibase = mb;
                        treeArtifacts.SelectedNode.Nodes.Add(anode);
                    }
                }
                HasArtifacts = true;
            }
        }

        private void mnuNewArtifact_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            string Class = menuItem.Text.Replace("New ", "");
            MetaBase imb = Loader.CreateInstance(Class);

            imb.Save(Guid.NewGuid());
            //imb.IsInDatabase = true;
            ArtifactNode anode = new ArtifactNode(imb.ToString());
            anode.ibase = imb;
            treeArtifacts.SelectedNode.Nodes.Add(anode);
            treeArtifacts.SelectedNode = anode;
            btnSave.Visible = true;
            CurrentNode = anode;
            HasArtifacts = true;
        }

        private void mnuRemoveArtifact_Click(object sender, EventArgs e)
        {
            treeArtifacts.SelectedNode.Remove();
            treeArtifacts.SelectedNode = treeArtifacts.Nodes[0];
            btnSave.Visible = false;
            SaveArtifacts();
        }

        private void treeArtifacts_AfterSelect(object sender, TreeViewEventArgs e)
        {
            btnSave.Visible = false;
            if (e.Node is ArtifactNode)
            {
                if (propGrid.SelectedObject != null)
                {
                    IMetaBase imb = (IMetaBase)propGrid.SelectedObject;
                    imb.Save(Guid.NewGuid());
                }
                ArtifactNode anode = (ArtifactNode)e.Node;
                propGrid.SelectedObject = anode.mbase;
                btnSave.Visible = true;
            }
            else if (e.Node is ArtifactPropertiesNode)
            {
                if (propGrid.SelectedObject != null)
                {
                    IMetaBase imb = (IMetaBase)propGrid.SelectedObject;
                    imb.Save(Guid.NewGuid());
                }
                ArtifactPropertiesNode apnode = (ArtifactPropertiesNode)e.Node;
                if (apnode.ibase == null)
                {
                    DialogResult dialResult =
                        MessageBox.Show(this,"Define new properties for this association?", "Properties",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialResult == DialogResult.Yes)
                    {
                        apnode.ibase = Loader.CreateInstance(apnode.PropertiesClass);
                        apnode.ibase.Save(Guid.NewGuid());
                        propGrid.SelectedObject = apnode.ibase;
                    }
                    else
                    {
                        propGrid.SelectedObject = null;
                    }
                }
                else
                {
                    propGrid.SelectedObject = apnode.ibase;
                }
            }
            else
            {
                if (propGrid.SelectedObject != null)
                {
                    IMetaBase imb = (IMetaBase)propGrid.SelectedObject;
                    imb.Save(Guid.NewGuid());
                }
                propGrid.SelectedObject = null;
            }
            CurrentNode = e.Node;
        }

        private void treeArtifacts_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (propGrid.SelectedObject != null)
            {
                IMetaBase imb = (IMetaBase)propGrid.SelectedObject;
                imb.Save(Guid.NewGuid());
                if (CurrentNode != null)
                {
                    if (CurrentNode is ArtifactNode)
                    {
                        CurrentNode.Text = imb.ToString();
                    }
                }
            }
        }

        private void treeArtifacts_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point ClickPoint = new Point(e.X, e.Y);
                TreeNode ClickNode = treeArtifacts.GetNodeAt(ClickPoint);
                if (ClickNode == null)
                {
                    return;
                }
                // Convert from Tree coordinates to Screen coordinates 
                Point ScreenPoint = treeArtifacts.PointToScreen(ClickPoint);
                // Convert from Screen coordinates to Form coordinates 
                Point FormPoint = PointToClient(ScreenPoint);
                contextTree.MenuItems.Clear();
                // Retrieve some properties
                CurrentNode = ClickNode;
                if (ClickNode is ArtifactClassNode)
                {
                    // ok nou kan ons check watter artifactclassnode geclick is.
                    ArtifactClassNode acnode = (ArtifactClassNode)ClickNode;
                    MenuItem mnuNewArtifact = new MenuItem("New " + acnode.ArtifactClass);
                    mnuNewArtifact.Click += new EventHandler(mnuNewArtifact_Click);
                    contextTree.MenuItems.Add(mnuNewArtifact);
                    MenuItem mnuExistingArtifact = new MenuItem("Open Existing " + acnode.ArtifactClass);
                    mnuExistingArtifact.Click += new EventHandler(mnuExistingArtifact_Click);
                    contextTree.MenuItems.Add(mnuExistingArtifact);
                }
                if (ClickNode is ArtifactNode)
                {
                    contextTree.MenuItems.Clear();
                    ArtifactNode anode = (ArtifactNode)ClickNode;
                    MenuItem mnuRemoveArtifact = new MenuItem("Remove " + anode.ArtifactClass);
                    mnuRemoveArtifact.Click += new EventHandler(mnuRemoveArtifact_Click);
                    contextTree.MenuItems.Add(mnuRemoveArtifact);
                }
                contextTree.Show(this, FormPoint);
            }
        }

        private void propGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (propGrid.SelectedObject != null)
            {
                IMetaBase imb = (IMetaBase)propGrid.SelectedObject;
                //imb.Save(Guid.NewGuid());
                if (CurrentNode != null)
                {
                    if (CurrentNode is ArtifactNode)
                    {
                        CurrentNode.Text = imb.ToString();
                    }
                }
            }
        }

        #endregion Methods

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.propGrid = new MetaBuilder.MetaControls.MetaPropertyGrid();
            this.contextTree = new System.Windows.Forms.ContextMenu();
            this.treeArtifacts = new System.Windows.Forms.TreeView();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlProps = new System.Windows.Forms.Panel();
            this.pnlTree = new System.Windows.Forms.Panel();
            this.btnSave = new MetaControls.MetaButton();
            this.pnlTop.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlProps.SuspendLayout();
            this.pnlTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // propGrid
            // 
            this.propGrid.BackColor = System.Drawing.SystemColors.Control;
            this.propGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propGrid.Location = new System.Drawing.Point(0, 0);
            this.propGrid.Name = "propGrid";
            this.propGrid.Size = new System.Drawing.Size(340, 364);
            this.propGrid.TabIndex = 3;
            this.propGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(propGrid_PropertyValueChanged);
            // 
            // treeArtifacts
            // 
            this.treeArtifacts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeArtifacts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeArtifacts.Location = new System.Drawing.Point(0, 0);
            this.treeArtifacts.Name = "treeArtifacts";
            this.treeArtifacts.Size = new System.Drawing.Size(200, 364);
            this.treeArtifacts.TabIndex = 4;
            this.treeArtifacts.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeArtifacts_AfterSelect);
            this.treeArtifacts.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeArtifacts_MouseUp);
            this.treeArtifacts.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeArtifacts_BeforeSelect);
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.LightSlateGray;
            this.pnlTop.Controls.Add(this.btnSave);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTop.Location = new System.Drawing.Point(0, 364);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(540, 24);
            this.pnlTop.TabIndex = 5;
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.pnlProps);
            this.pnlContent.Controls.Add(this.pnlTree);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(540, 364);
            this.pnlContent.TabIndex = 6;
            // 
            // pnlProps
            // 
            this.pnlProps.Controls.Add(this.propGrid);
            this.pnlProps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProps.Location = new System.Drawing.Point(200, 0);
            this.pnlProps.Name = "pnlProps";
            this.pnlProps.Size = new System.Drawing.Size(340, 364);
            this.pnlProps.TabIndex = 1;
            // 
            // pnlTree
            // 
            this.pnlTree.Controls.Add(this.treeArtifacts);
            this.pnlTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTree.Location = new System.Drawing.Point(0, 0);
            this.pnlTree.Name = "pnlTree";
            this.pnlTree.Size = new System.Drawing.Size(200, 364);
            this.pnlTree.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(462, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save Object";
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ArtifactEditor
            // 
            this.ClientSize = new System.Drawing.Size(540, 388);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlTop);
            this.Name = "ArtefactEditor";
            this.Load += new System.EventHandler(this.ArtifactEditor_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlProps.ResumeLayout(false);
            this.pnlTree.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        
        #endregion
    }
}