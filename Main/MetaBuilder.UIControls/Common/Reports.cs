using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using x = MetaBuilder.BusinessFacade.Exports;
using System.Xml;
using Syncfusion.Windows.Forms.Grid;
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

            //Other
            //Testing adding of filter to predefined queries
            //MessageBox.Show(this,addWorkspaceFilter("select x,y from somewhere inner join GraphFile on x = y"));
            //MessageBox.Show(this,addWorkspaceFilter("select x,y from somewhere inner join GraphFile on x = y WHERE (x=1)"));
            //MessageBox.Show(this,addWorkspaceFilter("select x,y from somewhere inner join GraphFile on x = y ORDER BY x ascending"));
            //MessageBox.Show(this,addWorkspaceFilter("select x,y from somewhere inner join GraphFile on x = y WHERE (x=1) ORDER BY x ascending"));
            labelInfo.Visible = false;
            // buttonDataLoad.Visible = false;
            checkBoxServer.Visible = Core.Variables.Instance.IsServer;
        }

        // Private Methods (13) 


        public string Provider
        {
            get { return checkBoxServer.Checked ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider; }
        }


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

            if (openFile1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK && openFile1.FileName.Length > 0)
            {
                try
                {
                    dsReport.Clear();
                    //ds.ReadXml(openFile1.FileName);

                    dsReport.Tables.Add(ImportExcel(openFile1.FileName));
                    //MessageBox.Show(this,"imported");
                    dataSetTOdataGrid();
                    //MessageBox.Show(this,"Loaded");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "An error while loading the file into the datagrid has occurred and it has been logged", "Problem loading data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Core.Log.WriteLog(ex.ToString());
                    //MessageBox.Show(this,ex.ToString());
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

            if (saveFile1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK && saveFile1.FileName.Length > 0)
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

                                MessageBox.Show(this, "Saved to " + saveFile1.FileName, "File Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                        case "xls":
                            {
                                x.ExportDetails(dsReport.Tables[0], ReportsExport.ExportFormat.Excel, saveFile1.FileName);
                                //ds.WriteXml(saveFile1.FileName.Substring(saveFile1.FileName.Length - 3) + "xml");

                                MessageBox.Show(this, "Saved to " + saveFile1.FileName, "File Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "An error while saving the file has occurred and it has been logged", "Problem saving data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Core.Log.WriteLog(ex.ToString());
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

                string[] alphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", 
                                                    "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai","aj","ak","al","am","an","ao","ap","aq","ar","as","at","au","av","aw","ax","ay","az",
                                                    "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj","bk","bl","bm","bn","bo","bp","bq","br","bs","bt","bu","bv","bw","bx","by","bz"};
                int alphabetCounter = 0;
                int rowCounter = 1;

                string txt = GetText(alphabet[alphabetCounter], rowCounter, util);
                while (txt.Length > 0 || (alphabetCounter == 0))
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
                    if (MessageBox.Show(this, "There are more than 500 rows which will be imported. This could take a few minutes to a few hours depending on the number of rows and the speed of your computer. Would you like to continue", "Large Excel File - " + rc.ToString() + " rows detected", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Cancel)
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
                            Core.Log.WriteLog(ex.ToString());
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
                MessageBox.Show(this, x.ToString());
                return dt;
            }
            catch (Exception importException)
            {
                MessageBox.Show(this, importException.ToString());
                return dt;
            }
        }

        private void comboBoxWorkspace_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int id = 0;

            //b.TList<b.Workspace> w = d.DataRepository.Connections[Provider].Provider.WorkspaceProvider.GetAll();
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
        private void toolStripButtonSelectAll_Click(object sender, EventArgs e)
        {
            gridDataBoundGridMain.Selections.Clear();
            gridDataBoundGridMain.Selections.Add(GridRangeInfo.Table());
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
                    if (DialogResult.OK == p.ShowDialog(this))
                    {
                        pd.DocumentName = "Test Page Print";
                        pd.Print();
                    }
                    //pd.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "An error occurred attempting to preview the file to print - " + ex.Message, "Error Printing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        GridFilterBar filterBar = new GridFilterBar();
        private void dataSetTOdataGrid()
        {
            filterBar = new GridFilterBar();

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
                lblNoResults.Text = "";
                gridDataBoundGridMain.Visible = true;
            }
            else
            {
                lblNoResults.Visible = true;
                lblNoResults.Text = "The report yeilded no results";
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

            b.TList<b.Workspace> ws = d.DataRepository.Connections[Provider].Provider.WorkspaceProvider.GetAll();
            foreach (b.Workspace wsItem in ws)
            {
                if (cb.Items.Contains(wsItem.Name))
                    continue;

                cb.Items.Add(wsItem.Name);
            }

            cb.SelectedIndex = 0;
        }
        private void getAllClasses(ComboBox cb)
        {
            cb.Items.Clear();

            if (classesindb == null)
                classesindb = d.DataRepository.Classes(Provider);//.Provider.ClassProvider.GetAll();
            foreach (b.Class classItem in classesindb)
            {
                if (classItem.IsActive == false)
                    continue;
                if (cb.Items.Contains(classItem.Name))
                    continue;

                cb.Items.Add(classItem.Name);
            }

            //cb.SelectedIndex = 0;
            cb.Text = "[Select a class]";
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
                MessageBox.Show(this, xmlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
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
                    Core.Log.WriteLog(ex.ToString());
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

        private void contextFilterApplied(object sender, EventArgs e)
        {
            txtCustom.Text = contextFilter.Query;
        }

        private string addWorkspaceFilter(string query)
        {
            string viewFilter = "";

            //We need to add workspace filter using active diagram objects
            if (query.Contains(".WorkspaceName"))
            {
                //find the index of the end of .workspacename
                //find the index of the beginning of the string, backwards to first space
                foreach (string s in query.Split(' '))
                {
                    if (s.Contains(".WorkspaceName"))
                    {
                        viewFilter = s;
                    }
                }
            }
            else
            {
                //this means we have not selected the workspacename which is a problem obviously
                viewFilter = "";
            }

            if (viewFilter.Length > 0)
            {
                if (query.Contains("UNION")) //Union
                {
                    int unionCount = 0;
                    int newQueryCount = 0;
                    foreach (String s in query.Split(' '))
                    {
                        if (s.ToLower() == "union")
                        {
                            unionCount += 1;
                        }
                    }
                    //split the query at union and then just run the same code on it
                    string tempQuery = query;
                    query = "";
                    tempQuery = tempQuery.Replace(" UNION ", " ! ");
                    //add the filter
                    foreach (string s in tempQuery.Split('!'))
                    {
                        string singleQuery = s;
                        //get the viewFilter for this query
                        if (singleQuery.Contains(".WorkspaceName"))
                        {
                            //find the index of the end of .workspacename
                            //find the index of the beginning of the string, backwards to first space
                            foreach (string p in singleQuery.Split(' '))
                            {
                                if (p.Contains(".WorkspaceName"))
                                {
                                    viewFilter = p;
                                    //I dont know where the bracket at the beginning comes from
                                    viewFilter = viewFilter.Replace("(", "");
                                }
                            }
                        }
                        else
                        {
                            //this means we have not selected the workspacename which is a problem obviously
                            viewFilter = "";
                        }


                        //now 's' is a variable containt one query
                        if (viewFilter != "")
                        {
                            if (singleQuery.Contains("WHERE "))
                            {
                                if (singleQuery.Contains("ORDER BY "))
                                {
                                    //Insert before "o" order by - 1 to be in white space... and white spaces before and after
                                    int oPosition = singleQuery.IndexOf("ORDER BY");
                                    oPosition -= 1; //if its the begginned @ o else must be - 9
                                    singleQuery =
                                        singleQuery.Insert(oPosition,
                                                           " AND (" + viewFilter + " = '" + comboBoxWorkspace.Text +
                                                           "') ");
                                }
                                else
                                {
                                    singleQuery = singleQuery + " AND (" + viewFilter + " = '" + comboBoxWorkspace.Text +
                                                  "')";
                                }
                            }
                            else
                            {
                                if (singleQuery.Contains("ORDER BY "))
                                {
                                    //Insert before "o" order by - 1 to be in white space... and white spaces before and after
                                    int oPosition = singleQuery.IndexOf("ORDER BY");
                                    oPosition -= 1; //if its the begginned @ o else must be - 9
                                    singleQuery =
                                        singleQuery.Insert(oPosition,
                                                           " WHERE (" + viewFilter + " = '" + comboBoxWorkspace.Text +
                                                           "') ");
                                }
                                else
                                {
                                    singleQuery = singleQuery + " WHERE (" + viewFilter + " = '" +
                                                  comboBoxWorkspace.Text + "')";
                                }
                            }
                        }

                        //rebuild query one singleQuery at a time until no more unions exist
                        if (newQueryCount == unionCount)
                        {
                            query = query + singleQuery;
                        }
                        else
                        {
                            query = query + singleQuery + " UNION ";
                        }
                        newQueryCount += 1;
                    }
                } //Normal
                else
                {
                    if (query.Contains("WHERE "))
                    {
                        if (query.Contains("ORDER BY "))
                        {
                            //Insert before "o" order by - 1 to be in white space... and white spaces before and after
                            int oPosition = query.IndexOf("ORDER BY");
                            oPosition -= 1; //if its the begginned @ o else must be - 9
                            query =
                                query.Insert(oPosition, " AND (" + viewFilter + " = '" + comboBoxWorkspace.Text + "') ");
                        }
                        else
                        {
                            query = query + " AND (" + viewFilter + " = '" + comboBoxWorkspace.Text + "')";
                        }
                    }
                    else
                    {
                        if (query.Contains("ORDER BY "))
                        {
                            //Insert before "o" order by - 1 to be in white space... and white spaces before and after
                            int oPosition = query.IndexOf("ORDER BY");
                            oPosition -= 1; //if its the begginned @ o else must be - 9
                            query =
                                query.Insert(oPosition,
                                             " WHERE (" + viewFilter + " = '" + comboBoxWorkspace.Text + "') ");
                        }
                        else
                        {
                            query = query + " WHERE (" + viewFilter + " = '" + comboBoxWorkspace.Text + "')";
                        }
                    }
                }
            }
            return query;
        }
        private void go(string buttonName)
        {
            //dsReport.Clear();
            dsReport = new DataSet("Results");

            if (comboBoxWorkspace.Text != "All")
            {
                useFilter = true;
            }
            else
            {
                useFilter = false;
            }

            string query = "";

            if (buttonName.Length > 0)
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
                                MessageBox.Show(this, "Both a parent and a child class a required to be selected in order to continue", "Classes not complete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                            }

                            association = comboBoxAssociation.Text;
                            if (association == "")
                            {
                                MessageBox.Show(this, "An association has not been selected", "Invalid Association", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                            }
                            if (association == "All")
                            {
                                associationID = 0;
                            }
                            else
                            {
                                associationID = d.DataRepository.Connections[Provider].Provider.AssociationTypeProvider.GetByName(association).pkid;
                                //CAid
                            }

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

                            #region select for primary and secondary

                            string primaryClassName, secondaryClassName;
                            //primary
                            if (primaryClass == "Condition" || primaryClass == "ConditionalDescription" ||
                                primaryClass == "ConnectionSpeed" || primaryClass == "ConnectionType" ||
                                primaryClass == "ProbabilityOfRealization" || primaryClass == "ProbOfRealization" || primaryClass == "TimeIndicator")
                            {
                                primaryClassName = "Value";
                            }
                            else if (primaryClass == "Rationale")
                            {
                                primaryClassName = "UniqueReference";
                            }
                            else if (primaryClass == "Automation" || primaryClass == "SubsetIndicator")
                            {
                                primaryClassName = primaryClass + "Type";
                            }
                            else if (primaryClass == "CSF" || primaryClass == "StrategicStatement")
                            {
                                primaryClassName = "Name";
                            }
                            else if (primaryClass == "ResourceAvailability")
                            {
                                primaryClassName = "ResourceAvailabilityRating";
                            }
                            else
                            {
                                primaryClassName = "Name";
                            }
                            //secondary
                            if (secondaryClass == "Condition" || secondaryClass == "ConditionalDescription" ||
                                secondaryClass == "ConnectionSpeed" || secondaryClass == "ConnectionType" ||
                                secondaryClass == "ProbabilityOfRealization" || secondaryClass == "ProbOfRealization" || secondaryClass == "TimeIndicator")
                            {
                                secondaryClassName = "Value";
                            }
                            else if (secondaryClass == "Rationale")
                            {
                                secondaryClassName = "UniqueReference";
                            }
                            else if (secondaryClass == "Automation" || secondaryClass == "SubsetIndicator")
                            {
                                secondaryClassName = secondaryClass + "Type";
                            }
                            else if (secondaryClass == "CSF" || secondaryClass == "StrategicStatement")
                            {
                                secondaryClassName = "Name";
                            }
                            else if (secondaryClass == "ResourceAvailability")
                            {
                                secondaryClassName = "ResourceAvailabilityRating";
                            }
                            else
                            {
                                secondaryClassName = "Name";
                            }

                            #endregion

                            #endregion

                            #region Orphans Excluded : on graphfiles

                            query = "SELECT ";

                            if (primaryClass != "Rationale" && secondaryClass != "Rationale")
                                query += " DISTINCT ";

                            query += " METAView_" +
                       primaryClass +
                       "_Listing." + primaryClassName + " AS [" + primaryClass + "],METAView_" +
                       secondaryClass +
                       "_Listing" +
                       primaryClassCorrelation + "." + secondaryClassName + " AS [" + secondaryClass +
                       primaryClassCorrelation +
                       "], AssociationType.Name ,  ActiveDiagramObjects_Child.[File Name]";

                            query = query + " FROM METAView_" +
                                    primaryClass +
                                    "_Listing INNER JOIN ObjectAssociation ON METAView_" +
                                    primaryClass +
                                    "_Listing.pkid = ObjectAssociation.ObjectID AND  METAView_" +
                                    primaryClass +
                                    "_Listing.Machine = ObjectAssociation.ObjectMachine INNER JOIN METAView_" +
                                    secondaryClass +
                                    "_Listing AS [METAView_" +
                                    secondaryClass +
                                    "_Listing" + primaryClassCorrelation +
                                    "] ON ObjectAssociation.ChildObjectID = METAView_" +
                                    secondaryClass +
                                    "_Listing" +
                                    primaryClassCorrelation +
                                    ".pkid AND  ObjectAssociation.ChildObjectMachine = METAView_" +
                                    secondaryClass +
                                    "_Listing" +
                                    primaryClassCorrelation +
                                    ".Machine INNER JOIN ClassAssociation ON ObjectAssociation.CAid = ClassAssociation.CAid INNER JOIN AssociationType ON ClassAssociation.AssociationTypeID = AssociationType.pkid INNER JOIN ActiveDiagramObjects AS ActiveDiagramObjects_Child ON METAView_" +
                                    secondaryClass +
                                    "_Listing" +
                                    primaryClassCorrelation +
                                    ".pkid = ActiveDiagramObjects_Child.MetaObjectID AND  METAView_" +
                                    secondaryClass +
                                    "_Listing" +
                                    primaryClassCorrelation +
                                    ".Machine = ActiveDiagramObjects_Child.Machine INNER JOIN ActiveDiagramObjects AS ActiveDiagramObjects_Parent ON METAView_" +
                                    primaryClass +
                                    "_Listing.pkid = ActiveDiagramObjects_Parent.MetaObjectID AND  METAView_" +
                                    primaryClass +
                                    "_Listing.Machine = ActiveDiagramObjects_Parent.Machine ";

                            query = query + " WHERE (METAView_" +
                                    primaryClass +
                                    "_Listing." + primaryClassName + " IS NOT NULL) AND (METAView_" +
                                    secondaryClass +
                                    "_Listing" +
                                    primaryClassCorrelation + "." + secondaryClassName +
                                    " IS NOT NULL)";

                            #region Customised Queries For Complex Classes

                            if ((primaryClass == "Entity" && secondaryClass == "Attribute") || (primaryClass == "DataEntity" && secondaryClass == "DataAttribute"))
                            {
                                query =
                                    "SELECT   DISTINCT    METAView_DataEntity_Listing.Name AS [DataEntity Name], METAView_DataEntity_Listing.DataEntityType AS [DataEntity Type], METAView_DataAttribute_Listing.Name AS Attibute, ClassAssociation.Caption AS [DataAttribute Type], METAView_DataEntity_Listing.WorkspaceName AS [Workspace Name] FROM         ActiveDiagramObjects INNER JOIN METAView_DataEntity_Listing ON ActiveDiagramObjects.MetaObjectID = METAView_DataEntity_Listing.pkid AND ActiveDiagramObjects.Machine = METAView_DataEntity_Listing.Machine LEFT OUTER JOIN ActiveDiagramObjects AS ActiveDiagramObjects_1 INNER JOIN AssociationType INNER JOIN ClassAssociation ON AssociationType.pkid = ClassAssociation.AssociationTypeID INNER JOIN ObjectAssociation ON ClassAssociation.CAid = ObjectAssociation.CAid INNER JOIN METAView_DataAttribute_Listing ON ObjectAssociation.ChildObjectID = METAView_DataAttribute_Listing.pkid AND  ObjectAssociation.ChildObjectMachine = METAView_DataAttribute_Listing.Machine ON  ActiveDiagramObjects_1.MetaObjectID = METAView_DataAttribute_Listing.pkid AND  ActiveDiagramObjects_1.Machine = METAView_DataAttribute_Listing.Machine ON METAView_DataEntity_Listing.pkid = ObjectAssociation.ObjectID AND  METAView_DataEntity_Listing.Machine = ObjectAssociation.ObjectMachine WHERE (METAView_DataEntity_Listing.Name IS NOT NULL) ";
                            }
                            //TODO
                            else if (primaryClass == "CSF" && secondaryClass == "CSF" || primaryClass == "StrategicStatement" && secondaryClass == "StrategicStatement")
                            {
                                query =
                                    "SELECT     METAView_StrategicStatement_Listing.Name AS [Parent StrategicStatement Name], METAView_StrategicStatement_Listing.UniqueRef AS [Parent StrategicStatement UniqueRef], METAView_StrategicStatement_Listing_1.Name AS [Child StrategicStatement Name], METAView_StrategicStatement_Listing_1.UniqueRef AS [Child StrategicStatement UniqueRef],  AssociationType.Name AS Association, ActiveDiagramObjects.[File Name] FROM         ObjectAssociation INNER JOIN ClassAssociation ON ObjectAssociation.CAid = ClassAssociation.CAid INNER JOIN AssociationType ON ClassAssociation.AssociationTypeID = AssociationType.pkid INNER JOIN METAView_StrategicStatement_Listing ON ObjectAssociation.ObjectID = METAView_StrategicStatement_Listing.pkid AND  ObjectAssociation.ObjectMachine = METAView_StrategicStatement_Listing.Machine INNER JOIN METAView_StrategicStatement_Listing AS METAView_StrategicStatement_Listing_1 ON ObjectAssociation.ChildObjectID = METAView_StrategicStatement_Listing_1.pkid AND  ObjectAssociation.ChildObjectMachine = METAView_StrategicStatement_Listing_1.Machine INNER JOIN ActiveDiagramObjects AS ActiveDiagramObjects_1 ON METAView_StrategicStatement_Listing_1.pkid = ActiveDiagramObjects_1.MetaObjectID AND  METAView_StrategicStatement_Listing_1.Machine = ActiveDiagramObjects_1.Machine INNER JOIN ActiveDiagramObjects ON METAView_StrategicStatement_Listing.pkid = ActiveDiagramObjects.MetaObjectID AND  METAView_StrategicStatement_Listing.Machine = ActiveDiagramObjects.Machine WHERE (ActiveDiagramObjects.[File Name] IS NOT NULL) ";
                            }
                            else if (primaryClass == "GovernanceMechanism" && secondaryClass == "GovernanceMechanism")
                            {
                                query =
                                    "SELECT     METAView_GovernanceMechanism_Listing.Name AS [Parent GovernanceMechanism Name], METAView_GovernanceMechanism_Listing.UniqueRef AS [Parent GovernanceMechanism ID], METAView_GovernanceMechanism_Listing_1.Name AS [Child GovernanceMechanism Name], METAView_GovernanceMechanism_Listing_1.UniqueRef AS [Child GovernanceMechanism ID],  AssociationType.Name AS Association, ActiveDiagramObjects.[File Name] FROM         ObjectAssociation INNER JOIN ClassAssociation ON ObjectAssociation.CAid = ClassAssociation.CAid INNER JOIN AssociationType ON ClassAssociation.AssociationTypeID = AssociationType.pkid INNER JOIN METAView_GovernanceMechanism_Listing ON ObjectAssociation.ObjectID = METAView_GovernanceMechanism_Listing.pkid AND  ObjectAssociation.ObjectMachine = METAView_GovernanceMechanism_Listing.Machine INNER JOIN METAView_GovernanceMechanism_Listing AS METAView_GovernanceMechanism_Listing_1 ON ObjectAssociation.ChildObjectID = METAView_GovernanceMechanism_Listing_1.pkid AND  ObjectAssociation.ChildObjectMachine = METAView_GovernanceMechanism_Listing_1.Machine INNER JOIN ActiveDiagramObjects AS ActiveDiagramObjects_1 ON METAView_GovernanceMechanism_Listing_1.pkid = ActiveDiagramObjects_1.MetaObjectID AND  METAView_GovernanceMechanism_Listing_1.Machine = ActiveDiagramObjects_1.Machine INNER JOIN ActiveDiagramObjects ON METAView_GovernanceMechanism_Listing.pkid = ActiveDiagramObjects.MetaObjectID AND  METAView_GovernanceMechanism_Listing.Machine = ActiveDiagramObjects.Machine WHERE (ActiveDiagramObjects.[File Name] IS NOT NULL) ";
                            }
                            else if ((primaryClass == "CSF" || primaryClass == "StrategicStatement") && secondaryClass == "GovernanceMechanism")
                            {
                                query =
                                    "SELECT     METAView_StrategicStatement_Listing.Name AS [Parent StrategicStatement Name], METAView_StrategicStatement_Listing.UniqueRef AS [Parent StrategicStatement UniqueRef], METAView_GovernanceMechanism_Listing.Name AS [Governance Mechanism Name],  METAView_GovernanceMechanism_Listing.UniqueRef AS [Governance Mechanism ID], AssociationType.Name AS Association,  ActiveDiagramObjects.[File Name] FROM         ObjectAssociation INNER JOIN ClassAssociation ON ObjectAssociation.CAid = ClassAssociation.CAid INNER JOIN AssociationType ON ClassAssociation.AssociationTypeID = AssociationType.pkid INNER JOIN METAView_StrategicStatement_Listing ON ObjectAssociation.ObjectID = METAView_StrategicStatement_Listing.pkid AND  ObjectAssociation.ObjectMachine = METAView_StrategicStatement_Listing.Machine INNER JOIN ActiveDiagramObjects ON ActiveDiagramObjects.MetaObjectID = METAView_StrategicStatement_Listing.pkid AND  ActiveDiagramObjects.Machine = METAView_StrategicStatement_Listing.Machine INNER JOIN METAView_GovernanceMechanism_Listing ON ObjectAssociation.ChildObjectID = METAView_GovernanceMechanism_Listing.pkid AND  ObjectAssociation.ChildObjectMachine = METAView_GovernanceMechanism_Listing.Machine INNER JOIN ActiveDiagramObjects AS ActiveDiagramObjects_1 ON METAView_GovernanceMechanism_Listing.pkid = ActiveDiagramObjects_1.MetaObjectID AND  METAView_GovernanceMechanism_Listing.Machine = ActiveDiagramObjects_1.Machine WHERE     (ActiveDiagramObjects.[File Name] IS NOT NULL)";
                            }
                            else if (primaryClass == "GovernanceMechanism" && (secondaryClass == "CSF" || secondaryClass == "StrategicStatement"))
                            {
                                query =
                                    "SELECT     METAView_StrategicStatement_Listing.Name AS [Parent StrategicStatement Name], METAView_StrategicStatement_Listing.UniqueRef AS [Parent StrategicStatement UniqueRef], METAView_GovernanceMechanism_Listing.Name AS [Governance Mechanism Name],  METAView_GovernanceMechanism_Listing.UniqueRef AS [Governance Mechanism ID], AssociationType.Name AS Association,  ActiveDiagramObjects_1.[File Name] FROM         ActiveDiagramObjects INNER JOIN METAView_StrategicStatement_Listing ON ActiveDiagramObjects.MetaObjectID = METAView_StrategicStatement_Listing.pkid AND  ActiveDiagramObjects.Machine = METAView_StrategicStatement_Listing.Machine INNER JOIN ActiveDiagramObjects AS ActiveDiagramObjects_1 INNER JOIN METAView_GovernanceMechanism_Listing ON ActiveDiagramObjects_1.MetaObjectID = METAView_GovernanceMechanism_Listing.pkid AND  ActiveDiagramObjects_1.Machine = METAView_GovernanceMechanism_Listing.Machine INNER JOIN ObjectAssociation INNER JOIN ClassAssociation ON ObjectAssociation.CAid = ClassAssociation.CAid INNER JOIN AssociationType ON ClassAssociation.AssociationTypeID = AssociationType.pkid ON  METAView_GovernanceMechanism_Listing.pkid = ObjectAssociation.ObjectID AND METAView_GovernanceMechanism_Listing.Machine = ObjectAssociation.ObjectMachine ON  METAView_StrategicStatement_Listing.pkid = ObjectAssociation.ChildObjectID AND METAView_StrategicStatement_Listing.Machine = ObjectAssociation.ChildObjectMachine WHERE     (ActiveDiagramObjects_1.[File Name] IS NOT NULL)";
                            }
                            else if (primaryClass == "CSF" || primaryClass == "StrategicStatement")
                            {
                                query =
                                    "SELECT    DISTINCT   METAView_StrategicStatement_Listing.UniqueRef AS [StrategicStatement UniqueRef], METAView_StrategicStatement_Listing.Name AS [StrategicStatement UniqueRef], METAView_StrategicStatement_Listing.GapType , METAView_" +
                                    secondaryClass +
                                    "_Listing" +
                                    primaryClassCorrelation +
                                    "." + secondaryClassName +
                                    " AS [" + secondaryClass +
                                    "], AssociationType.Name, ActiveDiagramObjects_Child.[File Name] FROM         METAView_StrategicStatement_Listing INNER JOIN ObjectAssociation ON METAView_StrategicStatement_Listing.pkid = ObjectAssociation.ObjectID AND  METAView_StrategicStatement_Listing.Machine = ObjectAssociation.ObjectMachine INNER JOIN METAView_" +
                                    secondaryClass +
                                    "_Listing" +
                                    primaryClassCorrelation +
                                    " ON ObjectAssociation.ChildObjectID = METAView_" +
                                    secondaryClass +
                                    "_Listing" +
                                    primaryClassCorrelation +
                                    ".pkid AND  ObjectAssociation.ChildObjectMachine = METAView_" +
                                    secondaryClass +
                                    "_Listing" +
                                    primaryClassCorrelation +
                                    ".Machine INNER JOIN ClassAssociation ON ObjectAssociation.CAid = ClassAssociation.CAid INNER JOIN AssociationType ON ClassAssociation.AssociationTypeID = AssociationType.pkid INNER JOIN ActiveDiagramObjects AS ActiveDiagramObjects_Child ON METAView_" +
                                    secondaryClass +
                                    "_Listing" +
                                    primaryClassCorrelation +
                                    ".pkid = ActiveDiagramObjects_Child.MetaObjectID AND  METAView_" +
                                    secondaryClass +
                                    "_Listing" +
                                    primaryClassCorrelation +
                                    ".Machine = ActiveDiagramObjects_Child.Machine INNER JOIN ActiveDiagramObjects AS ActiveDiagramObjects_Parent ON METAView_StrategicStatement_Listing.pkid = ActiveDiagramObjects_Parent.MetaObjectID AND  METAView_StrategicStatement_Listing.Machine = ActiveDiagramObjects_Parent.Machine WHERE (METAView_" +
                                    secondaryClass +
                                    "_Listing" +
                                    primaryClassCorrelation +
                                    "." + secondaryClassName +
                                    " IS NOT NULL)";
                            }
                            else if (primaryClass == "GovernanceMechanism")
                            {
                                query =
                                    "SELECT    DISTINCT   METAView_GovernanceMechanism_Listing.[UniqueRef] AS [Governance ID], METAView_GovernanceMechanism_Listing.Name AS [Governance Name], METAView_" +
                                    secondaryClass + "_Listing" + primaryClassCorrelation + "." + secondaryClassName +
                                    " AS [" + secondaryClass +
                                    "], AssociationType.Name, ActiveDiagramObjects_Child.[File Name] FROM         METAView_GovernanceMechanism_Listing INNER JOIN ObjectAssociation ON METAView_GovernanceMechanism_Listing.pkid = ObjectAssociation.ObjectID AND  METAView_GovernanceMechanism_Listing.Machine = ObjectAssociation.ObjectMachine INNER JOIN METAView_" +
                                    secondaryClass + "_Listing" + primaryClassCorrelation +
                                    " ON ObjectAssociation.ChildObjectID = METAView_" +
                                    secondaryClass + "_Listing" + primaryClassCorrelation +
                                    ".pkid AND  ObjectAssociation.ChildObjectMachine = METAView_" +
                                    secondaryClass + "_Listing" + primaryClassCorrelation +
                                    ".Machine INNER JOIN ClassAssociation ON ObjectAssociation.CAid = ClassAssociation.CAid INNER JOIN AssociationType ON ClassAssociation.AssociationTypeID = AssociationType.pkid INNER JOIN ActiveDiagramObjects AS ActiveDiagramObjects_Child ON METAView_" +
                                    secondaryClass + "_Listing" + primaryClassCorrelation +
                                    ".pkid = ActiveDiagramObjects_Child.MetaObjectID AND  METAView_" +
                                    secondaryClass + "_Listing" + primaryClassCorrelation +
                                    ".Machine = ActiveDiagramObjects_Child.Machine INNER JOIN ActiveDiagramObjects AS ActiveDiagramObjects_Parent ON METAView_GovernanceMechanism_Listing.pkid = ActiveDiagramObjects_Parent.MetaObjectID AND  METAView_GovernanceMechanism_Listing.Machine = ActiveDiagramObjects_Parent.Machine WHERE     (METAView_" +
                                    secondaryClass + "_Listing" + primaryClassCorrelation + "." + secondaryClassName +
                                    " IS NOT NULL)";
                            }
                            else if (secondaryClass == "StrategicStatement")
                            {
                                query =
                                    "SELECT    DISTINCT   METAView_StrategicStatement_Listing.UniqueRef AS [StrategicStatement UniqueRef], METAView_StrategicStatement_Listing.Name AS [StrategicStatement Name], METAView_StrategicStatement_Listing.GapType , METAView_" +
                                    primaryClass +
                                    "_Listing." + primaryClassName + " AS [" + primaryClass +
                                    "], AssociationType.Name, ActiveDiagramObjects_Child.[File Name] FROM         ActiveDiagramObjects AS ActiveDiagramObjects_Parent INNER JOIN  METAView_StrategicStatement_Listing ON ActiveDiagramObjects_Parent.MetaObjectID = METAView_StrategicStatement_Listing.pkid AND  ActiveDiagramObjects_Parent.Machine = METAView_StrategicStatement_Listing.Machine INNER JOIN ActiveDiagramObjects AS ActiveDiagramObjects_Child INNER JOIN METAView_" +
                                    primaryClass +
                                    "_Listing ON ActiveDiagramObjects_Child.MetaObjectID = METAView_" +
                                    primaryClass +
                                    "_Listing.pkid AND  ActiveDiagramObjects_Child.Machine = METAView_" +
                                    primaryClass +
                                    "_Listing.Machine INNER JOIN ClassAssociation INNER JOIN ObjectAssociation ON ClassAssociation.CAid = ObjectAssociation.CAid INNER JOIN AssociationType ON ClassAssociation.AssociationTypeID = AssociationType.pkid ON  METAView_" +
                                    primaryClass +
                                    "_Listing.pkid = ObjectAssociation.ObjectID AND METAView_" +
                                    primaryClass +
                                    "_Listing.Machine = ObjectAssociation.ObjectMachine ON  METAView_StrategicStatement_Listing.pkid = ObjectAssociation.ChildObjectID AND METAView_StrategicStatement_Listing.Machine = ObjectAssociation.ChildObjectMachine WHERE     (METAView_" +
                                    primaryClass +
                                    "_Listing." + primaryClassName +
                                    " IS NOT NULL)";
                            }
                            else if (secondaryClass == "GovernanceMechanism")
                            {
                                query =
                                    "SELECT    DISTINCT   METAView_GovernanceMechanism_Listing.[UniqueRef] AS [Governance ID], METAView_GovernanceMechanism_Listing.Name AS [Governance Name], METAView_" +
                                    primaryClass +
                                    "_Listing." + primaryClassName +
                                    " AS [" + primaryClass +
                                    "], AssociationType.Name, ActiveDiagramObjects_Child.[File Name] FROM         ActiveDiagramObjects AS ActiveDiagramObjects_Parent INNER JOIN METAView_GovernanceMechanism_Listing ON ActiveDiagramObjects_Parent.MetaObjectID = METAView_GovernanceMechanism_Listing.pkid AND  ActiveDiagramObjects_Parent.Machine = METAView_GovernanceMechanism_Listing.Machine INNER JOIN ActiveDiagramObjects AS ActiveDiagramObjects_Child INNER JOIN METAView_" +
                                    primaryClass +
                                    "_Listing ON ActiveDiagramObjects_Child.MetaObjectID = METAView_" +
                                    primaryClass +
                                    "_Listing.pkid AND  ActiveDiagramObjects_Child.Machine = METAView_" +
                                    primaryClass +
                                    "_Listing.Machine INNER JOIN ClassAssociation INNER JOIN ObjectAssociation ON ClassAssociation.CAid = ObjectAssociation.CAid INNER JOIN AssociationType ON ClassAssociation.AssociationTypeID = AssociationType.pkid ON  METAView_" +
                                    primaryClass +
                                    "_Listing.pkid = ObjectAssociation.ObjectID AND METAView_" +
                                    primaryClass +
                                    "_Listing.Machine = ObjectAssociation.ObjectMachine ON  METAView_GovernanceMechanism_Listing.pkid = ObjectAssociation.ChildObjectID AND METAView_GovernanceMechanism_Listing.Machine = ObjectAssociation.ChildObjectMachine WHERE     (METAView_" +
                                    primaryClass +
                                    "_Listing." + primaryClassName +
                                    " IS NOT NULL)";
                            }
                            else
                            {
                                //Defaulted to query built on its own
                            }

                            #endregion

                            if (useFilter)
                            {
                                query = query + " AND (METAView_" +
                                        primaryClass +
                                        "_Listing.WorkspaceName = '" + comboBoxWorkspace.Text + "') ";
                            }

                            #endregion

                            #region Orphans Included : not on graphfiles

                            //this will get all objects, even those not in graphfiles
                            if (includeOrphans)
                            {
                                query = "SELECT ";

                                if (primaryClass != "Rationale" && secondaryClass != "Rationale")
                                    query += " DISTINCT ";

                                query += "  METAView_" +
                                            primaryClass +
                                            "_Listing." + primaryClassName + " AS [" + primaryClass + "],METAView_" +
                                            secondaryClass +
                                            "_Listing" +
                                            primaryClassCorrelation + "." + secondaryClassName + " AS [" + secondaryClass +
                                            primaryClassCorrelation +
                                            "], AssociationType.Name ";

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

                                query = query + " WHERE (METAView_" + primaryClass +
                                        "_Listing." + primaryClassName + " is not NULL) AND (METAView_" + secondaryClass +
                                        "_Listing" + primaryClassCorrelation +
                                        "." + secondaryClassName + " is not NULL)";

                                if (primaryClass == "Entity" && secondaryClass == "Attribute" || (primaryClass == "DataEntity" && secondaryClass == "DataAttribute"))
                                {
                                    query =
                                        "SELECT  DISTINCT     METAView_DataEntity_Listing.Name AS [DataEntity Name], METAView_DataEntity_Listing.DataEntityType AS [DataEntity Type], METAView_DataAttribute_Listing.Name AS Attibute, ClassAssociation.Caption AS [DataAttribute Type], METAView_DataEntity_Listing.WorkspaceName AS [Workspace Name] FROM         METAView_DataEntity_Listing LEFT OUTER JOIN AssociationType INNER JOIN ClassAssociation ON AssociationType.pkid = ClassAssociation.AssociationTypeID INNER JOIN ObjectAssociation ON ClassAssociation.CAid = ObjectAssociation.CAid INNER JOIN METAView_DataAttribute_Listing ON ObjectAssociation.ChildObjectID = METAView_DataAttribute_Listing.pkid AND  ObjectAssociation.ChildObjectMachine = METAView_DataAttribute_Listing.Machine ON METAView_DataEntity_Listing.pkid = ObjectAssociation.ObjectID AND  METAView_DataEntity_Listing.Machine = ObjectAssociation.ObjectMachine WHERE     (METAView_DataAttribute_Listing.Name IS NOT NULL) ";
                                }

                                if (useFilter)
                                {
                                    query = query + " AND (METAView_" + primaryClass + "_Listing.WorkspaceName = '" +
                                            comboBoxWorkspace.Text + "')";
                                }
                            }

                            #endregion

                            #region Add Association

                            if (associationID > 0)
                            {
                                if (includeOrphans)
                                {
                                    query = query + " AND (ClassAssociation.AssociationTypeID = " + associationID + ") ";
                                }
                                else
                                {
                                    query = query + " AND (AssociationType.pkid = " + associationID + ") ";
                                }
                            }

                            #endregion

                            try
                            {
                                DataSet ds = new DataSet();
                                SqlCommand cmd =
                                    new SqlCommand(query, new SqlConnection(checkBoxServer.Checked ? Core.Variables.Instance.ServerConnectionString : Core.Variables.Instance.ConnectionString));
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandTimeout = 6000;
                                SqlDataAdapter dap = new SqlDataAdapter();
                                dap.SelectCommand = cmd;
                                dap.Fill(ds);
                                dsReport = ds;
                                //dsReport = d.DataRepository.Provider.ExecuteDataSet(CommandType.Text, query);
                                labelInfo.Text = dsReport.Tables[0].Rows.Count.ToString() + Environment.NewLine + System.DateTime.Now.TimeOfDay.ToString();
                                dataSetTOdataGrid();
                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show(this,ex.ToString());
                                MessageBox.Show(this, "The report failed to run, the error has been logged", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                LogEntry lentry = new LogEntry();
                                lentry.Message = query + Environment.NewLine + ex.ToString();
                                lentry.Title = "Report Fail";
                                Logger.Write(lentry);
                            }

                            break;
                        }
                    case "custom":
                        try
                        {
                            if (txtCustom.Text.ToLower().Contains("select "))
                            {
                                dsReport = d.DataRepository.Connections[Provider].Provider.ExecuteDataSet(CommandType.Text, txtCustom.Text);
                                labelInfo.Text = dsReport.Tables[0].Rows.Count.ToString() + Environment.NewLine + System.DateTime.Now.TimeOfDay.ToString();
                                dataSetTOdataGrid();
                            }
                            else
                            {
                                d.DataRepository.Connections[Provider].Provider.ExecuteNonQuery(CommandType.Text, txtCustom.Text);
                            }
                        }
                        catch (SqlException sqlex)
                        {
                            //MessageBox.Show(this,ex.ToString());
                            MessageBox.Show(this, sqlex.Message, "SQL Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LogEntry lentry = new LogEntry();
                            lentry.Message = txtCustom.Text + Environment.NewLine + sqlex.ToString();
                            lentry.Title = "Custom Report Fail";
                            Logger.Write(lentry);
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(this,ex.ToString());
                            MessageBox.Show(this, "The custom report failed to run, the error has been logged", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LogEntry lentry = new LogEntry();
                            lentry.Message = txtCustom.Text + Environment.NewLine + ex.ToString();
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
                                //Workspace Filter
                                if (useFilter)
                                {
                                    //TODO This must be converted to active diagram objects
                                    query = addWorkspaceFilter(query);
                                }

                                //remove trimfilename
                                //if (checkBoxRemoveFileNameTrim.Checked) query = removeTFN(query);
                                SqlCommand cmd = new SqlCommand();

                                cmd.CommandType = CommandType.Text;
                                cmd.CommandTimeout = 6000;
                                cmd.CommandText = query;
                                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                                cmd.Connection = new SqlConnection(checkBoxServer.Checked ? Core.Variables.Instance.ServerConnectionString : Core.Variables.Instance.ConnectionString);
                                //dsReport = new DataSet("Results");
                                dap.Fill(dsReport);

                                //dsReport = d.DataRepository.Provider.ExecuteDataSet(cmd);
                                //dsReport = d.DataRepository.Provider.ExecuteDataSet(CommandType.Text, query);
                                labelInfo.Text = dsReport.Tables[0].Rows.Count.ToString() + Environment.NewLine + System.DateTime.Now.TimeOfDay.ToString();
                                dataSetTOdataGrid();

                                //set name of report above results
                                //it will not be visible if there are results
                                if (!lblNoResults.Visible)
                                {
                                    lblNoResults.Visible = true;
                                    lblNoResults.Text = treeViewPredefined.SelectedNode.Text;
                                }

                                break;
                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show(this,ex.ToString());
                                MessageBox.Show(this, "The predefined report failed to run, the error has been logged", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                LogEntry lentry = new LogEntry();
                                lentry.Message = treeViewPredefined.SelectedNode.Text + Environment.NewLine + query + Environment.NewLine + ex.ToString();
                                lentry.Title = "Predefined Report Fail";
                                Logger.Write(lentry);
                            }
                            break;
                        }
                }

            if (dsReport.Tables.Count > 0)
            {
                if (dsReport.Tables[0].Rows.Count > 0)
                {
                    tabControl1.SelectedIndex = 1;
                    foreach (GridBoundColumn col in gridDataBoundGridMain.GridBoundColumns)
                    {
                        col.StyleInfo.AutoSize = true;
                    }
                }
                else
                {
                    MessageBox.Show(this, "The report yielded no results", "No results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tabControl1.SelectedIndex = 0;
                }
            }
            else
            {
                tabControl1.SelectedIndex = 0;
            }
        }

        #endregion

        #region Basic

        private void retrieveObjects(string objectType)
        {
            if (objectType == "")
                objectType = "All";

            //string query = "SELECT * FROM dbo.METAView_" + objectType + "_listing";

            //string query = "SELECT   METAView_" + objectType +
            //               "_Listing.*,dbo.TrimFileName(GraphFile.Name) AS FileName, GraphFile.WorkspaceName AS [Diagram Workspace] FROM GraphFile INNER JOIN GraphFileObject ON GraphFile.pkid = GraphFileObject.GraphFileID AND GraphFile.Machine = GraphFileObject.GraphFileMachine INNER JOIN METAView_" +
            //               objectType + "_Listing ON GraphFileObject.MetaObjectID = METAView_" + objectType +
            //               "_Listing.pkid AND GraphFileObject.MachineID = METAView_" + objectType +
            //               "_Listing.Machine WHERE GraphFile.IsActive = 1 ";

            string query = "SELECT   ";

            if (objectType != "Rationale" && objectType != "Software")
                query += " DISTINCT  ";
            query += " METAView_" + objectType +
                "_Listing.*, ActiveDiagramObjects.[File Name],WorkspaceName AS [Object Workspace] FROM ActiveDiagramObjects INNER JOIN METAView_" +
                objectType + "_Listing ON ActiveDiagramObjects.MetaObjectID = METAView_" + objectType +
                "_Listing.pkid AND  ActiveDiagramObjects.Machine = METAView_" + objectType + "_Listing.Machine";
            if (useFilter)
            {
                //Append workspace
                query = query + " AND (METAView_" + objectType + "_Listing.WorkspaceName = '" + comboBoxWorkspace.Text + "')";
            }
            //Load
            try
            {
                DataSet ds = new DataSet();
                SqlCommand cmd =
                    new SqlCommand(query, new SqlConnection(checkBoxServer.Checked ? Core.Variables.Instance.ServerConnectionString : Core.Variables.Instance.ConnectionString));
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 6000;
                SqlDataAdapter dap = new SqlDataAdapter();
                dap.SelectCommand = cmd;
                dap.Fill(ds);
                dsReport = ds;
                //dsReport = d.DataRepository.Provider.ExecuteDataSet(CommandType.Text, query);
                labelInfo.Text = dsReport.Tables[0].Rows.Count.ToString() + Environment.NewLine + System.DateTime.Now.TimeOfDay.ToString();
                dataSetTOdataGrid();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(this,ex.ToString());
                MessageBox.Show(this, "This may be because there are too many records to return.", "Cannot return results");
                LogEntry lentry = new LogEntry();
                lentry.Message = query + Environment.NewLine + ex.ToString();
                lentry.Title = "Basic Report Fail";
                Logger.Write(lentry);
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
                a.Items.Clear();
                //Parent Class
                b.TList<b.ClassAssociation> parentAssociation = d.DataRepository.Connections[Provider].Provider.ClassAssociationProvider.GetByParentClass(p.Text);
                //Child Class
                //b.TList<b.ClassAssociation> childAssociation = d.DataRepository.Connections[Provider].Provider.ClassAssociationProvider.GetByChildClass(c.Text);

                a.Items.Add("All");
                foreach (b.ClassAssociation pAssociationItem in parentAssociation)
                {
                    b.AssociationType pAssociationType = d.DataRepository.Connections[Provider].Provider.AssociationTypeProvider.GetBypkid(pAssociationItem.AssociationTypeID);
                    //Where they match Add
                    if (pAssociationItem.ChildClass == c.Text)
                        a.Items.Add(pAssociationType.Name.Trim());
                    //foreach (b.ClassAssociation cAssociationItem in childAssociation)
                    //{
                    //    b.AssociationType cAssociationType = d.DataRepository.Connections[Provider].Provider.AssociationTypeProvider.GetBypkid(cAssociationItem.AssociationTypeID);

                    //    if (pAssociationType.Name.Trim() == cAssociationType.Name.Trim())
                    //    {
                    //        if (!a.Items.Contains(pAssociationType.Name.Trim()))
                    //        {
                    //            a.Items.Add(pAssociationType.Name.Trim());
                    //        }
                    //    }
                    //}
                }
                if (p.Text == "Process" && c.Text == "DataView")
                {
                    a.SelectedItem = "Create";
                }
                else
                {
                    a.SelectedItem = "Mapping";
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

        private string myDescription = "";
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
                    labelNodeDescription.Text = "";
                }
                else
                {
                    //MessageBox.Show(this,query);
                    buttonPredefinedGo.Enabled = true;
                    buttonPredefinedGo.Tag = query;
                    labelNodeDescription.Text = myDescription;
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
                    if (node.Attributes["Description"] != null)
                    {
                        myDescription = node.Attributes["Description"].Value;
                    }

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
                Core.Log.WriteLog(ex.ToString());
                return "No Query";
            }
        }
        private string removeTFN(string query)
        {
            query = query.Replace("dbo.TrimFileName", "");
            return query;
        }

        #endregion

        //This removes the filter if there is one
        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            filterBar.RowFilter = "";
            filterBar.ResetFilterRow(gridDataBoundGridMain);
        }

        private void Reports_Load(object sender, EventArgs e)
        {

        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.Dispose(Disposing);
        }
    }
}