using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MetaBuilder.Meta;
using XPTable;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Graphing.Shapes.Binding;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.UIControls.GraphingUI.Tools;
using Northwoods.Go;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Collections.ObjectModel;

namespace MetaBuilder.UIControls.Dialogs
{
    public partial class RefreshData : Form
    {
        private MetaBuilder.Graphing.Containers.GraphView view;
        public MetaBuilder.Graphing.Containers.GraphView View
        {
            get { return view; }
            set { view = value; }
        }

        private Dictionary<string, List<DifferingData>> differingObjects;
        public Dictionary<string, List<DifferingData>> DifferingObjects
        {
            get { return differingObjects; }
            set { differingObjects = value; }
        }

        public RefreshData()
        {
            InitializeComponent();
            table1.CellDoubleClick += new XPTable.Events.CellMouseEventHandler(table1_CellDoubleClick);
            table2.CellDoubleClick += new XPTable.Events.CellMouseEventHandler(table2_CellDoubleClick);

            foreach (XPTable.Models.Column col in table1.ColumnModel.Columns)
                col.Sortable = false;
            foreach (XPTable.Models.Column col in table2.ColumnModel.Columns)
                col.Sortable = false;
        }

        private void table1_CellDoubleClick(object sender, XPTable.Events.CellMouseEventArgs e)
        {
            SelectNode(e);
        }
        private void table2_CellDoubleClick(object sender, XPTable.Events.CellMouseEventArgs e)
        {
            SelectNode(e);
        }
        private void SelectNode(XPTable.Events.CellMouseEventArgs e)
        {
            if (e.Cell.Row.Tag is DifferingData)
            {
                DifferingData dd = e.Cell.Row.Tag as DifferingData;
                if (dd.Node != null)
                {
                    ScrollToNode(dd.Node);
                }
            }
            else
            {
                DifferingLinkData ddLink = e.Cell.Row.Tag as DifferingLinkData;
                if (ddLink != null)
                {
                    View.ScrollRectangleToVisible(ddLink.ObjectOnDiagram.Bounds);
                    view.Selection.Clear();
                    view.Selection.Add(ddLink.ObjectOnDiagram);
                }
            }
        }

        private void ScrollToNode(IMetaNode node)
        {
            if (view != null)
            {
                foreach (IMetaNode n in view.ViewController.GetNestedNodes(view.Document))
                {
                    if (n == node)
                    {
                        GoObject o = n as GoObject;
                        view.ScrollRectangleToVisible(o.Bounds);
                        view.Selection.Clear();
                        view.Selection.Add(o);
                    }
                }
            }
        }

        private void FormatRow(XPTable.Models.Row r, bool isDiagramTable, bool onDiagram, bool inDB)
        {
            r.RowStyle = new XPTable.Models.RowStyle();
            if (onDiagram && inDB)
            {
                r.RowStyle.BackColor = Color.Yellow;
                if (!isDiagramTable)
                    r.Cells[0].Checked = true;
                return;
            }
            if (onDiagram && isDiagramTable)
            {
                r.Cells[0].Checked = true;
                r.RowStyle.BackColor = Color.LightGreen;
                return;
            }

            if (inDB && (!(isDiagramTable)))
            {
                r.RowStyle.BackColor = Color.LightGreen;
                r.Cells[0].Checked = true;
                return;
            }

            r.RowStyle.BackColor = Color.LightPink;
        }

        private bool hasRows;
        public bool HasRows
        {
            get { return tableModelDB.Rows.Count > 0 && tableModel1.Rows.Count > 0 && tableModelDB.Rows.Count == tableModel1.Rows.Count; }
        }

        public void Populate()
        {
            foreach (KeyValuePair<string, List<DifferingData>> kvp in DifferingObjects)
            {
                List<DifferingData> DifferingObject = kvp.Value;
                foreach (DifferingData dd in DifferingObject)
                {
                    if (dd.DifferenceType == EDifferenceType.Equal)
                        continue;
                    MetaBase mb1 = dd.ObjectOnDiagram;
                    MetaBase mb2 = dd.ObjectInDatabase;

                    XPTable.Models.Row r = new XPTable.Models.Row();
                    r.Cells.Add(new XPTable.Models.Cell(0));
                    bool onDiagram = mb1 != null;
                    bool inDB = mb2 != null;
                    if (onDiagram)
                    {
                        r.Cells.Add(new XPTable.Models.Cell(mb1.ToString()));
                        r.Cells.Add(new XPTable.Models.Cell(mb1._ClassName));

                        if (mb1.WorkspaceName != null)
                            r.Cells.Add(new XPTable.Models.Cell(mb1.WorkspaceName.ToString()));
                        else
                            r.Cells.Add(new XPTable.Models.Cell(""));

                        r.Cells[0].Tag = mb1;
                        r.Cells[0].Enabled = inDB;
                        FormatRow(r, true, onDiagram, inDB);
                    }
                    else
                    {
                        r.Cells.Add(new XPTable.Models.Cell("Not on Diagram"));
                        r.Cells.Add(new XPTable.Models.Cell(""));
                        r.Cells.Add(new XPTable.Models.Cell(""));
                        r.RowStyle = new XPTable.Models.RowStyle();
                        r.RowStyle.BackColor = Color.LightPink;
                        FormatRow(r, true, onDiagram, inDB);
                    }
                    r.Tag = dd;
                    this.tableModel1.Rows.Add(r);

                    XPTable.Models.Row rDB = new XPTable.Models.Row();
                    rDB.Cells.Add(new XPTable.Models.Cell(1));
                    FormatRow(rDB, false, onDiagram, inDB);

                    if (mb2 != null)
                    {
                        rDB.Cells.Add(new XPTable.Models.Cell(mb2.ToString()));
                        rDB.Cells.Add(new XPTable.Models.Cell(mb2._ClassName));
                        rDB.Cells.Add(new XPTable.Models.Cell(mb2.WorkspaceName.ToString()));
                        rDB.Tag = dd;
                        rDB.Cells[0].Enabled = inDB;
                        rDB.Cells[0].Tag = mb2;
                    }
                    else
                    {
                        rDB.Cells.Add(new XPTable.Models.Cell("Not in DB"));
                        rDB.Cells.Add(new XPTable.Models.Cell(""));
                        rDB.Cells.Add(new XPTable.Models.Cell(""));
                        rDB.RowStyle = new XPTable.Models.RowStyle();
                        rDB.Cells[0].Enabled = inDB;
                        FormatRow(rDB, false, onDiagram, inDB);
                        rDB.Tag = null;
                    }
                    this.tableModelDB.Rows.Add(rDB);
                }
            }
        }

        public void PopulateLinks(LinkComparer linkComparer)
        {
            this.Text += " - Comparing Links ";
            Timer linkTimer = new Timer();
            linkTimer.Tick += new EventHandler(linkTimer_Tick);
            linkTimer.Interval = 300;
            linkTimer.Start();

            while (linkComparer.IsBusy)
            {
                System.Windows.Forms.Application.DoEvents();
            }

            linkTimer.Stop();
            linkTimer.Dispose();

            foreach (DifferingLinkData changedLink in linkComparer.ChangedLinks)
            {
                XPTable.Models.Row rDiagram = new XPTable.Models.Row();
                rDiagram.Cells.Add(new XPTable.Models.Cell(0));
                rDiagram.Cells.Add(new XPTable.Models.Cell("Link")); //changedLink.ObjectOnDiagram.ToString()));
                rDiagram.Cells.Add(new XPTable.Models.Cell(changedLink.ObjectOnDiagram.AssociationType.ToString()));
                rDiagram.Cells.Add(new XPTable.Models.Cell(""));
                rDiagram.Cells[0].Tag = changedLink;
                rDiagram.Cells[0].Enabled = (changedLink.ObjectInDatabase != null); ;
                rDiagram.Tag = changedLink;
                FormatRow(rDiagram, true, true, changedLink.ObjectInDatabase != null);
                this.tableModel1.Rows.Add(rDiagram);

                XPTable.Models.Row rDatabase = new XPTable.Models.Row();
                rDatabase.Cells.Add(new XPTable.Models.Cell(1));
                rDatabase.Cells.Add(new XPTable.Models.Cell(changedLink.ObjectInDatabase != null ? "Link" : "Not in DB"));
                rDatabase.Cells.Add(new XPTable.Models.Cell(changedLink.ObjectInDatabase != null ? changedLink.ObjectInDatabase.AssociationType.ToString() : ""));
                rDatabase.Cells.Add(new XPTable.Models.Cell(""));
                rDatabase.Cells[0].Tag = changedLink;
                rDatabase.Cells[0].Enabled = (changedLink.ObjectInDatabase != null);
                rDatabase.Tag = changedLink;
                FormatRow(rDatabase, false, true, changedLink.ObjectInDatabase != null);
                this.tableModelDB.Rows.Add(rDatabase);
            }

            foreach (DifferingLinkData dData in linkComparer.RemovedFromModelLinks)
            {
                XPTable.Models.Row rDiagram = new XPTable.Models.Row();
                rDiagram.Cells.Add(new XPTable.Models.Cell(0));
                rDiagram.Cells.Add(new XPTable.Models.Cell("Not On Diagram")); //changedLink.ObjectOnDiagram.ToString()));
                rDiagram.Cells.Add(new XPTable.Models.Cell(""));
                rDiagram.Cells.Add(new XPTable.Models.Cell(""));
                rDiagram.Cells[0].Tag = null;
                rDiagram.Cells[0].Enabled = false;
                rDiagram.Tag = null;
                FormatRow(rDiagram, true, false, true);
                this.tableModel1.Rows.Add(rDiagram);

                XPTable.Models.Row rDatabase = new XPTable.Models.Row();
                rDatabase.Cells.Add(new XPTable.Models.Cell(1));
                rDatabase.Cells.Add(new XPTable.Models.Cell("Link"));
                rDatabase.Cells.Add(new XPTable.Models.Cell(dData.DBAssociationType.ToString()));
                rDatabase.Cells.Add(new XPTable.Models.Cell(""));
                rDatabase.Cells[0].Tag = null; //this must be a qlink
                rDatabase.Cells[0].Enabled = true;
                rDatabase.Tag = null; //this must be a qlink
                FormatRow(rDatabase, false, false, true);
                this.tableModelDB.Rows.Add(rDatabase);
            }

            this.Text = this.Text.Replace(" - Comparing Links ", "");
            this.Text = this.Text.Replace(".", "");

            BringToFront();
        }

        private void linkTimer_Tick(object sender, EventArgs e)
        {
            if (this.Text.Contains("....."))
                this.Text = this.Text.Replace(".....", "");
            this.Text += ".";

            System.Windows.Forms.Application.DoEvents();
        }

        private void table1_CellMouseUp(object sender, XPTable.Events.CellMouseEventArgs e)
        {
            DifferingData dd = e.Cell.Row.Tag as DifferingData;
            if (dd != null)
            {
                MetaBase mb = dd.ObjectOnDiagram;
                if (e.Column == 0)
                {
                    DeselectCorrespondingRecord(mb, table2, e);
                }
                propertyGrid1.SelectedObject = mb;
            }
            else
            {
                if (e.Cell.Row.Tag is DifferingLinkData)
                {
                    MetaBuilder.MetaControls.MetaPropertyGridDocker.QlinkObject q = new MetaBuilder.MetaControls.MetaPropertyGridDocker.QlinkObject((e.Cell.Row.Tag as DifferingLinkData).ObjectOnDiagram);
                    propertyGrid1.SelectedObject = q;

                    table2.TableModel.Rows[e.Cell.Row.Index].Cells[0].Checked = !e.Cell.Checked;
                }
            }

            table2.TableModel.Selections.AddCell(e.CellPos);
            table2.EnsureVisible(e.CellPos);
            bindDatabase(table2);
        }

        private void table2_CellMouseUp(object sender, XPTable.Events.CellMouseEventArgs e)
        {
            DifferingData dd = e.Cell.Row.Tag as DifferingData;
            if (dd != null)
            {
                MetaBase mb = dd.ObjectInDatabase;
                if (e.Column == 0)
                {
                    DeselectCorrespondingRecord(mb, table1, e);
                }
                propertyGrid2.SelectedObject = mb;
            }
            else
            {
                if (e.Cell.Row.Tag is DifferingLinkData)
                {
                    MetaBuilder.MetaControls.MetaPropertyGridDocker.QlinkObject q = new MetaBuilder.MetaControls.MetaPropertyGridDocker.QlinkObject((e.Cell.Row.Tag as DifferingLinkData).ObjectInDatabase);
                    propertyGrid2.SelectedObject = q;

                    table1.TableModel.Rows[e.Cell.Row.Index].Cells[0].Checked = !e.Cell.Checked;
                }
            }
            table1.TableModel.Selections.AddCell(e.CellPos);
            table1.EnsureVisible(e.CellPos);
            bindDiagram(table1);
        }

        //5 November 2013
        private void bindDiagram(XPTable.Models.Table table)
        {
            if (table.SelectedItems.Length > 0)
            {
                if (table.SelectedItems[0].Cells[0].Tag is DifferingLinkData)
                {
                    MetaBuilder.MetaControls.MetaPropertyGridDocker.QlinkObject q = new MetaBuilder.MetaControls.MetaPropertyGridDocker.QlinkObject((table.SelectedItems[0].Cells[0].Tag as DifferingLinkData).ObjectOnDiagram);
                    propertyGrid1.SelectedObject = q;
                    return;
                }
                propertyGrid1.SelectedObject = table.SelectedItems[0].Cells[0].Tag;
            }
        }
        //5 November 2013
        private void bindDatabase(XPTable.Models.Table table)
        {
            if (table.SelectedItems.Length > 0)
            {
                if (table.SelectedItems[0].Cells[0].Tag is DifferingLinkData)
                {
                    MetaBuilder.MetaControls.MetaPropertyGridDocker.QlinkObject q = new MetaBuilder.MetaControls.MetaPropertyGridDocker.QlinkObject((table.SelectedItems[0].Cells[0].Tag as DifferingLinkData).ObjectInDatabase);
                    propertyGrid2.SelectedObject = q;
                    return;
                }
                propertyGrid2.SelectedObject = table.SelectedItems[0].Cells[0].Tag;
            }
        }

        private void DeselectCorrespondingRecord(MetaBase mb, XPTable.Models.Table tbl, XPTable.Events.CellMouseEventArgs e)
        {
            tbl.TableModel.Rows[e.Cell.Row.Index].Cells[0].Checked = !e.Cell.Checked;
            tbl.EnsureVisible(e.Cell.Row.Index, 0);
            /*//table.TableModel.Selections.AddCell(new XPTable.Models.CellPos(selectedIndices[i], 0));
            foreach (XPTable.Models.Row r in tbl.TableModel.Rows)
            {
                if (mb != null)
                {
                    MetaBase mbR = r.Cells[0].Tag as MetaBase;
                    if (mbR != null)
                        if (mbR.pkid == mb.pkid && mbR.MachineName == mb.MachineName)
                        {
                            r.Cells[0].Checked = !ischecked;
                        }
                }
                else
                {
                    r.Cells[0].Checked = !ischecked;
                }
            }
             * */
        }

        private void CopyProperty(PropertyGrid gridFrom, PropertyGrid gridTo)
        {
            MetaBase mb = gridTo.SelectedObject as MetaBase;
            MetaBase mbThis = gridFrom.SelectedObject as MetaBase;
            if (mb != null && mbThis != null)
            {
                //if (gridFrom.SelectedObject != null && gridTo.SelectedObject != null)
                //{
                mb.Set(gridFrom.SelectedGridItem.Label, mbThis.Get(gridFrom.SelectedGridItem.Label));
                //}
            }
        }

        private void propertyGrid2_DoubleClick(object sender, EventArgs e)
        {
            CopyProperty(propertyGrid2, propertyGrid1);
        }
        private void propertyGrid1_DoubleClick(object sender, EventArgs e)
        {
            CopyProperty(propertyGrid1, propertyGrid2);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            //Dictionary<DifferingData, int> selectedValues = new Dictionary<DifferingData, int>();
            for (int i = 0; i < table1.TableModel.Rows.Count; i++)
            {
                XPTable.Models.Row r1 = table1.TableModel.Rows[i];
                XPTable.Models.Row r2 = table2.TableModel.Rows[i];

                if (r1.Tag is DifferingLinkData && r2.Tag is DifferingLinkData)
                {
                    DifferingLinkData diagramLinkData = r1.Tag as DifferingLinkData;
                    DifferingLinkData databaseLinkData = r2.Tag as DifferingLinkData;
                    if (r2.Cells[0].Checked && databaseLinkData != null && databaseLinkData.ObjectInDatabase != null)
                    {
                        diagramLinkData.ObjectOnDiagram.AssociationType = databaseLinkData.ObjectInDatabase.AssociationType;
                    }
                    continue;
                }

                DifferingData ddDiagram = null;
                DifferingData ddDB = null;
                if (r1.Tag != null)
                    ddDiagram = r1.Tag as DifferingData;

                if (r2.Tag != null)
                    ddDB = r2.Tag as DifferingData;

                //if (ddDiagram == null && ddDB != null)
                //{
                //    // in DB, not on diagram.
                //    //Console.WriteLine(ddDB.ToString());
                //}

                if (ddDiagram != null && ddDB != null)
                {
                    if (r2.Cells[0].Checked)
                        // in DB, and on diagram.
                        if (ddDiagram.ObjectOnDiagram == null && ddDiagram.Section != null)
                        {
                            RepeaterSection rsection = ddDiagram.Section;
                            CollapsingRecordNodeItem item = rsection.AddItemFromCode() as CollapsingRecordNodeItem;
                            item.MetaObject = ddDB.ObjectInDatabase;
                            item.HookupEvents();
                            item.BindToMetaObjectProperties();
                            item.Shadowed = true;
                        }
                        else
                        {
                            if (ddDiagram.ObjectOnDiagram == null || ddDB.Node == null)
                            {
                                //add this node to the diagram since it was selected?
                            }
                            else
                            {
                                ddDB.ObjectInDatabase.CopyPropertiesToObject(ddDiagram.ObjectOnDiagram);
                                //ddDiagram.ObjectOnDiagram.OnChanged(EventArgs.Empty);
                                ddDiagram.Node.HookupEvents();
                                ddDiagram.Node.BindToMetaObjectProperties();
                                (ddDiagram.Node as GoObject).Shadowed = true;
                            }
                        }
                    //Console.WriteLine(ddDB.ToString());
                }

                //if (ddDiagram != null && ddDB == null)
                //{
                //    // Not in DB, but on diagram. no update.

                //}
            }
            Close();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (XPTable.Models.Row r in table1.TableModel.Rows)
            {
                r.Cells[0].Checked = true;
            }
            foreach (XPTable.Models.Row r in table2.TableModel.Rows)
            {
                r.Cells[0].Checked = false;
            }
        }

        private void btnSelectAll2_Click(object sender, EventArgs e)
        {
            foreach (XPTable.Models.Row r in table2.TableModel.Rows)
            {
                r.Cells[0].Checked = true;
            }
            foreach (XPTable.Models.Row r in table1.TableModel.Rows)
            {
                r.Cells[0].Checked = false;
            }
        }

        private void table1_SelectionChanged(object sender, XPTable.Events.SelectionEventArgs e)
        {
            if (table1.Tag == null)
            {
                table1.Tag = 0;
                SelectCorrespondingRow(table2, e.NewSelectedIndicies);
                table1.Tag = null;
            }
        }

        private void SelectCorrespondingRow(XPTable.Models.Table table, int[] selectedIndices)
        {
            //DONT USE THE INDEX!

            table.TableModel.Selections.Clear();
            for (int i = 0; i < selectedIndices.Length; i++)
            {
                table.TableModel.Selections.AddCell(new XPTable.Models.CellPos(selectedIndices[i], 0));
            }
        }

        private void table2_SelectionChanged(object sender, XPTable.Events.SelectionEventArgs e)
        {
            if (table2.Tag == null)
            {
                table2.Tag = 0;
                SelectCorrespondingRow(table1, e.NewSelectedIndicies);
                table2.Tag = null;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOK_Click(this, EventArgs.Empty);
                return;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnCancel_Click(this, EventArgs.Empty);
                return;
            }
            base.OnKeyUp(e);
        }

        private void metaButtonExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog exportFileDialog = new SaveFileDialog();
            exportFileDialog.InitialDirectory = Core.Variables.Instance.ExportsPath;
            exportFileDialog.FileName = "Differing Data - " + Core.strings.GetFileNameWithoutExtension(View.Document.Name) + ".xls";
            exportFileDialog.Title = "Choose a filename to export the differences between data.";
            exportFileDialog.AutoUpgradeEnabled = true;
            exportFileDialog.AddExtension = true;
            DialogResult result = exportFileDialog.ShowDialog();

            if (exportFileDialog.FileName.Length == 0 || result != DialogResult.OK)
                return;

            MetaBuilder.BusinessFacade.Exports.ExcelUtil excelUtil = new MetaBuilder.BusinessFacade.Exports.ExcelUtil();

            excelUtil.OpenExcel();
            excelUtil.CreateWorkbook();

            string[] alphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", 
                                                "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai","aj","ak","al","am","an","ao","ap","aq","ar","as","at","au","av","aw","ax","ay","az",
                                                "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj","bk","bl","bm","bn","bo","bp","bq","br","bs","bt","bu","bv","bw","bx","by","bz",
                                                "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj","ck","cl","cm","cn","co","cp","cq","cr","cs","ct","cu","cv","cw","cx","cy","cz",
                                                "da", "db", "dc", "dd", "de", "df", "dg", "dh", "di", "dj","dk","dl","dm","dn","do","dp","dq","dr","ds","dt","du","dv","dw","dx","dy","dz"};

            Dictionary<string, _Worksheet> WorkbookSheets = new Dictionary<string, _Worksheet>();
            Dictionary<string, int> ClassRowIndex = new Dictionary<string, int>();
            //key is metakey and value is list of data for that key
            int rowIndex = 3;
            foreach (KeyValuePair<string, List<DifferingData>> k in DifferingObjects)
            {
                KeyValuePair<string, List<DifferingData>> kvp = k;
                foreach (DifferingData d in kvp.Value)
                {
                    DifferingData dd = d;
                    _Worksheet classSheet = null;
                    int alphabetCounter = 0;
                    WorkbookSheets.TryGetValue(dd.ObjectOnDiagram.Class, out classSheet);
                    if (classSheet == null)
                    {
                        rowIndex = 3;

                        excelUtil.AddSheet(dd.ObjectOnDiagram.Class);
                        WorkbookSheets.Add(dd.ObjectOnDiagram.Class, excelUtil.CurrentSheet);
                        WorkbookSheets.TryGetValue(dd.ObjectOnDiagram.Class, out classSheet);
                        ClassRowIndex.Add(dd.ObjectOnDiagram.Class, rowIndex);

                        #region Columns

                        classSheet.get_Range("A1", "A1").Value2 = "Diagram";
                        classSheet.get_Range("A2", "A2").Value2 = "PKID";
                        classSheet.get_Range("B2", "B2").Value2 = "Machine";
                        classSheet.get_Range("C2", "C2").Value2 = "Class";
                        alphabetCounter = 3;
                        foreach (PropertyInfo info in dd.ObjectOnDiagram.GetMetaPropertyList(false))
                        {
                            classSheet.get_Range(alphabet[alphabetCounter] + "2", alphabet[alphabetCounter] + "2").Value2 = info.Name;
                            alphabetCounter++;
                        }
                        classSheet.get_Range(alphabet[alphabetCounter] + "2", alphabet[alphabetCounter] + "2").Value2 = "Difference";
                        alphabetCounter++;

                        //dont increment
                        classSheet.get_Range(alphabet[alphabetCounter] + "1", alphabet[alphabetCounter] + "1").Value2 = "Database";

                        classSheet.get_Range(alphabet[alphabetCounter] + "2", alphabet[alphabetCounter] + "2").Value2 = "PKID";
                        alphabetCounter++;
                        classSheet.get_Range(alphabet[alphabetCounter] + "2", alphabet[alphabetCounter] + "2").Value2 = "Machine";
                        alphabetCounter++;
                        classSheet.get_Range(alphabet[alphabetCounter] + "2", alphabet[alphabetCounter] + "2").Value2 = "Class";
                        alphabetCounter++;

                        foreach (PropertyInfo info in dd.ObjectOnDiagram.GetMetaPropertyList(false))
                        {
                            classSheet.get_Range(alphabet[alphabetCounter] + "2", alphabet[alphabetCounter] + "2").Value2 = info.Name;
                            alphabetCounter++;
                        }

                        classSheet.get_Range("A1", alphabet[alphabetCounter] + "2").Font.Bold = true;
                        #endregion
                    }
                    else
                    {
                        rowIndex = -1;
                        classSheet.ToString();
                        ClassRowIndex.TryGetValue(dd.ObjectOnDiagram.Class, out rowIndex);
                    }
                    if (rowIndex < 0)
                    {
                        rowIndex.ToString();
                        continue;
                    }
                    bool diagram = false;
                    alphabetCounter = 0;
                    if (dd.ObjectOnDiagram != null)
                    {
                        diagram = true; //always true?
                        classSheet.get_Range(alphabet[alphabetCounter] + rowIndex, alphabet[alphabetCounter] + rowIndex).Value2 = dd.ObjectOnDiagram.pkid;
                        alphabetCounter++;
                        classSheet.get_Range(alphabet[alphabetCounter] + rowIndex, alphabet[alphabetCounter] + rowIndex).Value2 = dd.ObjectOnDiagram.MachineName;
                        alphabetCounter++;
                        classSheet.get_Range(alphabet[alphabetCounter] + rowIndex, alphabet[alphabetCounter] + rowIndex).Value2 = dd.ObjectOnDiagram.Class;
                        alphabetCounter++;
                        //classSheet.get_Range(alphabet[alphabetCounter] + "2", alphabet[alphabetCounter] + "2").Value2.Value2 = dd.ObjectOnDiagram.ToString();
                        foreach (PropertyInfo info in dd.ObjectOnDiagram.GetMetaPropertyList(false))
                        {
                            classSheet.get_Range(alphabet[alphabetCounter] + rowIndex, alphabet[alphabetCounter] + rowIndex).Value2 = dd.ObjectOnDiagram.Get(info.Name);
                            alphabetCounter++;
                        }
                    }
                    bool database = false;
                    int midalphabet = alphabetCounter;
                    alphabetCounter++; //add a column for difference
                    if (dd.ObjectInDatabase != null)
                    {
                        database = true;
                        classSheet.get_Range(alphabet[alphabetCounter] + rowIndex, alphabet[alphabetCounter] + rowIndex).Value2 = dd.ObjectInDatabase.pkid;
                        alphabetCounter++;
                        classSheet.get_Range(alphabet[alphabetCounter] + rowIndex, alphabet[alphabetCounter] + rowIndex).Value2 = dd.ObjectInDatabase.MachineName;
                        alphabetCounter++;
                        classSheet.get_Range(alphabet[alphabetCounter] + rowIndex, alphabet[alphabetCounter] + rowIndex).Value2 = dd.ObjectInDatabase.Class;
                        alphabetCounter++;
                        //classSheet.get_Range(alphabet[alphabetCounter] + "2", alphabet[alphabetCounter] + "2").Value2 = dd.ObjectInDatabase.ToString();
                        foreach (PropertyInfo info in dd.ObjectOnDiagram.GetMetaPropertyList(false))
                        {
                            classSheet.get_Range(alphabet[alphabetCounter] + rowIndex, alphabet[alphabetCounter] + rowIndex).Value2 = dd.ObjectInDatabase.Get(info.Name);
                            alphabetCounter++;
                        }
                        alphabetCounter--; //decrease by 1 because we dont need an extra colored column
                    }
                    else
                    {
                        alphabetCounter += 3;
                        alphabetCounter += dd.ObjectOnDiagram.GetMetaPropertyList(false).Count;
                    }

                    if (diagram && database)
                    {
                        //yellow
                        Range rngDiagram = classSheet.get_Range("A" + rowIndex, alphabet[midalphabet - 1] + rowIndex);
                        rngDiagram.Interior.Color = XlRgbColor.rgbYellow;

                        classSheet.get_Range(alphabet[midalphabet] + rowIndex.ToString(), alphabet[midalphabet] + rowIndex.ToString()).Value2 = "Changed";

                        Range rngDatabase = classSheet.get_Range(alphabet[midalphabet + 1] + rowIndex, alphabet[alphabetCounter - 1] + rowIndex);
                        rngDatabase.Interior.Color = XlRgbColor.rgbYellow;
                    }
                    else if (diagram)
                    {
                        //green then red
                        Range rngDiagram = classSheet.get_Range("A" + rowIndex, alphabet[midalphabet - 1] + rowIndex);
                        rngDiagram.Interior.Color = XlRgbColor.rgbGreen;

                        classSheet.get_Range(alphabet[midalphabet] + rowIndex.ToString(), alphabet[midalphabet] + rowIndex.ToString()).Value2 = "Added";

                        Range rngDatabase = classSheet.get_Range(alphabet[midalphabet + 1] + rowIndex, alphabet[alphabetCounter - 1] + rowIndex);
                        rngDatabase.Interior.Color = XlRgbColor.rgbRed;
                    }
                    else if (database)
                    {
                        //this will never happen
                        //excelUtil.CurrentSheet.get_Range("E" + rowIndex.ToString(), "E" + rowIndex.ToString()).Value2 = "Unknown";
                    }
                    rowIndex++;
                    ClassRowIndex.Remove(dd.ObjectOnDiagram.Class);
                    ClassRowIndex.Add(dd.ObjectOnDiagram.Class, rowIndex);
                }
            }
            excelUtil.ExcelApp.DisplayAlerts = false;
            excelUtil.SaveFile(exportFileDialog.FileName, false);
            //excelUtil.CloseExcel();
            //excelUtil = null;

            //System.Diagnostics.Process.Start(exportFileDialog.FileName);
            excelUtil.ExcelApp.DisplayAlerts = true;
            excelUtil.ExcelApp.Visible = true;

            //GC.Collect()
        }
    }
}