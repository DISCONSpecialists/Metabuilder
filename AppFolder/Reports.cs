using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using x = MetaBuilder.BusinessFacade.Exports;
using System.Xml;
using Syncfusion.Windows.Forms.Grid;
using System.Data.Common;
using System.Data.SqlClient;

namespace MetaBuilder.UIControls.Common
{
    public partial class Reports : Form
    {

        #region Fields (7)

        private DataSet dsReport = new DataSet("Results");
        private bool styleApplied = false;
        private b.TList<b.Class> classesindb;
        private bool includeOrphans;
        private bool useFilter = false;
        string path = Application.StartupPath + "\\Reports.xml";
        //string path = "C:\\Reports.xml";
        private TreeNode rootNode;
        #endregion Fields

        #region Constructors (1)

        public Reports()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods (14)

        // Public Methods (1) 

        public void PrepareForm()
        {
            //Load workspace names
            getAllWorkspaces(comboBoxWorkspace);
            //Load classes
            //Basic
            getAllClasses(comboBoxObjectType);
            //General
            getAllClasses(comboBoxParentClass);
            getAllClasses(comboBoxChildClass);
            /* 
            //The Tab has been commented out in Reports.designer.cs
            //Advanced
            getAllClasses(comboBoxaParentClass);
            getAllClasses(comboBoxaChildClass);
            getAllClasses(comboBoxaChildChildClass);
            //Predefined Load
            */

            // Load XML in background
            // Threaded code is hard to debug, so comment the following three lines and uncomment the loadXML() one
            bgWorker.RunWorkerAsync();
            /*
            ThreadStart tsLoadXMLInBackground = new ThreadStart(loadXML);
            System.Threading.Thread thread = new Thread(tsLoadXMLInBackground);
            thread.Start();*/
            // loadXML(); // uncomment this line to enable easy debugging

            //Other
            //Testing adding of filter to predefined queries
            //MessageBox.Show(addWorkspaceFilter("select x,y from somewhere inner join GraphFile on x = y"));
            //MessageBox.Show(addWorkspaceFilter("select x,y from somewhere inner join GraphFile on x = y WHERE (x=1)"));
            //MessageBox.Show(addWorkspaceFilter("select x,y from somewhere inner join GraphFile on x = y ORDER BY x ascending"));
            //MessageBox.Show(addWorkspaceFilter("select x,y from somewhere inner join GraphFile on x = y WHERE (x=1) ORDER BY x ascending"));
            labelInfo.Visible = false;
            // buttonDataLoad.Visible = false;
        }

        // Private Methods (13) 

        private void bgWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            loadXML();
        }
        private void bgWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                // Add items to tree
                treeViewPredefined.Nodes.Clear();
                treeViewPredefined.Nodes.Add(rootNode);
                treeViewPredefined.ExpandAll();
            }
            catch
            {
            }
        }

        private void buttonDataLoad_Click(object sender, EventArgs e)
        {
            //Load Typed DataSet
            OpenFileDialog openFile1 = new OpenFileDialog();
            openFile1.Title = "Select a file to load";
            openFile1.DefaultExt = "*.csv";
            openFile1.Filter = "Comma Seperated Value File (*.csv)| *.csv | Excel Spreadsheet (*.xls)| *.xls";

            if (openFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK && openFile1.FileName.Length > 0)
            {
                try
                {
                    dsReport.Clear();
                    //ds.ReadXml(openFile1.FileName);

                    dsReport.Tables.Add(ImportExcel(openFile1.FileName));
                    //MessageBox.Show("imported");
                    dataSetTOdataGrid();
                    //MessageBox.Show("Loaded");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            openFile1.Dispose();
        }
        private void buttonDataSave_Click(object sender, EventArgs e)
        {
            if (gridDataBoundGridMain.Model.RowCount <= 0)
                return;

            SaveFileDialog saveFile1 = new SaveFileDialog();
            saveFile1.Title = "Save the data in the grid";
            saveFile1.DefaultExt = "*.csv";
            saveFile1.Filter = "Comma Seperated Value File (*.csv)| *.csv | Excel Spreadsheet (*.xls)| *.xls";

            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFile1.FileName.Length > 0)
            {
                try
                {
                    ReportsExport x = new ReportsExport("Win");
                    switch (saveFile1.FileName.Substring(saveFile1.FileName.Length - 3))
                    {
                        case "csv":
                            {
                                x.ExportDetails(dsReport.Tables[0], ReportsExport.ExportFormat.CSV, saveFile1.FileName);
                                //ds.WriteXml(saveFile1.FileName.Substring(saveFile1.FileName.Length - 3) + "xml");

                                MessageBox.Show("Saved to " + saveFile1.FileName, "File Saved");
                                break;
                            }
                        case "xls":
                            {
                                x.ExportDetails(dsReport.Tables[0], ReportsExport.ExportFormat.Excel, saveFile1.FileName);
                                //ds.WriteXml(saveFile1.FileName.Substring(saveFile1.FileName.Length - 3) + "xml");

                                MessageBox.Show("Saved to " + saveFile1.FileName, "File Saved");
                                break;
                            }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            saveFile1.Dispose();
        }
        private string GetText(string col, int row, x.ExcelUtil util)
        {
            return (string)(string)util.CurrentSheet.get_Range(col + row.ToString(), col + row.ToString()).Text;
        }
        private DataTable ImportExcel(string strFilePath)
        {
            DataTable dt = new DataTable();
            //progressBarMain.Value = 0;
            x.ExcelUtil util = new x.ExcelUtil();
            util.OpenExcel();

            int rowFetcher;
            try
            {
                util.OpenFile(strFilePath); //edit

                //List<string> errenousimportrows = new List<string>();

                string[] alphabet =
                    new string[]
                        {
                            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r",
                            "s",
                            "t", "u", "v", "w", "x", "y", "z", "aa", "ab", "ac", "ad", "ae", "af", "ag"
                        };
                int alphabetCounter = 0;
                int rowCounter = 1;

                string txt = GetText(alphabet[alphabetCounter], rowCounter, util);
                while (!string.IsNullOrEmpty(txt) || (alphabetCounter == 0))
                {
                    //cols.Add(txt);
                    dt.Columns.Add(txt);
                    alphabetCounter++;
                    txt = GetText(alphabet[alphabetCounter], rowCounter, util);
                }

                int NumberOfColumns = alphabetCounter;
                rowCounter++;
                //alphabetCounter = 0;

                int rc = 0; //Minus 1 for header row
                for (alphabetCounter = 0; alphabetCounter < NumberOfColumns; alphabetCounter++)
                {
                    if (string.IsNullOrEmpty(GetText(alphabet[alphabetCounter], 2, util))) // && alphabetCounter == 0)
                    {
                        rc = util.CurrentSheet.UsedRange.Rows.Count - 2; //Blank row
                    }
                    else
                    {
                        rc = util.CurrentSheet.UsedRange.Rows.Count - 1; //No Blank row
                    }
                }
                //errenousimportrows.Clear();
                //progressBarMain.Maximum = rc;
                Cursor = Cursors.WaitCursor;
                if (rc > 500)
                {
                    if (MessageBox.Show("There are more than 500 rows which will be imported. This could take a few minutes to a few hours depending on the number of rows and the speed of your computer. Would you like to continue", "Large Excel File - " + rc.ToString() + " rows detected", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return dt;
                    }
                }

                //Each row
                for (rowCounter = 2; rowCounter <= rc; rowCounter++)
                // for (rowCounter = 2; rowCounter <= rc; rowCounter++)
                {
                    rowFetcher = rowCounter;
                    //NewRow
                    System.Data.DataRow dr = dt.NewRow();
                    //Each Column
                    for (alphabetCounter = 0; alphabetCounter < NumberOfColumns; alphabetCounter++)
                    {
                        //if (string.IsNullOrEmpty(GetText(alphabet[alphabetCounter], rowFetcher, util)) && alphabetCounter == 0) //End of rows?
                        //{
                        //rowCounter = rc; //Next round will kill loop but since first row column is empty this may skip rows
                        //}
                        try
                        {
                            dr[alphabetCounter] = (GetText(alphabet[alphabetCounter], rowFetcher, util));
                        }
                        catch (Exception ex)
                        {
                            //errenousimportrows.Add(rowFetcher.ToString()); //rowfetcher for row number that had problem
                        }
                        //Next Column
                    }
                    //Add Row
                    dt.Rows.Add(dr);
                    //Next Row
                    //progressBarMain.Increment(progressBarMain.Step);
                }

                util.CloseExcel();
                //progressBarMain.Value = progressBarMain.Maximum;
                Cursor = Cursors.Default;
                return dt;
            }
            catch (ObjectDisposedException x)
            {
                MessageBox.Show(x.ToString());
                return dt;
            }
            catch (Exception importException)
            {
                MessageBox.Show(importException.ToString());
                return dt;
            }
        }

        private void comboBoxWorkspace_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int id = 0;

            //b.TList<b.Workspace> w = d.DataRepository.WorkspaceProvider.GetAll();
            //foreach (b.Workspace wi in w)
            //{
            //    if (wi.Name == comboBoxWorkspace.Text)
            //    {
            //        id = wi.WorkspaceTypeId;
            //        break;
            //    }
            //}
            //comboBoxWorkspace.Tag = id.ToString();
        }
        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            this.gridDataBoundGridMain.Model.CutPaste.Copy();
        }
        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            this.gridDataBoundGridMain.Model.CutPaste.Cut();
        }
        private void toolStripButtonSelectAll_Click(object sender, EventArgs e)
        {
            gridDataBoundGridMain.Selections.Clear();
            gridDataBoundGridMain.Selections.Add(GridRangeInfo.Table());
        }
        private void toolStripButtonResetFilter_Click(object sender, EventArgs e)
        {
            //gridDataBoundGridMain.RemoveFilter();
        }
        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            if (gridDataBoundGridMain != null)
            {
                try
                {
                    GridPrintDocument pd = new GridPrintDocument(gridDataBoundGridMain, true);
                    PrintDialog p = new PrintDialog();
                    p.Document = pd;
                    p.UseEXDialog = true;
                    if (DialogResult.OK == p.ShowDialog())
                    {
                        pd.DocumentName = "Test Page Print";
                        pd.Print();
                    }
                    //pd.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred attempting to preview the file to print - " + ex.Message);
                }
            }
        }

        private void dataSetTOdataGrid()
        {

            GridFilterBar filterBar = new GridFilterBar();

            if (dsReport.Tables[0].Rows.Count > 0)
            {
                //Clears grid
                gridDataBoundGridMain.DataSource = null;
                gridDataBoundGridMain.DataMember = null;

                //v.ResetProperties();

                //Sets grid
                gridDataBoundGridMain.DataSource = dsReport;
                gridDataBoundGridMain.DataMember = dsReport.Tables[0].TableName;

                //Removes None and Custom strings
                filterBar.GridFilterBarStrings[0] = "";
                filterBar.GridFilterBarStrings[1] = "";
                //Grid Filter
                filterBar.WireGrid(gridDataBoundGridMain);

                //Grid Settings
                gridDataBoundGridMain.EnableEdit = false;
                gridDataBoundGridMain.ThemesEnabled = false;
                gridDataBoundGridMain.Properties.Buttons3D = false;
                gridDataBoundGridMain.Properties.RowHeaders = false; //Hides row headers
                gridDataBoundGridMain.AllowResizeToFit = true;
                gridDataBoundGridMain.AllowDragSelectedCols = false;

                gridDataBoundGridMain.Model.ColWidths.ResizeToFit(GridRangeInfo.Cols(1, dsReport.Tables[0].Columns.Count));
                gridDataBoundGridMain.Model.RowHeights.ResizeToFit(GridRangeInfo.Rows(1, dsReport.Tables[0].Rows.Count));
                if (!styleApplied)
                {
                    applyStyle();
                }
                gridDataBoundGridMain.AllowDrop = true;
                //Refresh Grid
                gridDataBoundGridMain.Refresh();

                lblNoResults.Visible = false;
                gridDataBoundGridMain.Visible = true;
            }
            else
            {
                lblNoResults.Visible = true;
                gridDataBoundGridMain.Visible = false;
                //Clears grid
                gridDataBoundGridMain.DataSource = null;
                gridDataBoundGridMain.DataMember = null;
            }
        }
        private void applyStyle()
        {
            gridDataBoundGridMain.ResetProperties();

            //Grid Settings
            gridDataBoundGridMain.EnableEdit = false;
            gridDataBoundGridMain.ThemesEnabled = false;
            gridDataBoundGridMain.Properties.Buttons3D = false;
            gridDataBoundGridMain.Properties.RowHeaders = false; //Hides row headers
            gridDataBoundGridMain.AllowResizeToFit = true;
            gridDataBoundGridMain.AllowDragSelectedCols = false;
            gridDataBoundGridMain.AllowDrop = true;


            // http://www.syncfusion.com/support/forums/message.aspx?MessageID=53966
            // http://www.syncfusion.com/Support/article.aspx?id=560

            styleApplied = true;
        }

        #endregion Methods

        #region load

        private void getAllWorkspaces(ComboBox cb)
        {
            cb.Items.Clear();

            cb.Items.Add("All");

            b.TList<b.Workspace> ws = d.DataRepository.WorkspaceProvider.GetAll();
            foreach (b.Workspace wsItem in ws)
            {
                if (cb.Items.Contains(wsItem.Name))
                {
                }
                else
                {
                    cb.Items.Add(wsItem.Name);
                }
            }

            cb.SelectedIndex = 0;
        }
        private void getAllClasses(ComboBox cb)
        {

            cb.Items.Clear();

            if (classesindb == null)
                classesindb = d.DataRepository.ClassProvider.GetAll();
            foreach (b.Class classItem in classesindb)
            {
                cb.Items.Add(classItem.Name);
            }

            cb.SelectedIndex = 0;
        }

        #region Load Xml

        private void loadXML()
        {
            try
            {
                // SECTION 1. Create a DOM Document and load the XML data into it.
                XmlDocument dom = new XmlDocument();
                dom.Load(path);

                // SECTION 2. Initialize the TreeView control.
                treeViewPredefined.Nodes.Clear();
                rootNode = new TreeNode("Pre-Defined Reports");//dom.DocumentElement.Name));


                // SECTION 3. Populate the TreeView with the DOM nodes.
                AddNode(dom.DocumentElement, rootNode);

            }
            catch (XmlException xmlEx)
            {
                MessageBox.Show(xmlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void AddNode(XmlNode inXmlNode, TreeNode inTreeNode)
        {
            XmlNode xNode;
            TreeNode tNode;
            XmlNodeList nodeList;
            int i;

            // Loop through the XML nodes until the leaf is reached.
            // Add the nodes to the TreeView during the looping process.
            if (inXmlNode.HasChildNodes)
            {
                nodeList = inXmlNode.ChildNodes;
                for (i = 0; i <= nodeList.Count - 1; i++)
                {
                    xNode = inXmlNode.ChildNodes[i];
                    inTreeNode.Nodes.Add(new TreeNode(xNode.Attributes["Title"].Value));//.Name));
                    tNode = inTreeNode.Nodes[i];
                    AddNode(xNode, tNode);
                }
            }
            else
            {
                //// Here you need to pull the data from the XmlNode based on the
                //// type of node, whether attribute values are required, and so forth.
                try
                {
                    if (inXmlNode.Attributes[0].Value.Trim() != "")
                        inTreeNode.Text = inXmlNode.Attributes["Title"].Value.Trim();
                    else
                        inTreeNode.Text = "None";
                }
                catch (Exception ex)
                {
                    inTreeNode.Text = "None";
                }
            }
        }

        #endregion //Adapted from Microsofts website

        #endregion

        #region go

        private void buttonBasicGo_Click(object sender, EventArgs e)
        {
            go("Basic");
        }
        private void buttonGeneralGo_Click(object sender, EventArgs e)
        {
            go("General");
        }
        private void buttonAdvancedGo_Click(object sender, EventArgs e)
        {
            go("Advanced");
        }
        private void buttonPredefinedGo_Click(object sender, EventArgs e)
        {
            go("Predefined");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            go("custom");
        }

        private string addWorkspaceFilter(string query)
        {
            if (query.Contains("GraphFile"))
            {
                if (query.Contains(" WHERE "))
                {
                    if (query.Contains(" ORDER BY "))
                    {
                        //Insert before "o" order by - 1 to be in white space... and white spaces before and after
                        int oPosition = query.IndexOf("ORDER BY");
                        oPosition -= 1; //if its the begginned @ o else must be - 9
                        query =
                            query.Insert(oPosition, " AND (GraphFile.WorkspaceName = '" + comboBoxWorkspace.Text + "') ");
                    }
                    else
                    {
                        query = query + " AND (GraphFile.WorkspaceName = '" + comboBoxWorkspace.Text + "')";
                    }
                }
                else
                {
                    if (query.Contains(" ORDER BY "))
                    {
                        //Insert before "o" order by - 1 to be in white space... and white spaces before and after
                        int oPosition = query.IndexOf("ORDER BY");
                        oPosition -= 1; //if its the begginned @ o else must be - 9
                        query =
                            query.Insert(oPosition,
                                         " WHERE (GraphFile.WorkspaceName = '" + comboBoxWorkspace.Text + "') ");
                    }
                    else
                    {
                        query = query + " WHERE (GraphFile.WorkspaceName = '" + comboBoxWorkspace.Text + "')";
                    }
                }
            }
            else
            {
                //This will not be easy
            }
            return query;
        }
        private void go(string buttonName)
        {
            dsReport.Clear();

            if (comboBoxWorkspace.Text != "All")
            {
                useFilter = true;
            }
            else
            {
                useFilter = false;
            }

            string query;

            if (buttonName != null)
                switch (buttonName)
                {
                    case "Basic":
                        {
                            retrieveObjects(comboBoxObjectType.Text);
                            break;
                        }
                    case "General":
                        {
                            #region Prepare

                            string primaryClass,
                                   secondaryClass,
                                   association,
                                   primaryClassCorrelation,
                                   secondaryClassCorrelation;
                            int associationID;

                            primaryClass = comboBoxParentClass.Text;
                            secondaryClass = comboBoxChildClass.Text;
                            if (primaryClass == "" || secondaryClass == "")
                            {
                                MessageBox.Show(
                                    "Both a parent and a child class a required to be selected in order to continue",
                                    "Classes not complete");
                                break;
                            }

                            association = comboBoxAssociation.Text;
                            if (association == "")
                            {
                                MessageBox.Show("An association has not been selected", "Invalid Association");
                                break;
                            }
                            associationID = d.DataRepository.AssociationTypeProvider.GetByName(association).pkid; //CAid

                            if (primaryClass == secondaryClass)
                            {
                                primaryClassCorrelation = "_1";
                                //secondaryClassCorrelation = "_2"; //For Third Class?
                            }
                            else
                            {
                                primaryClassCorrelation = string.Empty;
                                //secondaryClassCorrelation = string.Empty; //For Third Class?
                            }

                            includeOrphans = checkBoxIncludeOrphans.Checked;

                            #endregion

                            #region Query

                            #region Orphans Excluded : on graphfiles

                            //SELECT
                            if ((primaryClass == "Condition" && secondaryClass == "Condition") ||
                                (primaryClass == "ConditionalDescription" && secondaryClass == "ConditionalDescription") ||
                                (primaryClass == "ConnectionSpeed" && secondaryClass == "ConnectionSpeed") ||
                                (primaryClass == "ConnectionType" && secondaryClass == "ConnectionType") ||
                                (primaryClass == "ProbOfRealization" && secondaryClass == "ProbOfRealization") ||
                                (primaryClass == "Rationale" && secondaryClass == "Rationale") ||
                                (primaryClass == "SelectorAttribute" && secondaryClass == "SelectorAttribute") ||
                                (primaryClass == "TimeIndicator" && secondaryClass == "TimeIndicator")
                                )
                            {
                                query = "SELECT dbo.TrimFileName(dbo.GraphFile.Name) AS [File Name], METAView_" +
                                        primaryClass +
                                        "_Listing.Value AS [" + primaryClass + "],METAView_" + secondaryClass +
                                        "_Listing" +
                                        primaryClassCorrelation + ".Value AS [" + secondaryClass +
                                        primaryClassCorrelation +
                                        "], AssociationType.Name ";
                            }
                            else if (primaryClass == "Condition" || primaryClass == "ConditionalDescription" ||
                                     primaryClass == "ConnectionSpeed" || primaryClass == "ConnectionType" ||
                                     primaryClass == "ProbOfRealization" || primaryClass == "Rationale" ||
                                     primaryClass == "SelectorAttribute" || primaryClass == "TimeIndicator")
                            {
                                query = "SELECT dbo.TrimFileName(dbo.GraphFile.Name) AS [File Name], METAView_" +
                                        primaryClass +
                                        "_Listing.Value AS [" + primaryClass + "],METAView_" + secondaryClass +
                                        "_Listing" +
                                        primaryClassCorrelation + ".Name AS [" + secondaryClass +
                                        primaryClassCorrelation +
                                        "], AssociationType.Name ";
                            }
                            else if (secondaryClass == "Condition" || secondaryClass == "ConditionalDescription" ||
                                     secondaryClass == "ConnectionSpeed" || secondaryClass == "ConnectionType" ||
                                     secondaryClass == "ProbOfRealization" || secondaryClass == "Rationale" ||
                                     secondaryClass == "SelectorAttribute" || secondaryClass == "TimeIndicator")
                            {
                                query =
                                    "SELECT dbo.TrimFileName(dbo.GraphFile.Name) AS [File Name], METAView_" +
                                    primaryClass +
                                    "_Listing.Name AS [" + primaryClass + "],METAView_" + secondaryClass +
                                    "_Listing" +
                                    primaryClassCorrelation + ".Value AS [" + secondaryClass +
                                    primaryClassCorrelation +
                                    "], AssociationType.Name ";
                            }
							//CSF!?
							else if (primaryClass == "CSF" && secondaryClass == "CSF")
                            {
                                query = "SELECT dbo.TrimFileName(dbo.GraphFile.Name) AS [File Name], METAView_" +
                                        primaryClass +
                                        "_Listing.Number AS [" + primaryClass + "],METAView_" + secondaryClass +
                                        "_Listing" +
                                        primaryClassCorrelation + ".Value AS [" + secondaryClass +
                                        primaryClassCorrelation +
                                        "], AssociationType.Name ";
                            }
                            else if (primaryClass == "CSF")
                            {
                                query = "SELECT dbo.TrimFileName(dbo.GraphFile.Name) AS [File Name], METAView_" +
                                        primaryClass +
                                        "_Listing.Number AS [" + primaryClass + "],METAView_" + secondaryClass +
                                        "_Listing" +
                                        primaryClassCorrelation + ".Name AS [" + secondaryClass +
                                        primaryClassCorrelation +
                                        "], AssociationType.Name ";
                            }
                            else if (secondaryClass == "CSF")
                            {
                                query =
                                    "SELECT dbo.TrimFileName(dbo.GraphFile.Name) AS [File Name], METAView_" +
                                    primaryClass +
                                    "_Listing.Name AS [" + primaryClass + "],METAView_" + secondaryClass +
                                    "_Listing" +
                                    primaryClassCorrelation + ".Number AS [" + secondaryClass +
                                    primaryClassCorrelation +
                                    "], AssociationType.Name ";
                            }
                            else
                            {
                                query =
                                    "SELECT dbo.TrimFileName(dbo.GraphFile.Name) AS [File Name], METAView_" +
                                    primaryClass +
                                    "_Listing.Name AS [" + primaryClass + "],METAView_" + secondaryClass +
                                    "_Listing" +
                                    primaryClassCorrelation + ".Name AS [" + secondaryClass +
                                    primaryClassCorrelation +
                                    "], AssociationType.Name ";
                            }

                            //query = "SELECT dbo.TrimFileName(dbo.GraphFile.Name) AS [File Name], METAView_" +
                            //        primaryClass +
                            //        "_Listing.*,METAView_" + secondaryClass + "_Listing" +
                            //        primaryClassCorrelation + ".* ";

                            //FROM
                            query = query +
                                    "FROM GraphFile INNER JOIN GraphFileAssociation ON GraphFile.pkid = GraphFileAssociation.GraphFileID AND GraphFile.Machine = GraphFileAssociation.GraphFileMachine INNER JOIN ObjectAssociation ON GraphFileAssociation.CAid = ObjectAssociation.CAid AND GraphFileAssociation.ObjectID = ObjectAssociation.ObjectID AND  GraphFileAssociation.ChildObjectID = ObjectAssociation.ChildObjectID AND GraphFileAssociation.ObjectMachine = ObjectAssociation.ObjectMachine AND  GraphFileAssociation.ChildObjectMachine = ObjectAssociation.ChildObjectMachine INNER JOIN ClassAssociation ON ObjectAssociation.CAid = ClassAssociation.CAid INNER JOIN AssociationType ON ClassAssociation.AssociationTypeId = AssociationType.pkid ";

                            query = query + " INNER JOIN METAView_" + primaryClass +
                                    "_Listing ON ObjectAssociation.ObjectID = METAView_" + primaryClass +
                                    "_Listing.pkid AND ObjectAssociation.ObjectMachine = METAView_" + primaryClass +
                                    "_Listing.Machine INNER JOIN ObjectAssociation AS ObjectAssociation_1 ON METAView_" +
                                    primaryClass + "_Listing.pkid = ObjectAssociation_1.ObjectID AND METAView_" +
                                    primaryClass +
                                    "_Listing.Machine = ObjectAssociation_1.ObjectMachine INNER JOIN METAView_" +
                                    secondaryClass + "_Listing AS METAView_" +
                                    secondaryClass + "_Listing" + primaryClassCorrelation +
                                    " ON ObjectAssociation_1.ChildObjectID = METAView_" +
                                    secondaryClass + "_Listing" + primaryClassCorrelation +
                                    ".pkid AND ObjectAssociation_1.ChildObjectMachine = METAView_" +
                                    secondaryClass + "_Listing" + primaryClassCorrelation + ".Machine";


                            if ((primaryClass == "Condition" && secondaryClass == "Condition") ||
                                (primaryClass == "ConditionalDescription" && secondaryClass == "ConditionalDescription") ||
                                (primaryClass == "ConnectionSpeed" && secondaryClass == "ConnectionSpeed") ||
                                (primaryClass == "ConnectionType" && secondaryClass == "ConnectionType") ||
                                (primaryClass == "ProbOfRealization" && secondaryClass == "ProbOfRealization") ||
                                (primaryClass == "Rationale" && secondaryClass == "Rationale") ||
                                (primaryClass == "SelectorAttribute" && secondaryClass == "SelectorAttribute") ||
                                (primaryClass == "TimeIndicator" && secondaryClass == "TimeIndicator")
                                )
                            {
                                query = query + " WHERE (METAView_" + primaryClass +
                                        "_Listing.Value is not NULL) AND (METAView_" + secondaryClass +
                                        "_Listing" + primaryClassCorrelation +
                                        ".Value is not NULL) AND (ClassAssociation.AssociationTypeID = " +
                                        associationID + ") AND GraphFile.IsActive = 1  ";
                            }
                            else if (primaryClass == "Condition" || primaryClass == "ConditionalDescription" ||
                                     primaryClass == "ConnectionSpeed" || primaryClass == "ConnectionType" ||
                                     primaryClass == "ProbOfRealization" || primaryClass == "Rationale" ||
                                     primaryClass == "SelectorAttribute" || primaryClass == "TimeIndicator")
                            {
                                query = query + " WHERE (METAView_" + primaryClass +
                                        "_Listing.Value is not NULL) AND (METAView_" + secondaryClass +
                                        "_Listing" + primaryClassCorrelation +
                                        ".Name is not NULL) AND (ClassAssociation.AssociationTypeID = " +
                                        associationID + ") AND GraphFile.IsActive = 1 ";
                            }
                            else if (secondaryClass == "Condition" || secondaryClass == "ConditionalDescription" ||
                                     secondaryClass == "ConnectionSpeed" || secondaryClass == "ConnectionType" ||
                                     secondaryClass == "ProbOfRealization" || secondaryClass == "Rationale" ||
                                     secondaryClass == "SelectorAttribute" || secondaryClass == "TimeIndicator")
                            {
                                query = query + " WHERE (METAView_" + primaryClass +
                                        "_Listing.Name is not NULL) AND (METAView_" + secondaryClass +
                                        "_Listing" + primaryClassCorrelation +
                                        ".Value is not NULL) AND (ClassAssociation.AssociationTypeID = " +
                                        associationID + ") AND GraphFile.IsActive = 1 ";
                            }
							//CSF!?
							else if (primaryClass == "CSF" && secondaryClass == "CSF")
                            {
                                query = query + " WHERE (METAView_" + primaryClass +
                                        "_Listing.Number is not NULL) AND (METAView_" + secondaryClass +
                                        "_Listing" + primaryClassCorrelation +
                                        ".Number is not NULL) AND (ClassAssociation.AssociationTypeID = " +
                                        associationID + ") AND GraphFile.IsActive = 1 ";
                            }
                            else if (primaryClass == "CSF")
                            {
                               query = query + " WHERE (METAView_" + primaryClass +
                                        "_Listing.Number is not NULL) AND (METAView_" + secondaryClass +
                                        "_Listing" + primaryClassCorrelation +
                                        ".Name is not NULL) AND (ClassAssociation.AssociationTypeID = " +
                                        associationID + ") AND GraphFile.IsActive = 1 ";
                            }
                            else if (secondaryClass == "CSF")
                            {
                                query = query + " WHERE (METAView_" + primaryClass +
                                        "_Listing.Name is not NULL) AND (METAView_" + secondaryClass +
                                        "_Listing" + primaryClassCorrelation +
                                        ".Number is not NULL) AND (ClassAssociation.AssociationTypeID = " +
                                        associationID + ") AND GraphFile.IsActive = 1 ";
                            }
                            else
                            {
                                query = query + " WHERE (METAView_" + primaryClass +
                                        "_Listing.Name is not NULL) AND (METAView_" + secondaryClass +
                                        "_Listing" + primaryClassCorrelation +
                                        ".Name is not NULL) AND (ClassAssociation.AssociationTypeID = " +
                                        associationID + ") AND GraphFile.IsActive = 1 ";
                            }

                            if (useFilter)
                            {
                                query = query + " AND (GraphFile.WorkspaceName = '" + comboBoxWorkspace.Text + "') ";
                            }

                            #endregion

                            #region Orphans Included : not on graphfiles

                            //this will get all objects, even those not in graphfiles
                            if (includeOrphans)
                            {
                                //SELECT
                                if ((primaryClass == "Condition" && secondaryClass == "Condition") ||
                                    (primaryClass == "ConditionalDescription" &&
                                     secondaryClass == "ConditionalDescription") ||
                                    (primaryClass == "ConnectionSpeed" && secondaryClass == "ConnectionSpeed") ||
                                    (primaryClass == "ConnectionType" && secondaryClass == "ConnectionType") ||
                                    (primaryClass == "ProbOfRealization" && secondaryClass == "ProbOfRealization") ||
                                    (primaryClass == "Rationale" && secondaryClass == "Rationale") ||
                                    (primaryClass == "SelectorAttribute" && secondaryClass == "SelectorAttribute") ||
                                    (primaryClass == "TimeIndicator" && secondaryClass == "TimeIndicator")
                                    )
                                {
                                    query = "SELECT METAView_" +
                                            primaryClass +
                                            "_Listing.Value AS [" + primaryClass + "],METAView_" + secondaryClass +
                                            "_Listing" +
                                            primaryClassCorrelation + ".Value AS [" + secondaryClass +
                                            primaryClassCorrelation +
                                            "], AssociationType.Name ";
                                }
                                else if (primaryClass == "Condition" || primaryClass == "ConditionalDescription" ||
                                         primaryClass == "ConnectionSpeed" || primaryClass == "ConnectionType" ||
                                         primaryClass == "ProbOfRealization" || primaryClass == "Rationale" ||
                                         primaryClass == "SelectorAttribute" || primaryClass == "TimeIndicator")
                                {
                                    query = "SELECT METAView_" +
                                            primaryClass +
                                            "_Listing.Value AS [" + primaryClass + "],METAView_" + secondaryClass +
                                            "_Listing" +
                                            primaryClassCorrelation + ".Name AS [" + secondaryClass +
                                            primaryClassCorrelation +
                                            "], AssociationType.Name ";
                                }
                                else if (secondaryClass == "Condition" || secondaryClass == "ConditionalDescription" ||
                                         secondaryClass == "ConnectionSpeed" || secondaryClass == "ConnectionType" ||
                                         secondaryClass == "ProbOfRealization" || secondaryClass == "Rationale" ||
                                         secondaryClass == "SelectorAttribute" || secondaryClass == "TimeIndicator")
                                {
                                    query = "SELECT METAView_" +
                                            primaryClass +
                                            "_Listing.Name AS [" + primaryClass + "],METAView_" + secondaryClass +
                                            "_Listing" +
                                            primaryClassCorrelation + ".Value AS [" + secondaryClass +
                                            primaryClassCorrelation +
                                            "], AssociationType.Name ";
                                }
								//CSF
								else if (primaryClass == "CSF" && secondaryClass == "CSF")
                                {
                                    query = "SELECT METAView_" +
                                            primaryClass +
                                            "_Listing.Number AS [" + primaryClass + "],METAView_" + secondaryClass +
                                            "_Listing" +
                                            primaryClassCorrelation + ".Number AS [" + secondaryClass +
                                            primaryClassCorrelation +
                                            "], AssociationType.Name ";
                                }
                                else if (primaryClass == "CSF")
                                {
                                    query = "SELECT METAView_" +
                                            primaryClass +
                                            "_Listing.Number AS [" + primaryClass + "],METAView_" + secondaryClass +
                                            "_Listing" +
                                            primaryClassCorrelation + ".Name AS [" + secondaryClass +
                                            primaryClassCorrelation +
                                            "], AssociationType.Name ";
                                }
                                else if (secondaryClass == "CSF")
                                {
                                    query = "SELECT METAView_" +
                                            primaryClass +
                                            "_Listing.Name AS [" + primaryClass + "],METAView_" + secondaryClass +
                                            "_Listing" +
                                            primaryClassCorrelation + ".Number AS [" + secondaryClass +
                                            primaryClassCorrelation +
                                            "], AssociationType.Name ";
                                }
                                else
                                {
                                    query = "SELECT METAView_" +
                                            primaryClass +
                                            "_Listing.Name AS [" + primaryClass + "],METAView_" + secondaryClass +
                                            "_Listing" +
                                            primaryClassCorrelation + ".Name AS [" + secondaryClass +
                                            primaryClassCorrelation +
                                            "], AssociationType.Name ";
                                }

                                //query =
                                //    "SELECT     METAView_" + primaryClass + "_Listing.*, METAView_" + secondaryClass +
                                //    "_Listing" + primaryClassCorrelation +
                                //    ".* ";

                                query = query + " FROM         METAView_" + primaryClass +
                                        "_Listing INNER JOIN ObjectAssociation AS ObjectAssociation ON METAView_" +
                                        primaryClass + "_Listing.pkid = ObjectAssociation.ObjectID AND  METAView_" +
                                        primaryClass +
                                        "_Listing.Machine = ObjectAssociation.ObjectMachine INNER JOIN METAView_" +
                                        secondaryClass +
                                        "_Listing AS METAView_" + secondaryClass +
                                        "_Listing" + primaryClassCorrelation +
                                        " ON ObjectAssociation.ChildObjectID = METAView_" + secondaryClass +
                                        "_Listing" + primaryClassCorrelation +
                                        ".pkid AND  ObjectAssociation.ChildObjectMachine = METAView_" +
                                        secondaryClass + "_Listing" + primaryClassCorrelation +
                                        ".Machine INNER JOIN ClassAssociation ON ObjectAssociation.CAid = ClassAssociation.CAid  INNER JOIN AssociationType ON ClassAssociation.AssociationTypeId = AssociationType.pkid ";

                                if ((primaryClass == "Condition" && secondaryClass == "Condition") ||
                                    (primaryClass == "ConditionalDescription" &&
                                     secondaryClass == "ConditionalDescription") ||
                                    (primaryClass == "ConnectionSpeed" && secondaryClass == "ConnectionSpeed") ||
                                    (primaryClass == "ConnectionType" && secondaryClass == "ConnectionType") ||
                                    (primaryClass == "ProbOfRealization" && secondaryClass == "ProbOfRealization") ||
                                    (primaryClass == "Rationale" && secondaryClass == "Rationale") ||
                                    (primaryClass == "SelectorAttribute" && secondaryClass == "SelectorAttribute") ||
                                    (primaryClass == "TimeIndicator" && secondaryClass == "TimeIndicator")
                                    )
                                {
                                    query = query + " WHERE (METAView_" + primaryClass +
                                            "_Listing.Value is not NULL) AND (METAView_" + secondaryClass +
                                            "_Listing" + primaryClassCorrelation +
                                            ".Value is not NULL) AND (ClassAssociation.AssociationTypeID = " +
                                            associationID + ") ";
                                }
                                else if (primaryClass == "Condition" || primaryClass == "ConditionalDescription" ||
                                         primaryClass == "ConnectionSpeed" || primaryClass == "ConnectionType" ||
                                         primaryClass == "ProbOfRealization" || primaryClass == "Rationale" ||
                                         primaryClass == "SelectorAttribute" || primaryClass == "TimeIndicator")
                                {
                                    query = query + " WHERE (METAView_" + primaryClass +
                                            "_Listing.Value is not NULL) AND (METAView_" + secondaryClass +
                                            "_Listing" + primaryClassCorrelation +
                                            ".Name is not NULL) AND (ClassAssociation.AssociationTypeID = " +
                                            associationID + ") ";
                                }
                                else if (secondaryClass == "Condition" || secondaryClass == "ConditionalDescription" ||
                                         secondaryClass == "ConnectionSpeed" || secondaryClass == "ConnectionType" ||
                                         secondaryClass == "ProbOfRealization" || secondaryClass == "Rationale" ||
                                         secondaryClass == "SelectorAttribute" || secondaryClass == "TimeIndicator")
                                {
                                    query = query + " WHERE (METAView_" + primaryClass +
                                            "_Listing.Name is not NULL) AND (METAView_" + secondaryClass +
                                            "_Listing" + primaryClassCorrelation +
                                            ".Value is not NULL) AND (ClassAssociation.AssociationTypeID = " +
                                            associationID + ") ";
                                }
								//CSF
								else if (primaryClass == "CSF" && secondaryClass == "CSF")
                                {
                                    query = query + " WHERE (METAView_" + primaryClass +
                                            "_Listing.Number is not NULL) AND (METAView_" + secondaryClass +
                                            "_Listing" + primaryClassCorrelation +
                                            ".Number is not NULL) AND (ClassAssociation.AssociationTypeID = " +
                                            associationID + ") ";
                                }
                                else if (primaryClass == "CSF")
                                {
                                    query = query + " WHERE (METAView_" + primaryClass +
                                            "_Listing.Number is not NULL) AND (METAView_" + secondaryClass +
                                            "_Listing" + primaryClassCorrelation +
                                            ".Name is not NULL) AND (ClassAssociation.AssociationTypeID = " +
                                            associationID + ") ";
                                }
                                else if (secondaryClass == "CSF")
                                {
                                    query = query + " WHERE (METAView_" + primaryClass +
                                            "_Listing.Name is not NULL) AND (METAView_" + secondaryClass +
                                            "_Listing" + primaryClassCorrelation +
                                            ".Number is not NULL) AND (ClassAssociation.AssociationTypeID = " +
                                            associationID + ") ";
                                }								
                                else
                                {
                                    query = query + " WHERE (METAView_" + primaryClass +
                                            "_Listing.Name is not NULL) AND (METAView_" + secondaryClass +
                                            "_Listing" + primaryClassCorrelation +
                                            ".Name is not NULL) AND (ClassAssociation.AssociationTypeID = " +
                                            associationID + ") ";
                                }

                                if (useFilter)
                                {
                                    query = query + " AND (METAView_" + primaryClass + "_Listing.WorkspaceName = '" +
                                            comboBoxWorkspace.Text + "')";
                                }
                            }

                            #endregion

                            #endregion

                            try
                            {
                                dsReport = d.DataRepository.Provider.ExecuteDataSet(CommandType.Text, query);
                                labelInfo.Text = dsReport.Tables[0].Rows.Count.ToString() + Environment.NewLine +
                                                 System.DateTime.Now.TimeOfDay.ToString();
                                dataSetTOdataGrid();
                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show(ex.ToString());
                                MessageBox.Show("The report failed to run");
                                LogEntry lentry = new LogEntry();
                                lentry.Message = ex.ToString();
                                lentry.Title = "Report Fail";
                                Logger.Write(lentry);
                            }

                            break;
                        }
                    case "custom":
                        try
                        {
                            dsReport = d.DataRepository.Provider.ExecuteDataSet(CommandType.Text, txtCustom.Text);
                            labelInfo.Text = dsReport.Tables[0].Rows.Count.ToString() + Environment.NewLine +
                                             System.DateTime.Now.TimeOfDay.ToString();
                            dataSetTOdataGrid();
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.ToString());
                            MessageBox.Show("The custom report failed to run");
                            LogEntry lentry = new LogEntry();
                            lentry.Message = ex.ToString();
                            lentry.Title = "Custom Report Fail";
                            Logger.Write(lentry);
                        }
                        break;
                    case "Advanced":
                        {
                            //Tab addition commented out
                            break;
                        }
                    case "Predefined":
                        {
                            try
                            {
                                if (buttonPredefinedGo.Tag == null)
                                    break;
                                query = buttonPredefinedGo.Tag.ToString();
                                //Workspace Filter?
                                if (useFilter)
                                {
                                    query = addWorkspaceFilter(query);
                                }

                                //remove trimfilename
                                if (checkBoxRemoveFileNameTrim.Checked) query = removeTFN(query);
                                SqlCommand cmd = new SqlCommand();

                                cmd.CommandType = CommandType.Text;
                                cmd.CommandTimeout = 6000;
                                cmd.CommandText = query;
                                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                                cmd.Connection = new SqlConnection(Core.Variables.Instance.ConnectionString);
                                dap.Fill(dsReport);
                                // Cannot resolve the collation conflict between "
                                // Latin1_General_CI_AI
                                // " and "
                                // COLLATE SQL_Latin1_General_CP1_CI_AS 
                                //" in the equal to operation.

                                //dsReport = d.DataRepository.Provider.ExecuteDataSet(cmd);
                                //dsReport = d.DataRepository.Provider.ExecuteDataSet(CommandType.Text, query);
                                labelInfo.Text = dsReport.Tables[0].Rows.Count.ToString() + Environment.NewLine + System.DateTime.Now.TimeOfDay.ToString();
                                dataSetTOdataGrid();
                                break;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                            break;
                        }
                }
            tabControl1.SelectedIndex = 1;
            foreach (GridBoundColumn col in gridDataBoundGridMain.GridBoundColumns)
            {
                col.StyleInfo.AutoSize = true;

            }
        }

        #endregion

        #region Basic

        private void retrieveObjects(string objectType)
        {
            if (objectType == "") objectType = "All";

            //string query = "SELECT * FROM dbo.METAView_" + objectType + "_listing";

            string query = "SELECT   METAView_" + objectType +
                           "_Listing.*,dbo.TrimFileName(GraphFile.Name) AS FileName, GraphFile.WorkspaceName AS [Diagram Workspace] FROM GraphFile INNER JOIN GraphFileObject ON GraphFile.pkid = GraphFileObject.GraphFileID AND GraphFile.Machine = GraphFileObject.GraphFileMachine INNER JOIN METAView_" +
                           objectType + "_Listing ON GraphFileObject.MetaObjectID = METAView_" + objectType +
                           "_Listing.pkid AND GraphFileObject.MachineID = METAView_" + objectType +
                           "_Listing.Machine WHERE GraphFile.IsActive = 1 ";
			
            if (useFilter)
            {
                //Append workspace
                query = query + " AND (METAView_" + objectType + "_Listing.WorkspaceName = '" + comboBoxWorkspace.Text + "')";
            }
            //Load
            try
            {
                dsReport = d.DataRepository.Provider.ExecuteDataSet(CommandType.Text, query);
                labelInfo.Text = dsReport.Tables[0].Rows.Count.ToString() + Environment.NewLine + System.DateTime.Now.TimeOfDay.ToString();
                dataSetTOdataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region General

        private void comboBoxParentClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            getAllowedAssociation(comboBoxParentClass, comboBoxChildClass, comboBoxAssociation);
        }
        private void comboBoxChildClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            getAllowedAssociation(comboBoxParentClass, comboBoxChildClass, comboBoxAssociation);
        }
        private void getAllowedAssociation(ComboBox p, ComboBox c, ComboBox a)
        {
            if (tabControlReports.SelectedTab == tabPage1) //Basic
            {
            }
            else if (tabControlReports.SelectedTab == tabPage2) //General
            {
                a.Items.Clear();

                buttonGeneralGo.Enabled = false;
            }
            else if (tabControlReports.SelectedTab == tabPage3) //Advanced
            {
                a.Items.Clear();

                buttonAdvancedGo.Enabled = false;
            }
            else if (tabControlReports.SelectedTab == tabPage4) //Predefined
            {
            }

            if (p.Text != "" && c.Text != "")
            {
                //Parent Class
                b.TList<b.ClassAssociation> parentAssociation =
                   d.DataRepository.ClassAssociationProvider.GetByParentClass(p.Text);
                //Child Class
                b.TList<b.ClassAssociation> childAssociation =
                    d.DataRepository.ClassAssociationProvider.GetByChildClass(c.Text);

                foreach (b.ClassAssociation pAssociationItem in parentAssociation)
                {
                    b.AssociationType pAssociationType =
                       d.DataRepository.AssociationTypeProvider.GetBypkid(pAssociationItem.AssociationTypeID);
                    //Where they match Add
                    foreach (b.ClassAssociation cAssociationItem in childAssociation)
                    {
                        b.AssociationType cAssociationType =
                       d.DataRepository.AssociationTypeProvider.GetBypkid(cAssociationItem.AssociationTypeID);

                        if (pAssociationType.Name == cAssociationType.Name)
                        {
                            if (a.Items.Contains(pAssociationType.Name))
                            {
                            }
                            else
                            {
                                a.Items.Add(pAssociationType.Name);
                            }
                        }
                    }
                }
            }

            if (tabControlReports.SelectedTab == tabPage1) //Basic
            {
            }
            else if (tabControlReports.SelectedTab == tabPage2) //General
            {
                if (comboBoxAssociation.Items.Count > 0)
                {
                    buttonGeneralGo.Enabled = true;
                    comboBoxAssociation.SelectedIndex = 0;
                }
                else
                {
                    buttonGeneralGo.Enabled = false;
                }
            }
            else if (tabControlReports.SelectedTab == tabPage3) //Advanced
            {
                if (comboBoxaAssociation1.Items.Count > 0 && comboBoxaAssociation2.Items.Count > 0)
                {
                    buttonAdvancedGo.Enabled = true;

                    comboBoxaAssociation1.SelectedIndex = 0;
                    comboBoxaAssociation2.SelectedIndex = 0;
                }
                else
                {
                    buttonAdvancedGo.Enabled = false;
                }
            }
            else if (tabControlReports.SelectedTab == tabPage4) //Predefined
            {
            }

        }

        #endregion

        #region Advanced
        //The Tab has been commented out in Reports.designer.cs
        private void comboBoxaParentClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            getAllowedAssociation(comboBoxaParentClass, comboBoxaChildClass, comboBoxaAssociation1);
        }
        private void comboBoxaChildClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            getAllowedAssociation(comboBoxaParentClass, comboBoxaChildClass, comboBoxaAssociation1);
        }
        private void comboBoxaAssociation1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void comboBoxaChildChildClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            getAllowedAssociation(comboBoxaChildClass, comboBoxaChildChildClass, comboBoxaAssociation2);
        }
        private void comboBoxaAssociation2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #endregion

        #region Predefined

        private void treeViewPredefined_AfterSelect(object sender, TreeViewEventArgs e)
        {
            buttonPredefinedGo.Enabled = false;
            //On selecting a non child node, query xml for nodes attribute
            TreeNode tn;

            if (treeViewPredefined.SelectedNode == null && treeViewPredefined.SelectedNode.Nodes.Count > 0)
            {
                //no null nodes
            }
            else
            {
                tn = treeViewPredefined.SelectedNode;
                string query = searchXML(tn.Text);
                if (query == "" || query == "No Query")
                {
                    buttonPredefinedGo.Enabled = false;
                    buttonPredefinedGo.Tag = "";
                }
                else
                {
                    //MessageBox.Show(query);
                    buttonPredefinedGo.Enabled = true;
                    buttonPredefinedGo.Tag = query;
                }
            }
        }
        private string searchXML(string QueryName)
        {
            if (QueryName == "" || QueryName == null) return "No Query";
            XmlDocument dom = new XmlDocument();
            dom.Load(path);
            try
            {
                XmlNode node =
                    dom.DocumentElement.SelectSingleNode("//ReportItem[contains(@Title, '" + QueryName + "')]");

                if (node != null)
                {
                    if (node.Attributes["Query"] != null)
                    {
                        return node.Attributes["Query"].Value;
                    }
                }
                return "No Query";
                /*
                //If any item contains the attribute title == queryname select the attribute query value
                string s = dom.DocumentElement.SelectSingleNode("//ReportItem[contains(@Title, '" + QueryName + "')]").Attributes["Query"].Value.ToString();

                return s;*/
            }
            catch (Exception ex)
            {
                return "No Query";
            }
        }
        private string removeTFN(string query)
        {
            query = query.Replace("dbo.TrimFileName", "");
            return query;
        }

        #endregion
    }
}
