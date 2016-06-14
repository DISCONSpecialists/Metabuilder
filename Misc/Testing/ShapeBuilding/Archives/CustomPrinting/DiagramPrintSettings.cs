using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
namespace ShapeBuilding.CustomPrinting
{
    public class DiagramPrintSettings
    {

        private PageSettings pageSet;

        public PageSettings PageSet
        {
            get { return pageSet; }
            set { pageSet = value; }
        }
	
        private PrinterSettings printSettings;
        public PrinterSettings PrintSettings
        {
            get { return printSettings; }
            set { printSettings = value; }
        }

        private bool cancelled;

        public bool Cancelled
        {
            get { return cancelled; }
            set { cancelled = value; }
        }

        private bool isLandscape;

        public bool IsLandscape
        {
            get { return isLandscape; }
            set { isLandscape = value; }
        }

        public DiagramPrintSettings()
        {
            adjustment = PageAdjustment.Normal;
        }
        private PageAdjustment adjustment;

        public PageAdjustment Adjustment
        {
            get { return adjustment; }
            set { adjustment = value; }
        }
        public enum PageAdjustment
        {
            Normal,Fit,NoCalculations
        }





    }
}
