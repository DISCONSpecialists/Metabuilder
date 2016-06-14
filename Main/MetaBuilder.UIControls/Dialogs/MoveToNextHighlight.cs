using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Northwoods.Go;
using MetaBuilder.Meta;
using MetaBuilder.UIControls.GraphingUI;

namespace MetaBuilder.UIControls.Dialogs
{

    public partial class MoveToNextHighlight : Form
    {
        private List<HighlightedObject> highlights;

        public List<HighlightedObject> Highlights
        {
            get { return highlights; }
            set
            {
                highlights = value;
                BindTable();
            }
        }

        public MoveToNextHighlight()
        {
            InitializeComponent();
        }

        int currentIndex = 0;
        public void UpdateControls()
        {
            if (highlights != null)
            {
                btnPrevious.Enabled = (currentIndex > 0);
                btnNext.Enabled = currentIndex < highlights.Count - 1;
            }
            else
            {
                btnPrevious.Enabled = false;
                btnNext.Enabled = false;
            }
        }

        public void BindTable()
        {
            foreach (HighlightedObject hlO in highlights)
            {
                XPTable.Models.Row r = new XPTable.Models.Row();
                r.Cells.Add(new XPTable.Models.Cell(hlO.MetaObject.ToString()));
                r.Cells.Add(new XPTable.Models.Cell(hlO.MetaObject._ClassName));
                r.Cells.Add(new XPTable.Models.Cell(Core.strings.GetFileNameOnly(hlO.DocumentName)));
                r.Tag = hlO;
                this.tableModel1.Rows.Add(r);
            }
        }

        public void NavigateTo(HighlightedObject hlObj)
        {
            // DockingForm.DockForm
        }

        private void table1_CellClick(object sender, XPTable.Events.CellMouseEventArgs e)
        {
            // do navigation
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            currentIndex++;
            Navigate();
            UpdateControls();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            currentIndex--;
            Navigate();
            UpdateControls();
        }

        private void Navigate()
        {
            if (Highlights.Count > currentIndex)
            {
                HighlightedObject hlo = highlights[currentIndex];
                DockingForm.DockForm.NavigateToHighlightedObject(hlo);
            }
        }

        private void table1_CellDoubleClick(object sender, XPTable.Events.CellMouseEventArgs e)
        {
            HighlightedObject hlSelected = e.Cell.Row.Tag as HighlightedObject;
            int counter = 0;
            int foundIndex = 0;
            foreach (HighlightedObject hlo in highlights)
            {
                if (hlo == hlSelected)
                {
                    foundIndex = counter;
                }
                counter++;
            }

            currentIndex = foundIndex;
            Navigate();
            UpdateControls();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            DockingForm.DockForm.RemoveHighlights();
            Close();
        }
    }

    public class HighlightedObject
    {
        public HighlightedObject(MetaBase mb, GoObject goObj, string diagramName)
        {
            this.MetaObject = mb;
            this.GoObj = goObj;
            this.DocumentName = diagramName;
        }
        private MetaBase metaObject;
        public MetaBase MetaObject
        {
            get { return metaObject; }
            set { metaObject = value; }
        }

        private string documentName;
        public string DocumentName
        {
            get { return documentName; }
            set { documentName = value; }
        }
        private GoObject goObj;
        public GoObject GoObj
        {
            get { return goObj; }
            set { goObj = value; }
        }
    }
}