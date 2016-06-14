using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using MetaBuilder.Meta;
using MetaBuilder.MetaControls.Tasks;

namespace MetaBuilder.MetaControls
{
    public partial class MergeObjects : Form
    {

        #region Fields (1)

        bool UpdatingSource;

        #endregion Fields

        #region Constructors (1)

        private bool Server = false;
        public MergeObjects(bool server)
        {
            Server = server;
            InitializeComponent();
            DuplicateObjectSpecifications = new List<DuplicateObjectSpec>();
            objPropSource.ViewInContext += new ViewInContextEventHandler(objProp_ViewInContext);
            objPropTarget.ViewInContext += new ViewInContextEventHandler(objProp_ViewInContext);
            objPropTarget.OpenDiagram += new EventHandler(objProp_OpenDiagram);
            objPropSource.OpenDiagram += new EventHandler(objProp_OpenDiagram);
            if (Server)
                this.Text += " (Server)";
        }

        #endregion Constructors

        #region Delegates and Events (2)

        // Events (2) 

        [Browsable(false)]
        public event EventHandler OpenDiagram;

        [Browsable(false)]
        public event ViewInContextEventHandler ViewInContext;

        #endregion Delegates and Events

        #region Methods (21)

        // Public Methods (2) 

        public void Start()
        {
            dupeIndex = -1;
            buttonReplaceAll.Visible = false;// DuplicateObjectSpecifications.Count > 1;
            GoToNextSpec();
        }

        public void UpdateInterface()
        {
            listSource.Items.Clear();
            listTargets.Items.Clear();
            foreach (MetaBase match in GetCurrentSpec().Matches)
            {
                // if (match.pkid > 0 && match.MachineName != null)
                {
                    CustomListItem clitem = new CustomListItem();
                    clitem.Tag = match;
                    if (match.pkid == 0)
                    {
                        clitem.Caption = match.ToString() + " [Not Saved]";
                    }
                    else
                    {
                        clitem.Caption = match.ToString();// +" " + match._ClassName + " [" + match.pkid.ToString() + "," + match.MachineName.ToString() + "]";
                    }
                    listSource.Items.Add(clitem);
                }
            }

            try
            {
                if (listSource.Items.Count > 0)
                {
                    listSource.SetItemChecked(0, true);
                    listSource.SelectedIndex = 0;
                }
                UpdateTargetsAfterSourceChanged();

                listTargets.SelectedIndex = 0;

                BindObjectProps(listSource, objPropSource);
                BindObjectProps(listTargets, objPropTarget);

                //if (GetCurrentSpec().Task.IsComplete)
                //{
                //    //weve already done this 1 we should check its items!
                //}

                lblCounter.Text = "Item " + (dupeIndex + 1).ToString() + " of " + duplicateobjectSpecifications.Count.ToString();

                btnPrevious.Enabled = dupeIndex > 0;
                if (dupeIndex == DuplicateObjectSpecifications.Count - 1)
                {
                    btnNext.Enabled = false;
                }
                else
                {
                    btnNext.Text = "Next >>";
                    btnNext.Enabled = true;
                }

                UpdateCopyButton();
            }
            catch
            {
            }

            btnAll_Click(this, EventArgs.Empty);
        }

        // Protected Methods (2) 

        protected void OnOpenDiagram(object sender)
        {
            if (OpenDiagram != null)
            {
                OpenDiagram(sender, EventArgs.Empty);
            }
        }

        protected void OnViewInContext(MetaBase mbase)
        {
            if (ViewInContext != null)
                ViewInContext(mbase);
        }

        // Private Methods (17) 

        private void BindObjectProps(CheckedListBox sourceListBox, ObjectProperties controlToUpdate)
        {
            CustomListItem clitem = sourceListBox.SelectedItem as CustomListItem;
            controlToUpdate.MyMetaObject = clitem.Tag as MetaBase;

            //Update selected item on either side whenever we bind and there is nothing selected
            if (listTargets.SelectedItem == null && listTargets.Items.Count > 0)
                listTargets.SelectedIndex = 0;
            if (listSource.SelectedItem == null && listSource.Items.Count > 0)
                listSource.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        //Merge Button
        private void btnCopyReplace_Click(object sender, EventArgs e)
        {
            GetCurrentSpec().Replacements = new Dictionary<MetaBase, MetaBase>();
            CustomListItem selectedSource = listSource.CheckedItems[0] as CustomListItem;
            MetaBase source = selectedSource.Tag as MetaBase;

            //clear all replacements
            GetCurrentSpec().Replacements.Clear();
            //can use later for next previous integration

            for (int i = 0; i < listTargets.Items.Count; i++)
            {
                if (listTargets.GetItemChecked(i))
                {
                    CustomListItem listItem = listTargets.Items[i] as CustomListItem;
                    MetaBase mbToBeReplaced = listItem.Tag as MetaBase;
                    GetCurrentSpec().Replacements.Add(mbToBeReplaced, source);
                }
            }
            if (GetCurrentSpec().Task != null)
                GetCurrentSpec().Task.IsComplete = true;

            DialogResult = DialogResult.OK;
            Close();
        }

        /* private void CopyProperties(MetaBase from, MetaBase to)
         {

             foreach (PropertyInfo pinfo in from.GetMetaPropertyList())
             {
                 if (from.Get(pinfo.Name) != null)
                 {
                     if (to.Get(pinfo.Name) != null)
                     {
                         if (to.Get(pinfo.Name).ToString() == "")
                         {
                             to.Set(pinfo.Name, from.Get(pinfo.Name));
                         }
                     }
                     else
                     {
                         to.Set(pinfo.Name, from.Get(pinfo.Name));
                     }
                 }
             }
             UpdateGrid();
             if (from == GetCurrentSpec().FirstInstance)
                 btnUseFirst.Select();
             else
                 btnUseSecond.Select();
         }*/
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (listSource.CheckedItems.Count > 0)
            {
                GetCurrentSpec().Replacements = new Dictionary<MetaBase, MetaBase>();
                CustomListItem selectedSource = listSource.CheckedItems[0] as CustomListItem;
                MetaBase source = selectedSource.Tag as MetaBase;

                //clear all replacements
                GetCurrentSpec().Replacements.Clear();
                //can use later for next previous integration

                for (int i = 0; i < listTargets.Items.Count; i++)
                {
                    if (listTargets.GetItemChecked(i))
                    {
                        CustomListItem listItem = listTargets.Items[i] as CustomListItem;
                        MetaBase mbToBeReplaced = listItem.Tag as MetaBase;
                        GetCurrentSpec().Replacements.Add(mbToBeReplaced, source);
                    }
                }
                if (GetCurrentSpec().Task != null)
                    GetCurrentSpec().Task.IsComplete = true;
            }
            GoToNextSpec();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (dupeIndex > 0)
            {
                dupeIndex--;
                if (dupeIndex < DuplicateObjectSpecifications.Count)
                {
                    UpdateInterface();
                }

                if (dupeIndex == 0)
                {
                    btnPrevious.Enabled = false;
                }
            }
        }

        private void GoToNextSpec()
        {
            dupeIndex++;
            if (dupeIndex > DuplicateObjectSpecifications.Count - 1)
            {
                UpdateInterface();
            }

            if (dupeIndex == DuplicateObjectSpecifications.Count - 1)
            {
                btnNext.Enabled = false;
            }
            UpdateInterface();
        }

        private void listSource_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!UpdatingSource)
            {
                UpdatingSource = true;
                for (int i = 0; i < listSource.Items.Count; i++)
                    listSource.SetItemChecked(i, false);
                listSource.SetItemChecked(e.Index, true);
                listSource.SelectedIndex = e.Index;
                UpdateTargetsAfterSourceChanged();
                UpdatingSource = false;
            }

        }

        //private void listSource_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (!UpdatingSource)
        //    {
        //        UpdatingSource = true;
        //        for (int i = 0; i < listSource.Items.Count; i++)
        //            listSource.SetItemChecked(i, false);
        //        listSource.SetItemChecked(listSource.SelectedIndex, true);
        //        UpdateTargetsAfterSourceChanged();
        //        UpdatingSource = false;
        //    }
        //}

        private void listSource_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            BindObjectProps(listSource, objPropSource);
        }

        private void listTargets_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            listTargets.SelectedIndex = e.Index;
            BindObjectProps(listTargets, objPropTarget);
            UpdateCopyButton();
        }

        private void listTargets_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindObjectProps(listTargets, objPropTarget);
            UpdateCopyButton();
        }

        private void MergeObjects_Load(object sender, EventArgs e)
        {

        }

        private void MoveToNextSpec()
        {
            dupeIndex++;
            if (dupeIndex < DuplicateObjectSpecifications.Count)
            {
                UpdateInterface();
            }
            else
            {
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        void objProp_OpenDiagram(object sender, EventArgs e)
        {
            OnOpenDiagram(sender);
        }

        void objProp_ViewInContext(MetaBase mbase)
        {
            OnViewInContext(mbase);
        }

        private void UpdateCopyButton()
        {
            int counter = 0;
            bool oneIsSaved = false;
            for (int i = 0; i < listTargets.Items.Count; i++)
            {
                if (listTargets.GetItemChecked(i))
                {
                    counter++;
                    CustomListItem listItem = listTargets.Items[i] as CustomListItem;
                    MetaBase mb = listItem.Tag as MetaBase;
                    if (mb.pkid > 0)
                        oneIsSaved = true;
                }
            }
            //if (counter == 0)
            //{
            //    //no targets selected!
            //    return;
            //}

            bool sourceNotSaved = false;
            CustomListItem sourceItem = null;
            int sourceitemindex = -1;
            for (int i = 0; i < listSource.Items.Count; i++)
            {
                if (listSource.GetItemChecked(i))
                {
                    sourceItem = listSource.Items[i] as CustomListItem;
                    sourceitemindex = i;
                    MetaBase mb = sourceItem.Tag as MetaBase;
                    if (mb.pkid == 0)
                    {
                        sourceNotSaved = true;
                    }
                    break;
                }
            }

            if ((oneIsSaved) && (sourceNotSaved))
            {
                if (btnCopyReplace.Enabled)
                {
                    btnAll.Enabled = btnNext.Enabled = btnPrevious.Enabled = btnCopyReplace.Enabled = false;
                    MessageBox.Show(this, "Cannot replace a saved object with an unsaved one", "Merge Duplicate Selection Problem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //for (int i = 0; i < listTargets.Items.Count; i++)
                    //{
                    //    listTargets.SetItemChecked(i, false);
                    //}
                }
                else
                {

                }
            }
            else
            {
                btnCopyReplace.Enabled = true;

                if (btnNext.Enabled != false)
                    btnNext.Enabled = true;

                if (btnPrevious.Enabled != false)
                    btnPrevious.Enabled = true;

                //btnCopyReplace.Enabled = counter > 0;
            }
        }

        private void UpdateTargetsAfterSourceChanged()
        {
            listTargets.Items.Clear();
            foreach (MetaBase match in GetCurrentSpec().Matches)
            {
                if (match.pkid > 0 && match.MachineName != null)
                {
                    if (GetCurrentSpec().Task == null)
                    {
                        CustomListItem currentlyCheckedSource = listSource.CheckedItems[0] as CustomListItem;
                        if (currentlyCheckedSource.Tag != match)
                        {
                            CustomListItem clitemTarget = new CustomListItem();
                            clitemTarget.Tag = match;
                            clitemTarget.Caption = match.ToString();// +" " + match._ClassName + " [" + match.pkid.ToString() + "," + match.MachineName.ToString() + "]";
                            listTargets.Items.Add(clitemTarget);
                        }
                    }
                    else
                    {
                        CustomListItem currentlyCheckedSource = listSource.CheckedItems[0] as CustomListItem;
                        if (currentlyCheckedSource.Tag != match)
                        {
                            CustomListItem clitemTarget = new CustomListItem();
                            clitemTarget.Tag = match;
                            clitemTarget.Caption = match.ToString();// +" " + match._ClassName + " [" + match.pkid.ToString() + "," + match.MachineName.ToString() + "]";
                            listTargets.Items.Add(clitemTarget);
                        }
                    }
                }
                else
                {
                    if ((listSource.SelectedItem as CustomListItem).Tag == match)
                        continue;
                    CustomListItem clitemNotSaved = new CustomListItem();
                    clitemNotSaved.Tag = match;
                    clitemNotSaved.Caption = match.ToString() + " [Not Saved]";// +" " + match._ClassName;
                    listTargets.Items.Add(clitemNotSaved);
                }
            }

            btnAll_Click(this, EventArgs.Empty);
        }

        #endregion Methods

        #region Specs

        int dupeIndex = -1;
        private DuplicateObjectSpec GetCurrentSpec()
        {
            return duplicateobjectSpecifications[dupeIndex];
        }
        private List<DuplicateObjectSpec> duplicateobjectSpecifications;
        public List<DuplicateObjectSpec> DuplicateObjectSpecifications
        {
            get { return duplicateobjectSpecifications; }
            set { duplicateobjectSpecifications = value; }
        }

        #endregion

        private void btnAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listTargets.Items.Count; i++)
            {
                //Invert check state
                //listTargets.SetItemChecked(i, !listTargets.GetItemChecked(i));
                listTargets.SetItemChecked(i, true);
            }
        }
        //4 March 2013
        private void buttonReplaceAll_Click(object sender, EventArgs e)
        {
            for (int replaceAllIndex = 0; replaceAllIndex < DuplicateObjectSpecifications.Count; replaceAllIndex++)
            {
                btnAll_Click(sender, e);
                btnCopyReplace_Click(sender, e);
            }
        }
    }

    public class DuplicateObjectSpec
    {

        #region Fields (3)

        private List<MetaBase> matches;
        private Dictionary<MetaBase, MetaBase> replacements;
        private DuplicationTask task;

        #endregion Fields

        #region Constructors (1)

        public DuplicateObjectSpec()
        {
            Replacements = new Dictionary<MetaBase, MetaBase>();
        }

        #endregion Constructors

        #region Properties (3)

        public List<MetaBase> Matches
        {
            get { return matches; }
            set { matches = value; }
        }

        //VALUE IS THE METAOBJECT THAT WILL BE KEPT
        public Dictionary<MetaBase, MetaBase> Replacements
        {
            get { return replacements; }
            set { replacements = value; }
        }

        public DuplicationTask Task
        {
            get { return task; }
            set { task = value; }
        }

        #endregion Properties

    }
}