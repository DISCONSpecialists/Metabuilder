using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using InteropWord2K;

namespace MetaBuilder.BusinessFacade.Tools
{
    public class WordHelper
    {

        public static List<string> GetBookmarksInDocument(string fullfilename)
        {
            List<string> retval = new List<string>();
            try
            {
                object missing = Missing.Value;
                object what = WdGoToItem.wdGoToBookmark;
                Application application = OpenFile(fullfilename);
                application.Visible = false;
                foreach (InteropWord2K.Bookmark bmrk in application.ActiveDocument.Bookmarks)
                {
                    retval.Add(bmrk.Name);
                }
                application.Quit(ref missing, ref missing, ref missing);
             }catch{}

            return retval;
        }

        public static void OpenFileInWord(string fullfilename, string bookmarkname)
        {
            try
            {
                object missing = Missing.Value;
                object bookmarkName = bookmarkname;
                object what = WdGoToItem.wdGoToBookmark;
                Application application = OpenFile(fullfilename);
                if (bookmarkname != null)
                    if (bookmarkname.Length > 0)
                        try
                        {
                            application.Selection.GoTo(ref what, ref missing, ref missing, ref bookmarkName);
                        }
                        catch
                        {
                        }
            }
            catch { }
        }

        private static Application OpenFile(string fullfilename)
        {
            Application application;
			object readOnly = false;
			object isVisible = true;
			object missing = Missing.Value;
			application = new ApplicationClass();
			application.Visible = true;
         

            object filename = fullfilename;
          //  application.ChangeFileOpenDirectory(file_path);
            application.Documents.Open(ref filename, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
            return application;
        }
    }
}
