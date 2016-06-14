using System;
using System.Drawing;
using System.Drawing.Printing;

namespace MetaBuilder.Graphing.Formatting
{
    public class DiagramPrintSettings
    {
        #region Fields (13)

        private PageAdjustment adjustment;
        private bool cancelled;
        private RectangleF contentBounds;
        private int copies;
        private int fitAcross;
        private int fitDown;
        private float hardMarginX;
        private float hardMarginY;
        private PageSettings pageSet;
        private PrinterSettings printSettings;
        private float ratio;
        private RectangleF sheetBounds;
        private bool showPageBreaks;

        #endregion Fields

        #region Enums (1)

        public enum PageAdjustment
        {
            Normal,
            Fit,
            Expand
        }

        #endregion Enums

        #region Constructors (1)

        public DiagramPrintSettings()
        {
            PageSet = new PageSettings();
            PrintSettings = new PrinterSettings();
            PrintSettings.DefaultPageSettings.Margins = PageSet.Margins;
            adjustment = PageAdjustment.Normal;
            FitAcross = 1;
            FitDown = 1;
        }

        #endregion Constructors

        #region Properties (14)

        public PageAdjustment Adjustment
        {
            get { return adjustment; }
            set { adjustment = value; }
        }

        public bool Cancelled
        {
            get { return cancelled; }
            set { cancelled = value; }
        }

        public RectangleF ContentBounds
        {
            get { return contentBounds; }
            set { contentBounds = value; }
        }

        public int Copies
        {
            get { return copies; }
            set { copies = value; }
        }

        public int FitAcross
        {
            get { return fitAcross; }
            set { fitAcross = value; }
        }

        public int FitDown
        {
            get { return fitDown; }
            set { fitDown = value; }
        }

        public float HardMarginX
        {
            get { return hardMarginX; }
            set { hardMarginX = value; }
        }

        public float HardMarginY
        {
            get { return hardMarginY; }
            set { hardMarginY = value; }
        }

        public PageSettings PageSet
        {
            get { return pageSet; }
            set { pageSet = value; }
        }

        public RectangleF PrintableArea
        {
            get { return PageSet.PrintableArea; }
        }

        public PrinterSettings PrintSettings
        {
            get { return printSettings; }
            set { printSettings = value; }
        }

        public float Ratio
        {
            get { return ratio; }
            set { ratio = value; }
        }

        public RectangleF SheetBounds
        {
            get { return sheetBounds; }
            set { sheetBounds = value; }
        }

        public bool ShowPageBreaks
        {
            get { return showPageBreaks; }
            set { showPageBreaks = value; }
        }

        #endregion Properties

        #region Methods (2)

        // Public Methods (1) 
        //Version win8version = new Version(6, 2, 9200, 0);
        public void CalculateRatio()
        {
            Ratio = 1f;

            float fitAcross = Convert.ToSingle(FitAcross);//, System.Globalization.CultureInfo.InvariantCulture);
            float fitDown = Convert.ToSingle(FitDown);//, System.Globalization.CultureInfo.InvariantCulture);

            float widthToUse = (PageSet.Landscape) ? PageSet.PaperSize.Height : PageSet.PaperSize.Width;
            float heightToUse = (PageSet.Landscape) ? PageSet.PaperSize.Width : PageSet.PaperSize.Height;

            #region ratio fix

            float xratioW = (PageSet.Landscape) ? 30f / 1169f : 30f / 827f; // 2480px @ 300dpi A4 1169 0.025 : 0.036
            float xratioH = (PageSet.Landscape) ? 30f / 827f : 30f / 1169f; // 3508px @ 300dpi A4 827 0.036 : 0.025
            //A4 = +-30f
            float amountW = widthToUse * xratioW;
            if (amountW > 100)
                amountW = 100f;
            float amountH = heightToUse * xratioH;
            if (amountH > 100)
                amountH = 100f;

            #endregion

            float innerSumWidth = (fitAcross * (widthToUse - (PageSet.Margins.Left + PageSet.Margins.Right + amountW)));
            float innerSumHeight = (fitDown * (heightToUse - (PageSet.Margins.Top + PageSet.Margins.Bottom + amountH)));

            float w = ContentBounds.Width;
            float h = ContentBounds.Height;

            Ratio = Math.Min(innerSumWidth / w, innerSumHeight / h);

            //if (Adjustment == PageAdjustment.Fit)
            //{
            //    // Get ratio
            //    Ratio = Math.Min(innerSumWidth / w, innerSumHeight / h);
            //    if (Ratio > 1)
            //        Ratio = 1f;
            //    //if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= win8version)
            //    //Core.Log.WriteLog("DiagramPrintSettings::CalculateRatio::Ratio(Fit)::" + Environment.OSVersion.Version.ToString() + "==>" + Ratio.ToString());
            //}

            //if (Adjustment == PageAdjustment.Expand)
            //{
            //    // Get ratio
            //    Ratio = Math.Min(innerSumWidth / w, innerSumHeight / h);
            //    if (Ratio < 1)
            //        Ratio = 1f;
            //    //if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= win8version)
            //    //Core.Log.WriteLog("DiagramPrintSettings::CalculateRatio::Ratio(Expand)::" + Environment.OSVersion.Version.ToString() + "==>" + Ratio.ToString());
            //}

            //if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= win8version)
            //{
            Core.Log.WriteLog("DiagramPrintSettings::CalculateRatio([" + PageSet.PrinterResolution.ToString() + "] " + PrintSettings.PrinterName + " " + PageSet.PaperSize.PaperName + Environment.NewLine +
                "PaperWidth:" + widthToUse + "|PaperHeight:" + heightToUse + Environment.NewLine + "|Ratio:" + Ratio + Environment.NewLine +
                "[W:" + xratioW + "{" + amountW + "} H:" + xratioH + "{" + amountH + "}])::" + Environment.OSVersion.Version.ToString() + Environment.NewLine +
                "FitAcross : " + FitAcross + "==>" + fitAcross + Environment.NewLine + "FitDown : " + FitDown + "==>" + fitDown + Environment.NewLine +
                "innerSumWidth : fitAcross * " + (widthToUse - PageSet.Margins.Left - PageSet.Margins.Right) + "==>" + innerSumWidth.ToString() + Environment.NewLine +
                "innerSumHeight : fitDown * " + (heightToUse - PageSet.Margins.Top - PageSet.Margins.Bottom) + "==>" + innerSumHeight.ToString());
            //}

            UpdateSheetBounds(innerSumWidth, innerSumHeight);
        }

        // Private Methods (1) 

        private void UpdateSheetBounds(float w, float h)
        {
            // Update Sheet Size
            PointF sheetPos = new PointF(ContentBounds.X - PageSet.Margins.Left, ContentBounds.Y - PageSet.Margins.Top);
            //new PointF(ContentBounds.X, ContentBounds.Y);
            //new PointF(ContentBounds.X - PageSet.Margins.Left, ContentBounds.Y - PageSet.Margins.Top);
            float sheetWidth = (w / Ratio) + PageSet.Margins.Left + PageSet.Margins.Right;
            float sheetHeight = (h / Ratio) + PageSet.Margins.Top + PageSet.Margins.Bottom;

            //if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= win8version)
            //Core.Log.WriteLog("DiagramPrintSettings::UpdateSheetBounds::sheetPos::" + Environment.OSVersion.Version.ToString() + "==>" + sheetPos.ToString() + Environment.NewLine + "w(innerSumWidth/Ratio+Margins*2)==>" + sheetWidth + "h(innerSumHeight/Ratio+Margins*2)==>" + sheetHeight);

            SheetBounds = new RectangleF(sheetPos, new SizeF(sheetWidth, sheetHeight));
        }

        #endregion Methods
    }
}