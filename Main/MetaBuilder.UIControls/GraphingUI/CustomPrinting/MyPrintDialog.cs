using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Tools;
using Northwoods.Go;
using MetaBuilder.Graphing.Formatting;
using System.Runtime.InteropServices;
using System.Management;
using System.Collections.ObjectModel;
namespace MetaBuilder.UIControls.GraphingUI.CustomPrinting
{

    //KIP c7800 - KIP Custom Form (841x6000)

    public partial class MyPrintDialog : Form
    {
        [DllImport("winspool.Drv", EntryPoint = "DocumentPropertiesW", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        static extern int DocumentProperties(IntPtr hwnd, IntPtr hPrinter, [MarshalAs(UnmanagedType.LPWStr)] string pDeviceName, IntPtr pDevModeOutput, IntPtr pDevModeInput, int fMode);

        [DllImport("kernel32.dll")]
        static extern IntPtr GlobalLock(IntPtr hMem);
        [DllImport("kernel32.dll")]
        static extern bool GlobalUnlock(IntPtr hMem);
        [DllImport("kernel32.dll")]
        static extern bool GlobalFree(IntPtr hMem);

        public bool ReadOnly = false;

        #region Fields (1)

        private bool UpdatingFromView;

        #endregion Fields

        #region Constructors (1)

        public MyPrintDialog(bool ReadOnly)
        {
            InitializeComponent();
            myView.SheetStyle = GoViewSheetStyle.WholeSheet;
            myView.BackgroundHasSheet = true;
            myView.Document.Changed += new GoChangedEventHandler(Document_Changed);
            myView.SelectionMoved += new EventHandler(myView_SelectionMoved);
            myView.Grid.Visible = false;
            myView.PageBreaksChanged += new EventHandler(myView_PageBreaksChanged);
            myView.SheetChanged += new EventHandler(OnSheetChanged);
            FormClosing += new FormClosingEventHandler(MyPrintDialog_FormClosing);

            myView.AllowEdit = false;
            myView.AllowLink = false;
            //make readonly if original is
            myView.AllowMove = myView.AllowDrop = myView.AllowCopy = myView.AllowKey = myView.AllowDelete = myView.AllowInsert = myView.AllowReshape = myView.AllowResize = !ReadOnly;

            cbBlackAndWhite.Visible = false;

            InitPrinters();
        }

        #endregion Constructors

        private float frameWidth;
        public float FrameWidth
        {
            get { return frameWidth; }
            set { frameWidth = value; }
        }
        private float frameHeight;
        public float FrameHeight
        {
            get { return frameHeight; }
            set { frameHeight = value; }
        }

        #region Properties (3)

        public GoDocument Document
        {
            get { return myView.Document; }
            set
            {
                initprinter = true;
                if (PrinterSettings.InstalledPrinters.Count == 0)
                {
                    MessageBox.Show(this, "No printers installed", "Please install one and try again", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.DialogResult = DialogResult.Cancel;
                }
                myView.Document = value;

                if (myView.Document is NormalDiagram)
                {
                    NormalDiagram ndiagram = myView.Document as NormalDiagram;
                    FrameWidth = ndiagram.DocumentFrame.Width;
                    FrameHeight = ndiagram.DocumentFrame.Height;

                    if (ndiagram.LastPrintSettings != null)
                    {
                        try
                        {
                            myView.MyPrintSettings = ndiagram.LastPrintSettings;

                            // test to see if the last printer used is installed on this machine
                            bool found = false;
                            int foundI = 0;
                            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                            {
                                if (PrinterSettings.InstalledPrinters[i] == ndiagram.LastPrintSettings.PrintSettings.PrinterName)
                                {
                                    foundI = i;
                                    found = true;
                                    break;
                                }
                            }

                            if (found)
                            {
                                BindToPrintSettings();
                                if (radioLandscape.Checked)
                                {
                                    radioOrientationChanged(radioLandscape, EventArgs.Empty);
                                }
                                else
                                {
                                    radioOrientationChanged(radioPortrait, EventArgs.Empty);
                                }
                            }
                            else
                            {
                                ndiagram.LastPrintSettings = new DiagramPrintSettings();
                                myView.MyPrintSettings = new DiagramPrintSettings();
                                SetDefaults();
                                BindToPrintSettings();
                            }
                        }
                        catch
                        {
                            myView.MyPrintSettings = new DiagramPrintSettings();
                            SetDefaults();
                            BindToPrintSettings();
                        }
                    }
                    else
                    {
                        myView.MyPrintSettings = new DiagramPrintSettings();
                        SetDefaults();
                        BindToPrintSettings();
                    }

                    this.MyView.RescaleWithCenter(0.2F, new PointF(this.MyView.Document.Bounds.Width / 2, this.MyView.Document.Bounds.Height / 2));
                }
                else
                {
                    FrameWidth = myView.Width;
                    FrameHeight = myView.Height;
                    myView.MyPrintSettings = new DiagramPrintSettings();
                    SetDefaults();
                    BindToPrintSettings();
                }

                myView.Document.TopLeft = myView.Sheet.Position;
                initprinter = false;
            }
        }

        public PrintableView MyView
        {
            get { return myView; }
            set
            {
                myView = value;
            }
        }

        public GoSheet Sheet
        {
            get { return myView.Sheet; }
            set { myView.Sheet = value; }
        }

        #endregion Properties

        #region Delegates and Events (1)


        // Events (1) 

        /// <summary>
        /// Trickle event to parent form
        /// </summary>
        public event EventHandler SheetChanged;


        #endregion Delegates and Events

        #region Methods (19)

        // Public Methods (1) 

        public void BindToPrintSettings()
        {
            UpdatingFromView = true;

            numericUpDownTop.Value = myView.MyPrintSettings.PageSet.Margins.Top;
            numericUpDownBottom.Value = myView.MyPrintSettings.PageSet.Margins.Bottom;
            numericUpDownLeft.Value = myView.MyPrintSettings.PageSet.Margins.Left;
            numericUpDownRight.Value = myView.MyPrintSettings.PageSet.Margins.Right;

            foreach (PrinterObject printer in printers)
            {
                if (printer.Name == myView.MyPrintSettings.PrintSettings.PrinterName)
                {
                    comboBoxPrinter.SelectedItem = printer;
                    break;
                }
            }

            PaperSize selectSize = null;
            foreach (PaperSize p in comboBoxPage.Items)
            {
                if (p.PaperName == myView.MyPrintSettings.PageSet.PaperSize.PaperName)
                {
                    selectSize = p;
                    break;
                }
            }

            if (selectSize == null)
                selectSize = myView.MyPrintSettings.PrintSettings.DefaultPageSettings.PaperSize;
            myView.MyPrintSettings.PageSet.PaperSize = selectSize;
            comboBoxPage.SelectedItem = selectSize;

            //MyView.MyPrintSettings.PageSet.Color;
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
            myView.DoPageSetup();
        }

        // Protected Methods (2) 

        protected void Document_Changed(Object sender, GoChangedEventArgs evt)
        {
            if (evt.Hint == GoDocument.ChangedTopLeft)
            {
                myView.GridOrigin = myView.DocumentTopLeft;
            }
        }

        protected void OnSheetChanged(object sender, EventArgs e)
        {
            if (SheetChanged != null && sender is GoSheet)
                SheetChanged(sender, e);
            UpdatingFromView = true;
            radioLandscape.Checked = myView.MyPrintSettings.PageSet.Landscape;
            radioPortrait.Checked = !radioLandscape.Checked;
            UpdatingFromView = false;
            float pscalePretty = (myView.PrintScale * 10f);
            this.Text = "Print Setup - Scaled to " + pscalePretty.ToString("{0:#%}"); ;

            if (myView.Document is NormalDiagram)
            {
                NormalDiagram ndiagram = myView.Document as NormalDiagram;
                ndiagram.LastPrintSettings = myView.MyPrintSettings;
            }
        }

        // Private Methods (16) 

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (myView.Document.Name.IndexOf("\\") == -1)
                {
                    myView.Document.Name = "Diagram";
                }
                myView.MyPrintSettings.PrintSettings.Copies = short.Parse(numCopies.Value.ToString());
                //myView.MyPrintSettings.Adjustment = DiagramPrintSettings.PageAdjustment.NoCalculations;
                myView.Print();
                if (myView.Document.Name == "Diagram")
                    myView.Document.Name = "";
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "A problem has occurred while sending the document to the printer. Please make sure you are able to communicate with the printer", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Core.Log.WriteLog(ex.ToString());
            }
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            myView.PrintPreview();
        }

        private void btnResizeToSheet_Click(object sender, EventArgs e)
        {
            myView.UpdateSize(MetaBuilder.Graphing.Controllers.GraphViewController.CropType.ToSheet);
            myView.DoPageSetup();
        }

        private void btnSetupToFit_Click(object sender, EventArgs e)
        {
            OpenPrinterPropertiesDialog(myView.MyPrintSettings.PrintSettings);
            myView.MyPrintSettings.PageSet = myView.MyPrintSettings.PrintSettings.DefaultPageSettings;
            comboBoxPage.Items.Clear();
            PaperSize selectSize = null;
            foreach (PaperSize p in myView.MyPrintSettings.PrintSettings.PaperSizes)
            {
                comboBoxPage.Items.Add(p);
                if (p.PaperName == myView.MyPrintSettings.PrintSettings.DefaultPageSettings.PaperSize.PaperName)
                    selectSize = p;
            }
            comboBoxPage.SelectedItem = selectSize;
            UpdatePrintSettings(); //this is auto from setting selecteditem above
        }

        private void cbBlackAndWhite_CheckedChanged(object sender, EventArgs e)
        {
            //MyView.SetColor(cbBlackAndWhite.Checked);            
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

        private void cbShowPageBreaks_CheckedChanged(object sender, EventArgs e)
        {
            myView.Grid.Visible = cbShowPageBreaks.Checked;
        }

        private void OpenPrinterPropertiesDialog(PrinterSettings printerSettings)
        {
            printerSettings.DefaultPageSettings.PaperSize = selectedPage;
            printerSettings.DefaultPageSettings.Margins = MyView.MyPrintSettings.PageSet.Margins;

            IntPtr hDevMode = printerSettings.GetHdevmode(printerSettings.DefaultPageSettings);
            IntPtr pDevMode = GlobalLock(hDevMode);
            int sizeNeeded = DocumentProperties(this.Handle, IntPtr.Zero, printerSettings.PrinterName, pDevMode, pDevMode, 0);
            IntPtr devModeData = Marshal.AllocHGlobal(sizeNeeded);
            DocumentProperties(this.Handle, IntPtr.Zero, printerSettings.PrinterName, devModeData, pDevMode, 14);
            GlobalUnlock(hDevMode);
            printerSettings.SetHdevmode(devModeData);
            printerSettings.DefaultPageSettings.SetHdevmode(devModeData);
            GlobalFree(hDevMode);
            Marshal.FreeHGlobal(devModeData);
        }

        private void lblPrinterName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string prevPrinterName = lblPrinterName.Text;
            PrintDialog pd = new PrintDialog();
            pd.PrinterSettings.PrinterName = lblPrinterName.Text;

            //pd.PrinterSettings = myView.MyPrintSettings.PrintSettings;
            pd.UseEXDialog = true;
            pd.ShowDialog(this);

            this.lblPrinterName.Text = pd.PrinterSettings.PrinterName;
            //OpenPrinterPropertiesDialog(pd.PrinterSettings);
            this.lblPageSize.Text = pd.PrinterSettings.DefaultPageSettings.PaperSize.PaperName;
            myView.MyPrintSettings.PrintSettings.PrinterName = pd.PrinterSettings.PrinterName;
            myView.MyPrintSettings.PrintSettings = pd.PrinterSettings;

            if (prevPrinterName != pd.PrinterSettings.PrinterName)
            {
                lblPageSize.Text = pd.PrinterSettings.DefaultPageSettings.PaperSize.PaperName;
                myView.MyPrintSettings.PageSet = pd.PrinterSettings.DefaultPageSettings;
                //myView.MyPrintSettings.PageSet.PrinterSettings.PrinterName = pd.PrinterSettings.PrinterName;

                myView.MyPrintSettings.PageSet.PaperSize = pd.PrinterSettings.DefaultPageSettings.PaperSize;
            }
            myView.DoPageSetup();
            myView.Refresh();
            OnSheetChanged(sender, e);
        }

        private void MyPrintDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MyView.BrushesRemoved)
            {
                DocumentController controller = new DocumentController(myView.Document);
                controller.ApplyBrushes();
            }
        }

        private void myView_PageBreaksChanged(object sender, EventArgs e)
        {
            cbShowPageBreaks.Checked = myView.Grid.Visible;
        }

        void myView_SelectionMoved(object sender, EventArgs e)
        {
            //cbBlackAndWhite_CheckedChanged(sender, e);
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

        private void PrintTest_Load(object sender, EventArgs e)
        {
            //myView.Refresh();
            // myView.Document.TopLeft = new PointF(Sheet.Position.X + Sheet.TopLeftMargin.Width + 1, Sheet.Position.Y + Sheet.TopLeftMargin.Height + 1);
        }

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

        private void SetDefaults()
        {
            PrintDocument pd = new PrintDocument();
            //lblPrinterName.Text = pd.PrinterSettings.PrinterName + " (Default)";
            //lblPageSize.Text = pd.DefaultPageSettings.PaperSize.PaperName + " (Default)";

            myView.MyPrintSettings.PageSet = pd.PrinterSettings.DefaultPageSettings;
            myView.MyPrintSettings.PrintSettings = pd.PrinterSettings;

            myView.MyPrintSettings.Adjustment = MetaBuilder.Graphing.Formatting.DiagramPrintSettings.PageAdjustment.Normal;
            myView.MyPrintSettings.PageSet.Color = true;
            //myView.MyPrintSettings.PageSet.Landscape = false;
            myView.MyPrintSettings.Ratio = 1;
            myView.MyPrintSettings.Copies = 1;
            myView.MyPrintSettings.FitAcross = 1;
            myView.MyPrintSettings.FitDown = 1;
            myView.MyPrintSettings.PageSet.Landscape = pd.PrinterSettings.DefaultPageSettings.Landscape;

            myView.MyPrintSettings.PrintSettings.DefaultPageSettings.Margins.Bottom = int.Parse(numericUpDownBottom.Value.ToString());
            myView.MyPrintSettings.PrintSettings.DefaultPageSettings.Margins.Top = int.Parse(numericUpDownTop.Value.ToString());
            myView.MyPrintSettings.PrintSettings.DefaultPageSettings.Margins.Left = int.Parse(numericUpDownLeft.Value.ToString());
            myView.MyPrintSettings.PrintSettings.DefaultPageSettings.Margins.Right = int.Parse(numericUpDownRight.Value.ToString());

            myView.DoPageSetup();
        }

        #endregion Methods

        private void margin_ValueChanged(object sender, EventArgs e)
        {
            if (initprinter || UpdatingFromView)
                return;
            try
            {
                myView.MyPrintSettings.PrintSettings.DefaultPageSettings.Margins.Bottom = int.Parse(numericUpDownBottom.Value.ToString());
                myView.MyPrintSettings.PrintSettings.DefaultPageSettings.Margins.Top = int.Parse(numericUpDownTop.Value.ToString());
                myView.MyPrintSettings.PrintSettings.DefaultPageSettings.Margins.Left = int.Parse(numericUpDownLeft.Value.ToString());
                myView.MyPrintSettings.PrintSettings.DefaultPageSettings.Margins.Right = int.Parse(numericUpDownRight.Value.ToString());
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog("MyPrintDialog::margin_ValueChanged::" + ex.ToString());
            }
            myView.DoPageSetup();
        }

        private Collection<PrinterObject> printers = null;
        private bool initprinter = false;
        public void InitPrinters()
        {
            initprinter = true;

            //ManagementScope objScope = new ManagementScope(ManagementPath.DefaultPath); //For the local Access
            //objScope.Connect();

            //SelectQuery selectQuery = new SelectQuery();
            //selectQuery.QueryString = "Select * from win32_Printer";
            //ManagementObjectSearcher MOS = new ManagementObjectSearcher(objScope, selectQuery);
            //ManagementObjectCollection MOC = MOS.Get();

            //ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

            printers = new Collection<PrinterObject>();
            //foreach (ManagementObject printer in MOC)
            foreach (string s in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                //PrinterObject p = new PrinterObject(printer);
                PrinterObject p = new PrinterObject(s);
                printers.Add(p);

                comboBoxPrinter.Items.Add(p);
            }
            comboBoxPrinter.DisplayMember = "Name";
            if (printers.Count > 0)
                comboBoxPrinter.SelectedIndex = 0;

            initprinter = false;
        }

        private PrinterObject selectedPrinter { get { return comboBoxPrinter.SelectedItem as PrinterObject; } }
        private PaperSize selectedPage { get { return comboBoxPage.SelectedItem as PaperSize; } }
        private void comboBoxPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPrinter.SelectedItem == null)
                return;
            //Core.Log.WriteLog("PrinterChanged::" + Environment.OSVersion.Version.ToString() + "::" + selectedPrinter.Name);

            myView.MyPrintSettings.PrintSettings.PrinterName = selectedPrinter.Name;
            comboBoxPage.Items.Clear();
#if DEBUG
            if (FrameWidth > 0 && FrameHeight > 0)
                comboBoxPage.Items.Add(CalculatePaperSize());
#endif
            foreach (PaperSize p in myView.MyPrintSettings.PrintSettings.PaperSizes)
                comboBoxPage.Items.Add(p);

            comboBoxPage.DisplayMember = "PaperName";
            comboBoxPage.SelectedIndex = 0;
            UpdatePrintSettings();
        }
        private void comboBoxPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Core.Log.WriteLog("PageChanged::" + Environment.OSVersion.Version.ToString() + "::" + selectedPage.PaperName);
            UpdatePrintSettings();
        }

        private void UpdatePrintSettings()
        {
            if (initprinter)
                return;
            myView.MyPrintSettings.PrintSettings.PrinterName = selectedPrinter.Name;

            //myView.MyPrintSettings.PrintSettings = pd.PrinterSettings;
            //myView.MyPrintSettings.PageSet = pd.PrinterSettings.DefaultPageSettings;

            myView.MyPrintSettings.PrintSettings.DefaultPageSettings.PaperSize = selectedPage;
            myView.MyPrintSettings.PageSet.PaperSize = selectedPage;
            myView.DoPageSetup();
            OnSheetChanged(this, EventArgs.Empty);
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            InitPrinters();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                myView.Document.Changed -= Document_Changed;
                myView.SelectionMoved -= myView_SelectionMoved;
                myView.PageBreaksChanged -= myView_PageBreaksChanged;
                myView.SheetChanged -= OnSheetChanged;
                FormClosing -= MyPrintDialog_FormClosing;

                printers = null;

                myView.MyPrintSettings = null;
                myView.Dispose();
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public PaperSize CalculatePaperSize()
        {
            Single xDpi, yDpi;

            using (Graphics g = this.CreateGraphics())
            {
                xDpi = g.DpiX;
                yDpi = g.DpiY;
            }

            //* 0.393701
            //(((pixels / dpi)[Inches] * TOCENTIMETRE) * TOMILLIMETRE)
            int w = int.Parse((Math.Round((((FrameWidth / xDpi) * 2.54) * 10), 0, MidpointRounding.AwayFromZero)).ToString());
            int h = int.Parse((Math.Round((((FrameHeight / yDpi) * 2.54) * 10), 0, MidpointRounding.AwayFromZero)).ToString());

            PaperSize NewSize = new PaperSize();
            NewSize.RawKind = (int)PaperKind.Custom;
            NewSize.Width = w;
            NewSize.Height = h;
            NewSize.PaperName = "Calculated Custom(" + w + "*" + h + ")";

            return NewSize;
        }
    }

    public class PrinterObject
    {
        private ManagementObject printer;

        public ManagementObject Printer
        {
            get { return printer; }
            set { printer = value; }
        }

        public PrinterObject(ManagementObject p)
        {
            Printer = p;
        }
        public PrinterObject(string p)
        {
            //Printer = p;
            name = p;
        }
        private string name;
        public string Name { get { return name; } }

        //public string Name { get { return Printer["Name"].ToString(); } }
        //public bool Default { get { return bool.Parse(Printer["Default"].ToString()); } }

        //var name = printer.GetPropertyValue("Name");
        //var status = printer.GetPropertyValue("Status");
        //var isDefault = printer.GetPropertyValue("Default");
        //var isNetworkPrinter = printer.GetPropertyValue("Network");

    }
}