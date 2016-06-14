#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Modification and/or tampering with the code is also strictly prohibited, and
// punishable by law.
//
// Filename: GraphPalette.cs
// Author: Deon Fourie
// Last Modified: 2007-15-26
//

#endregion

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.Core.Storage;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Utilities;
using Northwoods.Go;
using b = MetaBuilder.BusinessLogic;

namespace MetaBuilder.Graphing.Containers
{
    /// <summary>
    /// The DisconPalette adds extra functionality to the GoPalette class, including a contextmenu, add/remove/copy/paste options.
    /// </summary>
    public class GraphPalette : GoPalette
    {
        #region Properties (1)

        private PaletteShapeComparer PalComparer
        {
            get { return Comparer as PaletteShapeComparer; }

        }

        #endregion Properties

        #region Methods (4)

        // Private Methods (4) 

        private void EnableContextMenuItems(bool IsOnObject, GoObject obj)
        {
            mnuRemoveShape.Enabled = IsOnObject;
            mnuRemoveShape.Tag = obj;
            mnuAddShape.Enabled = !IsOnObject;
        }

        private void GraphPalette_BackgroundContextClicked(object sender, GoInputEventArgs e)
        {
            EnableContextMenuItems(false, null);
            contextPalette.Show(this, e.MouseEventArgs.Location);
        }

        private void GraphPalette_ObjectContextClicked(object sender, GoObjectEventArgs e)
        {
            EnableContextMenuItems(true, e.GoObject.ParentNode);
            contextPalette.Show(this, e.MouseEventArgs.Location);
        }

        #endregion Methods

        #region Properties & Controls

        private ContextMenu contextPalette;
        private MenuItem mnuAddShape;
        private MenuItem mnuCloseStencil;
        private MenuItem mnuRemoveShape;
        private MenuItem mnuReplaceShape;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;

        public bool IsDirty
        {
            get { return Document.IsModified; }
            set { Document.IsModified = value; }
        }

        #endregion

        #region Constructors

        private int replaceIndex;

        public GraphPalette()
        {
            InitializeComponent();
            Document = new Stencil();
            BackColor = Color.MediumSeaGreen;
            DocScale = 0.4f;
            Comparer = new PaletteShapeComparer();
            // disabled!
            mnuAddShape.Visible = false;
            mnuRemoveShape.Visible = false;
            mnuReplaceShape.Visible = false;
            //#if DEBUG
            mnuAddShape.Visible = Variables.Instance.ShowDeveloperItems;
            mnuRemoveShape.Visible = Variables.Instance.ShowDeveloperItems;
            mnuReplaceShape.Visible = Variables.Instance.ShowDeveloperItems;
            //#endif
        }

        public override GoDocument Document
        {
            get { return base.Document; }
            set
            {
                if (value != null)
                    base.Document = value;
                try
                {
                    SortedList slist = new SortedList();
                    foreach (GoObject o in Document)
                    {
                        if (o is GoNode)
                        {
                            GoNode n = o as GoNode;
                            PalComparer.ObjectStack.Add(o);
                            //if (n.UserObject != null)
                            //{
                            //    slist.Add(n.UserObject, n);
                            //}
                        }
                    }
                    //foreach (GoObject o in slist.Values)
                    //{
                    //    PalComparer.ObjectStack.Add(o);
                    //}
                }
                catch
                {
                }
            }
        }

        private void InitializeComponent()
        {
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            contextPalette = new ContextMenu();
            mnuAddShape = new MenuItem();
            mnuRemoveShape = new MenuItem();
            mnuCloseStencil = new MenuItem();
            mnuReplaceShape = new MenuItem();
            SuspendLayout();
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileOk += openFileDialog1_FileOk;
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.FileOk += saveFileDialog1_FileOk;
            // 
            // contextPalette
            // 
            contextPalette.MenuItems.AddRange(new MenuItem[]
                                                  {
                                                      mnuAddShape,
                                                      mnuRemoveShape,
                                                      mnuCloseStencil,
                                                      mnuReplaceShape
                                                  });
            // Uitgehaal vir Broes this.contextPalette.Popup += new System.EventHandler(this.contextPalette_Popup);
            // 
            // mnuAddShape
            // 
            mnuAddShape.Index = 0;
            mnuAddShape.Text = "&Add Shape";
            mnuAddShape.Click += contextPaletteAddShape_Click;
            // 
            // mnuRemoveShape
            // 
            mnuRemoveShape.Index = 1;
            mnuRemoveShape.Text = "&Remove Shape";
            mnuRemoveShape.Click += contextPaletteRemoveShape_Click;
            // 
            // mnuReplaceShape
            // 
            mnuReplaceShape.Index = 2;
            mnuReplaceShape.Text = "&Replace Shape";
            mnuReplaceShape.Click += mnuReplaceShape_Click;
            // 
            // mnuCloseStencil
            // 
            mnuCloseStencil.Index = 3;
            mnuCloseStencil.Text = "&Close Stencil";
            mnuCloseStencil.Click += contextPaletteCloseStencil_Click;
            // 
            // GraphPalette
            // 
            GridCellSizeHeight = 100F;
            GridCellSizeWidth = 100F;
            ObjectContextClicked += GraphPalette_ObjectContextClicked;
            BackgroundContextClicked += GraphPalette_BackgroundContextClicked;
            DocumentChanged += DisconPalette_DocumentChanged;
            ResumeLayout(false);
        }

        private void mnuReplaceShape_Click(object sender, EventArgs e)
        {
            GoObject obj = mnuRemoveShape.Tag as GoObject;
            replaceIndex = PalComparer.ObjectStack.IndexOf(obj);
            PalComparer.ObjectStack.Remove(obj);
            obj.Remove();
            FileDialogSpecification spec = FilePathManager.Instance.GetSpecification(FileTypeList.Symbol);
            openFileDialog1.InitialDirectory =
                FilePathManager.Instance.GetSpecification(FileTypeList.Symbol).DefaultPath;
            openFileDialog1.Title = "Replace " + spec.Title;
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = spec.Filter;
            openFileDialog1.ShowDialog(this);
            Document.IsModified = true;
            LayoutItems();
        }

        #endregion

        #region Methods

        public void CheckModified()
        {
        }

        #endregion

        #region EventHandlers

        private void contextPaletteAddShape_Click(object sender, EventArgs e)
        {
            FileDialogSpecification spec = FilePathManager.Instance.GetSpecification(FileTypeList.Symbol);
            openFileDialog1.InitialDirectory = FilePathManager.Instance.GetSpecification(FileTypeList.Symbol).DefaultPath;
            openFileDialog1.Title = "Add " + spec.Title;
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = spec.Filter;
            openFileDialog1.ShowDialog(this);
        }

        private void contextPaletteRemoveShape_Click(object sender, EventArgs e)
        {
            GoObject obj = mnuRemoveShape.Tag as GoObject;
            PalComparer.ObjectStack.Remove(obj);
            obj.Remove();
            Document.IsModified = true;
            LayoutItems();
        }

        public event EventHandler CloseStencil;

        public void OnCloseStencil(EventArgs e)
        {
            if (CloseStencil != null)
                CloseStencil(this, e);
        }

        private void contextPaletteCloseStencil_Click(object sender, EventArgs e)
        {
            OnCloseStencil(e);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Orientation = Orientation.Vertical;
            //Sorting = SortOrder.Ascending;
            FileDialogSpecification spec = FilePathManager.Instance.GetSpecification(FileTypeList.Symbol);
            switch (openFileDialog1.Title)
            {
                case "Add Symbol":
                    foreach (string shapeFile in openFileDialog1.FileNames)
                    {
                        GraphNode mgnode = (GraphNode)StorageManipulator.FileSystemManipulator.LoadObject(shapeFile, spec);
                        Document.Add(mgnode);
                        mgnode.EditMode = false;
                        mgnode.Position = LastInput.DocPoint;
                        Document.IsModified = true;
                        FilePathManager.Instance.SetLastUsedPath(spec.FileType, strings.GetPath(openFileDialog1.FileName));
                        PalComparer.ObjectStack.Add(mgnode);
                    }
                    break;
                case "Replace Symbol":
                    foreach (string shapeFile in openFileDialog1.FileNames)
                    {
                        GraphNode mgnode = (GraphNode)StorageManipulator.FileSystemManipulator.LoadObject(shapeFile, spec);
                        Document.Add(mgnode);
                        mgnode.EditMode = false;
                        mgnode.Position = LastInput.DocPoint;
                        Document.IsModified = true;
                        FilePathManager.Instance.SetLastUsedPath(spec.FileType, strings.GetPath(openFileDialog1.FileName));
                        PalComparer.ObjectStack.Insert(replaceIndex, mgnode);
                    }
                    break;
            }
            this.AllowInsert = true;
            DocScale = 0.4f;
            LayoutItems();
        }

        private void DisconPalette_DocumentChanged(object sender, GoChangedEventArgs e)
        {
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //Utilities.StorageManipulator.FileSystemManipulator.Save(saveFileDialog1.FileName, (GraphDoc)this.Document);
            Document.IsModified = false;
        }

        #endregion
    }
}