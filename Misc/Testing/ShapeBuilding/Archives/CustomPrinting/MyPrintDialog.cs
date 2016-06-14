using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Tools;
using MetaBuilder.UIControls.GraphingUI.CustomPrinting;
using Northwoods.Go;
using MetaBuilder.Graphing.Formatting;
using System.Collections.ObjectModel;
namespace ShapeBuilding.Archives.CustomPrinting
{
    public partial class MyPrintDialog : Form
    {
        public GoSheet Sheet
        {
            get { return myView.Sheet; }
            set { myView.Sheet = value; }
        }

        public GoDocument Document
        {
            get { return myView.Document; }
            set 
            { 
                myView.Document = value;

                if (myView.Document is NormalDiagram)
                {
                    NormalDiagram ndiagram = myView.Document as NormalDiagram;
                    if (ndiagram.LastPrintSettings != null)
                    {
                        myView.MyPrintSettings = ndiagram.LastPrintSettings;
                        BindToPrintSettings();
                    }
                    else
                    {
                        SetDefaults();
                    }
                }
        
            }
        }

        public MyPrintDialog()
        {
            InitializeComponent();
            myView.SheetStyle = GoViewSheetStyle.WholeSheet;
            myView.BackgroundHasSheet = true;
            myView.Document.Changed += new GoChangedEventHandler(Document_Changed);
            myView.SelectionMoved += new EventHandler(myView_SelectionMoved);
            myView.Grid.Visible = false;
            myView.PageBreaksChanged += new EventHandler(myView_PageBreaksChanged);
            myView.SheetChanged += new EventHandler(OnSheetChanged);
            this.FormClosing += new FormClosingEventHandler(MyPrintDialog_FormClosing);
          
        }

        private void SetDefaults()
        {
            PrintDocument pd = new PrintDocument();
            lblPrinterName.Text = pd.PrinterSettings.PrinterName + " (Default)";
            lblPageSize.Text = pd.DefaultPageSettings.PaperSize.PaperName + " (Default)";
          
            myView.MyPrintSettings.PageSet = pd.PrinterSettings.DefaultPageSettings;
            myView.MyPrintSettings.PrintSettings = pd.PrinterSettings;
            myView.MyPrintSettings.Adjustment = MetaBuilder.Graphing.Formatting.DiagramPrintSettings.PageAdjustment.Normal;
            myView.MyPrintSettings.PageSet.Color = true;
            myView.MyPrintSettings.PageSet.Landscape = false;
            myView.MyPrintSettings.Ratio = 1;
            myView.MyPrintSettings.Copies = 1;
            myView.DoPageSetup();
        }

        public void BindToPrintSettings()
        {
            UpdatingFromView = true;
            lblPrinterName.Text = myView.MyPrintSettings.PrintSettings.PrinterName;
            lblPageSize.Text = myView.MyPrintSettings.PageSet.PaperSize.PaperName;
            cbBlackAndWhite.Checked = MyView.MyPrintSettings.PageSet.Color;
            switch (MyView.MyPrintSettings.Adjustment)
            {
                case DiagramPrintSettings.PageAdjustment.Expand:
                    radioInflate.Checked = true;
                    radioDontScale.Checked = false;
                    radioReduce.Checked = false;
                    break;
                case DiagramPrintSettings.PageAdjustment.Fit:
                    radioInflate.Checked = false;
                    radioDontScale.Checked = false;
                    radioReduce.Checked = true;
                    break;
                default:
                    radioReduce.Checked = false;
                    radioInflate.Checked = false;
                    radioDontScale.Checked = true;
                    break;
            }
            cbShowPageBreaks.Checked = myView.MyPrintSettings.ShowPageBreaks;
            numAcross.Value = Convert.ToDecimal(myView.MyPrintSettings.FitAcross);
            numDown.Value = Convert.ToDecimal(myView.MyPrintSettings.FitDown);
            numCopies.Value = Convert.ToDecimal(myView.MyPrintSettings.Copies);
            radioLandscape.Checked = myView.MyPrintSettings.PageSet.Landscape;
            radioPortrait.Checked = !radioLandscape.Checked;
            UpdatingFromView = false;
        }

        public PrintableView MyView
        {
            get { return myView; }
            set
            {
                myView = value;
            }
        }

        void MyPrintDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            DocumentController controller = new DocumentController(myView.Document);
            controller.ApplyBrushes();
        }

        void myView_SelectionMoved(object sender, EventArgs e)
        {
            cbBlackAndWhite_CheckedChanged(sender,e);
        }

        private void myView_PageBreaksChanged(object sender, EventArgs e)
        {
            cbShowPageBreaks.Checked = myView.Grid.Visible;
        }

        protected void Document_Changed(Object sender, GoChangedEventArgs evt)
        {
            if (evt.Hint == GoDocument.ChangedTopLeft)
            {
                myView.GridOrigin = myView.DocumentTopLeft;
            }
        }

        private void PrintTest_Load(object sender, EventArgs e)
        {
            //myView.Refresh();
            // myView.Document.TopLeft = new PointF(Sheet.Position.X + Sheet.TopLeftMargin.Width + 1, Sheet.Position.Y + Sheet.TopLeftMargin.Height + 1);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (myView.Document.Name.IndexOf("\\") == -1)
            {
                myView.Document.Name = "Temp Diagram";
            }
            myView.MyPrintSettings.Copies = short.Parse(numCopies.Value.ToString());
            //myView.MyPrintSettings.Adjustment = DiagramPrintSettings.PageAdjustment.NoCalculations;
            myView.Print();
            if (myView.Document.Name == "Temp Diagram")
                myView.Document.Name = "";
            Close();
        }

        /// <summary>
        /// Trickle event to parent form
        /// </summary>
        public event EventHandler SheetChanged;

        protected void OnSheetChanged(object sender, EventArgs e)
        {
            if (SheetChanged != null && sender is GoSheet)
                SheetChanged(sender, e);
            UpdatingFromView = true;
            lblPageSize.Text = myView.MyPrintSettings.PrintSettings.DefaultPageSettings.PaperSize.PaperName;
            lblPrinterName.Text = myView.MyPrintSettings.PrintSettings.PrinterName;
            radioLandscape.Checked = myView.MyPrintSettings.PrintSettings.DefaultPageSettings.Landscape;
            radioPortrait.Checked = !myView.MyPrintSettings.PrintSettings.DefaultPageSettings.Landscape;
            UpdatingFromView = false;
            float pscalePretty = (myView.PrintScale*100f);
            string percString = pscalePretty.ToString("{0:#%}");
            this.Text = "Print Setup - Scaled to " + percString;

            if (myView.Document is NormalDiagram)
            {
                NormalDiagram ndiagram = myView.Document as NormalDiagram;
                ndiagram.LastPrintSettings = myView.MyPrintSettings;
            }
        }

       

        private void btnResizeToSheet_Click(object sender, EventArgs e)
        {
            //myView.UpdateSize(MetaBuilder.Graphing.Containers.CropType.ToSheet);
        }

        private void cbShowPageBreaks_CheckedChanged(object sender, EventArgs e)
        {
            myView.Grid.Visible = cbShowPageBreaks.Checked;
        }

        private void numAcross_ValueChanged(object sender, EventArgs e)
        {
            myView.MyPrintSettings.FitAcross = Convert.ToInt32(numAcross.Value);
            myView.DoPageSetup();
            myView.Refresh();
        }

        private void numDown_ValueChanged(object sender, EventArgs e)
        {
            myView.MyPrintSettings.FitDown = int.Parse(numDown.Value.ToString());
            myView.DoPageSetup();
            myView.Refresh();
        }

        private bool UpdatingFromView;

      
        private void radioOrientationChanged(object sender, EventArgs e)
        {
            if (sender == radioLandscape)
            {
                radioPortrait.Checked = false;
                myView.MyPrintSettings.PageSet.Landscape = true;
                myView.DoPageSetup();
            }
            else
            {
                radioLandscape.Checked = false;
                myView.MyPrintSettings.PageSet.Landscape = false;
                myView.DoPageSetup();
            }
        }

        private void cbScaleToFit_CheckedChanged_1(object sender, EventArgs e)
        {
            if (!UpdatingFromView)
            {
                if (radioReduce.Checked)
                {
                    radioDontScale.Checked = false;
                    radioInflate.Checked = false;
                    myView.MyPrintSettings.Adjustment = DiagramPrintSettings.PageAdjustment.Fit;
                    myView.DoPageSetup();
                }
                if (radioDontScale.Checked)
                {
                    radioReduce.Checked = false;
                    radioInflate.Checked = false;
                    myView.MyPrintSettings.Adjustment = DiagramPrintSettings.PageAdjustment.Normal;
                    myView.DoPageSetup();
                }
                if (radioInflate.Checked)
                {
                    radioReduce.Checked = false;
                    radioDontScale.Checked = false;
                    myView.MyPrintSettings.Adjustment = DiagramPrintSettings.PageAdjustment.Expand;
                    myView.DoPageSetup();
                }
            }
        }

        private void cbBlackAndWhite_CheckedChanged(object sender, EventArgs e)
        {
            if (cbBlackAndWhite.Checked)
            {
                DocumentController controller = new DocumentController(myView.Document);
                controller.RemoveBrushes();
            }
            else
            {
                DocumentController controller = new DocumentController(myView.Document);
                controller.ApplyBrushes();
            }
        }

        private void lblPrinterName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            PrintDialog pd = new PrintDialog();
            pd.ShowDialog(this);
            return;
            PrinterSettings.StringCollection installedPrinters = PrinterSettings.InstalledPrinters;
            List<string> printers = new List<string>();
            IEnumerator enumerator = installedPrinters.GetEnumerator();
            while (enumerator.MoveNext())
            {
                printers.Add(enumerator.Current.ToString());
            }

            ChoosePrinter choosePrinter = new ChoosePrinter();
            choosePrinter.InstalledPrinters = printers;
            choosePrinter.SelectedPrinter = myView.MyPrintSettings.PrintSettings.PrinterName;
            DialogResult res = choosePrinter.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                lblPrinterName.Text = choosePrinter.SelectedPrinter;
                myView.MyPrintSettings.PrintSettings.PrinterName = lblPrinterName.Text;
                OnSheetChanged(this, EventArgs.Empty);
            }
            
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            myView.PrintPreview();
        }
    }
}