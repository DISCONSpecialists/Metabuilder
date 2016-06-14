//using Microsoft.Office.Core;
using System;
using System.Reflection;
using InteropWord2K;
using Variables=MetaBuilder.Core.Variables;
using Word = InteropWord2K;

namespace MetaBuilder.BusinessFacade.Exports
{
	/// <summary>
	/// Summary description for FSDToWord.
	/// </summary>
	public class HierarchicalToWord : HierarchicalToText
	{
        public HierarchicalToWord()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private Application application;
		private Document currentDocument;

        [STAThread]
		public override void Save(string filename)
		{
			OnExportCompleted(EventArgs.Empty);

            string FileName = Variables.Instance.ExportsPath + filename + ".doc";
		//	StreamWriter writer = new StreamWriter(FileName);

			object readOnly = false;
			object isVisible = true;
			object missing = Missing.Value;
			application = new ApplicationClass();
			application.Visible = true;
			currentDocument = application.Documents.Add(ref missing, ref missing, ref missing, ref missing);
            // Console.WriteLine(ReportText);
			//Clipboard.SetDataObject(ReportText, false);
			currentDocument.Paragraphs.Add(ref missing);
			currentDocument.Activate();
			//currentDocument.Paragraphs.Item(1).Range.Paste();
            currentDocument.Paragraphs.Item(1).Range.Text = ReportText;
			object fsd = (object) FileName;
			currentDocument.SaveAs(ref fsd, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
			//, ref missing, ref missing);
			//, ref missing);//, ref missing, ref missing);
			((_Document)currentDocument).Close(ref missing, ref missing, ref missing);
			((_Application)application).Quit(ref missing, ref missing, ref missing);
		}
	}
}