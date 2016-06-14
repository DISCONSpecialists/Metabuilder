using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace MetaBuilder.UIControls.SWOT
{
    public class SWOTwriter
    {

        #region Fields (3)

        private List<SWOTDetail> details;
        private string exportPath;
        private StringBuilder output;

        #endregion Fields

        #region Constructors (1)

        public SWOTwriter()
        {
        }

        #endregion Constructors

        #region Properties (2)

        public List<SWOTDetail> Details
        {
            get { return details; }
            set { details = value; }
        }

        public string ExportPath
        {
            get { return exportPath; }
            set { exportPath = value; }
        }

        #endregion Properties

        #region Methods (7)

        // Public Methods (1) 

        public void DoWrite()
        {
            output = new StringBuilder();
            AddHeader();
            AddItems();
            AddFooter();
            WriteFile();
        }

        // Private Methods (6) 

        private void AddFooter()
        {
            AppendLine("</TABLE></BODY></HTML>");
        }

        private void AddHeader()
        {
            AppendLine("<html><title>SWOT Analysis</title>");
            AppendLine("<head><style>");
            AppendLine(".header{");
            AppendLine("font-family:tahoma;font-size:12pt;font-weight:bold;}");
            AppendLine(".cellSummary{font-family:tahoma;font-size:10pt;font-weight:bold}");
            AppendLine(".cellText{font-family:tahoma;font-size:8pt;}");
            AppendLine("</style></head><BODY>");
        }

        private void AddItem(int index, int ItemCount)
        {
            {
                AppendLine("<TR><TD colspan=2 align=center><img src=\"SWOTItem" + ItemCount.ToString() + ".png\"></TD></TR>");
                AppendLine("<TR><TD colspan=2 class=\"cellSummary\" align=\"justify\" style=\"PAGE-BREAK-AFTER: always\" clear=\"all\">" + HttpUtility.HtmlEncode(details[index].ImplicationName) + "</TD></TR>");
            }
        }

        private void AddItems()
        {
            AppendLine("<Table>");
            AppendLine("<TR><TD colspan=2 align=center class=\"header\">SWOT Analysis</TD></TR>");
            AppendLine("<TR><TD colspan=2 align=center class=\"cellSummary\">Summary</TD></TR>");
            AppendLine("<TR><TD  colspan=2 align=center><img src=\"SWOTSummary.png\"></TD></TR>");
            AppendLine("<TR><TD colspan=2 valign=bottom  class=\"cellText\"><div  class=\"cellSummary\">Notation:</div>(-5,3) 1,5<BR>Implications 1 and 5 both have a SWOT Rating of:<BR>-5 (Strength/Weakness) <BR>3 (Opportunity/Threat)<BR><BR>(-5,3) 1,5,9<BR>Implications 1, 5 and 9 all have a SWOT Rating of:<BR>-5 (Strength/Weakness)<BR>3 (Opportunity/Threat)</TD></TR>");
            AppendLine("<TR><TD colspan=2 align=center class=\"cellSummary\">List of Implications</TD></TR>");
            int itemCount = 1;
            for (int i = 0; i < details.Count; i++)
            {
                if (details[i].IsDisplayed)
                {
                    AppendLine("<TR><TD colspan=2 class=\"cellText\">" + itemCount.ToString() + ") " +
                               HttpUtility.HtmlEncode(details[i].ImplicationName) + "</TD></TR>");
                    itemCount++;
                }
            }
            AppendLine("<TR><TD colspan=2 align=center class=\"cellSummary\" style=\"PAGE-BREAK-AFTER: always\" clear=\"all\"><hr></TD></TR>");
            // Add Main Item
            itemCount = 1;
            for (int i = 0; i < details.Count; i++)
            {
                if (details[i].IsDisplayed)
                {
                    AddItem(i, itemCount);
                    itemCount++;
                }
            }
        }

        private void AppendLine(string text)
        {
            output.Append(text + Environment.NewLine);
        }

        private void WriteFile()
        {
            StreamWriter swriter = File.CreateText(exportPath + "SWOTAnalysis.htm");
            if (swriter != null)
            {
                string outputS = output.ToString();
                swriter.WriteLine(outputS);
                swriter.Close();
            }
        }

        #endregion Methods

        #region Nested Classes (1)

        public class SWOTDetail
        {

            #region Fields (5)

            private string implicationName;
            private string uniqueReference;
            private bool isDisplayed;
            private int number;
            private int opportunityThreatRating;
            private int strengthWeaknessRating;

            #endregion Fields

            #region Constructors (1)

            public SWOTDetail()
            {
                IsDisplayed = true;
            }

            #endregion Constructors

            #region Properties (5)

            public string ImplicationName
            {
                get { return implicationName; }
                set { implicationName = value; }
            }
            public string UniqueReference
            {
                get { return uniqueReference; }
                set { uniqueReference = value; }
            }

            public bool IsDisplayed
            {
                get { return isDisplayed; }
                set { isDisplayed = value; }
            }

            public int Number
            {
                get { return number; }
                set { number = value; }
            }

            public int OpportunityThreatRating
            {
                get { return opportunityThreatRating; }
                set { opportunityThreatRating = value; }
            }

            public int StrengthWeaknessRating
            {
                get { return strengthWeaknessRating; }
                set { strengthWeaknessRating = value; }
            }

            #endregion Properties

            #region Methods (1)

            // Public Methods (1) 

            public override string ToString()
            {
                return (UniqueReference != null ? UniqueReference.ToString() + " " : "") + ImplicationName + " (" + opportunityThreatRating.ToString() + ", " + strengthWeaknessRating.ToString() + ")";
            }

            #endregion Methods

        }
        #endregion Nested Classes

    }
}