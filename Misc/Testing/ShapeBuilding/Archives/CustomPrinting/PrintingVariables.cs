using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;
namespace ShapeBuilding.CustomPrinting
{
    public class PrintingVariables
    {
        private SizeF documentSize;
        public SizeF DocumentSize
        {
            get { return documentSize; }
            set { documentSize = value; }
        }


        private Margins targetPageMargins;
        public Margins TargetPageMargins
        {
            get { return targetPageMargins; }
            set { targetPageMargins = value; }
        }

        private SizeF targetPageInner;
        public SizeF TargetPageInner
        {
            get { return targetPageInner; }
            set { targetPageInner = value; }
        }

        public SizeF TargetPageSize
        {
            get
            {
                SizeF retval = new SizeF();
                retval.Height = targetPageInner.Height + TargetPageMargins.Top + TargetPageMargins.Bottom;
                retval.Width = targetPageInner.Width + TargetPageMargins.Left + TargetPageMargins.Right;
                return retval;
            }
        }
        private bool landscape;
        public bool Landscape
        {
            get { return landscape; }
            set { landscape = value; }
        }

        private PrintJobType jobType;
        public PrintJobType JobType
        {
            get { return jobType; }
            set { jobType = value; }
        }

        private float ratio;
        public float Ratio
        {
            get { return ratio; }
            set { ratio = value; }
        }

        public enum PrintJobType
        {
            Fit, Normal
        }
        private int fitAcross;
        public int FitAcross
        {
            get { return fitAcross; }
            set { fitAcross = value; }
        }
        private int fitDown;
        public int FitDown
        {
            get { return fitDown; }
            set { fitDown = value; }
        }
        public Outcome CalculateRatio()
        {
            float widthRatio;
            float heightRatio;

            widthRatio = TargetPageInner.Width * FitAcross / (DocumentSize.Width + 10);
            heightRatio = TargetPageInner.Height * FitDown / (DocumentSize.Height + 10);
            if (Landscape)
            {
                if (TargetPageInner.Width<TargetPageInner.Height)
                {
                    widthRatio = TargetPageInner.Height / (DocumentSize.Width + 10);
                    heightRatio = TargetPageInner.Width / (DocumentSize.Height + 10);
                }
            }

            Ratio = Math.Min(widthRatio, heightRatio);
            // if Ratio < 1, printjob should be smaller
#if DEBUG
            Console.WriteLine("WidthRatio: " + widthRatio.ToString());
            Console.WriteLine("HeightRatio: " + heightRatio.ToString());
            Console.WriteLine("Ratio: " + Ratio.ToString());
#endif
            
        //    if (Ratio < 1)
            {
                decimal TruncatedRatio = Math.Round(decimal.Parse(Ratio.ToString()), 2); // rather print it a little bit smaller (f0kking borders!)
                //Ratio = float.Parse(TruncatedRatio.ToString());
            }

            

            Outcome retval = Outcome.NotSet;
            switch (JobType)
            {
                case PrintingVariables.PrintJobType.Normal:
                    break;
                case PrintingVariables.PrintJobType.Fit:
                    if (Ratio < 1)
                    {
                        retval = Outcome.ShrinkDrawing;
                    }
                    else if (Ratio > 1)
                    {
                        retval = Outcome.ExpandDrawing;
                    }
                    else
                    {
                        retval = Outcome.Normal;
                    }
                    break;
            }
#if DEBUG
            Console.WriteLine("Outcome:" + retval.ToString());
            Console.WriteLine("---------------------------------------");
#endif
            return retval;
        }

    }
}
