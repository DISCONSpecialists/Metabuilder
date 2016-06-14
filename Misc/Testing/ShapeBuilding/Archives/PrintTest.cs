using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Northwoods.Go;
using ShapeBuilding.CustomPrinting;
namespace ShapeBuilding
{
    public partial class MyPrintDialog : Form
    {

        public Northwoods.Go.GoDocument Document
        {
            get
            {
                return this.myView.Document;
            }
            set
            {
                this.myView.Document = value;
            }
        }
        CustomPrinting.Resizing r;
        public MyPrintDialog()
        {
            InitializeComponent();
            myView.SheetStyle = GoViewSheetStyle.WholeSheet;
            myView.BackgroundHasSheet = true;
            myView.Document.Changed += new GoChangedEventHandler(Document_Changed);
            myView.Grid.Visible = false;
            myView.PageBreaksChanged+=new EventHandler(myView_PageBreaksChanged);
            myView.FitAcross = 1;
            myView.FitDown = 1;
        }

        void myView_PageBreaksChanged(object sender, EventArgs e)
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
            r = new Resizing(myView);
            r.DoSetup();
         
        }

        private void btnPageSetup_Click(object sender, EventArgs e)
        {
            myView.PageSetup();
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            myView.PrintPreview();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (myView.Document.Name == "")
                myView.Document.Name = "Print.dgm";
            myView.PrinterSettings.Adjustment = DiagramPrintSettings.PageAdjustment.Normal;
            myView.Print();
            myView.Document.Name = "";
        }

        private void btnPrintToFit_Click(object sender, EventArgs e)
        {
            if (myView.PrinterSettings == null)
                myView.PrinterSettings = new DiagramPrintSettings();
            myView.Document.Name = "temp.dgm";
            myView.PrinterSettings.Adjustment = DiagramPrintSettings.PageAdjustment.Fit;
            myView.PrintToFit();
            myView.Document.Name = "";
        }

        private void btnSetupToFit_Click(object sender, EventArgs e)
        {
            if (myView.PrinterSettings == null)
                myView.PrinterSettings = new DiagramPrintSettings();
            myView.PrinterSettings.Adjustment = DiagramPrintSettings.PageAdjustment.Fit;
            
            myView.SetupToFit();
        }

        private void btnCropToDrawing_Click(object sender, EventArgs e)
        {
            myView.CropToDrawing();
        }

        private void btnResizeToSheet_Click(object sender, EventArgs e)
        {
            myView.CropFrameToSheet();
        }

        private void cbShowPageBreaks_CheckedChanged(object sender, EventArgs e)
        {
            myView.Grid.Visible = cbShowPageBreaks.Checked;
        }

        private void numAcross_ValueChanged(object sender, EventArgs e)
        {
            this.myView.FitAcross = int.Parse(numAcross.Value.ToString());
        }

        private void numDown_ValueChanged(object sender, EventArgs e)
        {
            this.myView.FitDown = int.Parse(numDown.Value.ToString());
        }


    }
}