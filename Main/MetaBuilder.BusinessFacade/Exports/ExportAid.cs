using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace MetaBuilder.BusinessFacade.Exports
{
    public static class ExportAid
    {
        public static string GetCustomFilename(string title, string filter)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = title;
            sfd.Filter = filter;
            sfd.InitialDirectory = Core.Variables.Instance.ExportsPath;
            //sfd.ShowDialog(this);
            if (sfd.ShowDialog() == DialogResult.Cancel)
                return "";
            return sfd.FileName;
        }

        public static bool CancelExport(string filename)
        {
            if (filename.Length > 0)
                if (Directory.Exists(Core.strings.GetPath(filename)))
                    return false;

            return true;
        }

    }
}
