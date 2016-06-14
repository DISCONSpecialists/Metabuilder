using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Northwoods.Go;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Tools;
using MetaBuilder.BusinessLogic;

namespace MetaBuilder.MetaControls
{
    public partial class ContextFilter : UserControl, IDisposable
    {

        //this is the document which it must filter
        //this can be set to null and be used to 'create' reports
        private GoDocument context;
        public GoDocument Context
        {
            get { return context; }
            set { context = value; }
        }

        public ContextFilter(GoDocument c, bool onlyVisible)
        {
            InitializeComponent();
            Context = c;
            bindingSourceTypes.DataSource = Enum.GetNames(typeof(FilterType));

            buttonReport.Visible = false;// Core.Variables.Instance.IsDeveloperEdition;
            splitContainer1.Panel2Collapsed = true;

            if (Context == null)
            {
                comboBoxParentType.Items.Remove("Workspace");
                label1.Text = "Dynamic Query Generator";
            }
            else
            {
                comboBoxParentType.Items.Remove("Association");
                comboBoxParentType.Items.Remove("Artefact");
                borderthis();
            }
        }

        bool bordered = false;
        private void borderthis()
        {
            if (bordered)
                return;

            addPanel(this, DockStyle.Top, Color.Black);
            addPanel(this, DockStyle.Bottom, Color.Black);
            addPanel(this, DockStyle.Left, Color.Black);
            addPanel(this, DockStyle.Right, Color.Black);

            addPanel(splitContainer1.Panel1, DockStyle.Top, Color.DarkGray);
            addPanel(splitContainer1.Panel1, DockStyle.Bottom, Color.DarkGray);
            addPanel(splitContainer1.Panel1, DockStyle.Left, Color.DarkGray);
            addPanel(splitContainer1.Panel1, DockStyle.Right, Color.DarkGray);

            addPanel(splitContainer1.Panel2, DockStyle.Top, Color.DarkGray);
            addPanel(splitContainer1.Panel2, DockStyle.Bottom, Color.DarkGray);
            addPanel(splitContainer1.Panel2, DockStyle.Left, Color.DarkGray);
            addPanel(splitContainer1.Panel2, DockStyle.Right, Color.DarkGray);
            bordered = true;
        }
        private void addPanel(Control control, DockStyle dock, Color backColor)
        {
            if (bordered)
                return;

            Panel p = new Panel();
            p.AutoSize = false;
            p.BorderStyle = BorderStyle.None;
            p.BackColor = backColor;
            p.Dock = dock;
            p.Width = 2;
            p.Height = 2;

            control.Controls.Add(p);
            p.SendToBack();
        }

        public FilterType filterTypeSwitch(Type o)
        {
            string type = o.ToString();

            if (type.Contains("Class"))
            {
                if (comboBoxParentType.Text.Contains("Art"))
                    return FilterType.Artefact;
                return FilterType.Class;
            }
            else if (type.Contains("ClassAssociation"))
            {
                return FilterType.Association;
            }
            else if (type.Contains("Association"))
            {
                return FilterType.AssociationType;
            }
            else if (type.Contains("Workspace"))
            {
                return FilterType.Workspace;
            }
            else if (type.Contains("GraphFile"))
            {
                return FilterType.Diagram;
            }
            else if (type.Contains("Field"))
            {
                return FilterType.Field;
            }

            return FilterType.Base;
        }

        private void comboBoxParentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxParentValue.Items.Clear();
            comboBoxParentValue.Enabled = true;
            if (comboBoxParentType.SelectedItem.ToString() == "Class" || comboBoxParentType.SelectedItem.ToString() == "Artefact")
            {
                foreach (BusinessLogic.Class c in DataAccessLayer.DataRepository.Classes(null))//.ClassProvider.GetAll()
                    if (c.IsActive == true)
                    {
                        comboBoxParentValue.Items.Add(c);
                    }
                comboBoxParentValue.DisplayMember = "Name";
            }
            else if (comboBoxParentType.SelectedItem.ToString() == "Workspace")
            {
                foreach (BusinessLogic.Workspace w in DataAccessLayer.DataRepository.WorkspaceProvider.GetAll())
                    comboBoxParentValue.Items.Add(w);
                comboBoxParentValue.DisplayMember = "Name";
            }
            else if (comboBoxParentType.SelectedItem.ToString() == "Association")
            {
                foreach (BusinessLogic.AssociationType a in DataAccessLayer.DataRepository.AssociationTypeProvider.GetAll())
                    comboBoxParentValue.Items.Add(a);
                comboBoxParentValue.DisplayMember = "Name";
            }
            else if (comboBoxParentType.SelectedItem.ToString() == "Diagram")
            {
                if (Context == null)
                {
                    BusinessLogic.GraphFile orphanFile = new MetaBuilder.BusinessLogic.GraphFile();
                    orphanFile.Name = "Orphan";
                    comboBoxParentValue.Items.Add(orphanFile);
                    BusinessLogic.GraphFile allFile = new MetaBuilder.BusinessLogic.GraphFile();
                    allFile.Name = "All";
                    comboBoxParentValue.Items.Add(allFile);
                }
                foreach (BusinessLogic.GraphFile d in DataAccessLayer.DataRepository.GraphFileProvider.GetAll())
                {
                    if (d.IsActive == true)
                    {
                        d.Name = Core.strings.GetFileNameOnly(d.Name);
                        comboBoxParentValue.Items.Add(d);
                    }
                }
                comboBoxParentValue.DisplayMember = "Name";
            }
            else
            {
                comboBoxParentValue.Enabled = false;
            }
        }
        private void buttonParentClear_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = true;
            listViewParents.Items.Clear();
            Apply();
        }
        private ContextFilterItem GetParentItemAtIndex(int index)
        {
            ListViewItem lastI = listViewParents.Items[index];
            return lastI.Tag as ContextFilterItem;
        }
        private void buttonParentAdd_Click(object sender, EventArgs e)
        {
            if (comboBoxParentValue.Enabled && comboBoxParentValue.SelectedItem != null)
            {
                //if you add association/artefact you require 2 classes before this
                //if you add a diagram you require 1 class before
                if (Context == null)
                    if (listViewParents.Items.Count > 0)
                    {
                        ContextFilterItem lastItem = GetParentItemAtIndex(listViewParents.Items.Count - 1);
                        if (comboBoxParentType.Text == "Diagram")
                        {
                            if (lastItem.FType != FilterType.Class)
                            {
                                if (lastItem.FType == FilterType.AssociationType)
                                {
                                    try
                                    {
                                        if (GetParentItemAtIndex(listViewParents.Items.Count - 2).FType != FilterType.Class)
                                        {
                                            MessageBox.Show("Cannot find the previous class");
                                            return;
                                        }
                                    }
                                    catch
                                    {
                                        MessageBox.Show("Cannot find the previous class");
                                        return;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Cannot find the previous class");
                                    return;
                                }
                            }
                        }
                        else if (comboBoxParentType.Text == "Association" || comboBoxParentType.Text == "Artefact") //requires 2
                        {
                            if (listViewParents.Items.Count < 2)
                            {
                                MessageBox.Show("Two classes are required");
                                return;
                            }
                            try
                            {
                                if (lastItem.FType == FilterType.Class && GetParentItemAtIndex(listViewParents.Items.Count - 2).FType == FilterType.Class)
                                {
                                    //can go
                                }
                                else
                                {
                                    try
                                    {
                                        if (GetParentItemAtIndex(listViewParents.Items.Count - 2).FType == FilterType.Class && GetParentItemAtIndex(listViewParents.Items.Count - 3).FType == FilterType.Class)
                                        {
                                            //can go
                                        }
                                        else
                                        {
                                            MessageBox.Show("Cannot find both classes");
                                            return;
                                        }
                                    }
                                    catch
                                    {
                                        MessageBox.Show("Cannot find both classes");
                                        return;
                                    }
                                }
                            }
                            catch
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        //you can only add class
                        if (comboBoxParentType.Text != "Class")
                        {
                            MessageBox.Show("You must first add a class or a workspace");
                            return;
                        }
                    }

                ContextFilterItem item = new ContextFilterItem(filterTypeSwitch(comboBoxParentValue.SelectedItem.GetType()), (string)comboBoxParentValue.SelectedItem.GetType().GetProperty("Name").GetValue(comboBoxParentValue.SelectedItem, null));
                ListViewItem i = new ListViewItem();
                i.Tag = item;
                i.Text = item.ToString();
                listViewParents.Items.Add(i);
                listViewParents.SelectedItems.Clear();
                i.Selected = true;
            }
            Apply();
        }
        public ListView ListViewParents { get { return listViewParents; } }
        private void buttonParentRemove_Click(object sender, EventArgs e)
        {
            if (listViewParents.SelectedItems != null)
                for (int i = 0; i < listViewParents.SelectedItems.Count; i++)
                {
                    listViewParents.Items.RemoveAt(listViewParents.SelectedItems[i].Index);
                }
            Apply();
        }
        private void listViewParents_SelectedIndexChanged(object sender, EventArgs e)
        {
            listViewChildren.Items.Clear();
            comboBoxChildValue.Text = "";
            splitContainer1.Panel2Collapsed = listViewParents.SelectedItems.Count != 1;
            if (splitContainer1.Panel2Collapsed)
                return;

            if (listViewParents.SelectedItems[0].Tag == null)
                return;

            ContextFilterItem item = listViewParents.SelectedItems[0].Tag as ContextFilterItem;
            if (item.FType == FilterType.Class || item.FType == FilterType.Artefact)
                splitContainer1.Panel2Collapsed = false;
            else
                splitContainer1.Panel2Collapsed = true;
            //load 
            comboBoxChildType.Items.Clear();

            if (Context == null)
            {
                BusinessLogic.Field fieldPKID = new MetaBuilder.BusinessLogic.Field();
                fieldPKID.Name = "pkID";
                comboBoxChildType.Items.Add(fieldPKID);
                BusinessLogic.Field fieldMachine = new MetaBuilder.BusinessLogic.Field();
                fieldMachine.Name = "Machine";
                comboBoxChildType.Items.Add(fieldMachine);
                BusinessLogic.Field fieldWorkspace = new MetaBuilder.BusinessLogic.Field();
                fieldWorkspace.Name = "Workspacename";
                comboBoxChildType.Items.Add(fieldWorkspace);
            }

            foreach (BusinessLogic.Field f in DataAccessLayer.DataRepository.FieldProvider.GetByClass(item.Value))
            {
                if (f.IsActive == true)
                {
                    comboBoxChildType.Items.Add(f);
                }
            }
            comboBoxChildType.DisplayMember = "Name";

            if (item.Items != null)
            {
                foreach (ContextFilterItem i in item.Items)
                {
                    ListViewItem ic = new ListViewItem();
                    ic.Tag = i;
                    ic.Text = i.ToString();
                    listViewChildren.Items.Add(ic);
                }
            }

            comboBoxChildValue.Text = "";
            comboBoxChildValue.Focus();
        }

        private void listViewChildren_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewChildren.SelectedItems.Count == 0 || listViewChildren.SelectedItems[0].Tag == null)
                return;
            ContextFilterItem item = listViewChildren.SelectedItems[0].Tag as ContextFilterItem;
            foreach (Field f in comboBoxChildType.Items)
            {
                if (f.Name == item.Value)
                {
                    comboBoxChildType.SelectedItem = f;
                    break;
                }
            }
            comboBoxChildValue.Text = item.FieldValue;
        }
        private void comboBoxChildValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewChildren.SelectedItems[0].Tag == null)
            {
            }
            else
            {
                ContextFilterItem item = listViewChildren.SelectedItems[0].Tag as ContextFilterItem;
                listViewChildren.SelectedItems[0].Text = item.FieldValue = comboBoxChildValue.Text;
            }
        }
        private void comboBoxChildValue_TextChanged(object sender, EventArgs e)
        {
            if (listViewChildren.SelectedItems.Count == 0 || listViewChildren.SelectedItems[0].Tag == null)
            {
                //dont update the selected one
                //comboBoxChildValue.Text = "";
            }
            else
            {
                ContextFilterItem item = listViewChildren.SelectedItems[0].Tag as ContextFilterItem;
                item.FieldValue = comboBoxChildValue.Text;
                listViewChildren.SelectedItems[0].Text = item.ToString();
                Apply();
            }
        }
        private void comboBoxChildValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonChildAdd_Click(sender, EventArgs.Empty);
                comboBoxChildType.SelectedItem = null;
                comboBoxChildType.Text = "";
                comboBoxChildValue.Text = "";
            }
        }
        private void buttonChildAdd_Click(object sender, EventArgs e)
        {
            if (comboBoxChildType.SelectedItem == null || listViewParents.Items.Count == 0)
                return;

            ContextFilterItem item = listViewParents.SelectedItems[0].Tag as ContextFilterItem;

            ContextFilterItem childItem = new ContextFilterItem(FilterType.Field, (comboBoxChildType.SelectedItem as BusinessLogic.Field).Name.ToString());
            childItem.FieldValue = comboBoxChildValue.Text;
            item.Items.Add(childItem);
            ListViewItem i = new ListViewItem();
            i.Tag = childItem;
            i.Text = childItem.ToString();
            listViewChildren.Items.Add(i);

            Apply();
        }
        private void buttonChildRemove_Click(object sender, EventArgs e)
        {
            ContextFilterItem item = listViewParents.SelectedItems[0].Tag as ContextFilterItem;
            if (listViewChildren.SelectedItems != null)
                for (int i = 0; i < listViewChildren.SelectedItems.Count; i++)
                {
                    item.Items.Remove(listViewChildren.Items[listViewChildren.SelectedItems[i].Index].Tag as ContextFilterItem);
                    listViewChildren.Items.RemoveAt(listViewChildren.SelectedItems[i].Index);
                }

            Apply();
        }
        private void buttonChildClear_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = true;

            if (listViewParents.SelectedItems.Count == 0)
                return;

            listViewChildren.Items.Clear();
            ContextFilterItem item = listViewParents.SelectedItems[0].Tag as ContextFilterItem;
            item.Items.Clear();
            Apply();
        }

        private string query;
        public string Query
        {
            get { return query; }
            set { query = value; }
        }

        List<string> usedClassNames;
        //this functions in the order that the items exist in
        public string Report()
        {
            Query = "";
            usedClassNames = new List<string>();
            string select = "SELECT ";
            string from = " FROM ";
            string where = " WHERE ";
            string associationWhere = " ";
            string fileWhere = "";
            bool first = true;
            int fileCount = 1;
            int classCount = 1;
            int CAcount = 1;
            int artefactCount = 1;
            ContextFilterItem previousItem = null;
            ContextFilterItem previousArtefact = null;
            foreach (ListViewItem lvi in listViewParents.Items)
            {
                ContextFilterItem item = lvi.Tag as ContextFilterItem;
                if (item.FType == FilterType.Class)
                {
                    string prefix = "METAView_" + item.Value + "_Listing_" + classCount;
                    string previousPrefix = (previousItem == null) ? "" : "METAView_" + previousItem.Value + "_Listing_" + (classCount - 1);

                    //SELECT
                    select += returnSelectForContextFilterItem(prefix, item);

                    //FROM
                    if (first)
                    {
                        first = false;
                        from += " METAView_" + item.Value + "_Listing AS [" + prefix + "] ";
                    }
                    //JOIN
                    else
                    {
                        from += " INNER JOIN ObjectAssociation AS [OA_" + classCount.ToString() + "] ON ";
                        from += previousPrefix + ".pkid = [OA_" + classCount.ToString() + "].ObjectID AND " + previousPrefix + ".Machine = [OA_" + classCount.ToString() + "].ObjectMachine ";
                        from += Environment.NewLine;
                        from += " INNER JOIN METAView_" + item.Value + "_Listing AS " + prefix + " ON ";
                        from += prefix + ".pkid = [OA_" + classCount.ToString() + "].ChildObjectID AND " + prefix + ".Machine = [OA_" + classCount.ToString() + "].ChildObjectMachine ";
                    }
                    //WHERE
                    where += returnWhereForContextFilterItem(prefix, item);
                    from += Environment.NewLine;

                    classCount++;

                    previousItem = item;
                }
                else if (item.FType == FilterType.AssociationType)
                {
                    if (previousItem == null || previousItem.FType != FilterType.Class)
                        continue;
                    from += " INNER JOIN ClassAssociation AS CA_" + CAcount.ToString() + " ON OA_" + (classCount - 1).ToString() + ".caid = CA_" + CAcount.ToString() + ".caid ";
                    from += Environment.NewLine;
                    from += " INNER JOIN AssociationType AS AT_" + CAcount.ToString() + " ON CA_" + CAcount.ToString() + ".AssociationTypeID = AT_" + CAcount.ToString() + ".pkid ";
                    from += Environment.NewLine;
                    associationWhere += " AT_" + CAcount.ToString() + ".Name LIKE '%" + item.Value.Trim() + "%' AND ";
                    associationWhere += Environment.NewLine;
                    CAcount++;
                }
                else if (item.FType == FilterType.Artefact)
                {
                    if (previousItem == null || previousItem.FType != FilterType.Class)
                        continue;
                    string artefactPrefix = "METAView_" + item.Value + "_Listing_" + artefactCount;
                    select += returnSelectForContextFilterItem(artefactPrefix, item);

                    //uses classcount to get get oa_classcount identifier
                    from += " INNER JOIN Artifact AS ART_" + artefactCount.ToString() + " ON OA_" + (classCount - 1).ToString() + ".Caid = ART_" + artefactCount.ToString() + ".Caid ";
                    from += Environment.NewLine;
                    from += " AND OA_" + (classCount - 1).ToString() + ".ObjectID = ART_" + artefactCount.ToString() + ".ObjectID AND OA_" + (classCount - 1).ToString() + ".ObjectMachine = ART_" + artefactCount.ToString() + ".ObjectMachine ";
                    from += Environment.NewLine;
                    from += " AND OA_" + (classCount - 1).ToString() + ".ChildObjectID = ART_" + artefactCount.ToString() + ".ChildObjectID AND OA_" + (classCount - 1).ToString() + ".ChildObjectMachine = ART_" + artefactCount.ToString() + ".ChildObjectMachine ";
                    from += Environment.NewLine;
                    //now the view ie : prefix
                    from += " INNER JOIN METAView_" + item.Value + "_Listing AS " + artefactPrefix + " ON ART_" + artefactCount.ToString() + ".ArtifactObjectID = " + artefactPrefix + ".pkid AND ART_" + artefactCount.ToString() + ".ArtefactMachine = " + artefactPrefix + ".Machine ";
                    from += Environment.NewLine;

                    where += returnWhereForContextFilterItem(artefactPrefix, item);

                    previousArtefact = item;
                    artefactCount++;
                }
                else if (item.FType == FilterType.Diagram)
                {
                    if (previousArtefact != null && previousArtefact.FType == FilterType.Artefact)
                    {
                        #region Artefact Diagram

                        string previousArtefactPrefix = (previousArtefact == null) ? "" : "METAView_" + previousArtefact.Value + "_Listing_" + (artefactCount - 1);
                        select += " Diagram_" + previousArtefact.Value + "_" + fileCount.ToString() + ".[File Name], ";
                        select += Environment.NewLine;
                        if (item.Value != "Orphan")
                        {
                            from += " INNER JOIN ActiveDiagramObjects AS Diagram_" + previousArtefact.Value + "_" + fileCount.ToString() + " ON " + previousArtefactPrefix + ".pkid = Diagram_" + previousArtefact.Value + "_" + fileCount.ToString() + ".MetaObjectID AND " + previousArtefactPrefix + ".Machine = Diagram_" + previousArtefact.Value + "_" + fileCount.ToString() + ".Machine";
                            from += Environment.NewLine;
                        }
                        else
                        {
                            //where object string concat not in (activediagramobjects concat)
                            fileWhere += " CONVERT(nVarChar(50)," + previousArtefactPrefix + ".pkid) + " + previousArtefactPrefix + ".Machine IN (SELECT CONVERT(nVarChar(50),MetaObjectID) + Machine FROM ActiveDiagramObjects) AND ";
                            fileWhere += Environment.NewLine;
                        }
                        if (item.Value != "All" && item.Value != "Orphan")
                        {
                            string filename = Core.strings.GetFileNameOnly(item.Value);
                            fileWhere = " Diagram_" + previousArtefact.Value + "_" + fileCount.ToString() + ".[File Name] = '" + filename + "' AND ";
                            fileWhere += Environment.NewLine;
                        }
                        fileCount++;

                        #endregion

                        continue;
                    }

                    if (previousItem == null || previousItem.FType != FilterType.Class)
                        continue;

                    string previousPrefix = (previousItem == null) ? "" : "METAView_" + previousItem.Value + "_Listing_" + (classCount - 1);
                    select += " Diagram_" + previousItem.Value + "_" + fileCount.ToString() + ".[File Name], ";
                    select += Environment.NewLine;
                    if (item.Value != "Orphan")
                    {
                        from += " INNER JOIN ActiveDiagramObjects AS Diagram_" + previousItem.Value + "_" + fileCount.ToString() + " ON " + previousPrefix + ".pkid = Diagram_" + previousItem.Value + "_" + fileCount.ToString() + ".MetaObjectID AND " + previousPrefix + ".Machine = Diagram_" + previousItem.Value + "_" + fileCount.ToString() + ".Machine";
                        from += Environment.NewLine;
                    }
                    else
                    {
                        //where object string concat not in (activediagramobjects concat)
                        fileWhere += " CONVERT(nVarChar(50)," + previousPrefix + ".pkid) + " + previousPrefix + ".Machine NOT IN (SELECT CONVERT(nVarChar(50),MetaObjectID) + Machine FROM ActiveDiagramObjects) AND ";
                        fileWhere += Environment.NewLine;
                    }
                    if (item.Value != "All" && item.Value != "Orphan")
                    {
                        string filename = Core.strings.GetFileNameOnly(item.Value);
                        fileWhere = " Diagram_" + previousItem.Value + "_" + fileCount.ToString() + ".[File Name] = '" + filename + "' AND ";
                        fileWhere += Environment.NewLine;
                    }
                    fileCount++;
                }
            }

            #region Cleanup
            select = select.Trim().TrimEnd(',');
            where += associationWhere + fileWhere;

            if (where.Contains("."))
                where = where.Trim().TrimEnd('D').TrimEnd('N').TrimEnd('A');
            else
                where = "";

            select += " ";
            #endregion

            Query = select + Environment.NewLine + from + where;

            return Query;
        }
        private string returnSelectForContextFilterItem(string prefix, ContextFilterItem item)
        {
            string s = "";
            if (item.FType == FilterType.Class || item.FType == FilterType.Artefact)
            {
                if (item.Items == null || item.Items.Count == 0)
                {
                    //if (item.FType == FilterType.Class)
                    s += prefix + ".*, ";
                }
                else
                {
                    foreach (ContextFilterItem i in item.Items)
                    {
                        if (i.FType == FilterType.Field)
                        {
                            s += prefix + "." + i.Value + ", ";
                        }
                    }
                }
                s += Environment.NewLine;
            }
            return s;
        }
        private string returnWhereForContextFilterItem(string prefix, ContextFilterItem item)
        {
            string s = "";
            if (item.FType == FilterType.Class || item.FType == FilterType.Artefact)
            {
                if (item.Items != null)
                {
                    foreach (ContextFilterItem i in item.Items)
                    {
                        if (i.FType == FilterType.Field)
                        {
                            if (i.FieldValue.Length > 0)
                                s += prefix + "." + i.Value + " LIKE '%" + i.FieldValue + "%' AND ";
                        }
                    }
                }
                s += Environment.NewLine;
            }
            return s;
        }

        public EventHandler Applied;
        public void OnApply()
        {
            if (Applied != null)
                Applied(this, EventArgs.Empty);
        }

        public List<string> workspacesAllowed = new List<string>();
        public void Apply()
        {
            List<string> associationsAllowed = new List<string>();
            workspacesAllowed = new List<string>();

            if (Context == null)
            {
                Report();
                OnApply();
                return;
            }

            if (listViewParents.Items.Count == 0)
            {
                foreach (GoObject obj in Context)
                {
                    obj.Visible = true;
                }

                return;
            }

            foreach (System.Windows.Forms.ListViewItem lvi in listViewParents.Items)
            {
                ContextFilterItem item = lvi.Tag as ContextFilterItem;
                item.Show = lvi.Checked;
                if (item.FType == FilterType.AssociationType)
                {
                    associationsAllowed.Add(item.Value.Trim());
                }
                else if (item.FType == FilterType.Workspace)
                {
                    workspacesAllowed.Add(item.Value.Trim());
                }
            }
            OnApply();

            foreach (GoObject obj in Context)
            {
                if (obj is ContextNode)
                {
                    bool show = checkBoxInvert.Checked ? true : false;
                    ContextNode node = obj as ContextNode;
                    foreach (System.Windows.Forms.ListViewItem lvi in listViewParents.Items)
                    {
                        ContextFilterItem item = lvi.Tag as ContextFilterItem;
                        if (item.FType == FilterType.Workspace) //WS filter happens in litegraphviewcontainer :(
                            continue;

                        if (node.MetaObject != null && (item.FType == FilterType.Class || item.FType == FilterType.Artefact))
                        {
                            bool breakAfterSet = false;
                            //field visible
                            if (node.MetaObject.Class == item.Value) //match class
                            {
                                if (item.Items == null || item.Items.Count == 0)
                                {
                                    show = true;
                                }
                                else
                                {
                                    string falseFields = "";
                                    #region Field values
                                    foreach (ContextFilterItem childItem in item.Items)
                                    {
                                        if (childItem.FType == FilterType.Field)
                                        {
                                            object val = node.MetaObject.Get(childItem.Value);
                                            if (val == null || val.ToString() == "")
                                            {
                                                if (childItem.FieldValue == null || childItem.FieldValue.Length == 0)
                                                {
                                                    //show = true;
                                                }
                                                else
                                                {
                                                    falseFields += childItem.Value;
                                                }
                                            }
                                            else if (val.ToString().ToLower().Contains(childItem.FieldValue.ToLower()))
                                            {
                                                //show = true;
                                            }
                                            else
                                            {
                                                falseFields += childItem.Value;
                                                //show = false;
                                            }
                                        }
                                    }
                                    #endregion

                                    show = falseFields.Length > 0 ? false : true;
                                }
                                breakAfterSet = true;
                            }
                            else
                            {
                                show = false;
                            }
                            //workspace secondary visible
                            if (workspacesAllowed.Count > 0 && show)
                            {
                                if (workspacesAllowed.Contains(node.MetaObject.WorkspaceName))
                                    show = true;
                                else
                                    show = false;
                            }
                            obj.Visible = checkBoxInvert.Checked ? !show : show;
                            if (breakAfterSet)
                                break;
                        }
                        else if (node.MetaObject == null && item.FType == FilterType.Diagram)
                        {
                            if (node.Visible) //workspace can make it not visible
                            {
                                if (node.Text.Contains(item.Value))
                                    show = true;

                                obj.Visible = checkBoxInvert.Checked ? !show : show; //filename can make a diagram in a visible workspace not visible
                            }
                        }
                        else
                        {
                            //if (node.Visible)
                            //    continue;
                            ////if (node.MetaObject == null)
                            ////    show = true;
                            ////else if (workspacesAllowed.Contains(node.MetaObject.Workspace)) //workspace check
                            ////    show = true;

                            //obj.Visible = show;
                        }
                    }

                    foreach (GoObject lnk in node.Links)
                    {
                        if (lnk is FishLink)
                        {
                            lnk.Visible = node.Visible;
                            continue;
                        }
                        if ((lnk as QLink).FromNode.GoObject.Visible && (lnk as QLink).ToNode.GoObject.Visible)
                            lnk.Visible = true;
                        else
                            lnk.Visible = false;
                    }
                }
            }
            if (associationsAllowed.Count > 0)
            {
                foreach (GoObject obj in Context)
                {
                    if (obj is QLink)
                    {
                        bool show = false;
                        if (associationsAllowed.Contains((obj as QLink).AssociationType.ToString().Trim()))
                            show = true;
                        obj.Visible = checkBoxInvert.Checked ? !show : show;
                        //make artefacts on this item invisible
                        //(obj as QLink).ToNode.GoObject.Visible = show;
                        //(obj as QLink).FromNode.GoObject.Visible = show;
                    }
                }
            }
        }

        private void buttonReport_Click(object sender, EventArgs e)
        {
            string r = Report();

            MessageBox.Show(r);
        }

        private void comboBoxChildType_SelectedIndexChanged(object sender, EventArgs e)
        {
            listViewChildren.SelectedItems.Clear();
            comboBoxChildValue.Text = "";
        }

        private void listViewChildren_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            //e.DrawBackground();
            //if (e.Item.Selected)
            //    if (!listViewChildren.Focused)
            //        e.Item.ForeColor = Color.LightBlue;
            //    else
            //        e.Item.ForeColor = Color.Black;
        }

        private void listViewParents_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            //e.DrawBackground();
            //if (e.Item.Selected)
            //    if (!listViewParents.Focused)
            //        e.Item.ForeColor = Color.LightBlue;
            //    else
            //        e.Item.ForeColor = Color.Black;
        }

        public List<ContextFilterItem> Filters
        {
            get
            {
                List<ContextFilterItem> filters = new List<ContextFilterItem>();
                foreach (System.Windows.Forms.ListViewItem lvi in listViewParents.Items)
                {
                    ContextFilterItem item = lvi.Tag as ContextFilterItem;
                    filters.Add(item);
                }
                return filters;
            }
        }


        #region IDisposable Members

        void IDisposable.Dispose()
        {
            Context = null;
        }

        #endregion

        private void checkBoxInvert_CheckedChanged(object sender, EventArgs e)
        {
            Apply();
        }
    }

    public enum FilterType
    {
        Base = 0,
        Class = 1,
        Association = 2,
        Artefact = 3,
        Workspace = 5,
        Diagram = 6,

        Field = 10,

        AssociationType = 15
    }

    public class ContextFilterItem
    {
        private bool enabled;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        private bool show;
        public bool Show
        {
            get { return show; }
            set { show = value; }
        }

        private FilterType type;
        public FilterType FType
        {
            get { return type; }
            set { type = value; }
        }

        //for class & association
        private List<ContextFilterItem> items;
        public List<ContextFilterItem> Items
        {
            get { return items; }
            set { items = value; }
        }

        private string val;
        public string Value
        {
            get { return val; }
            set { val = value; }
        }

        private string fval;
        public string FieldValue
        {
            get { return fval; }
            set { fval = value; }
        }

        public ContextFilterItem(FilterType t, string v, List<ContextFilterItem> children)
        {
            Enabled = true;
            Show = true;
            FType = t;
            Value = v;

            Items = children;
        }
        public ContextFilterItem(FilterType t, string v)
        {
            Enabled = true;
            Show = true;
            FType = t;
            Value = v;

            if (FType == FilterType.Class)
            {
                Items = new List<ContextFilterItem>();
            }
            else
                Items = null;
        }

        public override string ToString()
        {
            return FType.ToString() + " filter for " + Value + " " + FieldValue;// +" (" + (Enabled == true ? "Enabled" : "Disabled") + ")";
        }

    }
}