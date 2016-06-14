//using System;
//using System.ComponentModel;
//using System.Reflection;
//using System.Windows.Forms;
//using MetaBuilder.BusinessLogic;
//using MetaBuilder.Core;
//using MetaBuilder.Meta;
//using MetaBuilder.UIControls.GraphingUI;
//using System.Drawing;

//namespace MetaBuilder.UIControls.Common.EXCLUDED
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
//        Control docCommentControl = null;
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
//                Type controlType = control.GetType();
//                ToolStrip toolStrip = control as ToolStrip;
//                if (toolStrip != null)
//                {
//                    // Found toolstrip
//                    toolStrip.Items[4].Visible = false;
//                    break;
//                }
//                if (controlType.Name == "DocComment")
//                {
//                    docCommentControl = control;
//                }
//            }
//            ToolbarVisible = false;
//        }

//        #endregion Constructors

//        protected override void OnSelectedObjectsChanged(EventArgs e)
//        {
//            base.OnSelectedObjectsChanged(e);
//            //check for a name and find its control and triple its height
//            foreach (Control control in Controls)
//            {
//                Type controlType = control.GetType();
//                controlType.ToString();
//            }
//        }

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
//            if (keyData == Keys.F2)
//            {
//                return true;
//                //try
//                //{
//                //    //MessageBox.Show(this,this.SelectedGridItem.Value.ToString());
//                //    GridItem gi = this.SelectedGridItem;
//                //    MetaBase mo = this.SelectedObject as MetaBase;

//                //    Form f = new Form();
//                //    f.Size = new Size(300, 200);
//                //    Button btnOK = new Button();
//                //    btnOK.Text = "OK";
//                //    Button btnCancel = new Button();
//                //    btnCancel.Text = "Cancel";

//                //    TextBox txt = new TextBox();

//                //    f.Controls.Add(txt);
//                //    txt.Size = new Size(f.Width, f.Height - 50);
//                //    txt.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
//                //    txt.Multiline = true;
//                //    object o = mo.Get(gi.PropertyDescriptor.Name);
//                //    if (o != null)
//                //        txt.Text = mo.Get(gi.PropertyDescriptor.Name).ToString();
//                //    f.Controls.Add(btnOK);
//                //    f.Controls.Add(btnCancel);
//                //    btnCancel.Location = new Point(218, 150);
//                //    btnOK.Location = new Point(140, 150);
//                //    btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
//                //    btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
//                //    btnCancel.DialogResult = DialogResult.Cancel;
//                //    btnOK.DialogResult = DialogResult.OK;

//                //    f.StartPosition = FormStartPosition.CenterScreen;

//                //    f.ShowDialog();
//                //    DialogResult res = f.DialogResult;
//                //    if (res == DialogResult.OK)
//                //        mo.Set(gi.PropertyDescriptor.Name, txt.Text);

//                //    ProcessTabKey(true);

//                //}
//                //catch
//                //{
//                //}
//            }
//            if (keyData == Keys.Insert)
//            {
//                //// Console.WriteLine(SelectedGridItem.Label);
//                object o = SelectedObject;
//                MetaBase mbase = o as MetaBase;
//                PropertyInfo[] props = mbase.GetType().GetProperties();
//                foreach (PropertyInfo pinfo in props)
//                {
//                    if (pinfo.Name == SelectedGridItem.Label)
//                    {
//                        // Gotcha!
//                        if (pinfo.PropertyType.FullName.IndexOf("Generated") > -1) // class
//                        {
//                            string className = pinfo.PropertyType.Name;
//                            ObjectFinder ofinder = new ObjectFinder(false);
//                            ofinder.IncludeStatusCombo = true;
//                            ofinder.ExcludeStatuses.Add(VCStatusList.Obsolete);
//                            ofinder.ExcludeStatuses.Add(VCStatusList.MarkedForDelete);
//                            ofinder.AllowMultipleSelection = false;
//                            ofinder.LimitToClass = className;
//                            DialogResult res = ofinder.ShowDialog();
//                            if (res == DialogResult.OK)
//                            {
//                                if (ofinder.SelectedObjectsList.Count > 0)
//                                    mbase.Set(pinfo.Name, ofinder.SelectedObjectsList[0]);
//                            }
//                        }
//                    }
//                }
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
//                //    MetaBase mbase = SelectedObject as MetaBase;
//                //    mbase.OnChanged(e);
//            }
//        }

//        private void MetaPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
//        {
//            if (SelectedObject is MetaBase)
//            {
//                MetaBase mbase = SelectedObject as MetaBase;
//                if (e.OldValue != null && e.ChangedItem.Value == null)
//                {
//                    mbase.Set(e.ChangedItem.Label, e.OldValue);
//                }
//                mbase.OnChanged(e);
//                if (DockingForm.DockForm != null && Variables.Instance.CheckDuplicates)
//                {
//                    GraphViewContainer gvc = DockingForm.DockForm.GetCurrentGraphViewContainer();
//                    if (gvc != null)
//                    {
//                        DockingForm.DockForm.GetCurrentGraphViewContainer().MarkDuplicates(mbase);
//                    }
//                }
//            }
//        }

//        private void MetaPropertyGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
//        {
//            if (SelectedObject is MetaBase)
//            {
//                //       MetaBase mbase = SelectedObject as MetaBase;
//                //  mbase.OnChanged(e);
//            }
//        }

//        private void MetaPropertyGrid_SelectedObjectsChanged(object sender, EventArgs e)
//        {
//            try
//            {
//                MetaPropertyGridDocker docker = ParentForm as MetaPropertyGridDocker;
//                if (SelectedObject is MetaBase)
//                {
//                    MetaBase mbase = SelectedObject as MetaBase;
//                    if (docker != null)
//                    {
//                        docker.TabText = "Meta Properties: " + mbase._ClassName;
//                        docker.Text = "Meta Properties: " + mbase._ClassName;
//                    }
//                }
//                else
//                {
//                    if (docker != null)
//                    {
//                        docker.TabText = "Meta Properties";
//                        docker.Text = "Meta Properties";
//                    }
//                }
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

//        protected override void OnSelectedGridItemChanged(SelectedGridItemChangedEventArgs e)
//        {
//            base.OnSelectedGridItemChanged(e);
//            if (docCommentControl != null)
//                foreach (Control c in docCommentControl.Controls)
//                    c.Text = c.Text.Replace("-", "\n-");// +"\n NEWLINE COMMENT OVERRIDEN"; //put all the -on newlines
//        }
//    }
//}