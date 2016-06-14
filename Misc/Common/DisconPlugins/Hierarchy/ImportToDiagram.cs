using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Tools;
using MetaBuilder.Meta;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.PluginSDK;
using MetaBuilder.UIControls.GraphingUI;
using Northwoods.Go;
using Northwoods.Go.Layout;

namespace DisconPlugins.Hierarchy
{
    public class ImportToDiagramPlugin : IPlugin
    {

        #region Properties (1)

        public string Name
        {
            get { return "Import To Diagram"; }
        }

        #endregion Properties

        #region Methods (1)


        // Public Methods (1) 

        public bool PerformAction(IPluginContext context)
        {
            try
            {
                ImportToDiagram importer = new ImportToDiagram();
                importer.Import(context);
                return false;
            }
            catch
            {
                return true;
            }
        }


        #endregion Methods

    }

    public class PluginTextImportSpecification
    {

        #region Fields (3)

        private string className;
        private string defaultField;
        private string sourceFile;

        #endregion Fields

        #region Properties (3)

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        public string DefaultField
        {
            get { return defaultField; }
            set { defaultField = value; }
        }

        public string SourceFile
        {
            get { return sourceFile; }
            set { sourceFile = value; }
        }

        #endregion Properties

    }

    public class ImportToDiagram
    {

        #region Fields (2)

        private MetaBuilder.UIControls.Dialogs.QueryForTextImport chooseClass;
        private IPluginContext myContext;

        #endregion Fields

        #region Constructors (1)

        public ImportToDiagram()
        {

        }

        #endregion Constructors

        #region Methods (2)


        // Public Methods (1) 

        public void Import(IPluginContext context)
        {
            //if (context.CurrentStencil == null)
            //{
            //    MessageBox.Show(this,"Please open an applicable stencil first", "Open Stencil", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            myContext = context;

            DoImport();
        }

        // Private Methods (1) 

        private void DoImport()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Text Files|*.txt";
            openDialog.Multiselect = false;
            DialogResult resFile = openDialog.ShowDialog();
            if (resFile == DialogResult.OK)
            {
                chooseClass = new MetaBuilder.UIControls.Dialogs.QueryForTextImport();
                DialogResult result = chooseClass.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string ClassName = chooseClass.MyClass;
                    string defaultField = chooseClass.MyField;
                    PluginTextImportSpecification spec = new PluginTextImportSpecification();
                    spec.ClassName = ClassName;
                    spec.SourceFile = openDialog.FileName;
                    spec.DefaultField = defaultField;

                    TextFile tfile = new TextFile();
                    tfile.Specification = spec;

                    GraphNode myNodeToUse = null;
                    GraphNode newNode = (GraphNode)MetaBuilder.Core.Variables.Instance.ReturnShape(ClassName);
                    if (newNode != null)
                        myNodeToUse = (GraphNode)newNode.Copy();

                    // find the node to use
                    if (myNodeToUse == null && myContext != null && myContext.CurrentStencil != null)
                    {
                        foreach (GoObject o in myContext.CurrentStencil.Document)
                        {
                            if (o is GraphNode)
                            {
                                GraphNode node = o as GraphNode;
                                if (node.BindingInfo.BindingClass == ClassName)
                                {
                                    myNodeToUse = node.Copy() as GraphNode;
                                }
                            }
                        }
                    }

                    if (myNodeToUse == null)
                    {
                        DockingForm.DockForm.m_paletteDocker.stripButtonOpen_Click(this, EventArgs.Empty);
                        //MessageBox.Show(this,"Please open an applicable stencil first", "Open Stencil", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        myContext.CurrentGraphView.StartTransaction();
                        tfile.Import(myContext.CurrentGraphView.Document, myNodeToUse, true);

                        RootOnlyTreeLayout layout = new RootOnlyTreeLayout();
                        layout.Document = myContext.CurrentGraphView.Document;
                        layout.Alignment = GoLayoutTreeAlignment.Start;
                        layout.Arrangement = GoLayoutTreeArrangement.FixedRoots;
                        layout.Style = GoLayoutTreeStyle.LastParents;
                        layout.PerformLayout();

                        myContext.CurrentGraphView.FinishTransaction("Import to Diagram");
                    }
                }
            }
        }

        #endregion Methods

    }
    public class TextFile
    {

        #region Fields (7)

        private GoDocument doc;
        List<Item> items;
        private GraphNode nodeToUse;
        private TextReader reader;
        private PluginTextImportSpecification specification;
        public StringBuilder sReport;
        private bool validInput;

        #endregion Fields

        #region Constructors (1)

        public TextFile()
        {
        }

        #endregion Constructors

        #region Properties (2)

        public PluginTextImportSpecification Specification
        {
            get { return specification; }
            set { specification = value; }
        }

        public bool ValidInput
        {
            get { return validInput; }
            set { validInput = value; }
        }

        #endregion Properties

        #region Delegates and Events (1)


        // Events (1) 

        public event EventHandler Finished;


        #endregion Delegates and Events

        #region Methods (8)

        public bool DoTreeLayout;
        // Public Methods (1) 

        public void Import(GoDocument document, GraphNode node, bool doTreeLayout)
        {
            this.DoTreeLayout = doTreeLayout;
            nodeToUse = node;
            doc = document;
            sReport = new StringBuilder();
            if (Validate())
            {
                try
                {
                    if (Specification != null)
                    {
                        items = new List<Item>();

                        // done with the excel file, now import the text file (Modules)
                        reader = new StreamReader(Specification.SourceFile);
                        while (reader.Peek() != -1)
                        {
                            ConsumeTextLine(reader.ReadLine());
                        }
                        reader.Close();

                        foreach (Item line in items)
                        {
                            if ((line.Children.Count > 0) && (line.Parent == null))
                            {
                                SaveItem(line);
                            }
                        }
                    }
                }
                catch (Exception x)
                {
                    sReport.Append(x.ToString());
                }
            }

            TextWriter tw = null;
            if (File.Exists("C:\\program files\\discon specialists\\metabuilder\\metabuilder.exe"))
            {
                tw = new StreamWriter("C:\\program files\\discon specialists\\metabuilder\\metadata\\export\\ImportToDiagramReport.txt");
            }
            else
            {
                if (File.Exists("E:\\program files\\discon specialists\\metabuilder\\metabuilder.exe"))
                {
                    tw = new StreamWriter("E:\\program files\\discon specialists\\metabuilder\\metadata\\export\\ImportToDiagramReport.txt");
                }
            }

            if (tw != null)
            {
                tw.Write(sReport.ToString());
                tw.Close();
            }
            OnFinished(this, EventArgs.Empty);
        }

        // Protected Methods (1) 

        protected void OnFinished(object sender, EventArgs e)
        {
            if (Finished != null)
                Finished(sender, e);
        }

        // Private Methods (6) 

        //private void AddChild(GraphNode nodeParent, GraphNode child, int LinkType)
        //{
        //    /*GraphNode nodeCopy = nodeToUse.Copy() as GraphNode;
        //    nodeCopy.MetaObject = child.MBase;
        //    nodeCopy.Remove();
        //    doc.Add(nodeCopy);*/
        //    AddLink(nodeParent, child, LinkType);
        //}
        GraphNode coreParent;
        private void AddLink(GraphNode nodeParent, GraphNode nodeChild, int associationType)
        {
            //QLink.CreateLink();
            QLink slink = null;// new QLink();
            //slink.AssociationType = (LinkAssociationType)associationType;
            if (DoTreeLayout)
            {
                if (nodeParent == coreParent)
                    slink = QLink.CreateLink(nodeParent as GoNode, nodeChild as GoNode, associationType, MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Bottom, MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Top);
                else
                    slink = QLink.CreateLink(nodeParent as GoNode, nodeChild as GoNode, associationType, MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Bottom, MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Left);

                if (slink.FromPort == null)
                    slink.FromPort = GetBottomPort(nodeParent);
                if (slink.ToPort == null)
                    slink.ToPort = GetLeftPort(nodeChild);
            }
            else
            {
                slink = QLink.CreateLink(nodeParent as GoNode, nodeChild as GoNode, associationType, 0);

                if (slink.FromPort == null)
                    slink.FromPort = nodeParent.GetDefaultPort;
                if (slink.ToPort == null)
                    slink.ToPort = nodeChild.GetDefaultPort;
            }
            if (slink != null)
                doc.Add(slink);
        }

        private GoPort GetLeftPort(GraphNode node)
        {
            GoNodePortEnumerator portEnum = node.Ports.GetEnumerator();
            while (portEnum.MoveNext())
            {
                QuickPort port = portEnum.Current as QuickPort;
                if (port.Position.X == 200 && port.Position.Y == 150)
                    return port;
            }
            return node.GetDefaultPort;
        }
        private GoPort GetBottomPort(GraphNode node)
        {
            GoNodePortEnumerator portEnum = node.Ports.GetEnumerator();
            while (portEnum.MoveNext())
            {
                QuickPort port = portEnum.Current as QuickPort;
                if (port.Position.X == 260 && port.Position.Y == 195)
                    return port;
            }
            return node.GetDefaultPort;
        }

        public Item ParentLine;
        private void ConsumeTextLine(string text)
        {
            if (text.Trim().Length > 0)
            {
                text = text.TrimEnd();
                Item line = new Item(items.Count, text);
                items.Add(line);
                Item parent = GetParentItem(line);
                if (parent != null)
                {
                    parent.AddChild(line);
                }
                else
                {
                    ParentLine = line;
                }
            }
        }

        private Item GetParentItem(Item child)
        {
            Item testParentSDLine;

            for (int x = child.LineNumber; x >= 0; x--)
            {
                testParentSDLine = items[x];
                if (testParentSDLine.NumberOfTabs == child.NumberOfTabs - 1)
                {
                    return testParentSDLine;
                }
            }
            return null;
        }

        private GraphNode SaveItem(Item line)
        {
            MetaBase MetaObject = MetaBuilder.Meta.Loader.CreateInstance(Specification.ClassName);
            //line.PrepareWord();
            MetaObject.Set(Specification.DefaultField, line.WholeWord);
            line.MBase = MetaObject;
            GraphNode nodeParent = nodeToUse.Copy() as GraphNode;
            if (coreParent == null)
                coreParent = nodeParent;
            nodeParent.MetaObject = line.MBase;
            nodeParent.Remove();
            doc.Add(nodeParent);
            nodeParent.BindToMetaObjectProperties();
            //nodeParent.FireMetaObjectChanged(line, EventArgs.Empty);

            foreach (Item child in line.Children)
            {
                GraphNode nodeChild = SaveItem(child);
                try
                {
                    if (child.LinkType.HasValue)
                    {
                        if (AssociationManager.Instance.GetAssociationsForParentAndChildClasses(line.MBase._ClassName, child.MBase._ClassName).Count > 0)
                        {
                            AddLink(nodeParent, nodeChild, child.LinkType.Value);
                        }
                    }
                    else
                    {
                        //get default or LAST \/\/
                        foreach (MetaBuilder.BusinessLogic.ClassAssociation ca in AssociationManager.Instance.GetAssociationsForParentAndChildClasses(line.MBase._ClassName, child.MBase._ClassName))
                        {
                            if (ca.IsDefault)
                            {
                                child.LinkType = ca.AssociationTypeID;
                                break;
                            }
                            else
                                child.LinkType = ca.AssociationTypeID;
                        }
                        AddLink(nodeParent, nodeChild, child.LinkType.Value);
                    }
                }
                catch (Exception ex)
                {
                    //Metabuilder.Core.Log.WriteLog(ex.ToString());
                    sReport.Append("Trying to add a linktype of " + line.LinkTypeName + " (" + line.LinkType + ") between " + line.WholeWord + " (Line: " + line.LineNumber + ") and " + child.WholeWord + " (Line: " + child.LineNumber + ") generated a failure" + Environment.NewLine);
                }
            }
            return nodeParent;

        }

        private bool Validate()
        {
            bool validated = true;
            return validated;
            bool foundLinesEndingWithTabs = false;
            reader = new StreamReader(Specification.SourceFile);
            RegexOptions options = RegexOptions.Singleline;
            string regex = "^.*?$";

            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                MatchCollection matches = Regex.Matches(line, regex, options);
                foreach (Match match in matches)
                {
                    foundLinesEndingWithTabs = true;

                }
            }
            reader.Close();

            if (foundLinesEndingWithTabs)
            {
                sReport.Append("Cannot import - some lines end with tabs");
                validated = false;
            }
            validInput = validated;
            return validated;
        }

        #endregion Methods

        #region Nested Classes (1)


        public class Item
        {

            #region Fields (9)

            private List<Item> children;
            private int lineNumber;
            private int? linkType;
            private string linkTypeName;
            private MetaBase mbase;
            private int numberOfTabs;
            private string originalText;
            private Item parent;
            private string wholeWord;

            #endregion Fields

            #region Constructors (1)

            public Item(int LineNo, string text)
            {

                char splitChar = '\t';
                text = text.TrimEnd();
                OriginalText = text.TrimEnd(new char[] { splitChar });
                children = new List<Item>();

                string[] splitText = text.Split(splitChar);
                NumberOfTabs = splitText.Length - 1;
                LineNumber = LineNo;
            }

            #endregion Constructors

            #region Properties (9)

            public List<Item> Children
            {
                get { return children; }
                set { children = value; }
            }

            public int LineNumber
            {
                get { return lineNumber; }
                set { lineNumber = value; }
            }

            public int? LinkType
            {
                get { return linkType; }
                set { linkType = value; }
            }

            public string LinkTypeName
            {
                get { return linkTypeName; }
                set { linkTypeName = value; }
            }

            public MetaBase MBase
            {
                get { return mbase; }
                set { mbase = value; }
            }

            public int NumberOfTabs
            {
                get { return numberOfTabs; }
                set { numberOfTabs = value; }
            }

            public string OriginalText
            {
                get { return originalText; }
                set { originalText = value; }
            }

            public Item Parent
            {
                get { return parent; }
                set
                {
                    parent = value;
                }
            }

            public string WholeWord
            {
                get
                {
                    PrepareWord();
                    return wholeWord;
                }
                set
                {
                    wholeWord = value;
                }

            }

            #endregion Properties

            #region Methods (3)

            // Public Methods (2) 

            public void AddChild(Item line)
            {
                children.Add(line);
                line.Parent = this;
            }

            private void PrepareWord()
            {
                char splitChar = '\t';
                string[] splitText = originalText.Split(splitChar);
                this.WholeWord = GetWholeWord(splitText[splitText.Length - 1]).Trim();
            }

            // Private Methods (1) 

            private string GetWholeWord(string text)
            {
                if (this.Parent != null)
                {
                    string retval = "";
                    this.LinkType = null;

                    int result;
                    bool StartsWithNumber = int.TryParse(text.Substring(0, 1), out result);
                    bool HasPoint = text.IndexOf(". ") > -1;
                    //remove number
                    if (StartsWithNumber && HasPoint)
                    {
                        text = text.Substring(text.LastIndexOf(". ") + 2, text.Length - text.LastIndexOf(". ") - 2);
                    }
                    //get association and return text
                    if (text.StartsWith("["))
                    {
                        string rawAss = text.Substring(text.IndexOf("["), text.LastIndexOf("]") + 1);
                        string ass = rawAss.Replace("[", "").Replace("]", "").Trim().ToUpper();

                        if (ass.Length == 1)
                        {
                            switch (ass)
                            {
                                case "A":
                                    ass = "auxiliary";
                                    break;
                                case "C":
                                    ass = "classification";
                                    break;
                                case "D":
                                    ass = "decomposition";
                                    break;
                                case "M":
                                    ass = "mapping";
                                    break;
                                case "R":
                                    ass = "rationale";
                                    break;
                            }
                        }
                        else if (ass.Length == 3)
                        {
                            foreach (string t in Enum.GetNames(typeof(LinkAssociationType)))
                            {
                                if (t.ToUpper().StartsWith(ass))
                                {
                                    ass = t;
                                    break;
                                }
                            }
                        }

                        try
                        {
                            LinkAssociationType association = (LinkAssociationType)Enum.Parse(typeof(LinkAssociationType), ass, true);
                            LinkType = (int)association;
                            LinkTypeName = association.ToString();
                        }
                        catch
                        {
                            LinkTypeName = ass;
                        }
                        retval = text.Replace(rawAss, "");
                        return retval;

                        #region OLD
                        //if (text.Length > 4)
                        //{
                        //    string AllLinkIndicator = text.Substring(0, 6).Replace("[", "").Replace("]", "").Trim().ToLower(); //text
                        //    if (AllAssociationTypes == null) //This should be set so we dont call the db foreach line
                        //        AllAssociationTypes = DataAccessLayer.DataRepository.AssociationTypeProvider.GetAll();
                        //    //find the association whose name's first two letters = word in [XX]
                        //    foreach (BusinessLogic.AssociationType aType in AllAssociationTypes)
                        //    {
                        //        if (aType.Name.ToLower() == AllLinkIndicator)
                        //        {
                        //            LinkType = aType.pkid;
                        //            LinkTypeName = aType.Name.Trim();
                        //            break;
                        //        }
                        //    }
                        //    retval = text.Replace(text, "");
                        //}
                        //else
                        //{
                        //    string LinkTypeIndicator = text.Substring(0, 4);
                        //    switch (LinkTypeIndicator.ToUpper())
                        //    {
                        //        case "[A] ":
                        //            LinkType = 1;
                        //            LinkTypeName = "Auxiliary";
                        //            break;
                        //        case "[C] ":
                        //            LinkTypeName = "Classification";
                        //            LinkType = 2;
                        //            break;
                        //        case "[D] ":
                        //            LinkTypeName = "Decomposition";
                        //            LinkType = 3;
                        //            break;
                        //        case "[M] ":
                        //            LinkTypeName = "Mapping";
                        //            LinkType = 4;
                        //            break;
                        //        case "[R] ":
                        //            LinkTypeName = "Rationale";
                        //            LinkType = 4;
                        //            break;
                        //        default:
                        //            LinkType = null;
                        //            LinkTypeName = "";
                        //            break;
                        //    }
                        //    retval = text.Replace(LinkTypeIndicator, "");
                        //}
                        #endregion

                    }
                    else
                    {
                        retval = text;
                    }
                    return retval;
                }

                // this is a parent
                LinkType = null;
                return text;
            }

            #endregion Methods

        }
        #endregion Nested Classes

    }
}