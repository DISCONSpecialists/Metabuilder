using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using x = MetaBuilder.BusinessFacade.Exports;
using MetaBuilder.Meta;
using MetaBuilder.BusinessLogic;

using System.Reflection;
//using Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;

namespace ShapeBuilding
{
    public partial class Example : Form
    {

        System.Data.DataTable dt = new System.Data.DataTable("Excel Table");
        System.Data.DataTable errdt = new System.Data.DataTable("Error Table");
        System.Data.DataTable exdt = new System.Data.DataTable("Existing Table");
        int CAID = 0;

        public Example()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region Example of an insert
            /*
            b.Class newClass = new MetaBuilder.BusinessLogic.Class();
            newClass.Name = "Example Class";
            newClass.DescriptionCode = "DescCode";
            newClass.Category = "My Cat";
            newClass.ClassType = "Primary";
            d.DataRepository.ClassProvider.Save(newClass);*/
            #endregion

            #region Example of a delete
            b.Class classToDelete = d.DataRepository.ClassProvider.GetByName("Example Class");
            d.DataRepository.ClassProvider.Delete(classToDelete);
            #endregion

            #region Insert object association
            b.ObjectAssociation oa = new MetaBuilder.BusinessLogic.ObjectAssociation();
            oa.CAid = 0;
            oa.ObjectID = 0; // left object
            oa.ObjectMachine = "";// left object machine name
            oa.ChildObjectID = 0;//right object pkid
            oa.ChildObjectMachine = "";// right object machine name
            oa.VCStatusID = 7; // default is None
            d.DataRepository.ObjectAssociationProvider.Save(oa);
            #endregion

            #region List Insert
            b.TList<b.ObjectAssociation> batchList =
                new MetaBuilder.BusinessLogic.TList<MetaBuilder.BusinessLogic.ObjectAssociation>();

            batchList.Add(oa);
            // add more items

            d.DataRepository.ObjectAssociationProvider.Save(batchList);
            #endregion

            b.TList<b.Class> classesindb = d.DataRepository.ClassProvider.GetAll();
            foreach (b.Class classItem in classesindb)
            {
                this.textBoxClassList.Text += classItem.Name + Environment.NewLine;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //if (openFileDialog1.ShowDialog(this) is DialogResult.OK)
            openFileDialog1.ShowDialog(this);
            textboxExcelFilename.Text = openFileDialog1.FileName;
            try
            {
                //Clear selectable columns
                comboBoxLeftColumn.Items.Clear();
                comboBoxRightColumn.Items.Clear();
                //Get excel grid
                ImportExcel();
                //Now add left and right column selections
                foreach (System.Data.DataColumn dc in dt.Columns)
                {
                    comboBoxLeftColumn.Items.Add(dc.ColumnName.ToString());
                    comboBoxRightColumn.Items.Add(dc.ColumnName.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.ToString());
            }
        }

        private void ImportExcel()
        {
            progressBarMain.Value = 0;
            x.ExcelUtil util = new x.ExcelUtil(); //instance
            util.OpenExcel();

            try
            {
                util.OpenFile(textboxExcelFilename.Text); //edit

                string parentobject = (string)util.CurrentSheet.get_Range("A1", "A1").Text;

                List<string> cols = new List<string>();
                List<string> errenousimportrows = new List<string>();

                string[] alphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", 
                                                    "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai","aj","ak","al","am","an","ao","ap","aq","ar","as","at","au","av","aw","ax","ay","az",
                                                    "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj","bk","bl","bm","bn","bo","bp","bq","br","bs","bt","bu","bv","bw","bx","by","bz"};
                int alphabetCounter = 0;
                int rowCounter = 1;
                int rowFetcher = 0;

                string txt = GetText(alphabet[alphabetCounter], rowCounter, util);
                while (!string.IsNullOrEmpty(txt) || (alphabetCounter == 0))
                {
                    //cols.Add(txt);
                    dt.Columns.Add(txt);
                    alphabetCounter++;
                    txt = GetText(alphabet[alphabetCounter], rowCounter, util);
                }

                //foreach (string s in cols)
                //{
                //    dt.Columns.Add(s); //Adding column list ^^
                //}

                int NumberOfColumns = alphabetCounter;
                string[] values = new string[NumberOfColumns];
                rowCounter++;
                alphabetCounter = 0;

                System.Data.DataRow dr;

                //Each row
                int rc = util.CurrentSheet.Rows.Count; //65536 always :(
                errenousimportrows.Clear();
                progressBarMain.Maximum = rc;
                this.Cursor = Cursors.WaitCursor;
                excelGridView.DataSource = dt;

                ////Find a way to get the correct number of rows first
                //if (rc > 500)
                //{
                //    MessageBox.Show(this,"There are more than 500 rows which will be imported. This could take a few minutes to a few hours depending on the number of rows and the speed of your computer.", "Large Excel File - " + rc.ToString() + " rows detected",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                //}

                for (rowCounter = 2; rowCounter <= 100; rowCounter++) // for (rowCounter = 2; rowCounter <= rc; rowCounter++)
                {
                    rowFetcher = rowCounter;
                    //NewRow
                    dr = dt.NewRow();
                    //Each Column
                    for (alphabetCounter = 0; alphabetCounter < NumberOfColumns; alphabetCounter++)
                    {
                        if (string.IsNullOrEmpty(GetText(alphabet[alphabetCounter], rowFetcher, util)) && alphabetCounter == 0) //End of rows?
                        {
                            //rowCounter = rc; //Next round will kill loop, since first row column is empty this may skip rows
                        }
                        try
                        {
                            dr[alphabetCounter] = (GetText(alphabet[alphabetCounter], rowFetcher, util));
                        }
                        catch (Exception gex)
                        {
                            //Metabuilder.Core.Log.WriteLog(gex.ToString());
                            errenousimportrows.Add(rowFetcher.ToString()); //rowfetcher for row number that had problem
                        }
                        //Next Column
                    }
                    //Add Row
                    dt.Rows.Add(dr);
                    //Next Row
                    progressBarMain.Increment(progressBarMain.Step);
                }

                util.CloseExcel();
                progressBarMain.Value = progressBarMain.Maximum;
                this.Cursor = Cursors.Default;
            }
            catch (ObjectDisposedException x)
            {
                MessageBox.Show(this,x.ToString());
            }
            catch (Exception importException)
            {
                MessageBox.Show(this,importException.ToString());
            }

        }

        private string GetText(string col, int row, x.ExcelUtil util)
        {
            return (string)(string)util.CurrentSheet.get_Range(col + row.ToString(), col + row.ToString()).Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            findServers();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            findDatabases(comboBoxServerName.Text);
        }

        private void findServers()
        {
            /*  string[] servers = DBGrep.SqlLocator.GetServers();
              if (servers != null)
              {
                  comboBoxServerName.DataSource = servers;
                  //MessageBox.Show(this,"SQL Servers have been found");
                  comboBoxServerName.SelectedIndex = 0;
              }
              else
              {
                  MessageBox.Show(this,"No SQL servers found.");
              }*/
        }

        private void findDatabases(string server)
        {
            if (string.IsNullOrEmpty(server))
            {
                MessageBox.Show(this,"A server has yet to be selected", "Server not selected");
                return;
            }
            try
            {
                // Substitute your connection string below in conxString 
                server.ToUpper();
                String conxString = string.Empty;
                conxString = "Data Source=";
                conxString += server;
                conxString += "; Integrated Security=True;";

                using (SqlConnection sqlConx = new SqlConnection(conxString))
                {
                    sqlConx.Open();
                    System.Data.DataTable tblDatabases = sqlConx.GetSchema("Databases");
                    sqlConx.Close();

                    foreach (System.Data.DataRow row in tblDatabases.Rows)
                    {
                        comboBoxDatabaseName.Items.Add(row["database_name"]);
                    }
                }
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show(this,sqlex.ToString(), "SQL Server Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.ToString(), "Unknown Error");
            }
            dbCheck();
        }

        private bool dbCheck()
        {
            if (!string.IsNullOrEmpty(comboBoxDatabaseName.Text))
            {
                panelCriteria.Visible = true;
                return true;
            }
            else
            {
                panelCriteria.Visible = false;
                return false;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dbCheck())
            {
                DialogResult dr = MessageBox.Show(this,"Would you like to use this connection string?", "Alter connection string", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    //Add connection string and set as dafault?


                    //Populate from database
                    loadObjects();
                }
                else
                {
                    //loadObjects();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            loadObjects();
        }

        private void loadObjects()
        {
            this.comboBoxParentClass.Items.Clear();
            this.comboBoxChildClass.Items.Clear();
            comboBoxParentClass.Text = "";
            comboBoxChildClass.Text = "";

            b.TList<b.Class> classesindb = d.DataRepository.ClassProvider.GetAll();

            foreach (b.Class classItem in classesindb)
            {
                this.comboBoxParentClass.Items.Add(classItem.Name);
                this.comboBoxChildClass.Items.Add(classItem.Name);
            }

            this.comboBoxParentClass.Items.Add("None");
            this.comboBoxParentClass.SelectedText = "None";
            this.comboBoxChildClass.Items.Add("None");
            this.comboBoxChildClass.SelectedText = "None";

            listBoxParentFields.Items.Clear();
            listBoxChildFields.Items.Clear();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            childChanged();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            parentChanged();
        }

        private void parentChanged()
        {
            //Populate fields
            populateFields(comboBoxParentClass.Text, listBoxParentFields);
            populateAssociations();
        }

        private void childChanged()
        {
            //Populate fields
            populateFields(comboBoxChildClass.Text, listBoxChildFields);
            populateAssociations();
        }

        private void populateAssociations()
        {
            CAID = -1;
            comboBoxAssociationType.Items.Clear();
            comboBoxAssociationType.Visible = false;

            if (comboBoxParentClass.Text != "None" && comboBoxChildClass.Text != "None")
            {

                comboBoxAssociationType.Visible = true;

                b.TList<b.ClassAssociation> allAssociation = d.DataRepository.ClassAssociationProvider.GetByParentClass(comboBoxParentClass.Text);
                foreach (b.ClassAssociation associationItem in allAssociation)
                {
                    b.AssociationType associationType = d.DataRepository.AssociationTypeProvider.GetBypkid(associationItem.AssociationTypeID);
                    if (comboBoxAssociationType.Items.Contains(associationType.Name))
                    {
                        //Duplicates? Distinct?
                    }
                    else
                    {
                        comboBoxAssociationType.Items.Add(associationType.Name); //?
                    }
                }
            }
        }

        private void populateFields(string objectClass, System.Windows.Forms.ListBox lb)
        {
            //Clear
            lb.Items.Clear();
            //Get Fields
            b.TList<b.Field> fields = d.DataRepository.FieldProvider.GetByClass(objectClass);
            foreach (b.Field fieldItem in fields)
            {
                lb.Items.Add(fieldItem.Name);
            }
        }

        private void button2_Click(object sender, EventArgs e) //Start
        {

            b.TList<b.ObjectAssociation> batchList = new MetaBuilder.BusinessLogic.TList<MetaBuilder.BusinessLogic.ObjectAssociation>();

            errdt.Clear();
            exdt.Clear();

            progressBarMain.Value = 0;
            System.Data.DataRow dri;
            MetaObjectKey K;

            if (CAID <= 0)
            {
                MessageBox.Show(this,"You have not selected a type of association yet", "Association missing");
                return;
            }

            bool dontAdd = false;

            int rowc = -1;
            int rowfail = 0;
            int addedObjects = 0;

            int parentobjectID = 0;
            int childobjectID = 0;
            int vcStatusID = 7;

            string parentobjectMachine = string.Empty;
            string childobjectMachine = string.Empty;
            string machine = string.Empty;
            string vcMachineID = string.Empty;
            string map = string.Empty;
            string leftside = string.Empty;
            string rightside = string.Empty;

            progressBarMain.Maximum = dt.Rows.Count;

            try
            {
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    rowc++;

                    leftside = dr[comboBoxLeftColumn.Text].ToString().Trim();
                    rightside = dr[comboBoxRightColumn.Text].ToString().Trim();

                    if ((leftside == "") || (rightside == ""))
                    {
                        //null strings fail
                        rowfail++;
                    }
                    else
                    {
                        #region "Left Side"
                        try
                        {
                            //Check Left Side

                            b.TList<ObjectFieldValue> op = d.DataRepository.ObjectFieldValueProvider.GetByValueString(leftside);
                            if (op.Count > 0)
                            {
                                foreach (b.ObjectFieldValue o in op) //Should be one only?
                                {
                                    parentobjectID = o.ObjectID;
                                    K = new MetaObjectKey();
                                    K.pkid = parentobjectID;
                                    parentobjectMachine = d.DataRepository.MetaObjectProvider.Get(K).Machine.ToString();
                                    if (dontAdd) { }
                                    else
                                    {
                                        dontAdd = false;
                                    }
                                }
                            }
                            else
                            {
                                //This object does not exist
                                dri = errdt.NewRow();
                                dri[0] = leftside;
                                dri[1] = comboBoxParentClass.Text;
                                dri[2] = rowc.ToString();
                                errdt.Rows.Add(dri);
                                dontAdd = true;
                            }
                        }
                        catch (Exception lex)
                        {
                            MessageBox.Show(this,lex.ToString(), "Left Side Error on row " + rowc.ToString());
                        }
                        #endregion
                        #region "Right Side"
                        try
                        {
                            //Check Right Side

                            b.TList<ObjectFieldValue> po = d.DataRepository.ObjectFieldValueProvider.GetByValueString(rightside);
                            if (po.Count > 0)
                            {
                                foreach (b.ObjectFieldValue p in po) //Should be one only
                                {
                                    childobjectID = p.ObjectID;
                                    K = new MetaObjectKey();
                                    K.pkid = childobjectID;
                                    childobjectMachine = d.DataRepository.MetaObjectProvider.Get(K).Machine.ToString();
                                    if (dontAdd) { }
                                    else
                                    {
                                        dontAdd = false;
                                    }
                                }
                            }
                            else
                            {

                                //This object does not exist
                                dri = errdt.NewRow();
                                dri[0] = rightside;
                                dri[1] = comboBoxChildClass.Text;
                                dri[2] = rowc.ToString();
                                errdt.Rows.Add(dri);
                                dontAdd = true;
                            }
                        }
                        catch (Exception rex)
                        {
                            MessageBox.Show(this,rex.ToString(), "Right Side Error on row " + rowc.ToString());
                        }
                        #endregion
                        #region "Association"
                        try
                        {
                            //Check if association exists

                            //b.TList<ObjectAssociation> q = d.DataRepository.ObjectAssociationProvider.Find("caid = " + CAID + " and objectid = " + parentobjectID + " and childobjectid = " + childobjectID); //+ " and objectmachine = " + parentobjectMachine + " and childobjectmachine = " + childobjectMachine);//d.DataRepository.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(CAID, parentobjectID, childobjectID, parentobjectMachine, childobjectMachine);
                            int q = d.DataRepository.ObjectAssociationProvider.GetTotalItems("cAid = " + CAID + " AND ObjectID = " + parentobjectID + " AND ChildObjectID = " + childobjectID + " AND ObjectMachine = '" + parentobjectMachine + "' AND ChildObjectMachine = '" + childobjectMachine + "'", out q);

                            if (q > 0)
                            {
                                //This association already exists
                                dri = exdt.NewRow();
                                dri[0] = leftside;
                                dri[1] = map;
                                dri[2] = rightside;
                                dri[3] = rowc.ToString();
                                exdt.Rows.Add(dri);
                                dontAdd = true;
                            }
                            else
                            {
                                if (dontAdd)
                                {
                                    //either object does not exist
                                    //or
                                    //association exists now
                                }
                                else
                                {
                                    //Set variables
                                    b.ObjectAssociation objAss = new b.ObjectAssociation();
                                    objAss.ObjectID = parentobjectID;
                                    objAss.ObjectMachine = parentobjectMachine;

                                    objAss.ChildObjectID = childobjectID;
                                    objAss.ChildObjectMachine = childobjectMachine;

                                    objAss.VCStatusID = vcStatusID;
                                    objAss.VCMachineID = vcMachineID;
                                    objAss.Machine = machine;

                                    //Add it
                                    //d.DataRepository.ObjectAssociationProvider.Insert(objAss); 
                                    batchList.Add(objAss);
                                    addedObjects++;
                                }
                            }
                        }
                        catch (Exception aex)
                        {
                            MessageBox.Show(this,aex.ToString(), "Association Error on row " + rowc.ToString());
                        }
                        #endregion

                    }
                    progressBarMain.Increment(progressBarMain.Step);
                }
                //d.DataRepository.ObjectAssociationProvider.Save(batchList);
                progressBarMain.Value = progressBarMain.Maximum;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.ToString(), "Error - Click");
            }

            int difference = (rowc - rowfail);
            MessageBox.Show(this,"There were " + rowfail.ToString() + " rows which were skipped due to either the left or right class name being nothing." + Environment.NewLine + Environment.NewLine + addedObjects.ToString() + " associations have been added out of the " + difference.ToString() + " associations that should have been added", "Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
            consolidateErrors(dontAdd);
        }

        private void consolidateErrors(bool e)
        {
            if (e)
            {
                string eObjects = "There are " + errdt.Rows.Count + " objects which do not exist";
                string eAssociations = "There are " + exdt.Rows.Count + " associations which have already been created";

                DialogResult d = MessageBox.Show(this,eObjects + Environment.NewLine + Environment.NewLine + eAssociations + Environment.NewLine + "Would you like to view these errors now?", "View Errors?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (d == DialogResult.Yes)
                {
                    //Set datasources
                    dataGridViewExistingAssociations.DataSource = exdt;
                    dataGridViewNonExistentObjects.DataSource = errdt;
                    //Show panel
                    panelErrors.BringToFront();
                    panelErrors.Visible = true;
                }
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e) //Left
        {
            if (comboBoxLeftColumn.Text == "") buttonStart.Enabled = false;
            else buttonStart.Enabled = true;
            CAID = -1;
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e) //Right
        {
            if (comboBoxRightColumn.Text == "") buttonStart.Enabled = false;
            else buttonStart.Enabled = true;
            CAID = -1;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e) //Association
        {
            CAID = d.DataRepository.AssociationTypeProvider.GetByName(comboBoxAssociationType.Text.Trim()).pkid;
        }

        private void Example_Load(object sender, EventArgs e)
        {
            createTables();

            panelErrors.SendToBack();
            panelErrors.Visible = false;
        }

        private void createTables()
        {
            //Table for objects which dont exist
            errdt.Columns.Add(new System.Data.DataColumn("Non-Existent Object"));
            errdt.Columns.Add(new System.Data.DataColumn("Class"));
            errdt.Columns.Add(new System.Data.DataColumn("Row Number"));
            //Table for associations which did already exist
            exdt.Columns.Add(new System.Data.DataColumn("Parent Object"));
            exdt.Columns.Add(new System.Data.DataColumn("Association"));
            exdt.Columns.Add(new System.Data.DataColumn("Child Object"));
            exdt.Columns.Add(new System.Data.DataColumn("Row Number"));
        }

        private void panelCriteria_VisibleChanged(object sender, EventArgs e)
        {
            if (panelCriteria.Visible)
            {
                loadObjects();
            }
        }

        private void buttonCloseErrorPanel_Click(object sender, EventArgs e)
        {
            panelErrors.SendToBack();
            panelErrors.Visible = false;
        }

        private void buttonAddObjects_Click(object sender, EventArgs e)
        {
            object val;
            string pname;
            progressBarObjectAddition.Value = 0;
            progressBarObjectAddition.Maximum = dataGridViewNonExistentObjects.Rows.Count;
            progressBarObjectAddition.Visible = true;
            foreach (System.Data.DataRow dr in errdt.Rows)
            {
                MetaBase mb = Loader.CreateInstance(dr[1].ToString());
                pname = dr[1].ToString();
                val = dr[0].ToString();
                mb.Set(pname, val);
                //if (mb.IsInDatabase)
                //{
                //    //Pass
                //}
                //else
                //{
                //    //mb.Save(Guid.NewGuid());
                //}
                progressBarObjectAddition.Increment(progressBarObjectAddition.Step);
            }
            progressBarObjectAddition.Visible = false;
            //b.MetaObject newObject;
            //b.ObjectFieldValue newField;
            ////Add field?
            //newField = new ObjectFieldValue();
            //newField.ValueString = dr[0].ToString();
            ////d.DataRepository.ObjectFieldValueProvider.Insert(newField);

            ////Add object with this new field id
            //newObject = new MetaObject();
            //newObject.Class = dr[1].ToString();
            ////d.DataRepository.MetaObjectProvider.Insert(newObject);
        }
    }
}