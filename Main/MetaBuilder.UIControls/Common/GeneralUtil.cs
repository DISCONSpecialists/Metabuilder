using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.UIControls.GraphingUI;

namespace MetaBuilder.UIControls.Common
{
    public class GeneralUtil
    {

        #region Methods (2)

        // Public Methods (1) 

        public static void LaunchHyperlink(Hyperlink link)
        {
            switch (link.HyperlinkType)
            {
                case HyperlinkType.File:
                    if (link.IsWordDocument)
                    {
                        if (System.IO.File.Exists(link.Arguments))
                            MetaBuilder.BusinessFacade.Tools.WordHelper.OpenFileInWord(link.Arguments, link.BookmarkName);
                        else
                            System.Windows.Forms.MessageBox.Show("File not found");
                    }
                    else
                    {
                        if (System.IO.File.Exists(link.Arguments))
                            SafeStartProcess(link);
                        else
                            System.Windows.Forms.MessageBox.Show("File not found");
                    }
                    break;
                case HyperlinkType.Diagram:
                    if (System.IO.File.Exists(link.Arguments))
                        DockingForm.DockForm.OpenFileInApplicableWindow(link.Arguments, true);
                    else
                        System.Windows.Forms.MessageBox.Show("File not found");
                    break;

                case HyperlinkType.URL:
                    SafeStartProcess(link);
                    break;
            }
        }

        // Private Methods (1) 

        private static void SafeStartProcess(Hyperlink link)
        {
            try
            {
                Process.Start(link.Arguments);
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog("Cannnot open (" + link.Arguments + ") because of " + Environment.NewLine + ex.ToString());
            }
        }

        #endregion Methods

    }
}