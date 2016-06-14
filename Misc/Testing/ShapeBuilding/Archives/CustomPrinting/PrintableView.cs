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

namespace ShapeBuilding.Archives.CustomPrinting
{
    public class PrintableView : GoDrawView
    {
        //private PrintDocument printDocument;
        private DiagramPrintSettings myPrintSettings;
        public DiagramPrintSettings MyPrintSettings
        {
            get { return myPrintSettings; }
            set { myPrintSettings = value; }
        }

        public PrintableView()
        {
            myPrintSettings = new DiagramPrintSettings();
        }
        /* public void UpdateSize(MetaBuilder.Graphing.Controllers.CropType type)
         {
             if (Document is NormalDiagram)
             {
                 NormalDiagram ndiagram = Document as NormalDiagram;
                 switch (type)
                 {
                     case CropType.ToFrame:
                         // Adjust the sheet to fit the frame - do not adjust the sheet
                         Sheet.Size =
                             new SizeF(
                                 ndiagram.DocumentFrame.Width + Sheet.TopLeftMargin.Width + Sheet.BottomRightMargin.Width,
                                 ndiagram.DocumentFrame.Height + +Sheet.TopLeftMargin.Height +
                                 Sheet.BottomRightMargin.Height);
                         Sheet.Position =
                             new PointF(ndiagram.DocumentFrame.Position.X - Sheet.TopLeftMargin.Width,
                                        ndiagram.DocumentFrame.Position.Y - Sheet.TopLeftMargin.Width);

                         break;
                     case CropType.ToSheet:
                         //  Adjust the frame to fit the sheet - do not adjust the frame
                         ndiagram.DocumentFrame.Width = Sheet.Width - Sheet.TopLeftMargin.Width -
                                                        Sheet.BottomRightMargin.Width - 1;
                         ndiagram.DocumentFrame.Height = Sheet.Height - Sheet.TopLeftMargin.Height -
                                                         Sheet.BottomRightMargin.Height - 1;

                         ndiagram.DocumentFrame.Position =
                             new PointF(Sheet.Position.X + Sheet.TopLeftMargin.Width,
                                        Sheet.Position.Y + Sheet.TopLeftMargin.Height);

                         break;
                 }
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
             PointF newSheetPosition =
                 new PointF(leftTopMost.X + Sheet.TopLeftMargin.Width, leftTopMost.Y + Sheet.BottomRightMargin.Height);
             Sheet.Position = Grid.FindNearestGridPoint(newSheetPosition, Sheet);
            
             Document.TopLeft = Grid.FindNearestGridPoint(newSheetPosition, Sheet);
         }*/
        protected override void OnPaint(PaintEventArgs evt)
        {
            try
            {
                base.OnPaint(evt);
            }
            catch
            {
                //// Console.WriteLine("Error painting!");
            }
        }

        public event EventHandler SheetChanged;
        protected void OnSheetChanged(object sender, EventArgs e)
        {
            if (SheetChanged != null)
                SheetChanged(sender, e);
        }

        protected override void PrintDecoration(Graphics g, PrintPageEventArgs e, int hpnum, int hpmax, int vpnum, int vpmax)
        {
            //  base.PrintDecoration(g, e, hpnum, hpmax, vpnum, vpmax);
        }

        public event EventHandler PageBreaksChanged;

        protected void OnPageBreaksChanged(object sender, EventArgs e)
        {
            if (PageBreaksChanged != null)
                PageBreaksChanged(sender, e);
        }

        protected DialogResult ShowPageSetupDialog(PrintDocument pd, bool Prompt)
        {
            if (myPrintSettings == null)
                myPrintSettings = new DiagramPrintSettings();

            SetDocumentSettings(pd);

            #region Show the printer setup (if prompt = true)
            PageSetupDialog dlg = new PageSetupDialog();
            dlg.EnableMetric = true;
            dlg.AllowPrinter = true;
            dlg.AllowPaper = true;
            dlg.AllowOrientation = true;

            if (myPrintSettings.PageSet != null)
            {
                dlg.PageSettings = myPrintSettings.PageSet;
            }
            dlg.Document = pd;
            if (Prompt)
            {
                DialogResult result = dlg.ShowDialog(this);
                if (result != DialogResult.OK) return result;
            }
            #endregion

            Grid.Visible = false;
            #region Calculate Ratio
            myPrintSettings.PageSet = dlg.PageSettings;
            myPrintSettings.PageSet.Landscape = dlg.PageSettings.Landscape;
            myPrintSettings.PageSet.PaperSize = dlg.PageSettings.PaperSize;
            myPrintSettings.PageSet.Margins = dlg.PageSettings.Margins;
            myPrintSettings.HardMarginX = dlg.PageSettings.HardMarginX;
            myPrintSettings.HardMarginY = dlg.PageSettings.HardMarginY;


            myPrintSettings.ContentBounds = GetContentBounds();
            myPrintSettings.CalculateRatio();
            #endregion
            #region Update Sheet
            PrintScale = myPrintSettings.Ratio;
            Sheet.TopLeftMargin = new SizeF(myPrintSettings.PageSet.Margins.Left, myPrintSettings.PageSet.Margins.Top);
            Sheet.BottomRightMargin = new SizeF(myPrintSettings.PageSet.Margins.Right, myPrintSettings.PageSet.Margins.Bottom);

            Sheet.Bounds = myPrintSettings.SheetBounds;
            Grid.Visible = true;
            #endregion
            pd.PrintController = new StandardPrintController();
            CenterView();
            SheetStyle = GoViewSheetStyle.Sheet;
            SetupPageBreaks();
            return DialogResult.OK;
        }
        public void SetupPageBreaks()
        {
            int paperw = (MyPrintSettings.PageSet.Landscape) ? MyPrintSettings.PageSet.PaperSize.Height : MyPrintSettings.PageSet.PaperSize.Width;
            int paperh = (MyPrintSettings.PageSet.Landscape) ? MyPrintSettings.PageSet.PaperSize.Width : MyPrintSettings.PageSet.PaperSize.Height;
            int pagew = paperw - MyPrintSettings.PageSet.Margins.Left - MyPrintSettings.PageSet.Margins.Right;
            int pageh = paperh - MyPrintSettings.PageSet.Margins.Top - MyPrintSettings.PageSet.Margins.Bottom;
            GridCellSize = new SizeF(pagew / PrintScale, pageh / PrintScale);
            GridOrigin = PrintDocumentTopLeft;
            GridStyle = GoViewGridStyle.Line;
            GridPenDashStyle = DashStyle.Dash;
            GridColor = Color.Blue;
            BackColor = Color.WhiteSmoke;
        }

        private void CenterView()
        {
            SheetStyle = GoViewSheetStyle.Sheet;
            RectangleF b = ComputeDocumentBounds();
            PointF c = new PointF(b.X + b.Width / 2, b.Y + b.Height / 2);
            float s = DocScale;
            if (b.Width > 0 && b.Height > 0)
                RescaleWithCenter(s, c);
        }

        private void SetDocumentSettings(PrintDocument pd)
        {
            if (myPrintSettings.PageSet != null)
            {
                pd.DefaultPageSettings = myPrintSettings.PageSet.Clone() as PageSettings;
                pd.PrinterSettings.PrinterName = myPrintSettings.PageSet.PrinterSettings.PrinterName;
                pd.PrinterSettings = myPrintSettings.PrintSettings.Clone() as PrinterSettings;
                pd.PrinterSettings.DefaultPageSettings.PaperSize = myPrintSettings.PageSet.PaperSize;
                //  pd.PrinterSettings.DefaultPageSettings.Landscape = myPrintSettings.IsLandscape;
                pd.PrinterSettings.DefaultPageSettings.Margins = myPrintSettings.PageSet.Margins;
            }
        }

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
            GoViewSheetStyle oldSheetStyle = GoViewSheetStyle.None;
            if (myPrintSettings.Adjustment == DiagramPrintSettings.PageAdjustment.Fit)
            {
                oldSheetStyle = SheetStyle;
                SheetStyle = GoViewSheetStyle.None;
            }
            try
            {
                base.Print();
            }
            catch (Exception x)
            {
                MessageBox.Show(this, "Printing failed. The error has been logged.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogEntry logEntry = new LogEntry();
                logEntry.Message = x.Message + Environment.NewLine + "StackTrace:" + x.StackTrace + Environment.NewLine +
                                   "TargetSite:" + x.TargetSite;
                logEntry.Title = "Printing Failed";
                Logger.Write(logEntry);
            }
            SheetStyle = oldSheetStyle;
        }

        public void DoPageSetup()
        {
            PrintDocument doc = new PrintDocument();
            doc.PrintController = new StandardPrintController();
            Grid.Visible = true;
            ShowPageSetupDialog(doc, false);
            OnPageBreaksChanged(this, EventArgs.Empty);
        }
        public void PageSetup()
        {
            PrintDocument doc = new PrintDocument();
            doc.PrintController = new StandardPrintController();
            Grid.Visible = false;
            ShowPageSetupDialog(doc, true);
            Grid.Visible = true;
            OnPageBreaksChanged(this, EventArgs.Empty);
        }

        private RectangleF GetContentBounds()
        {
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
            RectangleF retval =
                new RectangleF(leftTopMost, new SizeF(rightBottom.X - leftTopMost.X, rightBottom.Y - leftTopMost.Y));
            return retval;
        }
    }
}