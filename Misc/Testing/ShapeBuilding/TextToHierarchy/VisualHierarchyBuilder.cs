using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Northwoods.Go;
using Northwoods.Go.Layout;
using MetaBuilder.Meta;
using System.Text.RegularExpressions;
using MetaBuilder.Graphing.Shapes;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using MetaBuilder.UIControls;
using Northwoods.Go.Draw;

namespace ShapeBuilding.TextToHierarchy
{
    public partial class VisualHierarchyBuilder : Form
    {
        #region Properties & Objects used throughout
        RootOnlyTreeLayout layout;
        b.GraphFile file;
        FastColoredTextBoxNS.AutocompleteMenu popupMenu;
        FastColoredTextBoxNS.AutocompleteMenu popupMenuLinkTypes;
        MetaBuilder.BusinessFacade.MetaHelper.AssociationHelper assHelper;
        b.TList<b.ObjectAssociation> associations;
        FastColoredTextBoxNS.TextStyle maroonStyle = new FastColoredTextBoxNS.TextStyle(Brushes.Maroon, null, FontStyle.Regular);
        FastColoredTextBoxNS.TextStyle blueStyle = new FastColoredTextBoxNS.TextStyle(Brushes.RoyalBlue, null, FontStyle.Regular);
        #endregion

        public VisualHierarchyBuilder()
        {
            InitializeComponent();
            InitControls();
            AddStickies();
        }

        public void AddStickies()
        {
            StickyNode snode = new StickyNode();
            snode.Text = "S1";
            goView1.Document.Add(snode);

            goView1.ReplaceMouseTool(typeof(GoDrawToolDragging), new StickyDraggingTool(goView1));
            goView1.ReplaceMouseTool(typeof(GoToolDragging), new StickyDraggingTool(goView1));
        }

        private void InitControls()
        {
            assHelper = new MetaBuilder.BusinessFacade.MetaHelper.AssociationHelper();
            popupMenu = new FastColoredTextBoxNS.AutocompleteMenu(textBox1);
            popupMenu.MinFragmentLength = 2;
            popupMenuLinkTypes = new FastColoredTextBoxNS.AutocompleteMenu(textBox1);
            b.TList<b.Class> classList = d.DataRepository.Classes(MetaBuilder.Core.Variables.Instance.ClientProvider);//].Provider.ClassProvider.GetAll();
            classList.Filter = "IsActive = 'True'";
            comboBox1.ValueMember = "Name";
            comboBox1.DisplayMember = "Name";
            comboBox1.DataSource = classList;

            List<FastColoredTextBoxNS.AutocompleteItem> classListAsString = new List<FastColoredTextBoxNS.AutocompleteItem>();
            foreach (b.Class c in classList)
            {
                if (c.IsActive.HasValue)
                {
                    if (c.IsActive.Value)
                    {
                        classListAsString.Add(new CustomAutocompleteItem(c.Name, "(" + c.Name + ")"));
                    }
                }
            }
            popupMenu.Items.SetAutocompleteItems(classListAsString);
            List<FastColoredTextBoxNS.AutocompleteItem> items = new List<FastColoredTextBoxNS.AutocompleteItem>();
            items.Add(new CustomAutocompleteItem("Auxiliary", "[A]"));
            items.Add(new CustomAutocompleteItem("Decomposition", "[D]"));
            items.Add(new CustomAutocompleteItem("Mapping", "[M]"));

            popupMenuLinkTypes.Items.SetAutocompleteItems(items);
            comboBox1.Text = "Function";
        }

        #region Layout
        private void SetLayoutDefaults()
        {
            layout = new RootOnlyTreeLayout();
            layout.Angle = 90f;
            layout.Alignment = GoLayoutTreeAlignment.CenterChildren;
            layout.AlternateDefaults.Alignment = GoLayoutTreeAlignment.Start;
            layout.NodeIndent = 0;
            layout.NodeSpacing = 10;
            layout.RowIndent = 10;
            layout.RowSpacing = 10;
            layout.LayerSpacing = 10;

            layout.AlternateDefaults.RowIndent = 10;
            layout.AlternateDefaults.RowSpacing = 15;
            layout.AlternateDefaults.NodeIndent = 25;
            layout.AlternateDefaults.NodeSpacing = 15;
            if (cbUseTextOnlyNodes.Checked)
            {
                layout.AlternateDefaults.LayerSpacing = -10;
            }
            else
            {
                layout.AlternateDefaults.LayerSpacing = 10;
            }
        }
        private void DoLayout()
        {
            layout.Document = goView1.Document;
            goView1.SuspendLayout();
            layout.PerformLayout();
            layout.LayoutNodesAndLinks();
            goView1.ResumeLayout();
        }
        #endregion

        #region Handle Events
        private bool _shiftTabKeyDown;
        private bool isPasting;
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            _shiftTabKeyDown = false;
            if (e.KeyCode == Keys.Tab && e.Shift)
            {
                _shiftTabKeyDown = true;
            }
            isPasting = false;
            if (e.KeyCode == Keys.V && e.Control)
            {
                isPasting = true;
            }
        }
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ProcessText();
            }
            if (e.KeyData == (Keys.K | Keys.Control))
            {
                //forced show (MinFragmentLength will be ignored)
                popupMenu.Show(true);
                e.Handled = true;
            }
            if (e.KeyData == (Keys.L | Keys.Control))
            {
                //forced show (MinFragmentLength will be ignored)
                popupMenuLinkTypes.Show(true);
                e.Handled = true;
            }

        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool isTab = e.KeyChar != '\t';
            bool isEmptyLine = textBox1.SelectionLength == 0;

            if (isEmptyLine || isTab)
            {
                return;
            }

            bool startNewLineAdded = false;
            bool endNewLineRemoved = false;
            string selection = textBox1.SelectedText;

            if (!selection.StartsWith(Environment.NewLine))
            {
                selection = Environment.NewLine + selection;
                startNewLineAdded = true;
            }

            if (selection.EndsWith(Environment.NewLine))
            {
                selection = selection.Substring(0, selection.Length
                    - Environment.NewLine.Length);
                endNewLineRemoved = true;
            }

            if (_shiftTabKeyDown)
            {
                selection = selection.Replace(Environment.NewLine + '\t',
                    Environment.NewLine);
            }
            else
            {
                selection = selection.Replace(Environment.NewLine,
                    Environment.NewLine + '\t');
            }

            if (startNewLineAdded)
            {
                selection = selection.Substring(Environment.NewLine.Length);
            }

            if (endNewLineRemoved)
            {
                selection += Environment.NewLine;
            }

            textBox1.SelectedText = selection;
            e.Handled = true;
        }
        private void textBox1_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {

            //clear previous highlighting
            e.ChangedRange.ClearStyle(maroonStyle);
            e.ChangedRange.ClearStyle(blueStyle);
            //highlight tags
            e.ChangedRange.SetStyle(maroonStyle, @"\[[^>]+\]");
            e.ChangedRange.SetStyle(blueStyle, @"\(\w*\)");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DoLayout();
        }
        private void textBox1_LineInserted(object sender, FastColoredTextBoxNS.LineInsertedEventArgs e)
        {
            if (!isPasting)
                ProcessText();
        }
        private void textBox1_LineRemoved(object sender, FastColoredTextBoxNS.LineRemovedEventArgs e)
        {
            ProcessText();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            CoreInjector cinjector = new CoreInjector();
            cinjector.InjectCoreVariables();
            EnsureStructFileExists();
            ClearPreviousData();
            // Save nodes
            SaveAssociations();
            SaveGraphFileAssociations();
            SaveBlob();
        }
        public static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        private void SaveBlob()
        {
            file.Blob = StrToByteArray(textBox1.Text);
            d.DataRepository.Connections[MetaBuilder.Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.Save(file);
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ProcessText();
        }
        #endregion

        #region Processing
        private void ProcessText()
        {
            this.goView1.Document.Clear();
            IList<string> lines = textBox1.Lines;
            treeView1.Nodes.Clear();
            GetTree(lines, this.treeView1);
            treeView1.ExpandAll();
            BuildDiagram();
            SetLayoutDefaults();
            propertyGrid1.SelectedObject = layout.AlternateDefaults;
            DoLayout();
        }
        private void BuildDiagram()
        {
            goView1.Document.Clear();
            foreach (LinkedTreeNode n in treeView1.Nodes)
            {
                AddDiagramNode(n, null);
            }
        }
        private void AddDiagramNode(LinkedTreeNode n, GoNode parent)
        {
            TreeBasicNode basicNode;
            MetaBase mb = null;
            try
            {
                mb = Loader.CreateInstance(n.OverrideClass);
                mb.Set("Name", n.Text.Trim());
            }
            catch
            {
                MessageBox.Show(this,"No such class (" + n.OverrideClass + ")");
            }

            if (cbUseTextOnlyNodes.Checked)
            {
                basicNode = new TreeTextNode();
            }
            else
            {
                basicNode = new TreeBasicNode();
            }
            if (n.Level < 2)
            {
                basicNode = new TreeBasicNode();
            }
            basicNode.SetMetaBase(mb);
            basicNode.Text = n.Text.Trim();
            goView1.Document.Add(basicNode as GoNode);
            #region Add Link
            if (parent != null)
            {
                GoPort prtTo = null;
                GoNodePortEnumerator portEnum = basicNode.Ports.GetEnumerator();
                while (portEnum.MoveNext())
                {
                    if (portEnum.Current is GoPort)
                        prtTo = portEnum.Current as GoPort;
                }

                GoPort prtFrom = null;
                GoNodePortEnumerator portEnumFrom = parent.Ports.GetEnumerator();
                while (portEnumFrom.MoveNext())
                {
                    if (portEnumFrom.Current is GoPort)
                        prtFrom = portEnumFrom.Current as GoPort;
                }
                QLink lnk = new QLink();
                if (n.AssociationType.HasValue)
                {
                    lnk.AssociationType = n.AssociationType.Value;
                }
                else
                {
                    lnk.Style = GoStrokeStyle.RoundedLine;
                    lnk.Orthogonal = true;
                    lnk.ToArrow = true;
                }
                lnk.FromPort = prtFrom;
                lnk.ToPort = prtTo;
                goView1.Document.Add(lnk);
            }
            #endregion
            foreach (LinkedTreeNode nChild in n.Nodes)
            {
                AddDiagramNode(nChild, basicNode);
            }
        }
        public void GetTree(IList<string> lines, TreeView obj)
        {
            foreach (string s in lines)
            {
                if (s.TrimStart('\t') != string.Empty)
                    try
                    {
                        ProcessLine(TabCount(s), s.TrimStart('\t'), obj);
                    }
                    catch { }
            }
        }
        private void ProcessLine(int depth, string item, TreeView obj)
        {
            if (item.Trim().Length > 0)
            {
                LinkedTreeNode ltn;
                if (depth == 0)
                {
                    ltn = GetNodeFromString(item);
                    obj.Nodes.Add(ltn);
                    return;
                }
                TreeNode tn = obj.Nodes[obj.Nodes.Count - 1];
                if (tn.Nodes.Count > 0)
                {
                    for (int j = 1; j < depth; j++)
                    {
                        tn = tn.Nodes[tn.Nodes.Count - 1];
                    }
                }
                ltn = GetNodeFromString(item);

                if (!ltn.AssociationType.HasValue)
                {
                    // set default associationtype
                    LinkedTreeNode ltnParent = tn as LinkedTreeNode;
                    MetaBuilder.BusinessFacade.MetaHelper.AllowedAssociationInfo info = assHelper.GetDefaultAllowedAssociationInfo(ltnParent.OverrideClass, ltn.OverrideClass);
                    if (info != null)
                    {
                        ltn.AssociationType = info.LinkAssociationType;
                    }
                }
                tn.Nodes.Add(ltn);
            }
        }
        #region Class & Connectors
        public LinkedTreeNode GetLinkType(string s, string searchForChar)
        {
            LinkedTreeNode retval = null;
            string regexstring = @"\[(?i)" + searchForChar + "]";
            Match m = Regex.Match(s, regexstring);
            if (m.Success)
            {
                retval = new LinkedTreeNode();
                switch (searchForChar.ToLower())
                {
                    case "a":
                        retval.AssociationType = LinkAssociationType.Auxiliary;
                        break;
                    case "c":
                        retval.AssociationType = LinkAssociationType.Classification;
                        break;
                    case "d":
                        retval.AssociationType = LinkAssociationType.Decomposition;
                        break;
                    case "m":
                        retval.AssociationType = LinkAssociationType.Mapping;
                        break;
                    default:
                        retval.AssociationType = null;
                        break;
                }
                retval.Text = Regex.Replace(s, regexstring, "");
                return retval;
            }
            return null;

        }
        public LinkedTreeNode GetNodeFromString(string s)
        {
            LinkedTreeNode retval = GetLinkType(s, "a");

            if (retval == null)
                retval = GetLinkType(s, "d");

            if (retval == null)
                retval = GetLinkType(s, "c");

            if (retval == null)
                retval = GetLinkType(s, "m");

            if (retval == null)
            {
                retval = new LinkedTreeNode();
                retval.Text = s;
                retval.AssociationType = null;
            }
            SetClass(retval);
            return retval;

        }
        public void SetClass(LinkedTreeNode node)
        {
            string classRegEx = @"\(\w*\)\w*";
            Match m = Regex.Match(node.Text, classRegEx);
            if (m.Success)
            {
                string classString = m.Value.Replace("(", "").Replace(")", "");

                string properlyCapitalized = GetCapitalized(classString);
                if (properlyCapitalized == null)
                {
                    node.OverrideClass = comboBox1.Text;
                }
                else
                {
                    node.OverrideClass = properlyCapitalized;
                }
                node.Text = node.Text.Replace(m.Value, "");
                node.Text = node.Text.Trim();

            }
            else
            {
                node.OverrideClass = this.comboBox1.Text;
            }


        }
        private string GetCapitalized(string sNotCap)
        {
            foreach (object o in comboBox1.Items)
            {
                b.Class c = o as b.Class;
                if (c.Name.ToLower() == sNotCap.ToLower())
                {
                    return c.Name;
                }
            }
            return null;
        }
        private static int TabCount(string str)
        {
            int j = 0;
            string tabString = "    ";
            string strWithoutTabs = str.TrimStart();
            if (strWithoutTabs.Length < str.Length)
            {
                int index = str.IndexOf(tabString);
                while (index > -1)
                {
                    j++;
                    index = str.IndexOf(tabString, index + tabString.Length);
                }
            }
            return j;
        }
        #endregion
        #endregion

        #region Database Access
        private void EnsureStructFileExists()
        {
            if (file == null)
            {
                file = new MetaBuilder.BusinessLogic.GraphFile();
                file.Blob = new byte[1];
                file.MajorVersion = 1;
                file.MinorVersion = 1;
                file.Machine = MetaBuilder.Core.Variables.Instance.UserDomainIdentifier;
                file.WorkspaceName = MetaBuilder.Core.Variables.Instance.CurrentWorkspaceName;
                file.WorkspaceTypeId = MetaBuilder.Core.Variables.Instance.CurrentWorkspaceTypeId;
                file.ModifiedDate = DateTime.Now;
                file.OriginalFileUniqueID = Guid.Empty;
                file.IsActive = true;
                file.FileTypeID = 1;
            }
            file.Name = txtStructureName.Text;
            d.DataRepository.GraphFileProvider.Save(file);

        }
        private void ClearPreviousData()
        {
            // Clear previous nodes associated with this diagram
            b.TList<b.GraphFileObject> gfos = d.DataRepository.Connections[MetaBuilder.Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine(file.pkid, file.Machine);
            b.TList<b.GraphFileAssociation> gfass = d.DataRepository.Connections[MetaBuilder.Core.Variables.Instance.ClientProvider].Provider.GraphFileAssociationProvider.GetByGraphFileIDGraphFileMachine(file.pkid, file.Machine);

            List<b.MetaObjectKey> moKeys = new List<MetaBuilder.BusinessLogic.MetaObjectKey>();
            foreach (b.GraphFileObject gfo in gfos)
            {
                b.MetaObjectKey mk = new MetaBuilder.BusinessLogic.MetaObjectKey();
                mk.pkid = gfo.MetaObjectID;
                mk.Machine = gfo.MachineID;
                moKeys.Add(mk);
            }

            foreach (b.MetaObjectKey mokey in moKeys)
            {
                MetaBuilder.BusinessFacade.MetaHelper.Singletons.GetObjectHelper().DeleteObject(mokey.pkid, mokey.Machine);
            }
            d.DataRepository.Connections[MetaBuilder.Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.Delete(gfos);
            d.DataRepository.Connections[MetaBuilder.Core.Variables.Instance.ClientProvider].Provider.GraphFileAssociationProvider.Delete(gfass);
        }
        private void SaveAssociations()
        {
            associations = new b.TList<MetaBuilder.BusinessLogic.ObjectAssociation>();
            foreach (GoObject o in this.goView1.Document)
            {
                if (o is TreeBasicNode)
                {
                    TreeBasicNode node = o as TreeBasicNode;
                    if (node.SourceLinks.Count == 0)
                    {
                        node.MetaObject.Save(Guid.NewGuid());
                        CreateGraphFileObject(node.MetaObject);
                        SaveLinks(node);
                    }
                }
            }
            d.DataRepository.Connections[MetaBuilder.Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(associations);
        }
        private void SaveGraphFileAssociations()
        {
            b.TList<b.GraphFileAssociation> gfas = new MetaBuilder.BusinessLogic.TList<MetaBuilder.BusinessLogic.GraphFileAssociation>();
            foreach (b.ObjectAssociation oa in associations)
            {
                b.GraphFileAssociation gfa = new MetaBuilder.BusinessLogic.GraphFileAssociation();
                gfa.CAid = oa.CAid;
                gfa.ObjectID = oa.ObjectID;
                gfa.ObjectMachine = oa.ObjectMachine;
                gfa.ChildObjectID = oa.ChildObjectID;
                gfa.ChildObjectMachine = oa.ChildObjectMachine;
                gfa.GraphFileID = file.pkid;
                gfa.GraphFileMachine = file.Machine;
                gfas.Add(gfa);
            }
            d.DataRepository.Connections[MetaBuilder.Core.Variables.Instance.ClientProvider].Provider.GraphFileAssociationProvider.Save(gfas);
        }
        private void CreateGraphFileObject(MetaBase mb)
        {
            b.GraphFileObject gfoS = new MetaBuilder.BusinessLogic.GraphFileObject();
            gfoS.MachineID = mb.MachineName;
            gfoS.MetaObjectID = mb.pkid;
            gfoS.GraphFileID = file.pkid;
            gfoS.GraphFileMachine = file.Machine;
            try
            {
                d.DataRepository.Connections[MetaBuilder.Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.Save(gfoS);
            }
            catch { }
        }
        private void SaveLinks(TreeBasicNode node)
        {
            foreach (IGoLink lnk in node.DestinationLinks)
            {
                TreeBasicNode nTo = lnk.ToNode as TreeBasicNode;
                nTo.MetaObject.Save(Guid.NewGuid());

                b.GraphFileObject gfo = new MetaBuilder.BusinessLogic.GraphFileObject();
                gfo.MachineID = nTo.MetaObject.MachineName;
                gfo.MetaObjectID = nTo.MetaObject.pkid;
                gfo.GraphFileID = file.pkid;
                gfo.GraphFileMachine = file.Machine;
                b.GraphFileObject gfoExisting = d.DataRepository.Connections[MetaBuilder.Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.Get(new b.GraphFileObjectKey(gfo));
                if (gfoExisting == null)
                    try
                    {
                        d.DataRepository.Connections[MetaBuilder.Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.Save(gfo);
                    }
                    catch
                    {
                    }

                b.ObjectAssociation oa = new b.ObjectAssociation();
                oa.ObjectID = node.MetaObject.pkid;
                oa.ObjectMachine = node.MetaObject.MachineName;
                oa.ChildObjectID = nTo.MetaObject.pkid;
                oa.ChildObjectMachine = nTo.MetaObject.MachineName;

                if (lnk is QLink)
                {
                    QLink qlink = lnk as QLink;
                    int CAid = assHelper.GetAssociation(node.MetaObject._ClassName, nTo.MetaObject._ClassName, (int)qlink.AssociationType).ID;
                    if (CAid > 0)
                    {
                        oa.CAid = CAid;
                        associations.Add(oa);
                    }
                }
                SaveLinks(nTo);
            }
        }
        #endregion

        //bool isLoading;
        private void btnLoad_Click(object sender, EventArgs e)
        {
            DiagramChooser diagramChooser = new DiagramChooser();
            diagramChooser.ShowDialog(this);

            if (diagramChooser.DialogResult == DialogResult.OK)
            {
                b.GraphFile file = diagramChooser.SelectedFile;
                //isLoading = true;
                LoadStructure(file);
            }
            ProcessText();
            //isLoading = false;
        }
        private void LoadStructure(b.GraphFile file)
        {
            //isLoading = true;
            b.GraphFile retrievedFile = MetaBuilder.DataAccessLayer.DataRepository.Connections[MetaBuilder.Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.GetBypkidMachine(file.pkid, file.Machine);
            MetaBuilder.Graphing.Persistence.GraphFileManager gfmanager = new MetaBuilder.Graphing.Persistence.GraphFileManager();
            try
            {
                MetaBuilder.Graphing.Containers.NormalDiagram diagram = gfmanager.RetrieveGraphDoc(retrievedFile);
                textBox1.Text = "";
                foreach (GoObject o in diagram)
                {
                    if (o is GoNode)
                    {
                        GoNode node = o as GoNode;
                        if (node.SourceLinks.Count == 0)
                        {
                            AddNodeItem(node, LinkAssociationType.Auxiliary, 0);
                        }
                    }
                }
            }
            catch
            {
                byte[] dBytes = retrievedFile.Blob;
                System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                textBox1.Text = enc.GetString(dBytes);
            }
            //maak seker die ding doen nie elke keer n layout vir elke node as mens load nie, selle as Paste.
        }
        private void AddNodeItem(GoNode node, LinkAssociationType lnkType, int Tabs)
        {
            if (node is IMetaNode)
            {
                IMetaNode imn = node as IMetaNode;
                StringBuilder sbTabs = new StringBuilder();
                for (int i = 0; i < Tabs; i++)
                {
                    sbTabs.Append("    ");
                }

                if (Tabs > 0)
                {
                    switch (lnkType)
                    {
                        case LinkAssociationType.Auxiliary:
                            sbTabs.Append("[A] ");
                            break;
                        case LinkAssociationType.Classification:
                            sbTabs.Append("[C] ");
                            break;
                        case LinkAssociationType.Mapping:
                            sbTabs.Append("[M] ");
                            break;
                    }
                }

                textBox1.Text += sbTabs.ToString() + imn.MetaObject.ToString() + Environment.NewLine;
                GoNodeLinkEnumerator linkEnum = node.DestinationLinks;
                while (linkEnum.MoveNext())
                {
                    if (linkEnum.Current.ToNode is GoNode && linkEnum.Current is QLink)
                    {
                        QLink lnk = linkEnum.Current as QLink;
                        AddNodeItem(linkEnum.Current.ToNode as GoNode, lnk.AssociationType, Tabs + 1);
                    }
                }
            }
        }
        private void btnSaveTextFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveTextDialog = new SaveFileDialog();
            saveTextDialog.DefaultExt = ".txt";
            saveTextDialog.Filter = "Text Files|*.txt";
            saveTextDialog.Title = "Save file as...";
            DialogResult result = saveTextDialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                System.IO.TextWriter tw = new System.IO.StreamWriter(saveTextDialog.FileName);

                foreach (string line in textBox1.Lines)
                {
                    tw.WriteLine(line);
                }
                // close the stream
                tw.Close();
            }
        }
        private void btnLoadText_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files|*.txt";
            DialogResult result = ofd.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                System.IO.TextReader reader = new System.IO.StreamReader(ofd.FileName);
                textBox1.Text = reader.ReadToEnd();
            }
        }
    }

    #region Custom Classes
    public class CustomAutocompleteItem : FastColoredTextBoxNS.AutocompleteItem
    {

        public CustomAutocompleteItem(string displayValue, string realValue)
        {
            this.Text = displayValue;
            this.RealValue = realValue;
        }
        private string realValue;

        public string RealValue
        {
            get { return realValue; }
            set { realValue = value; }
        }

        public override string GetTextForReplace()
        {
            if (RealValue != null)
                return RealValue;
            return base.GetTextForReplace();
        }

    }

    public class LinkedTreeNode : TreeNode
    {
        private LinkAssociationType? associationType;
        public LinkAssociationType? AssociationType
        {
            get { return associationType; }
            set { associationType = value; }
        }

        private string overrideClass;
        public string OverrideClass
        {
            get { return overrideClass; }
            set { overrideClass = value; }
        }
    }

    public interface IBasicMetaNode
    {
        void SetMetaBase(MetaBase mb);
    }

    public class RootOnlyTreeLayout : GoLayoutTree
    {
        public RootOnlyTreeLayout()
        {
            // set RootDefaults:
            this.Angle = 90;
            // set AlternateDefaults:
            this.AlternateDefaults.Angle = 0;
            this.AlternateDefaults.PortSpot = GoObject.BottomCenter;
            this.AlternateDefaults.ChildPortSpot = GoObject.MiddleLeft;
            this.AlternateDefaults.Alignment = GoLayoutTreeAlignment.Start;
        }
        // In the future, you can get the effect of this override by
        // specifying GoLayoutTree.Style = GoLayoutTreeStyle.RootOnly
        protected override void InitializeTreeNodeValues(GoLayoutTreeNode n)
        {
            GoLayoutTreeNode mom;
            if (n.Parent == null) mom = this.RootDefaults;
            else if (n.Parent.Parent == null) mom = this.AlternateDefaults;
            else mom = n.Parent;
            n.CopyInheritedPropertiesFrom(mom);
            n.Initialized = true;  // should already be true, but make sure
        }
    }

    public class TreeBasicNode : GoBasicNode, IBasicMetaNode
    {
        public virtual void SetMetaBase(MetaBase mb)
        {
            this.MetaObject = mb;
        }
        private MetaBase metaObject;
        public MetaBase MetaObject
        {
            get { return metaObject; }
            set { metaObject = value; }
        }
        public TreeBasicNode()
            : base()
        {
            GoRoundedRectangle rect = new GoRoundedRectangle();
            rect.Corner = new SizeF(2, 2);
            this.Shape = rect;
            this.Port.PortObject = this.Shape;
            this.Port.Style = GoPortStyle.None;
        }
        public override void LayoutChildren(GoObject childchanged)
        {
            base.LayoutChildren(childchanged);
            if (Label != null && Shape != null)
            {
                Shape.Bounds = Label.Bounds;
                RectangleF rect = Shape.Bounds;
                rect.Inflate(5, 5);
                Shape.Bounds = rect;
            }
        }
        public override string ToolTipText
        {
            get
            {
                if (MetaObject != null)
                {
                    return MetaObject.ToString() + " (" + MetaObject._ClassName + ")";
                }
                return base.ToolTipText;
            }
            set
            {
                base.ToolTipText = value;
            }
        }
    }

    public class TreeTextNode : TreeBasicNode, IBasicMetaNode
    {
        public override void SetMetaBase(MetaBase mb)
        {
            this.MetaObject = mb;
            if (classIndicator != null)
            {
                classIndicator.Text = mb._ClassName;
            }
        }

        private GoText classIndicator;
        public TreeTextNode()
            : base()
        {

            this.Port.PortObject = this.Shape;
            // this.Port.Style = GoPortStyle.None;

            GoText txtClass = new GoText();
            txtClass.FontSize = 6.5f;

            txtClass.TextColor = Color.Silver;
            txtClass.Text = "Class";
            txtClass.Italic = true;
            classIndicator = txtClass;
            Add(txtClass);
        }

        public override void LayoutChildren(GoObject childchanged)
        {
            float fHorizontalSize = 140f;
            base.LayoutChildren(childchanged);
            if (Label != null)
            {
                this.Label.FontSize = 7.5f;
                this.Label.Alignment = GoObject.MiddleLeft;

                Shape.Bounds = new RectangleF(new PointF(Label.Bounds.X - 5, Label.Bounds.Y), new SizeF(Label.Bounds.Width + 5, Label.Bounds.Height));


                Shape.Visible = false;
                RectangleF rect = Shape.Bounds;
                RectangleF originalRect = Shape.Bounds;
                rect.Inflate(2, 6.5f);
                if (Shape.Bounds.Width < fHorizontalSize)
                {
                    rect.Width = fHorizontalSize;
                }
                Label.Wrapping = true;
                Label.WrappingWidth = fHorizontalSize;


                Shape.Bounds = rect;
                if (classIndicator != null)
                {
                    classIndicator.Alignment = GoObject.TopRight;
                    classIndicator.Location = new PointF(Shape.Bounds.Right, Shape.Bounds.Top);
                }
                Shape.Bounds = originalRect;
            }
        }

        protected override RectangleF ComputeBounds()
        {
            if (Shape != null && classIndicator != null)
            {
                RectangleF rect = RectangleF.Union(Shape.Bounds, classIndicator.Bounds);
                return rect;
            }
            return base.ComputeBounds();
        }
        public override string ToolTipText
        {
            get
            {
                if (MetaObject != null)
                {
                    return MetaObject.ToString() + " (" + MetaObject._ClassName + ")";
                }
                return base.ToolTipText;

            }
            set
            {
                base.ToolTipText = value;
            }
        }
    }
    #endregion
}