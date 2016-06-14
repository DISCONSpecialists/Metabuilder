//using System;
//using System.ComponentModel;
//using System.Windows.Forms;
//using MetaBuilder.Meta;

//namespace MetaBuilder.MetaControls
//{
//    /// <summary>
//    /// A propertygrid that handles tab & shift tab keys.
//    /// </summary>
//    public class MetaPropertyGrid : PropertyGrid
//    {

//        #region Fields (1)

//        /// <summary> 
//        /// Required designer variable.
//        /// </summary>
//        private Container components = null;

//        #endregion Fields

//        #region Constructors (1)

//        public MetaPropertyGrid()
//        {
//            // This call is required by the Windows.Forms Form Designer.
//            InitializeComponent();
//            SelectedGridItemChanged += new SelectedGridItemChangedEventHandler(MetaPropertyGrid_SelectedGridItemChanged);
//            LostFocus += new EventHandler(MetaPropertyGrid_LostFocus);
//            SelectedObjectsChanged += new EventHandler(MetaPropertyGrid_SelectedObjectsChanged);
//            PropertyValueChanged += new PropertyValueChangedEventHandler(MetaPropertyGrid_PropertyValueChanged);
//            // Removes the PropertyPages button
//            foreach (Control control in Controls)
//            {
//                ToolStrip toolStrip = control as ToolStrip;
//                if (toolStrip != null)
//                {
//                    // Found toolstrip
//                    toolStrip.Items[4].Visible = false;
//                    break;
//                }
//            }
//        }

//        #endregion Constructors

//        #region Methods (8)


//        // Protected Methods (3) 

//        /// <summary> 
//        /// Clean up any resources being used.
//        /// </summary>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                if (components != null)
//                {
//                    components.Dispose();
//                }
//            }
//            base.Dispose(disposing);
//        }

//        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
//        {
//            if (keyData == (Keys.Tab | Keys.Shift))
//            {
//                MoveTabBack();
//                return true;
//            }
//            return base.ProcessCmdKey(ref msg, keyData);
//        }

//        protected override bool ProcessTabKey(bool forward)
//        {
//            int count = PropertyTabs[0].GetProperties(SelectedObject).Count;
//            /*for (int i = 0; i < count; i++)
//            {
//                System.// Console.WriteLine(this.PropertyTabs[0].GetProperties(SelectedObject)[i].Name);
//            }*/
//            GridItem selectedItem = SelectedGridItem;
//            GridItem root = selectedItem.Parent;
//            if (forward)
//            {
//                bool selectnextitem = false;
//                foreach (GridItem child in root.GridItems)
//                {
//                    if (selectnextitem)
//                    {
//                        child.Select();
//                        break;
//                    }
//                    if (child == selectedItem)
//                    {
//                        selectnextitem = true;
//                    }
//                }
//            }
//            return true;
//        }

//        // Private Methods (5) 

//        private void MetaPropertyGrid_LostFocus(object sender, EventArgs e)
//        {
//            if (SelectedObject is MetaBase)
//            {
//                //   MetaBase mbase = SelectedObject as MetaBase;
//                //    mbase.OnChanged(e);
//            }
//        }

//        private void MetaPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
//        {
//            if (SelectedObject is MetaBase)
//            {
//                //     MetaBase mbase = SelectedObject as MetaBase;
//                //     mbase.OnChanged(e);
//            }
//        }

//        private void MetaPropertyGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
//        {
//            if (SelectedObject is MetaBase)
//            {
//                //MetaBase mbase = SelectedObject as MetaBase;
//                //mbase.OnChanged(e);
//            }
//        }

//        private void MetaPropertyGrid_SelectedObjectsChanged(object sender, EventArgs e)
//        {
//            try
//            {
//                MetaPropertyGridDocker docker = ParentForm as MetaPropertyGridDocker;
//            }
//            catch
//            {
//            }
//        }

//        private void MoveTabBack()
//        {
//            GridItem selectedItem = SelectedGridItem;
//            GridItem root = selectedItem.Parent;
//            int itemcount = root.GridItems.Count;
//            bool selectnextitem = false;
//            for (int i = itemcount - 1; i >= 0; i--)
//            {
//                if (selectnextitem)
//                {
//                    root.GridItems[i].Select();
//                    break;
//                }
//                if (root.GridItems[i] == selectedItem)
//                {
//                    selectnextitem = true;
//                }
//            }
//        }

//        #endregion Methods

//        #region Component Designer generated code
//        /// <summary> 
//        /// Required method for Designer support - do not modify 
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            components = new System.ComponentModel.Container();
//        }
//        #endregion
//    }
//}

using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.Meta;
//using MetaBuilder.GraphingUI;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MetaBuilder.MetaControls
{
    /// <summary>
    /// A propertygrid that handles tab & shift tab keys.
    /// </summary>
    public class MetaPropertyGrid : PropertyGrid
    {
        #region Fields (1)

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        #endregion Fields

        #region Constructors (1)
        Control docCommentControl = null;
        public MetaPropertyGrid()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            SelectedGridItemChanged += new SelectedGridItemChangedEventHandler(MetaPropertyGrid_SelectedGridItemChanged);
            LostFocus += new EventHandler(MetaPropertyGrid_LostFocus);
            SelectedObjectsChanged += new EventHandler(MetaPropertyGrid_SelectedObjectsChanged);
            PropertyValueChanged += new PropertyValueChangedEventHandler(MetaPropertyGrid_PropertyValueChanged);
            // Removes the PropertyPages button
            foreach (Control control in Controls)
            {
                Type controlType = control.GetType();
                ToolStrip toolStrip = control as ToolStrip;
                if (toolStrip != null)
                {
                    // Found toolstrip
                    toolStrip.Items[4].Visible = false;
                    break;
                }
                if (controlType.Name == "DocComment")
                {
                    docCommentControl = control;
                }
            }
            ToolbarVisible = false;
        }

        #endregion Constructors

        private Collection<MetaBase> selectedMetaBases;
        public Collection<MetaBase> SelectedMetaBases { get { return selectedMetaBases; } set { selectedMetaBases = value; } }

        protected override void OnSelectedObjectsChanged(EventArgs e)
        {
            base.OnSelectedObjectsChanged(e);
            //check for a name and find its control and triple its height
            foreach (Control control in Controls)
            {
                Type controlType = control.GetType();
                controlType.ToString();
            }
        }

        #region Methods (8)

        // Protected Methods (3) 

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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F2)
            {
                return true;
            }
            else if (keyData == Keys.Enter)
            {
                base.ProcessCmdKey(ref msg, keyData);
                bool selectnextitem = false;
                foreach (GridItem child in SelectedGridItem.Parent.GridItems)
                {
                    if (selectnextitem)
                    {
                        child.Select();
                        return true;
                    }
                    if (child == SelectedGridItem)
                    {
                        selectnextitem = true;
                    }
                }
                if (selectnextitem) //we are at the end of the properties
                    SelectedGridItem.Parent.GridItems[0].Select();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override bool ProcessTabKey(bool forward)
        {
            //int count = PropertyTabs[0].GetProperties(SelectedObject).Count;
            /*for (int i = 0; i < count; i++)
            {
                System.// Console.WriteLine(this.PropertyTabs[0].GetProperties(SelectedObject)[i].Name);
            }*/
            if (forward)
            {
                //((this.Parent as MetaPropertyGridDocker).Parent as Docking.DockContent).ParentForm as dockingform
                bool selectnextitem = false;
                foreach (GridItem child in SelectedGridItem.Parent.GridItems)
                {
                    if (selectnextitem)
                    {
                        child.Select();
                        break;
                    }
                    if (child == SelectedGridItem)
                    {
                        selectnextitem = true;
                    }
                }
            }
            else
            {
                bool selectpreviousitem = false;
                GridItem previousItem = null;
                foreach (GridItem child in SelectedGridItem.Parent.GridItems)
                {
                    if (child == SelectedGridItem)
                    {
                        selectpreviousitem = true;
                        continue;
                    }
                    if (selectpreviousitem && previousItem != null)
                    {
                        previousItem.Select();
                        break;
                    }
                    else
                    {
                        previousItem = child;
                    }

                }
            }
            return true;
        }

        // Private Methods (5) 

        private void MetaPropertyGrid_LostFocus(object sender, EventArgs e)
        {

        }

        private void MetaPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (SelectedObject is MetaBase)
            {
                if (SelectedMetaBases != null)
                {
                    foreach (MetaBase mBase in SelectedMetaBases)
                    {
                        string Firstcls = SelectedMetaBases[0].Class;
                        //try
                        //{
                        if (Firstcls == "DataEntity" || Firstcls == "DataDomain" || Firstcls == "DataTable" || Firstcls.Contains("InformationArtefact"))
                            if (mBase.Class != Firstcls)
                                continue;

                        if (e.OldValue != null && e.ChangedItem.Value == null)
                            mBase.Set(e.ChangedItem.Label, e.OldValue);
                        else
                            mBase.Set(e.ChangedItem.Label, e.ChangedItem.Value);

                        mBase.OnChanged(e);
                        //}
                        //catch (Exception ex)
                        //{
                        //    ex.ToString();
                        //    //probably a different class
                        //}
                    }
                }
                else
                {
                    MetaBase mbase = SelectedObject as MetaBase;
                    if (e.OldValue != null && e.ChangedItem.Value == null)
                    {
                        mbase.Set(e.ChangedItem.Label, e.OldValue);
                    }
                    mbase.OnChanged(e);
                }
                //if (DockingForm.DockForm != null && Variables.Instance.CheckDuplicates)
                //{
                //    GraphViewContainer gvc = DockingForm.DockForm.GetCurrentGraphViewContainer();
                //    if (gvc != null)
                //    {
                //        DockingForm.DockForm.GetCurrentGraphViewContainer().MarkDuplicates(mbase);
                //    }
                //}
            }

            this.Refresh(); //this allows our domain properties to enable/disable
        }

        private void MetaPropertyGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {

        }

        private void MetaPropertyGrid_SelectedObjectsChanged(object sender, EventArgs e)
        {
            try
            {
                MetaPropertyGridDocker docker = ParentForm as MetaPropertyGridDocker;
                if (SelectedObject is MetaBase)
                {
                    MetaBase mbase = SelectedObject as MetaBase;
                    mbase.BrowsableProperties();
                    if (docker != null)
                    {
                        docker.Text = "Meta Properties: " + mbase._ClassName;
                        docker.TabText = docker.Text;
                    }
                }
                else
                {
                    if (docker != null)
                    {
                        docker.Text = "Meta Properties";
                        docker.TabText = docker.Text;
                    }
                }
            }
            catch
            {
            }
        }

        private void MoveTabBack()
        {
            GridItem selectedItem = SelectedGridItem;
            GridItem root = selectedItem.Parent;
            int itemcount = root.GridItems.Count;
            bool selectnextitem = false;
            for (int i = itemcount - 1; i >= 0; i--)
            {
                if (selectnextitem)
                {
                    root.GridItems[i].Select();
                    break;
                }
                if (root.GridItems[i] == selectedItem)
                {
                    selectnextitem = true;
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
            components = new System.ComponentModel.Container();
        }
        #endregion

        protected override void OnSelectedGridItemChanged(SelectedGridItemChangedEventArgs e)
        {
            base.OnSelectedGridItemChanged(e);
            if (docCommentControl != null)
                foreach (Control c in docCommentControl.Controls)
                    c.Text = c.Text.Replace("-", "\n-");// +"\n NEWLINE COMMENT OVERRIDEN"; //put all the -on newlines
        }
    }
}