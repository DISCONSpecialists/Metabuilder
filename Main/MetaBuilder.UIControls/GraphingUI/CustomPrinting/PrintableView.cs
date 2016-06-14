using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Windows.Forms;
using MetaBuilder.Graphing.Containers;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Northwoods.Go;
using Northwoods.Go.Draw;
using MetaBuilder.Graphing.Formatting;
using MetaBuilder.Graphing.Tools;

namespace MetaBuilder.UIControls.GraphingUI.CustomPrinting
{
    public class PrintableView : GoDrawView
    {

        #region Constructors (1)

        public PrintableView()
        {
            myPrintSettings = new DiagramPrintSettings();

            //  Sheet.TopLeftMargin = Sheet.BottomRightMargin = new SizeF(60, 60);
            VerticalRuler.Units = GoDrawUnit.Millimeter;
            HorizontalRuler.Units = GoDrawUnit.Millimeter;
            SheetStyle = GoViewSheetStyle.WholeSheet;
        }

        #endregion Constructors

        #region Delegates and Events (2)

        // Events (2) 

        public event EventHandler PageBreaksChanged;

        public event EventHandler SheetChanged;

        #endregion Delegates and Events

        #region Methods (9)

        // Public Methods (2) 

        public void SetupPageBreaks()
        {
            try
            {
                int paperw = (MyPrintSettings.PageSet.Landscape) ? MyPrintSettings.PageSet.PaperSize.Height : MyPrintSettings.PageSet.PaperSize.Width;
                int paperh = (MyPrintSettings.PageSet.Landscape) ? MyPrintSettings.PageSet.PaperSize.Width : MyPrintSettings.PageSet.PaperSize.Height;
                int pagew = paperw - MyPrintSettings.PageSet.Margins.Left - MyPrintSettings.PageSet.Margins.Right;
                int pageh = paperh - MyPrintSettings.PageSet.Margins.Top - MyPrintSettings.PageSet.Margins.Bottom;
                SizeF testSize = new SizeF(pagew / PrintScale, pageh / PrintScale);
                if (testSize.Width > 0 && testSize.Height > 0)
                    GridCellSize = testSize;
                else
                    GridCellSize = new SizeF(20, 20);
            }
            catch
            {
            }

            GridOrigin = PrintDocumentTopLeft;
            GridStyle = GoViewGridStyle.Line;
            this.Grid.LineDashStyle = DashStyle.Dash;
            this.GridLineColor = Color.Blue;
            BackColor = Color.White;
        }

        protected void aDocument_Changed(Object sender, GoChangedEventArgs evt)
        {
            if (evt.Hint == GoDocument.ChangedTopLeft)
            {
                GridOrigin = DocumentTopLeft;
            }
        }

        public void UpdateSize(MetaBuilder.Graphing.Controllers.GraphViewController.CropType type)
        {
            //this.SuspendLayout();
            if (Document is NormalDiagram)
            {
                NormalDiagram ndiagram = Document as NormalDiagram;
                switch (type)
                {
                    case MetaBuilder.Graphing.Controllers.GraphViewController.CropType.ToFrame:
                        // Adjust the sheet to fit the frame - do not adjust the sheet
                        Sheet.Size = new SizeF(PrintDocumentSize.Width + Sheet.TopLeftMargin.Width + Sheet.BottomRightMargin.Width, PrintDocumentSize.Height + Sheet.TopLeftMargin.Height + Sheet.BottomRightMargin.Height);
                        /*   Sheet.Position =  new PointF(ndiagram.DocumentFrame.Position.X - Sheet.TopLeftMargin.Width,  ndiagram.DocumentFrame.Position.Y - Sheet.TopLeftMargin.Height);*/
                        break;
                    case MetaBuilder.Graphing.Controllers.GraphViewController.CropType.ToSheet:
                        //  Adjust the frame to fit the sheet - do not adjust the frame
                        //float f = 20f;
                        //float f = 0f;
                        ndiagram.DocumentFrame.Frame.Width = ((Sheet.Width - Sheet.TopLeftMargin.Width) - Sheet.BottomRightMargin.Width);// -30f;// / PrintScale;
                        ndiagram.DocumentFrame.Frame.Height = ((Sheet.Height - Sheet.TopLeftMargin.Height) - Sheet.BottomRightMargin.Height);// -30f;// -f / PrintScale;
                        //ndiagram.DocumentFrame.Position = new PointF(Sheet.Position.X + Sheet.TopLeftMargin.Width + f / PrintScale, Sheet.Position.Y + Sheet.TopLeftMargin.Height + f / PrintScale);
                        ndiagram.DocumentFrame.Position = new PointF(Sheet.Position.X + Sheet.TopLeftMargin.Width, Sheet.Position.Y + Sheet.TopLeftMargin.Height);
                        //ndiagram.DocumentFrame.Frame.Width = ndiagram.DocumentFrame.Frame.Width;
                        //ndiagram.DocumentFrame.Frame.Height = ndiagram.DocumentFrame.Frame.Height;
                        //ndiagram.DocumentFrame.Reposition();
                        //this.DocPosition = new PointF(ndiagram.DocumentFrame.Position.X/PrintScale,ndiagram.DocumentFrame.Position.Y+2/PrintScale);
                        break;
                }
                //this.ResumeLayout();
                return;
            }
            PointF leftTopMost = new PointF(50000, 50000);
            PointF rightBottom = new PointF(-5000, -5000);
            foreach (GoObject o in Document)
            {
                if (!(o is GoSheet))
                {
                    if (o.Position.X + o.Width > rightBottom.X)
                    {
                        rightBottom.X = o.Position.X + o.Width;
                    }
                    if (o.Position.Y + o.Height > rightBottom.Y)
                    {
                        rightBottom.Y = o.Position.Y + o.Height;
                    }
                    if (o.Position.X < leftTopMost.X)
                    {
                        leftTopMost.X = o.Position.X;
                    }
                    if (o.Position.Y < leftTopMost.Y)
                    {
                        leftTopMost.Y = o.Position.Y;
                    }
                }
            }

            PointF newSheetPosition = new PointF(leftTopMost.X + Sheet.TopLeftMargin.Width, leftTopMost.Y + Sheet.TopLeftMargin.Height);
            Sheet.Position = Grid.FindNearestGridPoint(newSheetPosition, Sheet);

            Document.TopLeft = Sheet.Position;// Grid.FindNearestGridPoint(newSheetPosition, Sheet);
            this.ResumeLayout();
        }

        // Protected Methods (5) 

        protected void OnPageBreaksChanged(object sender, EventArgs e)
        {
            if (PageBreaksChanged != null)
                PageBreaksChanged(sender, e);
        }

        //protected override void OnPaint(PaintEventArgs evt)
        //{
        //    try
        //    {
        //        base.OnPaint(evt);
        //    }
        //    catch
        //    {
        //        //// Console.WriteLine("Error painting!");
        //    }
        //}
        private void RepositionDocument()
        {
            NormalDiagram ndiagram = this.Document as NormalDiagram;
            if (ndiagram != null)
            {
                ndiagram.DocumentFrame.Position = FindNearestGridPoint(new PointF(Sheet.Position.X + Sheet.TopLeftMargin.Width, Sheet.Position.Y + Sheet.TopLeftMargin.Height));
                //ndiagram.DocumentFrame.Position = FindNearestGridPoint(new PointF(Sheet.Position.X + Sheet.TopLeftMargin.Width, Sheet.Position.Y + Sheet.TopLeftMargin.Height));
                //ndiagram.DocumentFrame.Position = FindNearestGridPoint(new PointF(Sheet.Position.X, Sheet.Position.Y));
                //this.DocPosition = new PointF(Sheet.Position.X + Sheet.TopLeftMargin.Width,
                //ndiagram.SheetPosition = new PointF(Sheet.Position.X, Sheet.Position.Y);
                //Sheet.Position.Y + Sheet.TopLeftMargin.Height);
                //ndiagram.DocumentFrame.Position = new PointF(0, 0);
            }
        }
        protected void OnSheetChanged(object sender, EventArgs e)
        {
            RepositionDocument();
            if (SheetChanged != null)
                SheetChanged(sender, e);
        }

        protected override void PrintDecoration(Graphics g, PrintPageEventArgs e, int hpnum, int hpmax, int vpnum, int vpmax)
        {
            //  base.PrintDecoration(g, e, hpnum, hpmax, vpnum, vpmax);
        }

        protected DialogResult ShowPageSetupDialog(PrintDocument pd, bool Prompt)
        {
            if (myPrintSettings == null)
                myPrintSettings = new DiagramPrintSettings();

            SetDocumentSettings(pd);

            SetColor(myPrintSettings.PageSet.Color);

            #region Show the printer setup (if prompt = true)
            PageSetupDialog dlg = new PageSetupDialog();
            dlg.EnableMetric = true;
            dlg.AllowPrinter = false;
            dlg.ShowNetwork = false;
            dlg.AllowPaper = true;
            dlg.AllowOrientation = true;

            dlg.Document = pd;
            Sheet.TopLeftMargin = new SizeF(myPrintSettings.PageSet.Margins.Left, myPrintSettings.PageSet.Margins.Top);//myPrintSettings.PageSet.Margins.Left, myPrintSettings.PageSet.Margins.Top);
            Sheet.BottomRightMargin = new SizeF(myPrintSettings.PageSet.Margins.Right, myPrintSettings.PageSet.Margins.Bottom);//myPrintSettings.PageSet.Margins.Right, myPrintSettings.PageSet.Margins.Bottom);            

            if (myPrintSettings.PageSet != null)
            {
                dlg.PageSettings = myPrintSettings.PageSet;
            }

            dlg.PageSettings.Landscape = myPrintSettings.PageSet.Landscape;
            dlg.PageSettings.PaperSize = myPrintSettings.PageSet.PaperSize;

            if (Prompt)
            {
                DialogResult result = dlg.ShowDialog(this);
                if (result != DialogResult.OK)
                    return result;
            }
            #endregion

            #region Calculate Ratio

            myPrintSettings.PageSet = dlg.PageSettings;
            myPrintSettings.PageSet.PrinterSettings = dlg.PrinterSettings;
            myPrintSettings.PageSet.Landscape = dlg.PageSettings.Landscape;
            myPrintSettings.PageSet.PaperSize = dlg.PageSettings.PaperSize;

            //myPrintSettings.PageSet.Margins = dlg.PageSettings.Margins;
            //myPrintSettings.HardMarginX = dlg.PageSettings.HardMarginX;
            //myPrintSettings.HardMarginY = dlg.PageSettings.HardMarginY;

            myPrintSettings.ContentBounds = GetContentBounds();
            myPrintSettings.CalculateRatio();

            #endregion

            #region Update Sheet
            if (myPrintSettings.Ratio > 0)
            {
                PrintScale = myPrintSettings.Ratio;
            }
            //Sheet.TopLeftMargin = new SizeF(myPrintSettings.PageSet.Margins.Left, myPrintSettings.PageSet.Margins.Top);
            //Sheet.BottomRightMargin = new SizeF(myPrintSettings.PageSet.Margins.Right, myPrintSettings.PageSet.Margins.Bottom);//Sheet.TopLeftMargin;
            //new SizeF(myPrintSettings.PageSet.Margins.Right, myPrintSettings.PageSet.Margins.Bottom);

            Sheet.Bounds = myPrintSettings.SheetBounds;
            Grid.Visible = false;
            #endregion

            pd.PrintController = new StandardPrintController();
            CenterView();
            SheetStyle = GoViewSheetStyle.Sheet;
            SetupPageBreaks();
            OnSheetChanged(this, EventArgs.Empty);
            RepositionDocument();
            return DialogResult.OK;
        }

        // Private Methods (2) 

        private void CenterView()
        {
            SheetStyle = GoViewSheetStyle.Sheet;
            RectangleF b = ComputeDocumentBounds();
            PointF c = new PointF(b.X + b.Width / 2, b.Y + b.Height / 2);
            float s = DocScale;
            if (b.Width > 0 && b.Height > 0)
                RescaleWithCenter(s, c);
        }

        public bool BrushesRemoved;
        public void SetColor(bool color)
        {
            if (!color)
            {
                if (BrushesRemoved)
                    return;
                DocumentController controller = new DocumentController(Document);
                controller.RemoveBrushes();
                BrushesRemoved = true;
            }
            else
            {
                if (!BrushesRemoved)
                    return;
                DocumentController controller = new DocumentController(Document);
                controller.ApplyBrushes();
                BrushesRemoved = false;
            }
        }

        private void SetDocumentSettings(PrintDocument pd)
        {
            if (myPrintSettings.PageSet != null)
            {
                pd.PrinterSettings.PrinterName = myPrintSettings.PageSet.PrinterSettings.PrinterName;
                pd.DefaultPageSettings = myPrintSettings.PageSet as PageSettings;
                pd.PrinterSettings = myPrintSettings.PrintSettings.Clone() as PrinterSettings;

                //pd.PrinterSettings.DefaultPageSettings.PaperSize.Kind = myPrintSettings.PageSet.PaperSize.Kind;
                /*   string name = myPrintSettings.PageSet.PaperSize.PaperName;
                   pd.PrinterSettings.DefaultPageSettings.PaperSize =
                       new PaperSize(name, myPrintSettings.PageSet.PaperSize.Width,
                                     myPrintSettings.PageSet.PaperSize.Height);

                   /*pd.PrinterSettings.DefaultPageSettings.PaperSize = myPrintSettings.PageSet.PaperSize;
                   pd.PrinterSettings.DefaultPageSettings.PaperSize.Width = myPrintSettings.PageSet.PaperSize.Width;
                   pd.PrinterSettings.DefaultPageSettings.PaperSize.Height = myPrintSettings.PageSet.PaperSize.Height;*/

                //pd.PrinterSettings.DefaultPageSettings.PrinterSettings = myPrintSettings.PrintSettings;
                //pd.PrinterSettings.DefaultPageSettings.Landscape = myPrintSettings.PageSet.Landscape;
                //pd.PrinterSettings.DefaultPageSettings.Margins = myPrintSettings.PageSet.Margins;
            }
        }

        #endregion Methods

        #region Properties

        //private PrintDocument printDocument;
        private DiagramPrintSettings myPrintSettings;
        public DiagramPrintSettings MyPrintSettings
        {
            get { return myPrintSettings; }
            set { myPrintSettings = value; }
        }

        #endregion

        #region Printing Overrides

        protected override DialogResult PrintShowDialog(PrintDocument pd)
        {
            DialogResult res = DialogResult.Ignore;
            pd.PrintController = new StandardPrintController();
            if (ShowPageSetupDialog(pd, false) == DialogResult.OK)
            {
                //return base.PrintShowDialog(pd);
                return DialogResult.OK;
            }
            else
            {
                res = DialogResult.Cancel;
            }
            return res;
        }
        protected override void PrintPreviewShowDialog(PrintDocument pd)
        {
            pd.PrinterSettings = MyPrintSettings.PrintSettings;
            base.PrintPreviewShowDialog(pd);
        }
        public override void Print()
        {
            GoViewGridStyle oldgrid = GridStyle;
            try
            {
                GridStyle = GoViewGridStyle.None;
                base.Print();
            }
            catch (Exception x)
            {
                LogEntry logEntry = new LogEntry();
                logEntry.Message = x.Message + Environment.NewLine + "StackTrace:" + x.StackTrace + Environment.NewLine + "TargetSite:" + x.TargetSite;
                logEntry.Title = "Printing Failed";
                Logger.Write(logEntry);
                MessageBox.Show(this, "Printing failed. The error has been logged.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                GridStyle = oldgrid;
            }
        }

        #endregion

        public void DoPageSetup()
        {
            PrintDocument doc = new PrintDocument();
            doc.PrintController = new StandardPrintController();
            Grid.Visible = false;
            RepositionDocument();
            ShowPageSetupDialog(doc, false);
            OnPageBreaksChanged(this, EventArgs.Empty);
        }
        //public void PageSetup()
        //{
        //    PrintDocument doc = new PrintDocument();
        //    doc.PrinterSettings = MyPrintSettings.PrintSettings;
        //    doc.PrintController = new StandardPrintController();
        //    Grid.Visible = false;
        //    ShowPageSetupDialog(doc, true);
        //    Grid.Visible = true;
        //    OnPageBreaksChanged(this, EventArgs.Empty);
        //}

        private RectangleF GetContentBounds()
        {

            //if (Document is NormalDiagram)
            //{
            //    RectangleF x = (Document as NormalDiagram).DocumentFrame.Bounds;
            //    return x;
            //}

            //return this.DocExtent;
            // no need for the rest of this
            PointF leftTopMost = new PointF(59, 59);
            //PointF leftTopMost = new PointF(MyPrintSettings.PrintSettings.DefaultPageSettings.Margins.Left, MyPrintSettings.PrintSettings.DefaultPageSettings.Margins.Top);
            PointF rightBottom = new PointF(-50000, -50000);

            //if (this.Document is NormalDiagram)
            //    rightBottom = new PointF((this.Document as NormalDiagram).DocumentFrame.Right, (this.Document as NormalDiagram).DocumentFrame.Bottom);
            //else
            {
                foreach (GoObject o in Document)
                {
                    if (!(o is GoSheet))// && !(o is FrameLayerGroup) && !(o is FrameLayerRect))
                    {
                        if (o.Position.X + o.Width > rightBottom.X)
                        {
                            rightBottom.X = o.Position.X + o.Width;
                        }
                        if (o.Position.Y + o.Height > rightBottom.Y)
                        {
                            rightBottom.Y = o.Position.Y + o.Height;
                        }
                        //if (o.Position.X < leftTopMost.X)
                        //{
                        //    leftTopMost.X = o.Position.X;
                        //}
                        //if (o.Position.Y < leftTopMost.Y)
                        //{
                        //    leftTopMost.Y = o.Position.Y;
                        //}
                    }
                }
            }
            //rightBottom.X = rightBottom.X + 70;
            //rightBottom.Y = rightBottom.Y + 100;

            //leftTopMost.X = 60;
            //leftTopMost.Y = 60;

            RectangleF retval = new RectangleF(new PointF(leftTopMost.X, leftTopMost.Y), new SizeF(rightBottom.X - leftTopMost.X, rightBottom.Y - leftTopMost.Y));
            //RectangleF retval = new RectangleF(new PointF(leftTopMost.X, leftTopMost.Y), new SizeF(rightBottom.X, rightBottom.Y));
            return retval;
        }
    }
}