using System;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.Storage.RepositoryTemp;
using XPTable.Models;
using System.Collections;

namespace MetaBuilder.UIControls.GraphingUI
{
    public partial class LogDisplayer : Form
    {

        #region Constructors (1)

        public LogDisplayer()
        {
            InitializeComponent();
        }

        private bool WorkspaceTransfer;

        public bool MyProperty
        {
            get { return WorkspaceTransfer; }
            set { WorkspaceTransfer = value; }
        }

        public LogDisplayer(bool workspaceTransfer)
        {
            WorkspaceTransfer = workspaceTransfer;
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods (5)

        // Public Methods (3) 

        public void AddMessage(ActionResult result)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<ActionResult>(AddMessage), new object[] { result });
            }
            else
            {
                AddRowForResult(result);
                foreach (ActionResult res in result.InnerResults)
                {
                    AddRowForResult(res);
                }
            }
        }

        public void AddMessage(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(AddMessage), new object[] { message });
            }
            else
            {
                Row newRow = new Row();
                Cell c = new Cell();
                c.Text = message;
                newRow.Cells.Add(c);

                tableModel1.Rows.Add(newRow);
            }
        }

        public void ResetDesigner()
        {
            columnModel1.Columns.Clear();
            textColumn1.Text = "Message";
            textColumn1.Width = 500;
            columnModel1.Columns.Add(textColumn1);
            tableModel1.Rows.Clear();
            table1.GridLines = GridLines.Rows;
            Clear();
        }

        public void Clear()
        {
            tableModel1.Rows.Clear();
        }
        public int Messages { get { return tableModel1.Rows.Count; } }

        // Private Methods (2) 

        //ArrayList<ActionResult> errors = new List<ActionResult>();
        private void AddRowForResult(ActionResult result)
        {
            Row newRow = new Row();

            Cell cRepository = new Cell();
            cRepository.Text = result.Repository;
            newRow.Cells.Add(cRepository);

            Cell cItem = new Cell();
            cItem.Text = result.Message;
            newRow.Cells.Add(cItem);

            Cell cFromState = new Cell();
            cFromState.Text = result.FromState;
            newRow.Cells.Add(cFromState);

            Cell cToState = new Cell();
            cToState.Text = result.TargetState;
            newRow.Cells.Add(cToState);

            Cell c4 = new Cell();
            if (result.intermediate)
                c4 = new Cell(Color.Orange);
            else
                if (result.Success == true)
                    c4 = new Cell(Color.Green);
                else
                    c4 = new Cell(Color.Red);
            newRow.Cells.Add(c4);

            clrResult.ShowColorName = false;
            clrResult.ShowDropDownButton = false;

            tableModel1.Rows.Add(newRow);
            table1.AlternatingRowColor = Color.AliceBlue;
            table1.EnsureVisible(newRow.Index, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        #endregion Methods

        private void LogDisplayer_MouseLeave(object sender, EventArgs e)
        {
            //this.Close();
        }

        private void LogDisplayer_ResizeEnd(object sender, System.EventArgs e)
        {
            txtAction.Width = 330 + (this.Size.Width - this.MinimumSize.Width);
        }

        //public ArrayList<ActionResult> ReturnFailures()
        //{
        //    return errors;
        //}

    }
}